using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utility.Logging;
using System.Collections;
using Bitszer;

public sealed class GameManager : MonoBehaviour
{
    private readonly ILog _log = LogManager.GetLogger<GameManager>().Disable();
    
    [SerializeField] public PlayerResourcesMenu       playerResourcesMenu;
    [SerializeField] public PlayerResourceManager     playerResources;
    [SerializeField] public HeroesResourceRequirement heroResourceRequirement;
    [SerializeField] public PlayerHeroManager         playerHeroManager;
    [SerializeField] public EnemyHeroManager          enemyHeroManager;
    [SerializeField] public LevelProgression          levelProgression;
    [SerializeField] public PlayerSpawnManager        playerSpawn;
    [SerializeField] public EnemySpawnManager         enemySpawn;
    [SerializeField] public BattleEnvironmentsManager battleEnvironmentsManager;
    [SerializeField] public CameraScroll              cameraScroll;
    [SerializeField] public GameObject[]              towers;
    [SerializeField] public GameObject                LoadingScreen;
    [SerializeField] public GameObject                CloseAppButton;
    [SerializeField] public Text                      loadingLabel;
    [SerializeField] public Text                      loadingText;
    [SerializeField] public GameObject                Logo;
    [SerializeField] public Transform                 logoPos;

    private int _index;

    private void Awake()
    {
        if (battleEnvironmentsManager == null)
            _log.Error("BattleEnvironmentsManager dependency not set.");

        InitializeGameData();
        ShowLoading();
        Load();
    }

    private void OnEnable()
    {
        Events.OnAuctionHouseInitialized.AddListener(OnAuctionHouseInitialized);
    }

    private void OnDisable()
    {
        Events.OnAuctionHouseInitialized.RemoveListener(OnAuctionHouseInitialized);
    }

    /*
     * Initialization.
     */

    private void InitializeGameData()
    {
        heroResourceRequirement.Initialize();
        playerHeroManager.Initialize();
        enemyHeroManager.Initialize();
    }
    
    /*
     * Loading.
     */

    private void Load()
    {
        _log.Debug("Loading...");
        _log.Debug("Initializing AuctionHouse...");

        //OnAuctionHouseInitialized();
    }
    
    private void OnAuctionHouseInitialized()
    {
        _log.Debug("AuctionHouse initialized.");
        
        _log.Debug("Level progression loading...");
        levelProgression.LoadLevelProgression(() =>
        {
            _log.Debug("Level progression loaded.");
            _log.Debug("Player resources loading...");
            
            playerResources.LoadPlayerResources(() =>
            {
                _log.Debug("Player resources loaded.");
                
                _log.Debug("Setting up player's data.");
                playerHeroManager.SetUpPlayerData();
                _log.Debug("Player's data set up.");
                
                HideLoading();
            });
        });
    }
    
    /*
     * Loading State.
     */

