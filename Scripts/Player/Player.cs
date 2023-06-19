using UnityEngine;
using System.Collections;

public class Player : Mover
{
    public VariableJoystick variableJoystick;
    private Treadmill treadmill;
    private Stopwatch stopwatch;
    private HealthProgressBar healthProgressBar;
    private float baseSpeed;
    private float baseHealth;
    public int armor;
    public int evasion;
    public float regeneration;
    public float blademail = 0;
    private int magicShield = 0;
    public int magicShieldMax = 0;
    public bool isInvulnerable = false;
    private bool isBerserkMode = false;
    private bool isDodging = false;

    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource expSound;
    [SerializeField] private AudioSource chestSound;
    [SerializeField] private AudioSource potionSound;

    // Modifiers
    private float boxHealingPercentage;
    private float chestExperienceAmount;
    private float berserkRegenPercentage;
    private float healingPercentageMod;
    private int dodgeTime;
    private int reincarnationTimes;
    private int reincarnationInvulnerableTime;
    private float healthPercentageMod;
    private float dodgeBoost;
    private float berserkEntry;
    private float reincarnationHealth;
    private float reincarnationRadius;
    private float reincarnationKnockback;
    private float speedPercentageMod;
    
    protected override void Start()
    {
        base.Start();
        treadmill = GameManager.instance.treadmill;
        stopwatch = GameManager.instance.stopwatch;
        hitPoint = maxHitPoint;
        healthProgressBar = GameManager.instance.healthProgressBar;
        // Modifiers
        

        StartCoroutine("LateStart");
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        // Modifiers
        boxHealingPercentage = GameModifier.boxHealingPercentage;
        chestExperienceAmount = GameModifier.chestExperienceAmount;
        berserkRegenPercentage = GameModifier.berserkRegenPercentage;
        healingPercentageMod = GameModifier.healingPercentageMod;
        dodgeTime = GameModifier.dodgeTime;
        reincarnationTimes = GameModifier.reincarnationTimes;
        reincarnationInvulnerableTime = GameModifier.reincarnationInvulnerableTime;
        healthPercentageMod = GameModifier.healthPercentageMod;
        dodgeBoost = GameModifier.dodgeBoost;
        berserkEntry = GameModifier.berserkEntry;
        reincarnationHealth = GameModifier.reincarnationHealth;
        reincarnationRadius = GameModifier.reincarnationRadius;
        reincarnationKnockback = GameModifier.reincarnationKnockback; 
        speedPercentageMod = GameModifier.speedPercentageMod;
        
        
        // Changes
        baseSpeed = speed;
        baseHealth = maxHitPoint;
        ChangeSpeed();
        ChangeMaxHealth();
        healthProgressBar.ResetBar(maxHitPoint);
    }
    
