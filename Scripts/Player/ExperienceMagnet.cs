using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class ExperienceMagnet : MonoBehaviour, IUpgradeable
{
    
    
    [SerializeField] private GameObject upgradePrefab;
    private int level = 1;
    [SerializeField] private string description = "Increase pickup radius by 1. Knock back enemies every 15 seconds";
    [SerializeField] private float speed;
    [SerializeField] private float knockbackPower = 0;
    [SerializeField] private float knockbackCooldown = 0;
        
    private List<Collider2D> hits = new List<Collider2D>();
    [SerializeField] private ContactFilter2D filter;

    public int Level { get => level; set => level = value; }
    public string Description { get => description; set => description = value; }
    public GameObject UpgradePrefab { get => upgradePrefab; set => upgradePrefab = value; }

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius *= GameModifier.magnetRadiusPercentageMod;
    }
    
    private CircleCollider2D circleCollider;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Experience")
        {
            other.transform.Translate((transform.position - other.transform.position).normalized * speed * Time.deltaTime);
        }
    }
    // Levels
    public void LevelUp()
    {
        Level++;
        switch (level)
        {
            case 2: // Increase pickup radius. Knock enemies back every 15 seconds
                circleCollider.radius += 1;
                knockbackPower = 30f;//25 50 80
                knockbackCooldown = 15f;
                StartCoroutine("Impulse");
                description = "AttractionRune-3-upgradeDescr";
                break;
            case 3:
                circleCollider.radius += 1;
                knockbackCooldown -= 2f;
                description = "AttractionRune-4-upgradeDescr";
                break;
            case 4:
                circleCollider.radius += 1;
                knockbackPower = 60f;
                description = "AttractionRune-5-upgradeDescr";
                break;
            case 5:
                circleCollider.radius += 1;
                knockbackCooldown -= 2f;
                description = "AttractionRune-6-upgradeDescr";
                break;
            case 6: // Increase pickup radius. Increases knock back power
                circleCollider.radius += 1;
                knockbackPower = 90f;
                description = "max";
                GameManager.instance.abilities.Remove(this);
                break;
            // case 7: pull range enemies //?
        }
    }

    IEnumerator Impulse()
    {
        while (true)
        {
            circleCollider.OverlapCollider(filter, hits);
        
            foreach (Collider2D col in hits)
            {
                if (col.CompareTag("Entity"))
                {
                    float magnitude = (col.transform.position - transform.position).magnitude;
                    if (magnitude > 0)
                    {
                        float explosionForce;
                        if (magnitude > 1)
                        {
                            explosionForce = knockbackPower / magnitude;
                        }
                        else
                        {
                            explosionForce = knockbackPower;
                        }
                        Damage dmg = new Damage(0, transform.position, explosionForce);
                        col.SendMessage("ReceiveDamage", dmg);
                    }
                }
            }
            yield return new WaitForSeconds(knockbackCooldown);
        }
        
    }
    

    
}
