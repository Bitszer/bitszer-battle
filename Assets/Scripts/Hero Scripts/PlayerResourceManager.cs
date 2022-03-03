using System;
using UnityEngine;
using Utility.Logging;

public class PlayerResourceManager : MonoBehaviour
{
    private readonly ILog _log = LogManager.GetLogger<PlayerResourceManager>().Disable();

    [Header("Dependencies")]
    [SerializeField] private GameManager               gameManager = null;
    [SerializeField] private HeroesResourceRequirement heroesResourceRequirement = null;
    [SerializeField] private PlayerResourcesMenu       resourcesMenu = null;
    [SerializeField] private ResourcesWon              resourcesWon = null;
    
    public HeroResources PlayerResources { get; private set; }

    public Action OnPlayerResourcesChanged;

    private readonly int[] _percentage   = new int[2];
    private readonly int[] _divideFactor = new int[10];
    
    private Action _onPlayerResourcesLoaded;

    /*
     * Mono Behavior.
     */
    
    public void Start()
    {
        if (gameManager == null)
            _log.Error("GameManager dependency not set.");
        
        if (heroesResourceRequirement == null)
            _log.Error("HeroesResourceRequirement dependency not set.");

        var counter = 15;
        for (var i = 0; i < 10; i++)
        {
            _divideFactor[i] = counter;
            counter++;
        }
    }
    
    /*
     * Load & Save.
     */

    public void LoadPlayerResources(Action onComplete)
    {
        if (_onPlayerResourcesLoaded != null)
        {
            _log.Error("LoadPlayerResources: Already loading");
            return;
        }

        _onPlayerResourcesLoaded = onComplete;

        OnAuctionHouseSynchronized();
    }

    private void OnAuctionHouseSynchronized()
    {
        _log.Debug("OnAuctionHouseSynchronized");

        var playerResources = LoadPlayerResourcesFromPlayerPrefs();
        playerResources = CreateNewPlayerResources();

        PlayerResources = playerResources;

        if (_onPlayerResourcesLoaded != null)
        {
            _onPlayerResourcesLoaded.Invoke();
            _onPlayerResourcesLoaded = null;
        }
    }
    
    private HeroResources CreateNewPlayerResources()
    {
        var playerResources = new HeroResources();

        if (Debug.isDebugBuild)
        {
            playerResources.wood.Stick = 10000;
            playerResources.wood.Lumber = 10000;
            playerResources.wood.Ironwood = 10000;
            playerResources.wood.Bloodwood = 10000;

            playerResources.ore.Copper = 10000;
            playerResources.ore.Silver = 10000;
            playerResources.ore.Gold = 10000;
            playerResources.ore.Platinum = 10000;

            playerResources.food.Wheat = 10000;
            playerResources.food.Corn = 10000;
            playerResources.food.Rice = 10000;
            playerResources.food.Potatoes = 10000;

            playerResources.herbs.Sage = 10000;
            playerResources.herbs.Rosemary = 10000;
            playerResources.herbs.Chamomile = 10000;
            playerResources.herbs.Valerian = 10000;
        }

        return playerResources;
    }

    // After any transaction of resources this method should be called
    // This method saves the modified resources values to Prefs.
    public void SavePlayerResources()
    {
        if (PlayerResources == null)
        {
            _log.Error("PlayerResources is null.");
            return;
        }

        // Saving to game storage.
        SavePlayerResourcesToPlayerPrefs(PlayerResources);
        
        OnPlayerResourcesChanged?.Invoke();
    }

    private HeroResources LoadPlayerResourcesFromPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("player_resources"))
            return null;
        var json = PlayerPrefs.GetString("player_resources");
        return !string.IsNullOrEmpty(json) ? JsonUtility.FromJson<HeroResources>(json) 
                                           : new HeroResources();
    }
    
    private void SavePlayerResourcesToPlayerPrefs(HeroResources resources)
    {
        var json = JsonUtility.ToJson(resources);
        PlayerPrefs.SetString("player_resources", json);
    }
    
    /*
     * Operations.
     */

    // This method is called when user wants to upgrade the hero.
    // This method subtracts the resources required to unlock/upgrade any hero.
    public void SubtractTheResourceOnPurchase(int _heroIndex, int _heroLevel)
    {
        var requiredResources = heroesResourceRequirement.GetUnitRequiredResources(_heroIndex, _heroLevel);

        PlayerResources.wood.Stick -= requiredResources.wood.Stick;
        PlayerResources.wood.Lumber -= requiredResources.wood.Lumber;
        PlayerResources.wood.Ironwood -= requiredResources.wood.Ironwood;
        PlayerResources.wood.Bloodwood -= requiredResources.wood.Bloodwood;

        PlayerResources.ore.Copper -= requiredResources.ore.Copper;
        PlayerResources.ore.Silver -= requiredResources.ore.Silver;
        PlayerResources.ore.Gold -= requiredResources.ore.Gold;
        PlayerResources.ore.Platinum -= requiredResources.ore.Platinum;

        PlayerResources.food.Wheat -= requiredResources.food.Wheat;
        PlayerResources.food.Corn -= requiredResources.food.Corn;
        PlayerResources.food.Rice -= requiredResources.food.Rice;
        PlayerResources.food.Potatoes -= requiredResources.food.Potatoes;

        PlayerResources.herbs.Sage -= requiredResources.herbs.Sage;
        PlayerResources.herbs.Rosemary -= requiredResources.herbs.Rosemary;
        PlayerResources.herbs.Chamomile -= requiredResources.herbs.Chamomile;
        PlayerResources.herbs.Valerian -= requiredResources.herbs.Valerian;
        
        SavePlayerResources();
    }

    public void SubtractTheResourcesOnUltimateUpgrade(int _ultimateLevel)
    {
        var ultimateResources = heroesResourceRequirement.GetRequiredResourcesForUltimate(_ultimateLevel);

        PlayerResources.wood.Stick -= ultimateResources.wood.Stick;
        PlayerResources.wood.Lumber -= ultimateResources.wood.Lumber;
        PlayerResources.wood.Ironwood -= ultimateResources.wood.Ironwood;
        PlayerResources.wood.Bloodwood -= ultimateResources.wood.Bloodwood;

        PlayerResources.ore.Copper -= ultimateResources.ore.Copper;
        PlayerResources.ore.Silver -= ultimateResources.ore.Silver;
        PlayerResources.ore.Gold -= ultimateResources.ore.Gold;
        PlayerResources.ore.Platinum -= ultimateResources.ore.Platinum;

        PlayerResources.food.Wheat -= ultimateResources.food.Wheat;
        PlayerResources.food.Corn -= ultimateResources.food.Corn;
        PlayerResources.food.Rice -= ultimateResources.food.Rice;
        PlayerResources.food.Potatoes -= ultimateResources.food.Potatoes;

        PlayerResources.herbs.Sage -= ultimateResources.herbs.Sage;
        PlayerResources.herbs.Rosemary -= ultimateResources.herbs.Rosemary;
        PlayerResources.herbs.Chamomile -= ultimateResources.herbs.Chamomile;
        PlayerResources.herbs.Valerian -= ultimateResources.herbs.Valerian;
        
        SavePlayerResources();
    }

    public void SubtractTheResourcesOnTempleUpgrade(int _templeLevel)
    {
        var templeResources = heroesResourceRequirement.GetRequiredResourcesForTemple(_templeLevel);

        PlayerResources.wood.Stick -= templeResources.wood.Stick;
        PlayerResources.wood.Lumber -= templeResources.wood.Lumber;
        PlayerResources.wood.Ironwood -= templeResources.wood.Ironwood;
        PlayerResources.wood.Bloodwood -= templeResources.wood.Bloodwood;

        PlayerResources.ore.Copper -= templeResources.ore.Copper;
        PlayerResources.ore.Silver -= templeResources.ore.Silver;
        PlayerResources.ore.Gold -= templeResources.ore.Gold;
        PlayerResources.ore.Platinum -= templeResources.ore.Platinum;

        PlayerResources.food.Wheat -= templeResources.food.Wheat;
        PlayerResources.food.Corn -= templeResources.food.Corn;
        PlayerResources.food.Rice -= templeResources.food.Rice;
        PlayerResources.food.Potatoes -= templeResources.food.Potatoes;

        PlayerResources.herbs.Sage -= templeResources.herbs.Sage;
        PlayerResources.herbs.Rosemary -= templeResources.herbs.Rosemary;
        PlayerResources.herbs.Chamomile -= templeResources.herbs.Chamomile;
        PlayerResources.herbs.Valerian -= templeResources.herbs.Valerian;
        
        SavePlayerResources();
    }

    public void AddResourcesOnWin()
    {
        var rewardResources = MakeRandomRangeResources();

        PlayerResources.wood.Stick += rewardResources.wood.Stick;
        PlayerResources.wood.Lumber += rewardResources.wood.Lumber;
        PlayerResources.wood.Ironwood += rewardResources.wood.Ironwood;
        PlayerResources.wood.Bloodwood += rewardResources.wood.Bloodwood;

        PlayerResources.ore.Copper += rewardResources.ore.Copper;
        PlayerResources.ore.Silver += rewardResources.ore.Silver;
        PlayerResources.ore.Gold += rewardResources.ore.Gold;
        PlayerResources.ore.Platinum += rewardResources.ore.Platinum;

        PlayerResources.food.Wheat += rewardResources.food.Wheat;
        PlayerResources.food.Corn += rewardResources.food.Corn;
        PlayerResources.food.Rice += rewardResources.food.Rice;
        PlayerResources.food.Potatoes += rewardResources.food.Potatoes;

        PlayerResources.herbs.Sage += rewardResources.herbs.Sage;
        PlayerResources.herbs.Rosemary += rewardResources.herbs.Rosemary;
        PlayerResources.herbs.Chamomile += rewardResources.herbs.Chamomile;
        PlayerResources.herbs.Valerian += rewardResources.herbs.Valerian;

        resourcesWon.DisplayWonResources(rewardResources);
        resourcesMenu.UpgradeCharacterResources();
        
        SavePlayerResources();
    }

    private int MakeTenPercentage(float value, int levelFactor)
    {
        _percentage[0] = (int)(((value * 1.25f) * levelFactor) + ((((value * 1.25f) * levelFactor) * 10) / 100));
        _percentage[1] = (int)(((value * 1.25f) * levelFactor) - ((((value * 1.25f) * levelFactor) * 10) / 100));
        return _percentage[UnityEngine.Random.Range(0, _percentage.Length)];
    }

    // Make Random Range Reward Resources
    public HeroResources MakeRandomRangeResources()
    {
        var levelFactor = (int)(1.25f * CentralVariables.SELECTED_LEVEL);

        var rewardResources = new HeroResources();
        rewardResources.wood.Stick = MakeTenPercentage(25, levelFactor);
        rewardResources.wood.Lumber = MakeTenPercentage(10, levelFactor);
        rewardResources.wood.Ironwood = MakeTenPercentage(5, levelFactor);
        rewardResources.wood.Bloodwood = MakeTenPercentage(5, levelFactor);

        rewardResources.ore.Copper = MakeTenPercentage(50, levelFactor);
        rewardResources.ore.Silver = MakeTenPercentage(10, levelFactor);
        rewardResources.ore.Gold = MakeTenPercentage(15, levelFactor);
        rewardResources.ore.Platinum = MakeTenPercentage(25, levelFactor);

        rewardResources.food.Wheat = MakeTenPercentage(65, levelFactor);
        rewardResources.food.Corn = MakeTenPercentage(10, levelFactor);
        rewardResources.food.Rice = MakeTenPercentage(16, levelFactor);
        rewardResources.food.Potatoes = MakeTenPercentage(25, levelFactor);

        rewardResources.herbs.Sage = MakeTenPercentage(70, levelFactor);
        rewardResources.herbs.Rosemary = MakeTenPercentage(10, levelFactor);
        rewardResources.herbs.Chamomile = MakeTenPercentage(65, levelFactor);
        rewardResources.herbs.Valerian = MakeTenPercentage(20, levelFactor);

        return rewardResources;
    }

    public void CheatCode()
    {
        var rewardResources = MakeRandomRangeResources();
        rewardResources.wood.Stick = 9999;
        rewardResources.wood.Lumber = 9999;
        rewardResources.wood.Ironwood = 9999;
        rewardResources.wood.Bloodwood = 9999;

        rewardResources.ore.Copper = 9999;
        rewardResources.ore.Silver = 9999;
        rewardResources.ore.Gold = 9999;
        rewardResources.ore.Platinum = 9999;

        rewardResources.food.Wheat = 9999;
        rewardResources.food.Corn = 9999;
        rewardResources.food.Rice = 9999;
        rewardResources.food.Potatoes = 9999;

        rewardResources.herbs.Sage = 9999;
        rewardResources.herbs.Rosemary = 9999;
        rewardResources.herbs.Chamomile = 9999;
        rewardResources.herbs.Valerian = 9999;

        PlayerResources.wood.Stick += rewardResources.wood.Stick;
        PlayerResources.wood.Lumber += rewardResources.wood.Stick;
        PlayerResources.wood.Ironwood += rewardResources.wood.Stick;
        PlayerResources.wood.Bloodwood += rewardResources.wood.Stick;

        PlayerResources.ore.Copper += rewardResources.ore.Copper;
        PlayerResources.ore.Silver += rewardResources.ore.Silver;
        PlayerResources.ore.Gold += rewardResources.ore.Gold;
        PlayerResources.ore.Platinum += rewardResources.ore.Platinum;

        PlayerResources.food.Wheat += rewardResources.food.Wheat;
        PlayerResources.food.Corn += rewardResources.food.Corn;
        PlayerResources.food.Rice += rewardResources.food.Rice;
        PlayerResources.food.Potatoes += rewardResources.food.Potatoes;

        PlayerResources.herbs.Sage += rewardResources.herbs.Sage;
        PlayerResources.herbs.Rosemary += rewardResources.herbs.Rosemary;
        PlayerResources.herbs.Chamomile += rewardResources.herbs.Chamomile;
        PlayerResources.herbs.Valerian += rewardResources.herbs.Valerian;

        resourcesWon.DisplayWonResources(rewardResources);
        resourcesMenu.UpgradeCharacterResources();
        
        SavePlayerResources();
    }
}