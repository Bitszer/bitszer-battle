using UnityEngine;
using UnityEngine.UI;
using Utility.Logging;

public sealed class PlayerResourcesMenu : MonoBehaviour 
{
	private readonly ILog _log = LogManager.GetLogger<PlayerResourcesMenu>();
	
	public GameManager gameManager;
	
	[Space]
	public Sprite[] pictures;
	public Image CharacterPic;
	public Text CharacterName;
	public Text Description;
	[Space]
	public Text stickText;
	public Image stickFiller;
	public Text lumberText;
	public Image lumberFiller;
	public Text ironText;
	public Image ironFiller;
	public Text bloodText;
	public Image bloodFiller;
	[Space]
	public Text copperText;
	public Image copperFiller;
	public Text silverText;
	public Image silverFiller;
	public Text goldText;
	public Image goldFiller;
	public Text platinumText;
	public Image platinumFiller;
	[Space]
	public Text wheatText;
	public Image WheatFiller;
	public Text riceText;
	public Image riceFiller;
	public Text cornText;
	public Image cornFiller;
	public Text potatoesText;
	public Image potatoesFiller;
	[Space]
	public Text sageText;
	public Image sageFiller;
	public Text rosemaryText;
	public Image rosemaryFiller;
	public Text chamomileText;
	public Image chamomileFiller;
	public Text valerianText;
	public Image valerianFiller;

	HeroResources heroResource;
	HeroResources playerResource;
	
	/*
	 * Mono Behavior.
	 */

	private void Awake()
	{
		gameManager.playerResources.OnPlayerResourcesChanged += OnPlayerResourcesChanged;
	}

	/*
	 * Public.
	 */

	public void Refresh()
	{
		UpgradeCharacterResources();
	}

	public void UpgradeCharacterResources()
	{
		playerResource = gameManager.playerResources.PlayerResources;
		CharacterName.text = "My Assets";

		SetWoodData ();
		SetOreData ();
		SetFoodData ();
		SetHerbData ();
	}

	private void SetWoodData()
	{
		stickText.text = playerResource.wood.Stick.ToString();
		lumberText.text = playerResource.wood.Lumber.ToString();
		ironText.text = playerResource.wood.Ironwood.ToString();
		bloodText.text = playerResource.wood.Bloodwood.ToString();

		stickFiller.fillAmount = ClampRange.Clamp (0, playerResource.wood.Stick + ((playerResource.wood.Stick * 10) / 100), 0, 1, playerResource.wood.Stick);
		lumberFiller.fillAmount = ClampRange.Clamp (0, playerResource.wood.Lumber + ((playerResource.wood.Lumber * 10) / 100), 0, 1, playerResource.wood.Lumber);
		ironFiller.fillAmount = ClampRange.Clamp (0, playerResource.wood.Ironwood + ((playerResource.wood.Ironwood * 10) / 100), 0, 1, playerResource.wood.Ironwood);
		bloodFiller.fillAmount = ClampRange.Clamp (0, playerResource.wood.Bloodwood + ((playerResource.wood.Bloodwood * 10) / 100), 0, 1, playerResource.wood.Bloodwood);
	}

	private void SetOreData()
	{
		copperText.text = playerResource.ore.Copper.ToString();
		silverText.text = playerResource.ore.Silver.ToString();
		goldText.text = playerResource.ore.Gold.ToString();
		platinumText.text = playerResource.ore.Platinum.ToString();

		copperFiller.fillAmount = ClampRange.Clamp (0, playerResource.ore.Copper + ((playerResource.ore.Copper * 10) / 100), 0, 1, playerResource.ore.Copper);
		silverFiller.fillAmount = ClampRange.Clamp (0, playerResource.ore.Silver + ((playerResource.ore.Silver * 10) / 100), 0, 1, playerResource.ore.Silver);
		goldFiller.fillAmount = ClampRange.Clamp (0, playerResource.ore.Gold + ((playerResource.ore.Gold * 10) / 100), 0, 1, playerResource.ore.Gold);
		platinumFiller.fillAmount = ClampRange.Clamp (0, playerResource.ore.Platinum + ((playerResource.ore.Platinum * 10) / 100), 0, 1, playerResource.ore.Platinum);
	}

	private void SetFoodData()
	{
		wheatText.text = playerResource.food.Wheat.ToString();
		riceText.text = playerResource.food.Rice.ToString();
		cornText.text = playerResource.food.Corn.ToString();
		potatoesText.text = playerResource.food.Potatoes.ToString();

		WheatFiller.fillAmount = ClampRange.Clamp (0, playerResource.food.Wheat + ((playerResource.food.Wheat * 10) / 100), 0, 1, playerResource.food.Wheat);
		riceFiller.fillAmount = ClampRange.Clamp (0, playerResource.food.Rice + ((playerResource.food.Rice * 10) / 100), 0, 1, playerResource.food.Rice);
		cornFiller.fillAmount = ClampRange.Clamp (0, playerResource.food.Corn + ((playerResource.food.Corn * 10) / 100), 0, 1, playerResource.food.Corn);
		potatoesFiller.fillAmount = ClampRange.Clamp (0, playerResource.food.Potatoes + ((playerResource.food.Potatoes * 10) / 100), 0, 1, playerResource.food.Potatoes);
	}

	private void SetHerbData()
	{
		sageText.text = playerResource.herbs.Sage.ToString();
		rosemaryText.text = playerResource.herbs.Rosemary.ToString();
		chamomileText.text = playerResource.herbs.Chamomile.ToString();
		valerianText.text = playerResource.herbs.Valerian.ToString();

		sageFiller.fillAmount = ClampRange.Clamp (0, playerResource.herbs.Sage + ((playerResource.herbs.Sage * 10) / 100), 0, 1, playerResource.herbs.Sage);
		rosemaryFiller.fillAmount = ClampRange.Clamp (0, playerResource.herbs.Rosemary + ((playerResource.herbs.Rosemary * 10) / 100), 0, 1, playerResource.herbs.Rosemary);
		chamomileFiller.fillAmount = ClampRange.Clamp (0, playerResource.herbs.Chamomile + ((playerResource.herbs.Chamomile * 10) / 100), 0, 1, playerResource.herbs.Chamomile);
		valerianFiller.fillAmount = ClampRange.Clamp (0, playerResource.herbs.Valerian + ((playerResource.herbs.Valerian * 10) / 100), 0, 1, playerResource.herbs.Valerian);
	}
	
	/*
	 * Event Handlers.
	 */

	private void OnPlayerResourcesChanged()
	{
		_log.Debug("OnPlayerResourcesChanged");
		
		UpgradeCharacterResources();
	}
}
