using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Hero_Scripts;

public class PlayerHeroManager : MonoBehaviour
{
    [Header("Heroes")]
    [SerializeField] private List<HeroController> Heroes = null;

    [Header("Tower Skins")]
    [SerializeField] private Tower towerDay = null;
    [SerializeField] private Tower towerNight = null;

    [Header("Level Texts")]
    [SerializeField] private Text[] normalLevelText = null;
    [SerializeField] private Text templeNormalText    = null;
    [SerializeField] private Text ultimateNormalText  = null;
    [SerializeField] private Text ultimateMenuBarText = null;

    public readonly Temple   temple   = new Temple();
    public readonly Ultimate ultimate = new Ultimate();

    private HeroUnitData[] _heroesData;

    /*
     * Initialization.
     */

    public void Initialize()
    {
        if (_heroesData != null)
            throw new Exception("Already initialized.");

        _heroesData = new HeroUnitData[Heroes.Count];
        for (var i = 0; i < Heroes.Count; i++)
            _heroesData[i] = HeroUnitData.FromHeroPrototype(Heroes[i]);
    }

    /*
     * Unit Data.
     */

    public HeroUnitData GetUnitData(int id)
    {
        return _heroesData.FirstOrDefault(unitData => unitData.id == id);
    }

    public void ForEachUnitData(Action<HeroUnitData> callback)
    {
        foreach (var data in _heroesData)
            callback(data);
    }

    /*
     * Player Data Loading.
     */

    public void SetUpPlayerData()
    {
        if (CentralVariables.IsFirstTime())
        {
            var basePlayerDataJson = Resources.Load<TextAsset>("PlayerStats").text;
            var playerStatsResponse = JsonUtility.FromJson<PlayerBaseDataDto>(basePlayerDataJson);

            SetBaseTempleStats();
            SaveTempleData();

            SetBaseUltimateStats();
            SaveUltimateData();
            
            UpdateUnitsInitialStats(playerStatsResponse);
            
            foreach (var data in _heroesData)
                SaveUnitData(data);

            SetupTempleStats();
            UpdateButtons();
            return;
        }

        LoadTempleData();
        LoadUltimateData();
        
        foreach (var unitData in _heroesData)
            LoadUnitData(unitData);

        SetupTempleStats();
        UpdateButtons();
    }

    public void SetData()
    {
        SaveTempleData();
        SaveUltimateData();
        
        foreach (var data in _heroesData)
            SaveUnitData(data);
        
        UpdateButtons();
    }
    
    /*
     * Temple.
     */
    
    private void SetBaseTempleStats()
    {
        temple.level = 1;
        temple.health = temple.baseHealth;
    }
    
    private void SetupTempleStats()
    {
        towerDay.SetTowerHealth(temple.health);
        towerNight.SetTowerHealth(temple.health);
    }
    
    private void LoadTempleData()
    {
        temple.level = PlayerPrefs.GetInt("templelevel");
        temple.health = temple.health + (30f * temple.level);
        temple.isUpgradeAvailable = PlayerPrefs.GetInt("templeupgradeavailable") == 1;
    }
    
    private void SaveTempleData()
    {
        PlayerPrefs.SetInt("templelevel", temple.level);
        PlayerPrefs.SetInt("templeupgradeavailable", temple.isUpgradeAvailable ? 1 : 0);
    }

    /*
     * Ultimate.
     */

    public void UpdateUltimateStats()
    {
        ultimate.damage = 5 + (1 * ultimate.level);
        ultimate.coolDownTime = 20 - (2 * ultimate.level);
    }
    
    private void SetBaseUltimateStats()
    {
        ultimate.damage = 5 + (1 * ultimate.level);
        ultimate.coolDownTime = 20 - (2 * ultimate.level);
    }
    
    private void LoadUltimateData()
    {
        ultimate.level = PlayerPrefs.GetInt("ultimatelevel");
    }
    
    private void SaveUltimateData()
    {
        PlayerPrefs.SetInt("ultimatelevel", ultimate.level);
    }
    
    /*
     * Units.
     */

    private void LoadUnitData(HeroUnitData hero)
    {
        hero.Update(PlayerPrefs.GetFloat(hero.typeName + "_health"),
                    PlayerPrefs.GetFloat(hero.typeName + "_healthBase"),
                    PlayerPrefs.GetFloat(hero.typeName + "_damage"),
                    PlayerPrefs.GetFloat(hero.typeName + "_damageBase"),
                    PlayerPrefs.GetFloat(hero.typeName + "_speed"),
                    PlayerPrefs.GetFloat(hero.typeName + "_cooldown"),
                    PlayerPrefs.GetInt(hero.typeName + "_isunlockavailable") == 1,
                    PlayerPrefs.GetInt(hero.typeName + "_isupgradeavailable") == 1);
        hero.SetLevel(PlayerPrefs.GetInt(hero.typeName + "_level"));
    }

    private void SaveUnitData(HeroUnitData hero)
    {
        PlayerPrefs.SetInt(hero.typeName + "_level", hero.Level);
        // PlayerPrefs.SetFloat(hero.typeName + "_health", hero.Health);
        // PlayerPrefs.SetFloat(hero.typeName + "_healthBase", hero.HealthBase);
        // PlayerPrefs.SetFloat(hero.typeName + "_damage", hero.Damage);
        // PlayerPrefs.SetFloat(hero.typeName + "_damageBase", hero.DamageBase);
        // PlayerPrefs.SetFloat(hero.typeName + "_speed", hero.Speed);
        // PlayerPrefs.SetFloat(hero.typeName + "_cooldown", hero.CoolDown);
        PlayerPrefs.SetInt(hero.typeName + "_isunlockavailable", hero.IsUnlockAvailable ? 1 : 0);
        PlayerPrefs.SetInt(hero.typeName + "_isupgradeavailable", hero.IsUpgradeAvailable ? 1 : 0);
    }
    
    private void UpdateUnitsInitialStats(PlayerBaseDataDto response)
    {
        foreach (var unitData in _heroesData)
        {
            var dto = response.Get(unitData.typeName);
            unitData.SetLevel(dto.level);
        }
    }
    
    /*
     * Buttons.
     */

    public void UpdateButtons()
    {
        for (var i = 0; i < _heroesData.Length; i++)
        {
            var data = _heroesData[i];
            normalLevelText[data.id - 1].text = data.Level.ToString();
        }

        SetTempleButton();
        SetUltimateButton();
    }

    public void SetTempleButton()
    {
        templeNormalText.text = temple.level.ToString();
    }

    public void SetUltimateButton()
    {
        var level = ultimate.level.ToString();
        ultimateNormalText.text = level;
        ultimateMenuBarText.text = level;
    }

    public void SetTempleHealth()
    {
        temple.health = temple.baseHealth + (30f * temple.level);
        towerDay.SetTowerHealth(temple.health);
        towerNight.SetTowerHealth(temple.health);
    }
}