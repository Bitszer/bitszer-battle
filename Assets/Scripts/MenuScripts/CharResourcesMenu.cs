using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Utility.Logging;

public class CharResourcesMenu : MonoBehaviour
{
    private readonly ILog _log = LogManager.GetLogger<CharResourcesMenu>();
    
    [SerializeField] private SuperPower superPowerController = null;

    public GameManager       gameManager;
    public PlayerHeroManager playerHeroesManager;

    [Space]
    public Sprite[] pictures;

    public Sprite TemplePicture;
    public Sprite UltimatePicture;
    public Image  CharacterPic;
    public Text   CharacterName;
    public Text   ChatacterDescription;
    public Image  Damage;
    public Image  DamageExtend;
    public Image  Speed;
    public Image  Health;
    public Image  HealthExtend;
    public Text   Level;

    [Space]
    public Text stickText;

    public Image stickFiller;
    public Text  lumberText;
    public Image lumberFiller;
    public Text  ironText;
    public Image ironFiller;
    public Text  bloodText;
    public Image bloodFiller;

    [Space]
    public Text copperText;

    public Image copperFiller;
    public Text  silverText;
    public Image silverFiller;
    public Text  goldText;
    public Image goldFiller;
    public Text  platinumText;
    public Image platinumFiller;

    [Space]
    public Text wheatText;

    public Image WheatFiller;
    public Text  riceText;
    public Image riceFiller;
    public Text  cornText;
    public Image cornFiller;
    public Text  potatoesText;
    public Image potatoesFiller;

    [Space]
    public Text sageText;

    public Image sageFiller;
    public Text  rosemaryText;
    public Image rosemaryFiller;
    public Text  chamomileText;
    public Image chamomileFiller;
    public Text  valerianText;
    public Image valerianFiller;

    [Space]
    public Text[] ResourcesName;

    public Text[] ResourcesAmount;

    [Space]
    public Button UpgradeButton;

    public Sprite UnlockSprite;
    public Sprite UpgradeSprite;
    public Text   SpeedText;
    public Text   DamageText;
    public Text   HealthText;

    [Space]
    public GameObject LockImage;

    HeroResources heroResource;
    HeroResources playerResource;

    HeroResources templeResource;
    HeroResources ultimateResource;

    private int _unitId;
    
    /*
     * Mono Behavior.
     */

    private void Awake()
    {
        gameManager.playerResources.OnPlayerResourcesChanged += OnPlayerResourcesChanged;
    }

    
    

    private void SetWoodData()
    {
        if (playerResource.wood.Stick < heroResource.wood.Stick)
        {
            stickText.text = playerResource.wood.Stick + "/" + heroResource.wood.Stick;
        }
        else
        {
            stickText.text = heroResource.wood.Stick + "/" + heroResource.wood.Stick;
        }

        if (playerResource.wood.Lumber < heroResource.wood.Lumber)
        {
            lumberText.text = playerResource.wood.Lumber + "/" + heroResource.wood.Lumber;
        }
        else
        {
            lumberText.text = heroResource.wood.Lumber + "/" + heroResource.wood.Lumber;
        }

        if (playerResource.wood.Ironwood < heroResource.wood.Ironwood)
        {
            ironText.text = playerResource.wood.Ironwood + "/" + heroResource.wood.Ironwood;
        }
        else
        {
            ironText.text = heroResource.wood.Ironwood + "/" + heroResource.wood.Ironwood;
        }

        if (playerResource.wood.Bloodwood < heroResource.wood.Bloodwood)
        {
            bloodText.text = playerResource.wood.Bloodwood + "/" + heroResource.wood.Bloodwood;
        }
        else
        {
            bloodText.text = heroResource.wood.Bloodwood + "/" + heroResource.wood.Bloodwood;
        }

        if (heroResource.wood.Stick != 0)
        {
            stickFiller.fillAmount = ClampRange.Clamp(0, heroResource.wood.Stick, 0, 1, playerResource.wood.Stick);
        }
        else
        {
            stickFiller.fillAmount = 0;
        }

        if (heroResource.wood.Lumber != 0)
        {
            lumberFiller.fillAmount = ClampRange.Clamp(0, heroResource.wood.Lumber, 0, 1, playerResource.wood.Lumber);
        }
        else
        {
            lumberFiller.fillAmount = 0;
        }

        if (heroResource.wood.Ironwood != 0)
        {
            ironFiller.fillAmount = ClampRange.Clamp(0, heroResource.wood.Ironwood, 0, 1, playerResource.wood.Ironwood);
        }
        else
        {
            ironFiller.fillAmount = 0;
        }

        if (heroResource.wood.Bloodwood != 0)
        {
            bloodFiller.fillAmount = ClampRange.Clamp(0, heroResource.wood.Bloodwood, 0, 1, playerResource.wood.Bloodwood);
        }
        else
        {
            bloodFiller.fillAmount = 0;
        }
    }

