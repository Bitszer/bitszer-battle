using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utility.Logging;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    private readonly ILog _log = LogManager.GetLogger<EnemySpawnManager>();

    [Header("Dependencies")]
    [SerializeField] private EnemyHeroManager enemyHeroManager = null;

    [Header("Configuration")]
    [SerializeField] private Transform SpawnPosition = null;
    [SerializeField] private Tower towerDay   = null;
    [SerializeField] private Tower towerNight = null;

    private int   _spawnOrder; // Decides whether to Starts in acsending or in Decsending order -- CoolDown time of the hero
    private float _spawnDelay;
    private float _spawnDelayTime;
    private int   _spawnLimit;

    private float _requiredCoolDown      = 200;
    private bool  _heroAvailableForSpawn = false;
    private bool  _isFirstTime;

    private List<List<HeroUnitData>> Category    = new List<List<HeroUnitData>>();
    private List<HeroUnitData>       Combination = new List<HeroUnitData>();

    private HeroUnitData heroToSpawn;

    private int _numberOfCategory;

    /*
     * Mono Behavior.
     */
    
    private void Start()
    {
        _isFirstTime = true;

        if (enemyHeroManager == null)
            _log.Error("EnemyHeroManager dependency not set.");
    }

    /*
     * Public.
     */

    public void StartSpawning()
    {
        CentralVariables.EnemyMax = 0;
        CentralVariables.IS_GAME_END = false;
        CentralVariables.IS_GAME_STOPPED = false;

        UpdateDifficulty();
        UpdateEnemyHeroes();
        
        StopAllCoroutines();
        Spawn();
    }
    
    /*
     * Private.
     */
    
    private void UpdateDifficulty()
    {
        switch (CentralVariables.SELECTED_DIFFICULTY)
        {
            case 1:
                _spawnDelay = 5;
                _spawnDelayTime = 10;
                _spawnLimit = 8;
                break;
            case 2:
                _spawnDelay = 4;
                _spawnLimit = 10;
                _spawnDelayTime = 7;
                break;
            case 3:
                _spawnDelay = 2;
                _spawnLimit = 8;
                _spawnDelayTime = 5;
                break;
            case 4:
                _spawnDelay = 1;
                _spawnLimit = 6;
                _spawnDelayTime = 4;
                break;
            default:
                throw new Exception("Unknown difficulty.");
        }
    }

    private void UpdateEnemyHeroes()
    {
        enemyHeroManager.UpdateHeroesAvailability();

        var health = 400 + 40 * CentralVariables.SELECTED_LEVEL;
        towerDay.SetTowerHealth(health);
        towerNight.SetTowerHealth(health);
    }

    /*
     * Spawning.
     */
    
    private void Spawn()
    {
        // Add Some Variations to Delay Time.
        _spawnDelayTime = Random.Range(_spawnDelayTime - 1, _spawnDelayTime + 1);

        // Rondomly Increase Delay Time with probability of 1/5 for difficulty Level Casual and Expert
        if (CentralVariables.SELECTED_DIFFICULTY == 3 || CentralVariables.SELECTED_DIFFICULTY == 4)
        {
            if (Random.Range(0, 8) == 3)
            {
                _spawnDelayTime = 10;
            }
        }
        else
        {
            if (Random.Range(0, 8) == 3)
            {
                _spawnDelayTime = 8;
            }
        }

        StartCoroutine(Delay(_spawnDelayTime));
    }

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (CentralVariables.IS_GAME_END || CentralVariables.IS_GAME_STOPPED) 
            yield break;
        
        InstantiateEnemy(enemyHeroManager.GetRandomAvailableHeroData());
        Spawn();
    }
    
    /*
     * Instantiation.
     */

    private void InstantiateEnemy(HeroUnitData unitData)
    {
        if (CentralVariables.EnemyMax > _spawnLimit || CentralVariables.IS_GAME_END || CentralVariables.IS_GAME_STOPPED)
            return;

        var heroInstance = Instantiate(unitData.Prototype, SpawnPosition.position, SpawnPosition.rotation);
        heroInstance.transform.eulerAngles = new Vector3(heroInstance.transform.eulerAngles.x, heroInstance.transform.eulerAngles.y + 180, heroInstance.transform.eulerAngles.z);
        heroInstance.name = unitData.typeName + "_enemy";
        heroInstance.isEnemyCharacter = true;
        heroInstance.SetStats(unitData);

        CentralVariables.EnemyMax++;
        CentralVariables.EnemyUnitInstances.Add(heroInstance);
    }
    
    
    

    private void SpawnLoop()
    {
        //	Debug.Log ("---- MAX ----> " + CentralVariables.EnemyMax);
        if (!CentralVariables.IS_GAME_END && !CentralVariables.IS_GAME_STOPPED)
        {
            Combination.Clear();
            _spawnOrder = Random.Range(1, 3);

            // Temprory
            _spawnOrder = 1;

            if (_spawnOrder == 1)
            {
                _requiredCoolDown = 200;
            }
            else
            {
                _requiredCoolDown = 0;
            }

            // Temporary we can change this block.
            if (CentralVariables.SELECTED_DIFFICULTY == 1)
            {
                MakeCombination();
                StartCoroutine(DelayBetweenSpawn(15));
            }
            else if (CentralVariables.SELECTED_DIFFICULTY == 2)
            {
                MakeCombination();
                StartCoroutine(DelayBetweenSpawn(6));
            }
            else if (CentralVariables.SELECTED_DIFFICULTY == 3)
            {
                _spawnLimit = 6;
                MakeCombination();
                StartCoroutine(DelayBetweenSpawn(30));
                MakeCombination();
                StartCoroutine(DelayBetweenSpawn(18));
            }
            else
            {
                _spawnLimit = 6;
                MakeCombination();
                StartCoroutine(DelayBetweenSpawn(25));
                MakeCombination();
                StartCoroutine(DelayBetweenSpawn(20));
            }
        }
    }

    IEnumerator DelayBetweenSpawn(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        GetHeroForSpawn();
    }


    private void MakeCombination()
    {
        for (var i = 0; i < Category.Count; i++)
        {
            var tempRand = Random.Range(0, Category[i].Count);
            Combination.Add(Category[i][tempRand]);
        }
    }

    private void GetHeroForSpawn()
    {
        if (_spawnOrder == 1) // Spawn Hero with Maximum CoolDown Time.
        {
            float maxCoolDown = 0;

            for (int i = 0; i < Combination.Count; i++)
            {
                if (maxCoolDown < Combination[i].CoolDown && Combination[i].CoolDown < _requiredCoolDown)
                {
                    maxCoolDown = Combination[i].CoolDown;
                    heroToSpawn = Combination[i];
                    _heroAvailableForSpawn = true;
                }
            }

            if (_heroAvailableForSpawn && !CentralVariables.IS_GAME_END)
            {
                _requiredCoolDown = maxCoolDown;
                StartCoroutine(SpawnAfterCoolDown());
            }
            else
            {
                // Go Back to routine and make combination again.
                SpawnLoop();
            }
        }
        else // Spawn Hero with Minimum CoolDown Time.
        {
            float minCoolDown = 1000;

            for (int i = 0; i < Combination.Count; i++)
            {
                if (minCoolDown > Combination[i].CoolDown && Combination[i].CoolDown > _requiredCoolDown)
                {
                    minCoolDown = Combination[i].CoolDown;
                    heroToSpawn = Combination[i];
                    _heroAvailableForSpawn = true;
                }
            }

            if (_heroAvailableForSpawn)
            {
                _requiredCoolDown = minCoolDown;
                StartCoroutine(SpawnAfterCoolDown());
            }
            else
            {
                // Go Back to routine and make combination again.
                SpawnLoop();
            }
        }
    }


    // Here The Actuall Instantiation Happens
    // Spawn The Hero After the Given Cool Down
    IEnumerator SpawnAfterCoolDown()
    {
        if (_isFirstTime)
        {
            yield return new WaitForSeconds(_spawnDelay);
            _isFirstTime = false;
        }
        else
        {
            yield return new WaitForSeconds(20);
        }

        if (!CentralVariables.IS_GAME_END && !CentralVariables.IS_GAME_STOPPED)
        {
            if (_spawnOrder == 1)
            {
                _requiredCoolDown = _requiredCoolDown / 2;
                _heroAvailableForSpawn = false;
                InstantiateEnemy(heroToSpawn);
                if (_requiredCoolDown > 0)
                {
                    GetHeroForSpawn();
                }
                else
                {
                    SpawnLoop();
                }
            }
            else
            {
                _requiredCoolDown = _requiredCoolDown * 2;
                _heroAvailableForSpawn = false;
                InstantiateEnemy(heroToSpawn);
                if (_requiredCoolDown < 15)
                {
                    GetHeroForSpawn();
                }
                else
                {
                    SpawnLoop();
                }
            }
        }
    }
    
    
}