    private void FixedUpdate()
    {
        //Debug.Log(GameManager.instance.DefenceUpdates[0, 0]);
        float x = variableJoystick.Horizontal;
        float y = variableJoystick.Vertical;
        UpdateMotion(new Vector3(x, y, 0));

        if (x == 0 && y == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Experience"))
        {
            expSound.pitch = (Random.Range(0.4f, 1.6f));
            expSound.Play();
            float exp = col.GetComponent<ExperienceAmount>().experienceAmount;
            GameManager.instance.AddExperience(exp);
            Destroy(col.gameObject);
            GameManager.instance.gemAmount -= 1;
        }

        if (col.CompareTag("Flask"))
        {
            potionSound.pitch = (Random.Range(0.6f, 1.4f));
            potionSound.Play();
            AddHealth(maxHitPoint * boxHealingPercentage);
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Chest"))
        {
            chestSound.Play();
            GameManager.instance.AddExperience(GameManager.instance.maxExperience * chestExperienceAmount);
            Destroy(col.gameObject);
        }
    
        else if (col.CompareTag("Wall"))
        {
        
            if (col.name == "Floor00(Clone)")
            {
                treadmill.ChangeCenter(-1,1);
            }
            else if (col.name == "Floor01(Clone)")
            {
                treadmill.ChangeCenter(0,1);
            }
            else if (col.name == "Floor02(Clone)")
            {
                treadmill.ChangeCenter(1,1);
            }
            else if (col.name == "Floor10(Clone)")
            {
                treadmill.ChangeCenter(-1,0);
            }
            else if (col.name == "Floor12(Clone)")
            {
                treadmill.ChangeCenter(1,0);
            }
            else if (col.name == "Floor20(Clone)")
            {
                treadmill.ChangeCenter(-1,-1);
            }
            else if (col.name == "Floor21(Clone)")
            {
                treadmill.ChangeCenter(0,-1);
            }
            else if (col.name == "Floor22(Clone)")
            {
                treadmill.ChangeCenter(1,-1);
            }
        }
    }
    
    protected override void ReceiveDamage(Damage dmg) // Message
    {
        if (!isInvulnerable)
        {
            if (magicShield > 0) // blocked with magic shield
            {
                magicShield--;
                StartCoroutine("RestoreMagicShield");

                if (GameManager.instance.changeLanguage.id == 0)
                    GameManager.instance.ShowText("отражено", 40, Color.magenta, transform.position, Vector3.up * 50, 1f);
                else
                    GameManager.instance.ShowText("blocked", 40, Color.magenta, transform.position, Vector3.up * 50, 1f);
            }
            else
            {
                if (Random.Range(1,101) <= evasion) // miss
                {
                    if (GameManager.instance.changeLanguage.id == 0)
                        GameManager.instance.ShowText("промах", 40, new Color(153, 153, 255, 200), transform.position, Vector3.up * 50, 1f);
                    else
                        GameManager.instance.ShowText("miss", 40, new Color(153, 153, 255, 200), transform.position, Vector3.up * 50, 1f);
                }
                else
                {
                    hurtSound.pitch = (Random.Range(0.6f, 0.9f));
                    hurtSound.Play();
                    if (armor != 0)
                    {
                        if (dmg.damageeAmount - armor < 1) dmg.damageeAmount = 1;
                        else dmg.damageeAmount -=  armor;
                    }
                    base.ReceiveDamage(dmg);
                    healthProgressBar.AddValue(-dmg.damageeAmount);
                    
                    // dodge
                    if (!isDodging && dodgeTime > 0)
                    {
                        StartCoroutine("Dodge");
                    }
                    
                    //Berserk check
                    if (!isBerserkMode && berserkRegenPercentage > 0 && hitPoint / maxHitPoint < berserkEntry) 
                    {
                        isBerserkMode = true;
                        healingPercentageMod += berserkRegenPercentage;
                    }
                }
            }
        }
        else
        {
            GameManager.instance.ShowText("0", 40, new Color(119, 136, 153, 200), transform.position, Vector3.up * 50, 1f);
        }
    }
    
    protected override void Death()
    {
        if (reincarnationTimes > 0) // reincarnation
        {
            GetInvulnerable(reincarnationInvulnerableTime);

            if (GameManager.instance.changeLanguage.id == 0)
                GameManager.instance.ShowText("реинкарнация", 60, new Color(128, 0, 0, 255), transform.position, Vector3.up * 150, 2f);
            else
                GameManager.instance.ShowText("Reincarnation", 60, new Color(128, 0, 0, 255), transform.position, Vector3.up * 150, 2f);
            
            reincarnationTimes -= 1;
            hitPoint = ((int) (maxHitPoint * reincarnationHealth));
            healthProgressBar.SetValue(((int) (maxHitPoint * reincarnationHealth)));
            // explosion
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, reincarnationRadius);
            foreach (Collider2D col in colliders)
            {
                Debug.Log(col.name);
                if (col.CompareTag("Entity"))
                {
                    
                    
                    float magnitude = (col.transform.position - transform.position).magnitude;
                    
                    if (magnitude > 0)
                    {
                        float explosionKnockBack = reincarnationKnockback;
                        float explosionForce;
                        if (magnitude > 1)
                        {
                            explosionForce = explosionKnockBack / magnitude;
                        }
                        else
                        {
                            explosionForce = explosionKnockBack;
                        }
                        Damage dmg = new Damage(0, transform.position, explosionForce);
                        col.SendMessage("ReceiveDamage", dmg);
                    }
                }
            }
        }
        else
        {
            if (!isInvulnerable) GameManager.instance.EndGame(false);
        }
        
    }

    public void ChangeMaxHealth() // used when max health changed
    {
        if (hitPoint == maxHitPoint) // full hp
        {
            maxHitPoint = (baseHealth * healthPercentageMod);
            hitPoint = maxHitPoint;
        }
        else
        {
            hitPoint = (baseHealth * healthPercentageMod) * (hitPoint / maxHitPoint) ;
            maxHitPoint = (baseHealth * healthPercentageMod);
        }
        healthProgressBar.ChangeMax(maxHitPoint);
    }

    public void ChangeSpeed()
    {
        speed = baseSpeed * speedPercentageMod;
        Summon.pSpeed = speed;
    }
    
    IEnumerator Regeneration()
    {
        do
        {
            AddHealth(regeneration);
            yield return new WaitForSeconds(1);
        } while (true);
    }

    public void AddHealth(float hlth) // used when player get healed
    {
        if (hitPoint < maxHitPoint)
        {
            float tHelth = hlth * healingPercentageMod;
            hitPoint += tHelth;
            healthProgressBar.AddValue(tHelth);
            if (hitPoint > maxHitPoint)
            {
                hitPoint = maxHitPoint;
            }
            // off berserk mode
            if (isBerserkMode && hitPoint / maxHitPoint > berserkEntry)
            {
                healingPercentageMod -= berserkRegenPercentage;
                isBerserkMode = false;
            }
        }
        
    }

    public void AddMagicShield()
    {
        magicShieldMax++;
        magicShield++;
    }

    IEnumerator RestoreMagicShield()
    {
        yield return new WaitForSeconds(20);
        magicShield = magicShieldMax;
    }

    public void GetInvulnerable(int time)
    {
        if (!isInvulnerable) StartCoroutine("Invulnerable", time);
    }

    IEnumerator Invulnerable(int time)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(time);
        isInvulnerable = false;
    }

    IEnumerator Dodge()
    {
        isDodging = true;
        float deltaSpeed = speed * dodgeBoost;
        speed += deltaSpeed;
        yield return new WaitForSeconds(dodgeTime);
        isDodging = false;
        speed -= deltaSpeed;
    }
}