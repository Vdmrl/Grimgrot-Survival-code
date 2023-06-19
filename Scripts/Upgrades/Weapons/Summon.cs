using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Summon : Projectile
{
    public static float pSpeed; // player speed
    public static Summoner summoner;
    [SerializeField] private Vector3 dir = Vector3.zero; // direction

    [SerializeField] private AudioSource attackSound;

    protected override void Start()
    {
        // no start
    }

    public void SetDirection(Vector3 direction) // Message
    {
        dir = direction;
        //transform.Rotate(0,0,-90);
    }

    protected override void Move()
    {
        // move forward
        gameObject.transform.Translate((dir - transform.position).normalized * (Time.deltaTime * pSpeed * speed));
        const float mult = 0.2f;
        if (Math.Abs((math.round(transform.position.x * 10f) * 0.1f) - (math.round(dir.x * 10f) * 0.1f)) < mult && Math.Abs((math.round(transform.position.y * 10f) * 0.1f) - (math.round(dir.y * 10f) * 0.1f)) < mult)
        {
            dir = summoner.NewDirection();
        }
    }

    protected override void TouchEnemy(Collider2D coll)
    {
        attackSound.pitch = (UnityEngine.Random.Range(0.6f, 1.4f));
        attackSound.Play();
        base.TouchEnemy(coll);
    }
}