    private void ShowLoading()
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(ShowLoadingImpl());
    }

    private void HideLoading()
    {
        StartCoroutine(HideLoadingImpl());
    }

    private IEnumerator ShowLoadingImpl()
    {
        while (true)
        {
            if (_index == 0)
            {
                loadingText.text = ".";
                _index++;
            }
            else if (_index == 1)
            {
                loadingText.text = ". .";
                _index++;
            }
            else if (_index == 2)
            {
                loadingText.text = ". . .";
                _index = 0;
            }
            
            if (!ReachabiltyVarifier.checkInternet())
            {
                loadingText.gameObject.SetActive(false);
                loadingLabel.gameObject.SetActive(false);
                CloseAppButton.SetActive(true);
                yield break;
            }
        
            yield return new WaitForSeconds(0.5f);    
        }
    }
    
    private IEnumerator HideLoadingImpl()
    {
        yield return new WaitForSeconds(1.5f);
     
        StopAllCoroutines();
        
        LoadingScreen.SetActive(false);
        
        LeanTween.move(Logo, logoPos.position, 0.3f);
        LeanTween.scale(Logo, new Vector3(1, 1, 1), 0.3f);

        // Play BG Track For main menu.
        SoundManager.Instance.AdjustVolume(1);
        SoundManager.Instance.PlayBgMusic(true, SoundManager.BackGroundState.MainMenu);
        SoundManager.Instance.musicFader(4, SoundManager.Fade.In);
    }
    
    /*
     * Runtime.
     */
    
    public void SetLevelProgression()
    {
        var currentLevel = PlayerPrefs.GetInt("level");
        var currentDifficulty = PlayerPrefs.GetInt("difficulty[" + currentLevel + "]");

        levelProgression.SetLevelProgression(CentralVariables.SELECTED_LEVEL, CentralVariables.SELECTED_DIFFICULTY);
        levelProgression.SetLevelProgression(currentLevel, currentDifficulty);
        
        playerHeroManager.UpdateButtons();
    }

    public void StartGame()
    {
        SoundManager.Instance.AdjustVolume(0.3f);
        SoundManager.Instance.musicFader(5, SoundManager.Fade.In);
        SoundManager.Instance.PlayBgMusic(true, SoundManager.BackGroundState.Gameplay);

        CentralVariables.IS_GAME_STARTED = true;

        levelProgression.SetLevelProgression(CentralVariables.SELECTED_LEVEL, CentralVariables.SELECTED_DIFFICULTY);

        battleEnvironmentsManager.EnableEnvironment(CentralVariables.SELECTED_LEVEL);
        
        cameraScroll.Initialize();
        
        playerHeroManager.SetTempleHealth();
        playerHeroManager.UpdateUltimateStats();
        playerHeroManager.UpdateButtons();
        
        playerSpawn.StartGame();
        enemySpawn.StartSpawning();
        
        var towers = FindObjectsOfType<Tower>();
        foreach (var tower in towers)
            tower.ResetTower();
    }

    public void StopGame()
    {
        CentralVariables.IS_GAME_STOPPED = true;
        CentralVariables.IS_GAME_STARTED = false;

        for (var index = 0; index < towers.Length; index++)
            towers[index].GetComponent<Tower>().ResetTower();

        battleEnvironmentsManager.DisableAll();

        LeanTween.cancelAll();
        StopAllCoroutines();

        DestroyAllCharacters();
    }

    private void DestroyAllCharacters()
    {
        foreach (var unitInstance in CentralVariables.PlayerUnitsInstances)
            Destroy(unitInstance.gameObject);
        
        foreach (var unitInstance in CentralVariables.EnemyUnitInstances)
            Destroy(unitInstance.gameObject);
        
        CentralVariables.PlayerUnitsInstances.Clear();
        CentralVariables.EnemyUnitInstances.Clear();
    }

    public ReturnObject UpgradeTemple(Temple _temple)
    {
        var returnObject = new ReturnObject();
        if (_temple.level < 5)
        {
            if (heroResourceRequirement.IsTempleReadyForUpgrade(playerResources.PlayerResources, _temple.level))
            {
                playerResources.SubtractTheResourcesOnTempleUpgrade(_temple.level);
                _temple.level++;
                playerHeroManager.SetData();
                playerHeroManager.SetTempleButton();
                playerHeroManager.SetTempleHealth();
                playerResourcesMenu.UpgradeCharacterResources();
                returnObject.success = true;
            }
            else
            {
                returnObject.success = false;
                returnObject.message = 1;
            }
        }
        else
        {
            returnObject.success = false;
            returnObject.message = 2;
        }

        return returnObject;
    }
    
    public ReturnObject UpgradeUltimate(Ultimate _ultimate)
    {
        var returnObject = new ReturnObject();
        if (_ultimate.level < 5)
        {
            if (heroResourceRequirement.IsUltimateReadyForUpgrade(playerResources.PlayerResources, _ultimate.level))
            {
                playerResources.SubtractTheResourcesOnUltimateUpgrade(_ultimate.level);
                _ultimate.level++;
                playerHeroManager.SetData();
                playerHeroManager.SetUltimateButton();
                playerHeroManager.UpdateUltimateStats();
                playerResourcesMenu.UpgradeCharacterResources();
                returnObject.success = true;
            }
            else
            {
                returnObject.success = false;
                returnObject.message = 1;
            }
        }
        else
        {
            returnObject.success = false;
            returnObject.message = 2;
        }

        return returnObject;
    }

    public ReturnObject UpgradeHero(HeroUnitData _hero, int _heroIndex)
    {
        var retureObject = new ReturnObject();
        if (_hero.Level < 5)
        {
            if (heroResourceRequirement.IsReadyForUpgrade(playerResources.PlayerResources, _heroIndex, _hero.Level))
            {
                playerResources.SubtractTheResourceOnPurchase(_heroIndex, _hero.Level);

                _hero.SetLevel(_hero.Level + 1);

                playerHeroManager.SetData();
                playerResourcesMenu.UpgradeCharacterResources();

                retureObject.success = true;
            }
            else
            {
                retureObject.success = false;
                retureObject.message = 1;
            }
        }
        else
        {
            retureObject.success = false;
            retureObject.message = 2;
        }

        return retureObject;
    }
}