    private void SetOreData()
    {
        if (playerResource.ore.Copper < heroResource.ore.Copper)
        {
            copperText.text = playerResource.ore.Copper + "/" + heroResource.ore.Copper;
        }
        else
        {
            copperText.text = heroResource.ore.Copper + "/" + heroResource.ore.Copper;
        }

        if (playerResource.ore.Silver < heroResource.ore.Silver)
        {
            silverText.text = playerResource.ore.Silver + "/" + heroResource.ore.Silver;
        }
        else
        {
            silverText.text = heroResource.ore.Silver + "/" + heroResource.ore.Silver;
        }

        if (playerResource.ore.Gold < heroResource.ore.Gold)
        {
            goldText.text = playerResource.ore.Gold + "/" + heroResource.ore.Gold;
        }
        else
        {
            goldText.text = heroResource.ore.Gold + "/" + heroResource.ore.Gold;
        }

        if (playerResource.ore.Platinum < heroResource.ore.Platinum)
        {
            platinumText.text = playerResource.ore.Platinum + "/" + heroResource.ore.Platinum;
        }
        else
        {
            platinumText.text = heroResource.ore.Platinum + "/" + heroResource.ore.Platinum;
        }

        if (heroResource.ore.Copper != 0)
        {
            copperFiller.fillAmount = ClampRange.Clamp(0, heroResource.ore.Copper, 0, 1, playerResource.ore.Copper);
        }
        else
        {
            copperFiller.fillAmount = 0;
        }

        if (heroResource.ore.Silver != 0)
        {
            silverFiller.fillAmount = ClampRange.Clamp(0, heroResource.ore.Silver, 0, 1, playerResource.ore.Silver);
        }
        else
        {
            silverFiller.fillAmount = 0;
        }

        if (heroResource.ore.Gold != 0)
        {
            goldFiller.fillAmount = ClampRange.Clamp(0, heroResource.ore.Gold, 0, 1, playerResource.ore.Gold);
        }
        else
        {
            goldFiller.fillAmount = 0;
        }

        if (heroResource.ore.Platinum != 0)
        {
            platinumFiller.fillAmount = ClampRange.Clamp(0, heroResource.ore.Platinum, 0, 1, playerResource.ore.Platinum);
        }
        else
        {
            platinumFiller.fillAmount = 0;
        }
    }

    private void SetFoodData()
    {
        if (playerResource.food.Wheat < heroResource.food.Wheat)
        {
            wheatText.text = playerResource.food.Wheat + "/" + heroResource.food.Wheat;
        }
        else
        {
            wheatText.text = heroResource.food.Wheat + "/" + heroResource.food.Wheat;
        }

        if (playerResource.food.Rice < heroResource.food.Rice)
        {
            riceText.text = playerResource.food.Rice + "/" + heroResource.food.Rice;
        }
        else
        {
            riceText.text = heroResource.food.Rice + "/" + heroResource.food.Rice;
        }

        if (playerResource.food.Corn < heroResource.food.Corn)
        {
            cornText.text = playerResource.food.Corn + "/" + heroResource.food.Corn;
        }
        else
        {
            cornText.text = heroResource.food.Corn + "/" + heroResource.food.Corn;
        }

        if (playerResource.food.Potatoes < heroResource.food.Potatoes)
        {
            potatoesText.text = playerResource.food.Potatoes + "/" + heroResource.food.Potatoes;
        }
        else
        {
            potatoesText.text = heroResource.food.Potatoes + "/" + heroResource.food.Potatoes;
        }

        if (heroResource.food.Wheat != 0)
        {
            WheatFiller.fillAmount = ClampRange.Clamp(0, heroResource.food.Wheat, 0, 1, playerResource.food.Wheat);
        }
        else
        {
            WheatFiller.fillAmount = 0;
        }

        if (heroResource.food.Rice != 0)
        {
            riceFiller.fillAmount = ClampRange.Clamp(0, heroResource.food.Rice, 0, 1, playerResource.food.Rice);
        }
        else
        {
            riceFiller.fillAmount = 0;
        }

        if (heroResource.food.Corn != 0)
        {
            cornFiller.fillAmount = ClampRange.Clamp(0, heroResource.food.Corn, 0, 1, playerResource.food.Corn);
        }
        else
        {
            cornFiller.fillAmount = 0;
        }

        if (heroResource.food.Potatoes != 0)
        {
            potatoesFiller.fillAmount = ClampRange.Clamp(0, heroResource.food.Potatoes, 0, 1, playerResource.food.Potatoes);
        }
        else
        {
            potatoesFiller.fillAmount = 0;
        }
    }

