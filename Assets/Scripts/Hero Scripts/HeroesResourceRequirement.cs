using System;
using Hero_Scripts;
using UnityEngine;
using Utility.Logging;

public sealed class HeroesResourceRequirement : MonoBehaviour
{
    private readonly ILog _log = LogManager.GetLogger<HeroesResourceRequirement>();
    
    private HeroResources MilitiaResources;
    private HeroResources WarderResources;
    private HeroResources InfantryResources;
    private HeroResources HunterResources;
    private HeroResources CavalryResources;
    private HeroResources WizardResources;
    private HeroResources CatapultResources;
    private HeroResources DinosaurResources;
    private HeroResources DragonHawkResources;
    private HeroResources ArdlizResources;
    private HeroResources DragonSlayerResources;
    private HeroResources TitanResources;
    private HeroResources HippogryphyResources;
    private HeroResources BeastLordResources;
    private HeroResources TempleResources;
    private HeroResources UltimateResources;

    public void Initialize()
    {
        LoadResourcesRequirements();
    }

    private void LoadResourcesRequirements()
    {
        var json = Resources.Load<TextAsset>("HeroResources").text;
        if (string.IsNullOrEmpty(json))
        {
            _log.Error("Hero resources json is null or empty.");
            return;
        }

        ResourcesRequirementsDto resources;
        
        try
        {
            resources = JsonUtility.FromJson<ResourcesRequirementsDto>(json);
        }
        catch (Exception exception)
        {
            _log.Error($"Error parsing hero resources json: {json}");
            _log.Error(exception);
            throw;
        }

        MilitiaResources = resources.MilitiaResources;
        WarderResources = resources.WarderResources;
        InfantryResources = resources.InfantryResources;
        HunterResources = resources.HunterResources;
        CavalryResources = resources.CavalryResources;
        WizardResources = resources.WizardResources;
        CatapultResources = resources.CatapultResources;
        DinosaurResources = resources.DinosaurResources;
        DragonHawkResources = resources.DragonHawkResources;
        HippogryphyResources = resources.HippogryphyResources;
        ArdlizResources = resources.ArdlizResources;
        DragonSlayerResources = resources.DragonSlayerResources;
        TitanResources = resources.TitanResources;
        BeastLordResources = resources.BeastLordResources;
        TempleResources = resources.TempleResources;
        UltimateResources = resources.UltimateResources;
    }

    public HeroResources GetRequiredResourcesForTemple(int level)
    {
        return TempleResources.GetResourcesForLevel(level);
    }

    public HeroResources GetRequiredResourcesForUltimate(int level)
    {
        return UltimateResources.GetResourcesForLevel(level);
    }

    /*
     * Private.
     */

    public HeroResources GetUnitRequiredResources(int unitId, int level)
    {
        return GetUnitResources(unitId)?.GetResourcesForLevel(level);
    }

    private HeroResources GetUnitResources(int unitId)
    {
        switch (unitId)
        {
            case 1:  return MilitiaResources;
            case 2:  return WarderResources;
            case 3:  return InfantryResources;
            case 4:  return HunterResources;
            case 5:  return CavalryResources;
            case 6:  return WizardResources;
            case 7:  return CatapultResources;
            case 8:  return DinosaurResources;
            case 9:  return DragonHawkResources;
            case 10: return HippogryphyResources;
            case 11: return ArdlizResources;
            case 12: return DragonSlayerResources;
            case 13: return TitanResources;
            case 14: return BeastLordResources;
            case 15: return TempleResources;
            case 16: return UltimateResources;
            default:
                return null;
        }
    }

    public bool IsTempleReadyForUpgrade(HeroResources playerResources, int currentLevel)
    {
        if (TempleResources.IsEnoughForUpgrade(playerResources, currentLevel))
            return true;
        return false;
    }

    public bool IsUltimateReadyForUpgrade(HeroResources playerResources, int currentLevel)
    {
        if (UltimateResources.IsEnoughForUpgrade(playerResources, currentLevel))
            return true;
        return false;
    }

    public bool IsReadyForUpgrade(HeroResources playerResources, int unitId, int currentLevel)
    {
        if (currentLevel >= HeroUnitData.LevelMax)
            return false;

        var resources = GetUnitResources(unitId);
        if (resources.IsEnoughForUpgrade(playerResources, currentLevel))
            return true;

        return false;
    }
}