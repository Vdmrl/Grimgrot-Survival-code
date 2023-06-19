using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : Mover
{

    [SerializeField] private GameObject experienceGem;
    protected Transform playerTransform;
    
    
    public int damagePoint;
    private float playerImmuneTime = 1f;
    private float lastImmune;
    private float expAmount;
    private bool isSlowed = false;
    private bool isPoisoned = false;
    private bool isDamaging = false;
    protected bool isScared = false;
    public bool isMarked = false; // cryptozoologist
    private float deepWoundsMultipler;
    // Modifiers
    private static bool isMultiplied;
    private static float lifesteal;
    private static int explosionChance;
    private static int scarsChance;
    private static float cryptozoologistMultipler;
    private static float slowPercentage;
    private static int slowTime;
    private static int poisonTime;
    private static int freezeChance;
    private static float poisonPercentageDmg;
    private static int freezeTime;
    private static int scarsTime;
    private static int critChance;
    private static float critMultiplier;
    private static float deepWoundsAdd;
    private static float deathPercantage;
    private static float damagePercentageMod;
    private static float explosionRadius;
    private static float explosionPercentageDmg;
    private static float explosionKnockBack;
    protected override void Start()
    {
        //Modifiers
        if (!isMultiplied)
        {
            isMultiplied = true;
            lifesteal = GameModifier.lifesteal;
            explosionChance = GameModifier.explosionChance;
            scarsChance = GameModifier.scarsChance;
            cryptozoologistMultipler = GameModifier.cryptozoologistMultipler;
            slowPercentage = GameModifier.slowPercentage;
            slowTime = GameModifier.slowTime;
            poisonTime = GameModifier.poisonTime;
            freezeChance = GameModifier.freezeChance;
            poisonPercentageDmg = GameModifier.poisonPercentageDmg; 
            freezeTime = GameModifier.freezeTime;
            scarsTime = GameModifier.scarsTime;
            critChance = GameModifier.critChance;
            critMultiplier = GameModifier.critMultiplier;
            deepWoundsAdd = GameModifier.deepWoundsAdd;
            deathPercantage = GameModifier.deathPercantage;
            damagePercentageMod = GameModifier.damagePercentageMod;
            explosionRadius = GameModifier.explosionRadius;
            explosionPercentageDmg = GameModifier.explosionPercentageDmg;
            explosionKnockBack = GameModifier.explosionKnockBack;
        }
        
        
        base.Start();
        expAmount = experienceGem.GetComponent<ExperienceAmount>().experienceAmount;
        playerTransform = GameManager.instance.player.transform;
        deepWoundsMultipler = 1f;
        isDamaging = true;
    }
    
    protected void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            if (isDamaging && Time.time - lastImmune > playerImmuneTime)
            {
                lastImmune = Time.time;
                coll.gameObject.SendMessage("ReceiveDamage", new Damage(damagePoint));
                if (GameManager.instance.player.blademail > 0)
                {
                    Damage retDmg = new Damage((int)(damagePoint * GameManager.instance.player.blademail));
                    if (retDmg.damageeAmount < 1) retDmg.damageeAmount = 1;
                    ReceiveDamage(retDmg);
                } 
                
                // scars
                if (!isScared && scarsChance > 0)
                {
                    int ch = Random.Range(1, 101);
                    if (ch <= scarsChance)
                    {
                        if (GameManager.instance.changeLanguage.id == 0)
                            GameManager.instance.ShowText("cтрах", 28, new Color32(47, 79, 79, 255), transform.position, Vector3.up * 50, 1f);
                        else
                            GameManager.instance.ShowText("Scared", 28, new Color32(47, 79, 79,255), transform.position, Vector3.up * 50, 1f);
                        
                        StartCoroutine("Scared");
                    }
                }
            }
        }
    }
    
    protected override void TakeDamage(Damage dmg)
    {
        // modifiers
        dmg.damageeAmount = (int)(dmg.damageeAmount * damagePercentageMod);
        if (deepWoundsMultipler > 0) dmg.damageeAmount = (int)(dmg.damageeAmount * deepWoundsMultipler);
        if (critChance > 0 && Random.Range(1, 100 + 1) <= critChance)
        {
            dmg.damageeAmount = (int) (dmg.damageeAmount * critMultiplier);
        }
        base.TakeDamage(dmg);
        if (hitPoint > 0)
        {
            if (deepWoundsAdd > 0) deepWoundsMultipler += deepWoundsAdd;
            if (deathPercantage > 0 && (hitPoint / maxHitPoint) <= deathPercantage)
            {
                Death();

                if (GameManager.instance.changeLanguage.id == 0)
                    GameManager.instance.ShowText("казнён", 40, new Color32(0, 128, 128,255), transform.position, Vector3.up * 50, 1f);
                else
                    GameManager.instance.ShowText("executed", 40, new Color32(0, 128, 128, 255), transform.position, Vector3.up * 50, 1f);
            }
            else
            {
                if (slowPercentage != 0) StartCoroutine("Slowdown", dmg);
                if (poisonTime != 0) StartCoroutine("Poison", dmg);
                if (freezeChance != 0) StartCoroutine("Frozen", dmg);
            }
        }
    }

    protected override void Death()
    {
        GameManager.instance.kills++;
        if (lifesteal > 0) GameManager.instance.player.AddHealth(lifesteal);
        NecromasteryUpgrade.AddNecromastery(1);
        StartCoroutine(HonorableDeath());
    }
    
    IEnumerator HonorableDeath()
    {
        spriteRenderer.color = new Color(0.6f, 0, 0, 1);
        yield return new WaitForSeconds(timeBetweenColours);
        if (explosionChance > 0) Detonation();
        SpawnGem();
        Destroy(gameObject);
    }

    private void SpawnGem()
    {
        // check gems amount
        if (GameManager.instance.gemAmount >= GameManager.instance.maxGemsOnGround)
        {
            if (GameManager.instance.redGem == null) // no red gem
            {
                ExperienceAmount gem = Instantiate(GameManager.instance.redGemPrefab, transform.position,
                    Quaternion.Euler(0, 0, 0), GameManager.instance.experienceFolder);
                GameManager.instance.redGem = gem;
                GameManager.instance.gemAmount += 1;
            }

            if (isMarked) expAmount = (int)(expAmount * cryptozoologistMultipler);
            GameManager.instance.redGem.experienceAmount += expAmount;
            
        }
        else 
        {
            if (!isMarked)
            {
                Instantiate(experienceGem, transform.position, Quaternion.Euler(0,0,0), GameManager.instance.experienceFolder);
                GameManager.instance.gemAmount += 1;
            }
            else
            {
                ExperienceAmount gem = Instantiate(GameManager.instance.purpleGemPrefab, transform.position,
                    Quaternion.Euler(0, 0, 0), GameManager.instance.experienceFolder);
                gem.experienceAmount = (int)(expAmount * cryptozoologistMultipler);
                GameManager.instance.gemAmount += 1;
            }
            
        }
    }
    
    IEnumerator Slowdown(Damage dmg)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            float difference = speed * slowPercentage / 100f;
            speed -= difference;
            yield return new WaitForSeconds(slowTime);
            speed += difference;
            isSlowed = false;
        }
        
    }
    
    IEnumerator Poison(Damage dmg)
    {
        if (!isPoisoned)
        {
            isPoisoned = true;
            for (int i = 0; i < poisonTime; i++)
            {
                yield return new WaitForSeconds(1);
                int floatDmg = (int)(maxHitPoint * poisonPercentageDmg);
                if (floatDmg <= 0) floatDmg = 1;
                // GameManager.instance.ShowText(floatDmg.ToString(), 40, new Color32(127, 255, 0,255), transform.position, Vector3.up * 50, 1f); showed out of screen
                hitPoint -= floatDmg;
                if (hitPoint <= 0) hitPoint = 1;
            }
            isPoisoned = false;
        }
        
    }

    IEnumerator Frozen(Damage dmg)
    {
        if (!isFreezed && Random.Range(1, 100 + 1) <= freezeChance)
        {
            isFreezed = true;
            float tempSpeed = speed;
            speed = 0;  
            isDamaging = false;
            spriteRenderer.color = new Color32(0, 191, 255, 255);
            yield return new WaitForSeconds(freezeTime);
            spriteRenderer.color = Color.white;
            speed = tempSpeed;
            isDamaging = true;
            isFreezed = false;
        }
    }

    IEnumerator Scared()
    {
        isScared = true;
        yield return new WaitForSeconds(scarsTime);
        isScared = false;
        
    }

    private void Detonation()
    {
        if (Random.Range(1, 100 + 1) <= explosionChance) // chance
        {
            GameObject expl = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.Euler(0, 0, 0), GameManager.instance.effectsFolder);
            expl.transform.localScale *= explosionRadius*2;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D col in colliders)
            {
                if (col.CompareTag("Entity"))
                {
                    float magnitude = (col.transform.position - transform.position).magnitude;
                    
                    if (magnitude > 0)
                    {
                        int damage = (int) (maxHitPoint * explosionPercentageDmg);
                        int explosionDamage;
                        if (magnitude > 1)
                        {
                            explosionDamage = Mathf.RoundToInt(damage / magnitude);
                        }
                        else
                        {
                            explosionDamage = damage;
                        }
                        if (explosionDamage > 0)
                        {
                            float explosionForce;
                            if (magnitude > 1)
                            {
                                explosionForce = explosionKnockBack / magnitude;
                            }
                            else
                            {
                                explosionForce = explosionKnockBack;
                            }
                            Damage dmg = new Damage(explosionDamage, transform.position, explosionForce);
                            col.SendMessage("ReceiveDamage", dmg);
                        }
                    }
                }
            }
        }
    }
}