    private void SetHerbData()
    {
        if (playerResource.herbs.Sage < heroResource.herbs.Sage)
        {
            sageText.text = playerResource.herbs.Sage + "/" + heroResource.herbs.Sage;
        }
        else
        {
            sageText.text = heroResource.herbs.Sage + "/" + heroResource.herbs.Sage;
        }

        if (playerResource.herbs.Rosemary < heroResource.herbs.Rosemary)
        {
            rosemaryText.text = playerResource.herbs.Rosemary + "/" + heroResource.herbs.Rosemary;
        }
        else
        {
            rosemaryText.text = heroResource.herbs.Rosemary + "/" + heroResource.herbs.Rosemary;
        }

        if (playerResource.herbs.Chamomile < heroResource.herbs.Chamomile)
        {
            chamomileText.text = playerResource.herbs.Chamomile + "/" + heroResource.herbs.Chamomile;
        }
        else
        {
            chamomileText.text = heroResource.herbs.Chamomile + "/" + heroResource.herbs.Chamomile;
        }

        if (playerResource.herbs.Valerian < heroResource.herbs.Valerian)
        {
            valerianText.text = playerResource.herbs.Valerian + "/" + heroResource.herbs.Valerian;
        }
        else
        {
            valerianText.text = heroResource.herbs.Valerian + "/" + heroResource.herbs.Valerian;
        }

        if (heroResource.herbs.Sage != 0)
        {
            sageFiller.fillAmount = ClampRange.Clamp(0, heroResource.herbs.Sage, 0, 1, playerResource.herbs.Sage);
        }
        else
        {
            sageFiller.fillAmount = 0;
        }

        if (heroResource.herbs.Rosemary != 0)
        {
            rosemaryFiller.fillAmount = ClampRange.Clamp(0, heroResource.herbs.Rosemary, 0, 1, playerResource.herbs.Rosemary);
        }
        else
        {
            rosemaryFiller.fillAmount = 0;
        }

        if (heroResource.herbs.Chamomile != 0)
        {
            chamomileFiller.fillAmount = ClampRange.Clamp(0, heroResource.herbs.Chamomile, 0, 1, playerResource.herbs.Chamomile);
        }
        else
        {
            chamomileFiller.fillAmount = 0;
        }

        if (heroResource.herbs.Valerian != 0)
        {
            valerianFiller.fillAmount = ClampRange.Clamp(0, heroResource.herbs.Valerian, 0, 1, playerResource.herbs.Valerian);
        }
        else
        {
            valerianFiller.fillAmount = 0;
        }
    }

    public void NextCharacterResources()
    {
        _unitId++;
        if (_unitId > 16)
            _unitId = 1;
        ShowCharacterResources(_unitId);
    }

    public void LastCharacterResources()
    {
        _unitId--;
        if (_unitId < 1)
            _unitId = 16;
        ShowCharacterResources(_unitId);
    }

    private void DisplayTempleStats(Temple temple)
    {
        _unitId = 15;

        var templeMaxHealth = temple.baseHealth + 420;
        var incrementInTempleHealth = (30 * (temple.level + 1));
        var templeNextHealth = temple.health + incrementInTempleHealth;

        Health.fillAmount = ClampRange.Clamp(0, templeMaxHealth, 0, 1, temple.health);
        Damage.fillAmount = 0;
        Speed.fillAmount = 0;

        UpgradeButton.interactable = temple.level < 5;

        HealthExtend.fillAmount = ClampRange.Clamp(0, templeMaxHealth, 0, 1, templeNextHealth);
        DamageExtend.fillAmount = 0;

        DamageText.text = "0";
        SpeedText.text = "0";
        HealthText.text = temple.health + " + " + incrementInTempleHealth;
        
        if (temple.level == 5)
            HealthText.text = temple.health.ToString();
    }

