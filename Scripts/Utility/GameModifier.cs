using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModifier : MonoBehaviour
{
    //public static GameModifier instance;
    
    public static float healthPercentageMod;
    public static float damagePercentageMod;
    public static float attackSpeedPercentageMod;
    public static float speedPercentageMod;
    public static float healingPercentageMod;
    public static float experiencePercentageMod;
    public static float magnetRadiusPercentageMod;
    public static float goldPercentageMod;
    
    public static float slowPercentage;
    public static int slowTime;
    public static int critChance;
    public static float critMultiplier;
    public static float deepWoundsAdd;
    public static float poisonPercentageDmg;
    public static int poisonTime;
    public static int freezeTime;
    public static int freezeChance;
    public static int explosionChance;
    public static float explosionPercentageDmg;
    public static float explosionRadius;
    public static float explosionKnockBack;
    public static float deathPercantage;

    public static float berserkRegenPercentage;
    public static float berserkEntry;
    public static int scarsChance;
    public static int scarsTime;
    public static float lifesteal;
    public static int reincarnationTimes;
    public static float reincarnationHealth;
    public static float reincarnationRadius;
    public static float reincarnationKnockback;
    public static int reincarnationInvulnerableTime;

    public static float forthUpdateChance;
    public static int dodgeTime;
    public static float dodgeBoost;
    public static int cryptozoologistChance;
    public static float cryptozoologistMultipler;
    public static float boxHealingPercentage;
    public static int boxFlaskChance;
    public static int boxSecondSpawnInterval;
    public static int chestSecondSpawnInterval;
    public static float chestExperienceAmount;



    private void Start()
    {
        healthPercentageMod = 1;
        damagePercentageMod = 1;
        attackSpeedPercentageMod = 1;
        speedPercentageMod = 1;
        healingPercentageMod = 1;
        experiencePercentageMod = 1;
        magnetRadiusPercentageMod = 1;
        goldPercentageMod = 1;
        
        slowPercentage = 0;
        slowTime = 0;
        critChance = 0;
        critMultiplier = 1f;
        deepWoundsAdd = 0f;
        poisonTime = 0;
        poisonPercentageDmg = 0;
        freezeChance = 0;
        freezeTime = 0;
        explosionChance = 0;
        explosionPercentageDmg = 0;
        explosionRadius = 0;
        explosionKnockBack = 0;
        deathPercantage = 0;

        berserkRegenPercentage = 0;
        berserkEntry = 0;
        scarsChance = 0;
        scarsTime = 0;
        lifesteal = 0;
        reincarnationTimes = 0;
        reincarnationHealth = 0;
        reincarnationRadius = 0;
        reincarnationKnockback = 0;
        reincarnationInvulnerableTime = 0;

        forthUpdateChance = 0;
        dodgeTime = 0;
        dodgeTime = 0;
        cryptozoologistChance = 0;
        critMultiplier = 1;
        boxHealingPercentage = 0;
        boxFlaskChance = 0;
        boxSecondSpawnInterval = 0;
        chestSecondSpawnInterval = 0;
        chestExperienceAmount = 0;
        

        AttackUpgradesActivasion();
        DefenceUpgradesActivasion();
        SpecialUpgradesActivasion();
        
    }

    private void AttackUpgradesActivasion()
    {
        if (GameManager.instance.AttackUpdates[0, 0] != 0) // Increase damage by 10% per level
        {
            // ReSharper disable once PossibleLossOfFraction
            damagePercentageMod += GameManager.instance.AttackUpdates[0, 0] * 5 / 100f;
        }
        if (GameManager.instance.AttackUpdates[0, 1] != 0) // Reduce attack сooldown by 5% per level
        {
            // ReSharper disable once PossibleLossOfFraction
            attackSpeedPercentageMod += GameManager.instance.AttackUpdates[0, 1] * 2 / 100f;
        }
        if (GameManager.instance.AttackUpdates[0, 2] != 0) // slowdown enemy by 10 percent for 1 second per level
        {
            // ReSharper disable once PossibleLossOfFraction
            slowPercentage += GameManager.instance.AttackUpdates[0, 2] * 5;
            slowTime += GameManager.instance.AttackUpdates[0, 2];
        }
        if (GameManager.instance.AttackUpdates[1, 0] != 0) // weapons can have double damage with a 1% chance per level
        {
            // ReSharper disable once PossibleLossOfFraction
            critChance += GameManager.instance.AttackUpdates[1, 0];
            critMultiplier = 2f;
        }
        if (GameManager.instance.AttackUpdates[1, 1] != 0) // Each subsequent hit deals 1% more damage per level per level
        {
            // ReSharper disable once PossibleLossOfFraction
            deepWoundsAdd += GameManager.instance.AttackUpdates[1, 1] / 100f;
        }
        if (GameManager.instance.AttackUpdates[1, 2] != 0) // Enemies that take damage lose 1% health per second. Each level increases the duration of the poison by 2 seconds
        {
            // ReSharper disable once PossibleLossOfFraction
            poisonTime += GameManager.instance.AttackUpdates[1, 2] * 2;
            poisonPercentageDmg = 0.01f;
        } 
        if (GameManager.instance.AttackUpdates[2, 0] != 0) // With a 1% chance per level, the enemy can explode on death and reveal 5% of their maximum health per level
        {
            // ReSharper disable once PossibleLossOfFraction
            explosionChance = GameManager.instance.AttackUpdates[2, 0];
            explosionPercentageDmg += GameManager.instance.AttackUpdates[2, 0] * 5 / 100f;
            explosionRadius = 1;
            explosionKnockBack = 20; // for a while random value
        }
        if (GameManager.instance.AttackUpdates[2, 1] != 0) // Enemies below 2% health per level will die automatically.
        {
            // ReSharper disable once PossibleLossOfFraction
            deathPercantage += GameManager.instance.AttackUpdates[2, 1] * 2 / 100f;
        }
        if (GameManager.instance.AttackUpdates[2, 2] != 0) // Enemies get frozen for 3 seconds with a 1% chance per level
        {
            // ReSharper disable once PossibleLossOfFraction
            freezeChance += GameManager.instance.AttackUpdates[2, 2];
            freezeTime = 3;
        }
    }
    
    private void DefenceUpgradesActivasion()
    {
        if (GameManager.instance.DefenceUpdates[0, 0] != 0) // Increase deffence by 1 per level
        {
            GameManager.instance.player.armor += GameManager.instance.DefenceUpdates[0, 0];
        }
        if (GameManager.instance.DefenceUpdates[0, 1] != 0) // Increase evasion by 2% per level
        {
            GameManager.instance.player.evasion += GameManager.instance.DefenceUpdates[0, 1] * 2;
        }
        if (GameManager.instance.DefenceUpdates[0, 2] != 0) // Increase max health by 5% per level
        {
            healthPercentageMod += GameManager.instance.DefenceUpdates[0, 2] * 5 / 100f;
        }
        if (GameManager.instance.DefenceUpdates[1, 0] != 0) // return 10% damage per level
        {
            GameManager.instance.player.blademail += GameManager.instance.DefenceUpdates[1, 0] * 10 / 100f;
        }
        if (GameManager.instance.DefenceUpdates[1, 1] != 0) // If less than 10% of health remains, then health begins to recover faster by 20% per level
        {
            berserkRegenPercentage += GameManager.instance.DefenceUpdates[1, 1] * 20 / 100f;
            berserkEntry = 0.1f;
        }
        if (GameManager.instance.DefenceUpdates[1, 2] != 0) // regenerate 0.05 health per second
        {
            GameManager.instance.player.regeneration += GameManager.instance.DefenceUpdates[1, 2] * 5 / 100f;
        } 
        if (GameManager.instance.DefenceUpdates[2, 0] != 0) // The enemy is frightened by your scars and has a 5% chance per level to start running away from you for 3 seconds
        {
            scarsChance += GameManager.instance.DefenceUpdates[2, 0] * 5;
            scarsTime = 3;
        } 
        if (GameManager.instance.DefenceUpdates[2, 1] != 0) // For each enemy killed, 0.005 health is restored per level
        {
            lifesteal += GameManager.instance.DefenceUpdates[2, 1] * 5 / 1000f;
        }
        if (GameManager.instance.DefenceUpdates[2, 2] != 0) // Character can respawn once per game with 5% of max health per level
        {
            reincarnationTimes = 1;
            reincarnationHealth = GameManager.instance.DefenceUpdates[2, 2] * 5 / 100f;
            reincarnationRadius = 30;
            reincarnationKnockback = 150;
            reincarnationInvulnerableTime = 3;
        }
    }
    
    private void SpecialUpgradesActivasion()
    {
        if (GameManager.instance.SpecialUpdates[0, 0] != 0) // Increase experience by 5% per level
        {
            // ReSharper disable once PossibleLossOfFraction
            experiencePercentageMod += GameManager.instance.SpecialUpdates[0, 0] * 5 / 100f;
        }
        if (GameManager.instance.SpecialUpdates[0, 1] != 0) // Experience crystal attraction radius is increased by 5% per level
        {
            // ReSharper disable once PossibleLossOfFraction
            magnetRadiusPercentageMod += GameManager.instance.SpecialUpdates[0, 1] * 5 / 100f;
        }
        if (GameManager.instance.SpecialUpdates[0, 2] != 0) // Increases earned gold by 5% per level
        {
            // ReSharper disable once PossibleLossOfFraction
            goldPercentageMod += GameManager.instance.SpecialUpdates[0, 2] * 5 / 100f;
        }
        if (GameManager.instance.SpecialUpdates[1, 0] != 0) // The enemy has a 1% chance per level to be tagged with a rare creature sign. Such a creature can give 100% more experience per level
        {
            // ReSharper disable once PossibleLossOfFraction
            cryptozoologistChance += GameManager.instance.SpecialUpdates[1, 0];
            cryptozoologistMultipler += GameManager.instance.SpecialUpdates[1, 0];
        }
        if (GameManager.instance.SpecialUpdates[1, 1] != 0) // Increase speed by 5% per level
        {
            // ReSharper disable once PossibleLossOfFraction
            speedPercentageMod += GameManager.instance.SpecialUpdates[1, 1] * 3 / 100f;
        }
        if (GameManager.instance.SpecialUpdates[1, 2] != 0) // On the crate spawn map, depending on the chance of 20% per level, a health potion that restores 5% of health per level
        {
            // ReSharper disable once PossibleLossOfFraction
            boxFlaskChance += GameManager.instance.SpecialUpdates[1, 2] * 20;
            boxHealingPercentage += GameManager.instance.SpecialUpdates[1, 2] * 5 / 100f;
            boxSecondSpawnInterval = 60;
        }
        if (GameManager.instance.SpecialUpdates[2, 0] != 0) // a fourth upgrade option may appear with a 20% chance per level
        {
            // ReSharper disable once PossibleLossOfFraction
            forthUpdateChance += GameManager.instance.SpecialUpdates[2, 0] * 20;
        } 
        if (GameManager.instance.SpecialUpdates[2, 1] != 0) // After taking damage, you increase your speed by 10% of your level for 1 second
        {
            // ReSharper disable once PossibleLossOfFraction
            dodgeBoost += GameManager.instance.SpecialUpdates[2, 1] * 10 / 100f;
            dodgeTime = 1;
        }
        if (GameManager.instance.SpecialUpdates[2, 2] != 0) // You start to notice chests that weren't in the dungeon before. With each level, the treasure becomes more and more valuable. (5 percent of experience bar per level)
        {
            // ReSharper disable once PossibleLossOfFraction
            chestExperienceAmount += GameManager.instance.SpecialUpdates[2, 2] * 5 / 100f;
            chestSecondSpawnInterval = 60;
        }
    }
    
}
