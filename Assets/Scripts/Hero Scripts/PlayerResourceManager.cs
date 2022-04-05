using System;
using Bitszer;
using UnityEngine;
using Utility.Logging;

public class PlayerResourceManager : MonoBehaviour
{
    private readonly ILog _log = LogManager.GetLogger<PlayerResourceManager>().Enable();

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

    private void OnEnable()
    {
        MainMenu.OnReturnToGameButtonClicked += OnMyAssetsButtonClicked;
    }

    private void OnDisable()
    {
        MainMenu.OnReturnToGameButtonClicked -= OnMyAssetsButtonClicked;
    }

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

        playerResources.wood.StickId = "01FYCYA8JNWBCPZNSYJPGDXGDT";
        playerResources.wood.LumberId = "01FYCYATHWBDCJ06V4ZR4RY92K";
        playerResources.wood.IronwoodId = "01FYCYBP1Z7NF1MD3429JHJWCS";
        playerResources.wood.BloodwoodId = "01FYCYCM0SCD0PE4XW3P545CJY";

        playerResources.ore.CopperId = "01FYPKX6SXPEDH3MYQVTFQBZGJ";
        playerResources.ore.SilverId = "01FYPM6AS1RTWR9GE1693QG16F";
        playerResources.ore.GoldId = "01FYR28026P8SECR96893NG965";
        playerResources.ore.PlatinumId = "01FYR28Z9EC96FQRN7YS9EZTSS";

        playerResources.food.WheatId = "01FYR29K5JDVQRE0P141EGPXS2";
        playerResources.food.CornId = "01FYR2A453HX5R8661KD6NK3JP";
        playerResources.food.RiceId = "01FYR29WEEGJBC8BPXKHRQYW7C";
        playerResources.food.PotatoesId = "01FYR2AFGD2A670MZDT9JQZV82";

        playerResources.herbs.SageId = "01FYR2AYNV0YNF0F7KKPT9X8CQ";
        playerResources.herbs.RosemaryId = "01FYR2BHWNJBMJZVKQ6ABGHVPW";
        playerResources.herbs.ChamomileId = "01FYR2CCG7BF8E8BVHHJTKN2PT";
        playerResources.herbs.ValerianId = "01FYR2D3VV2Y4N6V423A9V84NE";

        StartCoroutine(AuctionHouse.Instance.GetMyInventoryByGame(20, "", result =>
        {
            if (result.data.getMyInventorybyGame.inventory.Count != 0)
            {
                foreach (var item in result.data.getMyInventorybyGame.inventory)
                {
                    var data = item.gameItem.itemName switch
                    {
                        "Stick" => playerResources.wood.Stick = item.ItemCount,
                        "Lumber" => playerResources.wood.Lumber = item.ItemCount,
                        "Ironwood" => playerResources.wood.Ironwood = item.ItemCount,
                        "Bloodwood" => playerResources.wood.Bloodwood = item.ItemCount,
                        "Copper" => playerResources.ore.Copper = item.ItemCount,
                        "Silver" => playerResources.ore.Silver = item.ItemCount,
                        "Gold" => playerResources.ore.Gold = item.ItemCount,
                        "Platinum" => playerResources.ore.Platinum = item.ItemCount,
                        "Wheat" => playerResources.food.Wheat = item.ItemCount,
                        "Corn" => playerResources.food.Corn = item.ItemCount,
                        "Rice" => playerResources.food.Rice = item.ItemCount,
                        "Potatoes" => playerResources.food.Potatoes = item.ItemCount,
                        "Sage" => playerResources.herbs.Sage = item.ItemCount,
                        "Rosemary" => playerResources.herbs.Rosemary = item.ItemCount,
                        "Chamomile" => playerResources.herbs.Chamomile = item.ItemCount,
                        "Valerian" => playerResources.herbs.Valerian = item.ItemCount,
                        _ => 0,
                    };
                }
            }
            else
            {
                playerResources.wood.Stick = 100;
                playerResources.wood.Lumber = 100;
                playerResources.wood.Ironwood = 100;
                playerResources.wood.Bloodwood = 100;

                playerResources.ore.Copper = 100;
                playerResources.ore.Silver = 100;
                playerResources.ore.Gold = 100;
                playerResources.ore.Platinum = 100;

                playerResources.food.Wheat = 100;
                playerResources.food.Corn = 100;
                playerResources.food.Rice = 100;
                playerResources.food.Potatoes = 100;

                playerResources.herbs.Sage = 100;
                playerResources.herbs.Rosemary = 100;
                playerResources.herbs.Chamomile = 100;
                playerResources.herbs.Valerian = 100;
            }
        }));

        return playerResources;
    }

    private void OnMyAssetsButtonClicked()
    {
        var playerResources = CreateNewPlayerResources();

        PlayerResources = playerResources;

        if (_onPlayerResourcesLoaded != null)
        {
            _onPlayerResourcesLoaded.Invoke();
            _onPlayerResourcesLoaded = null;
        }
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

        InventoryDelta[] inventories = new InventoryDelta[]
        {
            new InventoryDelta 
            { 
                itemId = PlayerResources.wood.StickId,
                itemCount = PlayerResources.wood.Stick,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.wood.LumberId,
                itemCount = PlayerResources.wood.Lumber,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.wood.IronwoodId,
                itemCount = PlayerResources.wood.Ironwood,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.wood.BloodwoodId,
                itemCount = PlayerResources.wood.Bloodwood,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.ore.CopperId,
                itemCount = PlayerResources.ore.Copper,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.ore.SilverId,
                itemCount = PlayerResources.ore.Silver,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.ore.GoldId,
                itemCount = PlayerResources.ore.Gold,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.ore.PlatinumId,
                itemCount = PlayerResources.ore.Platinum,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.food.WheatId,
                itemCount = PlayerResources.food.Wheat,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.food.CornId,
                itemCount = PlayerResources.food.Corn,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.food.RiceId,
                itemCount = PlayerResources.food.Rice,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.food.PotatoesId,
                itemCount = PlayerResources.food.Potatoes,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.herbs.SageId,
                itemCount = PlayerResources.herbs.Sage,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.herbs.RosemaryId,
                itemCount = PlayerResources.herbs.Rosemary,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.herbs.ChamomileId,
                itemCount = PlayerResources.herbs.Chamomile,
            },
            new InventoryDelta
            {
                itemId = PlayerResources.herbs.ValerianId,
                itemCount = PlayerResources.herbs.Valerian,
            },
        };

        StartCoroutine(AuctionHouse.Instance.PushInventory(inventories, result =>
        {
            if (result == null || result.data == null)
            {
                APIManager.Instance.SetError("Something went wrong!", "Okay", ErrorType.CustomMessage);
                APIManager.Instance.RaycastBlock(false);
            }
        }));
        
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
        rewardResources.ore.Platinum = MakeTenPercentage(3, levelFactor);

        rewardResources.food.Wheat = MakeTenPercentage(65, levelFactor);
        rewardResources.food.Corn = MakeTenPercentage(10, levelFactor);
        rewardResources.food.Rice = MakeTenPercentage(16, levelFactor);
        rewardResources.food.Potatoes = MakeTenPercentage(7, levelFactor);

        rewardResources.herbs.Sage = MakeTenPercentage(55, levelFactor);
        rewardResources.herbs.Rosemary = MakeTenPercentage(10, levelFactor);
        rewardResources.herbs.Chamomile = MakeTenPercentage(40, levelFactor);
        rewardResources.herbs.Valerian = MakeTenPercentage(5, levelFactor);

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