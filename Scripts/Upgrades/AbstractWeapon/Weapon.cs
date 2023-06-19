using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int level = 0; // 0 = off
    [SerializeField] protected int damagePoint = 1;
    [SerializeField] protected float pushForce = 0f;
    [SerializeField] protected float cooldown = 0.5f; // cooldown between auto attack
    [SerializeField] protected float size = 1f;
    [SerializeField] protected GameObject upgradePrefab;
    [SerializeField] protected string description = "no description";
    // Multiplier
    private float attackSpeedPercentageMod;
    

    public int Level { get=>level; set=>level = value; }
    public string Description { get => description; set => description = value; }
    public GameObject UpgradePrefab { get=>upgradePrefab; set => upgradePrefab = value; }



    //protected SpriteRenderer _spriteRenderer;
    protected Animator anim;
    protected float lastswing;
    
    protected virtual void Start()
    {
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        attackSpeedPercentageMod = GameModifier.attackSpeedPercentageMod;
        lastswing = Time.time;
        anim = GetComponent<Animator>();
        
    }

    protected virtual void Update()
    {
        //Debug.Log($"slow percentage = {slowPercentage}");
        //Debug.Log($"slow Time = {slowTime}");
        if (Time.time - lastswing > (cooldown / attackSpeedPercentageMod))
        {
            lastswing = Time.time;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        Debug.Log("Default attack");
    }
}
