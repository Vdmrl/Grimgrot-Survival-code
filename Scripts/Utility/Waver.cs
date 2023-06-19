using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

// пока не доделал
public class Waver : MonoBehaviour
{
    // Despawner
    public int maxEnemies;
    public static List<Enemy> enemies;
    
    // Spawner
    private Transform enemyFolder;
    // Screen size
    private static float outOfScreenRadius;

    // Distance from spawn start
    private static float spawnDistance = 3;

    // Distance from player during aroundPlayer spawning
    private static float aroundPlayerRadius;
    private Player player;

    public List<Wave> waves = new List<Wave>(0);

    // Logic
    private static int waveIndex = 0;
    private TimeSpan nextWavesTime;
    private int wavesLength;
    private bool isSpawned = false;
    
    // Multipliers
    private int boxFlaskChance;
    private float chestExperienceAmount;
    private int cryptozoologistChance;
    private int boxSecondSpawnInterval;
    private int chestSecondSpawnInterval;
    
    public enum wavePositions
    { 
        OutOfScreenCircle,
        AroundPlayerCircle,
    }
    
    public enum waveFormations
    { 
        Random, // enemy per second
        Group, // number of enemies
    }
    
    private void Start()
    {
        // Modifiers 
        boxFlaskChance = GameModifier.boxFlaskChance;
        chestExperienceAmount = GameModifier.chestExperienceAmount;
        cryptozoologistChance = GameModifier.cryptozoologistChance; 
        boxSecondSpawnInterval = GameModifier.boxSecondSpawnInterval;
        chestSecondSpawnInterval = GameModifier.chestSecondSpawnInterval;
        
        // Spawn
        waveIndex = 0;
        player = GameManager.instance.player;
        enemyFolder = GameManager.instance.enemyFolder;
        enemies = new List<Enemy>(100);
        outOfScreenRadius = Camera.main.orthographicSize * 1.2f; // Out of screen = out of main camera size + const
        aroundPlayerRadius =
            player.transform.localScale.x * 7; // Around player = player position + const
        // Converting time to ticks
        foreach (Wave wave in waves)
        {
            // Convert time to tics
            wave.StartTimeTics = TimeSpan.FromMinutes(wave.startTimeMinutes) +
                                 TimeSpan.FromSeconds(wave.startTimeSeconds);
            wave.FinishTimeTics = TimeSpan.FromMinutes(wave.finishTimeMinutes) +
                                  TimeSpan.FromSeconds(wave.finishTimeSeconds);
        }

        // Sort by startTime
        //waves.Sort();
        nextWavesTime = waves[0].StartTimeTics;
        wavesLength = waves.Count;
        if (wavesLength == 0) isSpawned = true;
        
        // Start Despawner
        StartCoroutine(Despawner());
        
        // Start spawn boxes
        if (boxFlaskChance > 0) StartCoroutine("BoxSpawner");
        // Start spawn chests
        if (chestExperienceAmount > 0) StartCoroutine("ChestSpawner");
    }

    // Spawning
    private void Update()
    {
        // save next wave time
        if (!isSpawned && GameManager.instance.stopwatch.time >= nextWavesTime)
        {
            StartCoroutine(SpawnEnemy());
            // next wave if exist
            if (wavesLength > waveIndex + 1) waveIndex++;
            else isSpawned = true;
            // time new of next wave
            nextWavesTime = waves[waveIndex].StartTimeTics;
        }
    }