    private void DisplayUltimateStats(Ultimate ultimate)
    {
        _unitId = 16;

        var MaxUltimateDamage = ultimate.baseDamage + 5;
        var currentUltimateDamage = ultimate.damage;
        var incrementInUltimateDamage = ultimate.level == 1 ? 2 : 1;
        var NextUltimateDamage = currentUltimateDamage + incrementInUltimateDamage;

        Health.fillAmount = 0;
        Damage.fillAmount = ClampRange.Clamp(0, MaxUltimateDamage, 0, 1, currentUltimateDamage);
        Speed.fillAmount = 0;

        UpgradeButton.interactable = ultimate.level < 5;
        HealthExtend.fillAmount = 0;
        DamageExtend.fillAmount = ClampRange.Clamp(0, MaxUltimateDamage, 0, 1, NextUltimateDamage);

        DamageText.text = currentUltimateDamage + " + " + incrementInUltimateDamage;
        SpeedText.text = "0";
        HealthText.text = "0";

        if (ultimate.level == 5)
            DamageText.text = currentUltimateDamage + "";
    }

    private void GetUnitsMaxStats(out float healthMax, out float damageMax, out float speedMax)
    {
        var healthMaxTemp = 0F;
        var damageMaxTemp = 0F;
        var speedMaxTemp = 0F;
        
        playerHeroesManager.ForEachUnitData(unitData =>
        {
            unitData.GetLevelStats(HeroUnitData.LevelMax, out var unitHealthMax, out var unitDamageMax);

            if (unitHealthMax > healthMaxTemp)
                healthMaxTemp = unitHealthMax;

            if (unitDamageMax > damageMaxTemp)
                damageMaxTemp = unitDamageMax;

            if (unitData.Speed > speedMaxTemp)
                speedMaxTemp = unitData.Speed;
        });
        
        healthMax = healthMaxTemp;
        damageMax = damageMaxTemp;
        speedMax = speedMaxTemp;
    }

    private void DisplayUnitStats(HeroUnitData unitData)
    {
        _unitId = unitData.id;

        CharacterPic.sprite = pictures[_unitId - 1];
        CharacterName.text = unitData.typeName;
        ChatacterDescription.text = unitData.description;
        
        Level.text = unitData.Level.ToString();

        GetUnitsMaxStats(out var healthMax, out var damageMax, out var speedMax);

        SpeedText.text = unitData.Speed.ToString("F1");
        Speed.fillAmount = ClampRange.Clamp(0, speedMax, 0, 1, unitData.Speed);
        
        UpgradeButton.interactable = !unitData.IsMaxLevel;
        
        if (unitData.IsLocked)
        {
            DamageText.text = unitData.DamageBase.ToString("F1");
            Damage.fillAmount = ClampRange.Clamp(0, damageMax, 0, 1, unitData.DamageBase);
            DamageExtend.fillAmount = 0f;
            
            HealthText.text = unitData.HealthBase.ToString("F1");
            Health.fillAmount = ClampRange.Clamp(0, healthMax, 0, 1, unitData.HealthBase);
            HealthExtend.fillAmount = 0f;
            return;
        }

        if (unitData.IsMaxLevel)
        {
            DamageText.text = unitData.Damage.ToString("F1");
            Damage.fillAmount = ClampRange.Clamp(0, damageMax, 0, 1, unitData.Damage);
            DamageExtend.fillAmount = 0F;
            
            HealthText.text = unitData.Health.ToString("F1");
            Health.fillAmount = ClampRange.Clamp(0, healthMax, 0, 1, unitData.Health);
            HealthExtend.fillAmount = 0F;
            return;
        }
        
        unitData.GetLevelStats(unitData.Level + 1, out var healthNext, out var damageNext);
        
        var healthIncrement = healthNext - unitData.Health;
        var damageIncrement = damageNext - unitData.Damage;
        
        DamageText.text = unitData.Damage.ToString("F1") + " + " + damageIncrement.ToString("F1");
        Damage.fillAmount = ClampRange.Clamp(0, damageMax, 0, 1, unitData.Damage);
        DamageExtend.fillAmount = ClampRange.Clamp(0, damageMax, 0, 1, damageNext);
            
        HealthText.text = unitData.Health.ToString("F1") + " + " + healthIncrement.ToString("F1");
        Health.fillAmount = ClampRange.Clamp(0, healthMax, 0, 1, unitData.Health);
        HealthExtend.fillAmount = ClampRange.Clamp(0, healthMax, 0, 1, healthNext);
    }

