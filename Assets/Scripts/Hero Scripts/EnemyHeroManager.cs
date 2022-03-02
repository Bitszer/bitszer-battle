using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Utility.Logging;
using Random = UnityEngine.Random;

public sealed class EnemyHeroManager : MonoBehaviour
{
    private readonly ILog _log = LogManager.GetLogger<EnemyHeroManager>().Disable();

    [Header("Dependencies")]
    [SerializeField] private LevelProgression levelProgressionManager = null;

    [Header("Configuration")]
    [SerializeField] private List<HeroController> heroes = null;

    private HeroUnitData[] _heroesData;

    public void Initialize()
    {
        if (_heroesData != null)
            throw new Exception("Already initialized.");

        _heroesData = new HeroUnitData[heroes.Count];
        for (var i = 0; i < heroes.Count; i++)
            _heroesData[i] = HeroUnitData.FromHeroPrototype(heroes[i]);
    }

    public void UpdateHeroesAvailability()
    {
        foreach (var data in _heroesData)
        {
            var level = levelProgressionManager.GetEntityLevel(data.typeName);
            data.SetLevel(level);

            if (level > 0)
                _log.Debug("Unit enabled: " + data.typeName + " " + level);
        }
    }

    public HeroUnitData GetRandomAvailableHeroData()
    {
        var available = _heroesData.Where(data => data.IsUnlocked).ToArray();
        return available[Random.Range(0, available.Length)];
    }
}