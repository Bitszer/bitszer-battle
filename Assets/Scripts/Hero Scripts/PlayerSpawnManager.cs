using UnityEngine;
using System.Collections;
using MenuScripts;
using Utility.Logging;

public sealed class PlayerSpawnManager : MonoBehaviour
{
    private readonly ILog _log = LogManager.GetLogger<PlayerSpawnManager>();

    private const int UnitsMax = 13;
    
    [Header("Dependencies")]
    [SerializeField] private PlayerHeroManager playerHeroManager = null;

    [Header("Units")]
    [SerializeField] private HudUnitControl[] unitsSlots = null;
    
    [Header("Mana Bar")]
    [SerializeField] private HudManaBar manaBar = null;

    [Header("Ultimate")]
    [SerializeField] private SuperPower superPower = null;
    
    [Header("Other")]
    [SerializeField] private Transform  SpawnPosition = null;

    private float _mana;
    private float _manaMax = 2000F;
    private bool  _isUltimateReady = true;
    
    private Coroutine _manaSpawningCoroutine;

    /*
     * Mono Behavior.
     */

    private void Start()
    {
        if (playerHeroManager == null)
            _log.Error("PlayerSpawnManager dependency not set.");

        _isUltimateReady = true;
    }
    
    /*
     * Public.
     */

    public void StartGame()
    {
        Debug.Log("Starting of StartGame()");

        _mana = 0;
        superPower.Initialize();

        StopCoroutine(SuperPowerCoolDown());
        _isUltimateReady = true;
        
        CentralVariables.PlayerMax = 2;
        
        UpdateSlots();
        UpdateManaBar();
        RunStartCoolDowns();
        
        if (_manaSpawningCoroutine != null)
            StopCoroutine(_manaSpawningCoroutine);
        
        _manaSpawningCoroutine = StartCoroutine(ManaGeneration());

        Debug.Log("Ending of StartGame()");
    }

    /*
     * Private.
     */
    
    private IEnumerator ManaGeneration()
    {
        while (!CentralVariables.IS_GAME_END)
        {
            yield return new WaitForSeconds(0.1f);

            _mana = Mathf.Min(_mana + Random.Range(3, 5), _manaMax);
            manaBar.UpdateMana(_mana, _manaMax);
        }
    }

    private void UpdateSlots()
    {
        var index = 0;
        playerHeroManager.ForEachUnitData(unitData =>
        {
            unitsSlots[index].SetUnitData(unitData);
            index++;
        });
    }

    private void UpdateManaBar()
    {
        manaBar.UpdateMana(_mana, _manaMax);
    }
    
    private void RunStartCoolDowns()
    {
        var index = 0;
        playerHeroManager.ForEachUnitData(unitData =>
        {
            if (unitData.IsUnlocked)
                unitsSlots[index].PlayCoolDownAnimation(unitsSlots[index].CoolDownInitialization);
            index++;
        });
    }

    /*
     * Super Power.
     */

    public void SuperPowerClicked()
    {
        if (!_isUltimateReady)
            return;
        _isUltimateReady = false;
        superPower.CastSuperPower();
        StartCoroutine(SuperPowerCoolDown());
    }

    IEnumerator SuperPowerCoolDown()
    {
        yield return new WaitForSeconds(20);
        _isUltimateReady = true;
    }

    /*
     * Units Spawning.
     */

    public bool TrySpawnUnit(int unitId)
    {
        if (CentralVariables.IS_GAME_END || CentralVariables.PlayerMax > UnitsMax)
            return false;

        var data = playerHeroManager.GetUnitData(unitId);
        if (data.SpawnCost > _mana)
            return false;

        SoundManager.Instance.PlayOneShot(SoundManager.SoundState.Spawn, 1);

        _mana -= data.SpawnCost;
        manaBar.UpdateMana(_mana, _manaMax);

        SpawnUnit(data);
        return true;
    }

    private void SpawnUnit(HeroUnitData data)
    {
        if (CentralVariables.IS_GAME_END)
            return;

        var heroInstance = Instantiate(data.Prototype, SpawnPosition.position, SpawnPosition.rotation);
        heroInstance.name = data.typeName;
        heroInstance.SetStats(data);

        CentralVariables.PlayerUnitsInstances.Add(heroInstance);
        CentralVariables.PlayerMax++;
    }
}