    public void ShowUltimateResources()
    {
        ultimateResource = gameManager.heroResourceRequirement.GetRequiredResourcesForUltimate(gameManager.playerHeroManager.ultimate.level);
        playerResource = gameManager.playerResources.PlayerResources;

        if (gameManager.playerHeroManager.ultimate.level > 0)
        {
            LockImage.SetActive(false);
            UpgradeButton.GetComponent<Image>().sprite = UpgradeSprite;
        }
        else
        {
            LockImage.SetActive(true);
            UpgradeButton.GetComponent<Image>().sprite = UnlockSprite;
        }

        CharacterPic.sprite = UltimatePicture;
        CharacterName.text = "Super Power";
        ChatacterDescription.text = "Ultimate Super Power";
        Level.text = "" + gameManager.playerHeroManager.ultimate.level;

        DisplayUltimateStats(gameManager.playerHeroManager.ultimate);

        heroResource = ultimateResource;

        SetWoodData();
        SetOreData();
        SetFoodData();
        SetHerbData();
    }

    public void ShowTempleResources()
    {
        templeResource = gameManager.heroResourceRequirement.GetRequiredResourcesForTemple(gameManager.playerHeroManager.temple.level);
        playerResource = gameManager.playerResources.PlayerResources;

        Debug.Log("@@@@ Temple Level -> " + gameManager.playerHeroManager.temple.level);

        if (gameManager.playerHeroManager.temple.level > 0)
        {
            LockImage.SetActive(false);
            UpgradeButton.GetComponent<Image>().sprite = UpgradeSprite;
        }
        else
        {
            LockImage.SetActive(true);
            UpgradeButton.GetComponent<Image>().sprite = UnlockSprite;
        }

        CharacterPic.sprite = TemplePicture;
        CharacterName.text = "Temple";
        ChatacterDescription.text = "Temple For Protection";
        Level.text = "" + gameManager.playerHeroManager.temple.level;

        DisplayTempleStats(gameManager.playerHeroManager.temple);

        heroResource = templeResource;

        SetWoodData();
        SetOreData();
        SetFoodData();
        SetHerbData();
    }

    public void ShowCharacterResources(int unitId)
    {
        if (unitId == 15)
        {
            ShowTempleResources();
            return;
        }

        if (unitId == 16)
        {
            ShowUltimateResources();
            return;
        }

        var unitData = playerHeroesManager.GetUnitData(unitId);
        heroResource = gameManager.heroResourceRequirement.GetUnitRequiredResources(unitId, unitData.Level);
        playerResource = gameManager.playerResources.PlayerResources;

        LockImage.SetActive(unitData.Level <= 0);
        UpgradeButton.GetComponent<Image>().sprite = unitData.Level > 0 ? UpgradeSprite : UnlockSprite;

        DisplayUnitStats(unitData);

        SetWoodData();
        SetOreData();
        SetFoodData();
        SetHerbData();
    }