    IEnumerator SpawnEnemy()
    {
        int localIndex = waveIndex;
        int maxEnemies = 0;
        int spawnDuration = 0;
        float timeBetweenSpawns = 0;
        int numEnemies = 0; // current number of spawned enemies
        Debug.Log($"Start spawning {waves[localIndex].waveName}");
        switch (waves[localIndex].waveFormation)
        {
            case waveFormations.Random: // Random spawning
                timeBetweenSpawns = 1 / (float)waves[localIndex].enemyPerSecond; // Time between each spawn
                spawnDuration = ((int)waves[localIndex].FinishTimeTics.TotalSeconds + 1 - // Time of wave
                                     (int)waves[localIndex].StartTimeTics.TotalSeconds);
                maxEnemies = spawnDuration * waves[localIndex].enemyPerSecond; // number of enemies
                while (numEnemies < maxEnemies) // placing and Spawning
                {
                    // Generate position depending on spawn types
                    Vector3 spawnPosition = RandomPosition(waves[localIndex].wavePosition);
            
                    // Spawning
                    Enemy en = Instantiate(waves[localIndex].enemy, spawnPosition, Quaternion.Euler(0,0,0), enemyFolder);
                    if (cryptozoologistChance > 0 &&  Random.Range(1,101) <= cryptozoologistChance) // mark
                    {
                        Instantiate(GameManager.instance.rareMark, en.transform.position, Quaternion.Euler(0,0,0), en.transform);
                        en.isMarked = true;
                    }
                    // Increment the number of enemies spawned
                    numEnemies++;
                    // add to enemies despawner
                    enemies.Add(en);
                    // Wait for the specified amount of time before spawning the next enemy
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }
                break;
            
            case waveFormations.Group: // Group spawning
                
                timeBetweenSpawns = waves[localIndex].timeBetweenGroups;
                spawnDuration = ((int)waves[localIndex].FinishTimeTics.TotalSeconds + 1 - // Time of wave
                                     (int)waves[localIndex].StartTimeTics.TotalSeconds);
                //maxEnemies = waves[localIndex].enemyGroupAmount * (spawnDuration / (int)timeBetweenSpawns)
                int groupAmount = spawnDuration / (int)timeBetweenSpawns + 1;
                int maxEnemiesGroup = waves[localIndex].enemyGroupAmount;
                float extraRange = math.pow(maxEnemiesGroup, 0.4f) - 1;
                float verticalSizeMult = waves[localIndex].enemy.GetComponent<BoxCollider2D>().size.y / waves[localIndex].enemy.GetComponent<BoxCollider2D>().size.x / 2.2f;
                float horizontalSizeMult = waves[localIndex].enemy.GetComponent<BoxCollider2D>().size.x / waves[localIndex].enemy.GetComponent<BoxCollider2D>().size.y / 2.2f;
                Vector3 randomizedSpawnPosition = Vector3.zero;
                float offset = waves[localIndex].enemy.GetComponent<BoxCollider2D>().size.x;
                float range;
                if (waves[localIndex].enemyGroupAmount <= 7)
                {
                    range = 0.4f;
                }
                else if (waves[localIndex].enemyGroupAmount <= 10)
                {
                    range = 0.6f;
                }
                else if (waves[localIndex].enemyGroupAmount <= 15)
                {
                    range = 0.8f;
                }
                else if (waves[localIndex].enemyGroupAmount <= 20)
                {
                    range = 0.9f;
                }
                else if (waves[localIndex].enemyGroupAmount <= 40)
                {
                    range = 1.15f;
                }
                else if (waves[localIndex].enemyGroupAmount <= 50)
                {
                    range = 1.25f;
                }
                else if (waves[localIndex].enemyGroupAmount <= 75)
                {
                    range = 1.5f;
                }
                else
                {
                    range = 2f;
                }
                
                for (int numGroup = 0; numGroup < groupAmount; numGroup++)
                {
                    // Generate position depending on spawn types
                    Vector3 spawnPosition = RandomPosition(waves[localIndex].wavePosition, extraRange);
                    
                    for (int numEnemiesGroup = 0; numEnemiesGroup < maxEnemiesGroup; numEnemiesGroup++)
                    {
                        if (verticalSizeMult >= 0.4 && verticalSizeMult <= 0.5)
                        {
                            randomizedSpawnPosition = new Vector3(spawnPosition.x + Random.Range(-offset,offset),
                                spawnPosition.y + Random.Range(-offset,offset), 0);
                            
                        }
                        else
                        {
                            
                            if (verticalSizeMult <= 1 / 2.2) // vertical size > horizontal size
                            {
                                // Resize according to vertical size
                                range *= horizontalSizeMult;
                                // Randomize positions for non-line spawn 
                                randomizedSpawnPosition = new Vector3(spawnPosition.x,
                                    spawnPosition.y + Random.Range(-range, range), 0);
                            }
                            else // horizontal size > vertical size
                            {
                                // Resize according to vertical size
                                range *= verticalSizeMult;
                                // Randomize positions for non-line spawn 
                                randomizedSpawnPosition = new Vector3(spawnPosition.x,
                                    spawnPosition.y + Random.Range(-range, range), 0);
                            }
                            
                        }
                        // Spawning
                        Enemy en = Instantiate(waves[localIndex].enemy, randomizedSpawnPosition, Quaternion.Euler(0,0,0), enemyFolder);
                        
                        if (cryptozoologistChance > 0 &&  Random.Range(1,101) <= cryptozoologistChance) // mark
                        {
                            Instantiate(GameManager.instance.rareMark, en.transform.position, Quaternion.Euler(0,0,0), en.transform);
                            en.isMarked = true;
                        }
                        
                        // add to enemies despawner
                        enemies.Add(en);
                    }
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }
                break;
        }
        
    }

