using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Entity
{
    [SerializeField] private AudioSource hurtSound;
    protected override void Death()
    {
        hurtSound.pitch = (0.6f);
        hurtSound.Play();
        if (Random.Range(1, 101) <= GameModifier.boxFlaskChance)
        {
            Instantiate(GameManager.instance.flask, transform.position, Quaternion.Euler(0, 0, 0), GameManager.instance.propsFolder);
        } 
        Destroy(gameObject);
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        hurtSound.pitch = (Random.Range(0.8f, 1.2f));
        hurtSound.Play();
        base.ReceiveDamage(dmg);
    }
}