    private void ShowInsufficientMessage()
    {
        var name = new List<string>();
        var amount = new List<int>();

        for (var i = 0; i < ResourcesAmount.Length; i++)
        {
            ResourcesName[i].gameObject.SetActive(false);
            ResourcesAmount[i].gameObject.SetActive(false);
        }

        if ((playerResource.wood.Stick - heroResource.wood.Stick) < 0)
        {
            name.Add("Stick");
            amount.Add(playerResource.wood.Stick - heroResource.wood.Stick);
        }
        else
        {
            name.Add("Stick");
            amount.Add(0);
        }

        if ((playerResource.wood.Lumber - heroResource.wood.Lumber) < 0)
        {
            name.Add("Lumber");
            amount.Add(playerResource.wood.Lumber - heroResource.wood.Lumber);
        }
        else
        {
            name.Add("Lumber");
            amount.Add(0);
        }

        if ((playerResource.wood.Ironwood - heroResource.wood.Ironwood) < 0)
        {
            name.Add("Ironwood");
            amount.Add(playerResource.wood.Ironwood - heroResource.wood.Ironwood);
        }
        else
        {
            name.Add("Ironwood");
            amount.Add(0);
        }

        if ((playerResource.wood.Bloodwood - heroResource.wood.Bloodwood) < 0)
        {
            name.Add("Bloodwood");
            amount.Add(playerResource.wood.Bloodwood - heroResource.wood.Bloodwood);
        }
        else
        {
            name.Add("Bloodwood");
            amount.Add(0);
        }

        if ((playerResource.ore.Copper - heroResource.ore.Copper) < 0)
        {
            name.Add("Copper");
            amount.Add(playerResource.ore.Copper - heroResource.ore.Copper);
        }
        else
        {
            name.Add("Copper");
            amount.Add(0);
        }

        if ((playerResource.ore.Silver - heroResource.ore.Silver) < 0)
        {
            name.Add("Silver");
            amount.Add(playerResource.ore.Silver - heroResource.ore.Silver);
        }
        else
        {
            name.Add("Silver");
            amount.Add(0);
        }

        if ((playerResource.ore.Gold - heroResource.ore.Gold) < 0)
        {
            name.Add("Gold");
            amount.Add(playerResource.ore.Gold - heroResource.ore.Gold);
        }
        else
        {
            name.Add("Gold");
            amount.Add(0);
        }

        if ((playerResource.ore.Platinum - heroResource.ore.Platinum) < 0)
        {
            name.Add("Platinum");
            amount.Add(playerResource.ore.Platinum - heroResource.ore.Platinum);
        }
        else
        {
            name.Add("Platinum");
            amount.Add(0);
        }

        if ((playerResource.food.Wheat - heroResource.food.Wheat) < 0)
        {
            name.Add("Wheat");
            amount.Add(playerResource.food.Wheat - heroResource.food.Wheat);
        }
        else
        {
            name.Add("Wheat");
            amount.Add(0);
        }

        if ((playerResource.food.Rice - heroResource.food.Rice) < 0)
        {
            name.Add("Rice");
            amount.Add(playerResource.food.Rice - heroResource.food.Rice);
        }
        else
        {
            name.Add("Rice");
            amount.Add(0);
        }

        if ((playerResource.food.Corn - heroResource.food.Corn) < 0)
        {
            name.Add("Corn");
            amount.Add(playerResource.food.Corn - heroResource.food.Corn);
        }
        else
        {
            name.Add("Corn");
            amount.Add(0);
        }

        if ((playerResource.food.Potatoes - heroResource.food.Potatoes) < 0)
        {
            name.Add("Potatoes");
            amount.Add(playerResource.food.Potatoes - heroResource.food.Potatoes);
        }
        else
        {
            name.Add("Potatoes");
            amount.Add(0);
        }

        if ((playerResource.herbs.Sage - heroResource.herbs.Sage) < 0)
        {
            name.Add("Sage");
            amount.Add(playerResource.herbs.Sage - heroResource.herbs.Sage);
        }
        else
        {
            name.Add("Sage");
            amount.Add(0);
        }

        if ((playerResource.herbs.Rosemary - heroResource.herbs.Rosemary) < 0)
        {
            name.Add("Rosemary");
            amount.Add(playerResource.herbs.Rosemary - heroResource.herbs.Rosemary);
        }
        else
        {
            name.Add("Rosemary");
            amount.Add(0);
        }

        if ((playerResource.herbs.Chamomile - heroResource.herbs.Chamomile) < 0)
        {
            name.Add("Chamomile");
            amount.Add(playerResource.herbs.Chamomile - heroResource.herbs.Chamomile);
        }
        else
        {
            name.Add("Chamomile");
            amount.Add(0);
        }

        if ((playerResource.herbs.Valerian - heroResource.herbs.Valerian) < 0)
        {
            name.Add("Valerian");
            amount.Add(playerResource.herbs.Valerian - heroResource.herbs.Valerian);
        }
        else
        {
            name.Add("Valerian");
            amount.Add(0);
        }


        for (int i = 0; i < ResourcesName.Length; i++)
        {
            if (i < amount.Count)
            {
                ResourcesAmount[i].transform.parent.gameObject.SetActive(true);
                ResourcesName[i].gameObject.SetActive(true);
                ResourcesAmount[i].gameObject.SetActive(true);
                ResourcesName[i].text = name[i];
                ResourcesAmount[i].text = amount[i] + "";
            }
            else
            {
                ResourcesName[i].gameObject.SetActive(false);
                ResourcesAmount[i].transform.parent.gameObject.SetActive(false);
                ResourcesName[i].text = "";
                ResourcesAmount[i].text = "";
            }
        }

        GetComponent<MainMenu>().ShowInsufficientMessage();
    }