    // generate random position depending on the spawn type
    private Vector3 RandomPosition(wavePositions wavePosition = wavePositions.OutOfScreenCircle, float extraRange = 0)
    {
        Vector3 position = new Vector3();
        float angle = Random.Range(0, 2 * Mathf.PI);
        switch (wavePosition)
        {
            case wavePositions.OutOfScreenCircle:
                // randomOutOfScreen
                position = new Vector3(GameManager.instance.player.transform.position.x,
                    player.transform.position.y, 0);
                position.x += (outOfScreenRadius + extraRange) * Mathf.Cos(angle) + Random.Range(0, spawnDistance);
                position.y += (outOfScreenRadius + extraRange) * Mathf.Sin(angle) + Random.Range(0, spawnDistance);
                break;
            
            case wavePositions.AroundPlayerCircle:
                // randomAroundPlayer
                position = new Vector3(GameManager.instance.player.transform.position.x,
                    player.transform.position.y, 0);
                position.x += (aroundPlayerRadius + extraRange) * Mathf.Cos(angle);
                position.y += (aroundPlayerRadius + extraRange) * Mathf.Sin(angle);
                break;
        }
        return position;
    }

    IEnumerator Despawner()
    {
        while (true)
        {
            while (enemies.Count > maxEnemies)
            {
                // every frame second check one enemy
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i] != null) DespawnEnemy(enemies[i]);
                    if (enemies.Count <= maxEnemies) break;
                    yield return new WaitForNextFrameUnit();
                } 
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void DespawnEnemy(Enemy en)
    {
        
        Vector3 pos = en.transform.position;
        if (pos.x >= player.transform.position.x + outOfScreenRadius ||
            pos.x <= player.transform.position.x - outOfScreenRadius ||
            pos.y >= player.transform.position.y + outOfScreenRadius ||
            pos.y <= player.transform.position.y - outOfScreenRadius)
        {
            enemies.Remove(en);
            Destroy(en.gameObject);
        }
    }

    // modifier
    IEnumerator BoxSpawner()
    {
        do
        {
            Instantiate(GameManager.instance.box, RandomPosition(), Quaternion.Euler(0, 0, 0), GameManager.instance.propsFolder);
            yield return new WaitForSeconds(boxSecondSpawnInterval);
        } while (true);
        
    }
    
    // modifier
    IEnumerator ChestSpawner()
    {
        //yield return new WaitForSeconds(30);
        do
        {
            Instantiate(GameManager.instance.chest, RandomPosition(), Quaternion.Euler(0, 0, 0), GameManager.instance.propsFolder);
            yield return new WaitForSeconds(chestSecondSpawnInterval);
        } while (true);
        
    }
    
    [Serializable]
    public class Wave : IComparable
    {
        [SerializeField] internal string waveName = "test";
        [SerializeField] internal Enemy enemy;
        [Header("Wave type")]
        [SerializeField] internal wavePositions wavePosition = wavePositions.OutOfScreenCircle;
        [SerializeField] internal waveFormations waveFormation = waveFormations.Random;
        [SerializeField] internal bool isBoss = false;
        [Header("Start")] [SerializeField] internal int startTimeMinutes = 0;
        [SerializeField] internal int startTimeSeconds = 0;
        internal TimeSpan StartTimeTics = new TimeSpan(0);

        [Space(5)] [Header("Finish")] [SerializeField]
        internal int finishTimeMinutes = 0;

        [SerializeField] internal int finishTimeSeconds = 0;
        internal TimeSpan FinishTimeTics = new TimeSpan(0);

        
        [Header("For Random formation")]
        [Range (0.0f, 100.0f)] [SerializeField] internal int enemyPerSecond = 0;
        [Header("For Group formation")]
        [Range (0.0f, 100.0f)][SerializeField] internal int enemyGroupAmount= 0; // max = 100
        [SerializeField] internal float timeBetweenGroups = 0;
        [Space(5)]
        [TextArea]
        public string aboutWave;

        public int CompareTo(object obj)
        {
            if (obj is Wave wave)
            {
                return (this.StartTimeTics - wave.StartTimeTics).Seconds;
            }

            return 0;
        }
    }
}