    private void ShowTempleInsufficientMessage(HeroResources _resources)
    {
        List<string> name = new List<string>();
        List<int> amount = new List<int>();

        for (int i = 0; i < ResourcesAmount.Length; i++)
        {
            ResourcesName[i].gameObject.SetActive(false);
            ResourcesAmount[i].gameObject.SetActive(false);
        }

        if ((playerResource.wood.Stick - _resources.wood.Stick) < 0)
        {
            name.Add("Stick");
            amount.Add(playerResource.wood.Stick - _resources.wood.Stick);
        }
        else
        {
            name.Add("Stick");
            amount.Add(0);
        }

        if ((playerResource.wood.Lumber - _resources.wood.Lumber) < 0)
        {
            name.Add("Lumber");
            amount.Add(playerResource.wood.Lumber - _resources.wood.Lumber);
        }
        else
        {
            name.Add("Lumber");
            amount.Add(0);
        }

        if ((playerResource.wood.Ironwood - _resources.wood.Ironwood) < 0)
        {
            name.Add("Ironwood");
            amount.Add(playerResource.wood.Ironwood - _resources.wood.Ironwood);
        }
        else
        {
            name.Add("Ironwood");
            amount.Add(0);
        }

        if ((playerResource.wood.Bloodwood - _resources.wood.Bloodwood) < 0)
        {
            name.Add("Bloodwood");
            amount.Add(playerResource.wood.Bloodwood - _resources.wood.Bloodwood);
        }
        else
        {
            name.Add("Bloodwood");
            amount.Add(0);
        }

        if ((playerResource.ore.Copper - _resources.ore.Copper) < 0)
        {
            name.Add("Copper");
            amount.Add(playerResource.ore.Copper - _resources.ore.Copper);
        }
        else
        {
            name.Add("Copper");
            amount.Add(0);
        }

        if ((playerResource.ore.Silver - _resources.ore.Silver) < 0)
        {
            name.Add("Silver");
            amount.Add(playerResource.ore.Silver - _resources.ore.Silver);
        }
        else
        {
            name.Add("Silver");
            amount.Add(0);
        }

        if ((playerResource.ore.Gold - _resources.ore.Gold) < 0)
        {
            name.Add("Gold");
            amount.Add(playerResource.ore.Gold - _resources.ore.Gold);
        }
        else
        {
            name.Add("Gold");
            amount.Add(0);
        }

        if ((playerResource.ore.Platinum - _resources.ore.Platinum) < 0)
        {
            name.Add("Platinum");
            amount.Add(playerResource.ore.Platinum - _resources.ore.Platinum);
        }
        else
        {
            name.Add("Platinum");
            amount.Add(0);
        }

        if ((playerResource.food.Wheat - _resources.food.Wheat) < 0)
        {
            name.Add("Wheat");
            amount.Add(playerResource.food.Wheat - _resources.food.Wheat);
        }
        else
        {
            name.Add("Wheat");
            amount.Add(0);
        }

        if ((playerResource.food.Rice - _resources.food.Rice) < 0)
        {
            name.Add("Rice");
            amount.Add(playerResource.food.Rice - _resources.food.Rice);
        }
        else
        {
            name.Add("Rice");
            amount.Add(0);
        }

        if ((playerResource.food.Corn - _resources.food.Corn) < 0)
        {
            name.Add("Corn");
            amount.Add(playerResource.food.Corn - _resources.food.Corn);
        }
        else
        {
            name.Add("Corn");
            amount.Add(0);
        }

        if ((playerResource.food.Potatoes - _resources.food.Potatoes) < 0)
        {
            name.Add("Potatoes");
            amount.Add(playerResource.food.Potatoes - _resources.food.Potatoes);
        }
        else
        {
            name.Add("Potatoes");
            amount.Add(0);
        }

        if ((playerResource.herbs.Sage - _resources.herbs.Sage) < 0)
        {
            name.Add("Sage");
            amount.Add(playerResource.herbs.Sage - _resources.herbs.Sage);
        }
        else
        {
            name.Add("Sage");
            amount.Add(0);
        }

        if ((playerResource.herbs.Rosemary - _resources.herbs.Rosemary) < 0)
        {
            name.Add("Rosemary");
            amount.Add(playerResource.herbs.Rosemary - _resources.herbs.Rosemary);
        }
        else
        {
            name.Add("Rosemary");
            amount.Add(0);
        }

        if ((playerResource.herbs.Chamomile - _resources.herbs.Chamomile) < 0)
        {
            name.Add("Chamomile");
            amount.Add(playerResource.herbs.Chamomile - _resources.herbs.Chamomile);
        }
        else
        {
            name.Add("Chamomile");
            amount.Add(0);
        }

        if ((playerResource.herbs.Valerian - _resources.herbs.Valerian) < 0)
        {
            name.Add("Valerian");
            amount.Add(playerResource.herbs.Valerian - _resources.herbs.Valerian);
        }
        else
        {
            name.Add("Valerian");
            amount.Add(0);
        }


        for (int i = 0; i < ResourcesName.Length; i++)
        {
            if (i < amount.Count)
            {
                ResourcesAmount[i].transform.parent.gameObject.SetActive(true);
                ResourcesName[i].gameObject.SetActive(true);
                ResourcesAmount[i].gameObject.SetActive(true);
                ResourcesName[i].text = name[i];
                ResourcesAmount[i].text = amount[i] + "";
            }
            else
            {
                ResourcesName[i].gameObject.SetActive(false);
                ResourcesAmount[i].transform.parent.gameObject.SetActive(false);
                ResourcesName[i].text = "";
                ResourcesAmount[i].text = "";
            }
        }

        GetComponent<MainMenu>().ShowInsufficientMessage();
    }

    private void ShowMaxLevelMessage()
    {
    }

    private void ShowLevelLockedMessage()
    {
        templeResource = gameManager.heroResourceRequirement.GetRequiredResourcesForTemple(gameManager.playerHeroManager.temple.level);
    }

    public void UpgradeTemple()
    {
        ReturnObject returnObject = gameManager.UpgradeTemple(gameManager.playerHeroManager.temple);
        if (returnObject.success)
        {
            ShowTempleResources();
            //			CharacterResources(15);
        }
        else
        {
            if (returnObject.message == 1)
            {
                templeResource = gameManager.heroResourceRequirement.GetRequiredResourcesForTemple(gameManager.playerHeroManager.temple.level);
                ShowTempleInsufficientMessage(templeResource);
            }
            else if (returnObject.message == 3)
            {
                ShowLevelLockedMessage();
            }
            else
            {
                ShowMaxLevelMessage();
            }
        }
    }

    public void UpgradeUltimatePower()
    {
        var returnObject = gameManager.UpgradeUltimate(gameManager.playerHeroManager.ultimate);
        if (returnObject.success)
        {
            ShowUltimateResources();
            superPowerController.UpdateLevel(gameManager.playerHeroManager.ultimate.level);
        }
        else
        {
            if (returnObject.message == 1)
            {
                HeroResources ultimateResources = gameManager.heroResourceRequirement.GetRequiredResourcesForUltimate(gameManager.playerHeroManager.ultimate.level);
                ShowTempleInsufficientMessage(ultimateResources);
            }
            else if (returnObject.message == 3)
            {
                ShowLevelLockedMessage();
            }
            else
            {
                ShowMaxLevelMessage();
            }
        }
    }

    public void UpgradeCharacter()
    {
        if (_unitId == 15)
        {
            UpgradeTemple();
            return;
        }

        if (_unitId == 16)
        {
            UpgradeUltimatePower();
            return;
        }

        var data = playerHeroesManager.GetUnitData(_unitId);

        var returnObject = gameManager.UpgradeHero(data, _unitId);
        if (returnObject.success)
        {
            ShowCharacterResources(_unitId);
            return;
        }

        if (returnObject.message == 1)
        {
            ShowInsufficientMessage();
        }
        else if (returnObject.message == 3)
        {
            ShowLevelLockedMessage();
        }
        else
        {
            ShowMaxLevelMessage();
        }
    }
    
    /*
     * Event Handlers.
     */
    
    private void OnPlayerResourcesChanged()
    {
        _log.Debug("OnPlayerResourcesChanged");
        
        if (playerResource == null)
            return;
        
        SetWoodData();
        SetOreData();
        SetFoodData();
        SetHerbData();
    }
}