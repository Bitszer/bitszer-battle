using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourcesWon : MonoBehaviour 
{
	public GameManager gameManager;

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

	void Start()
	{
	}

	private void SetWoodData(HeroResources _rewardedResources)
	{
		stickText.text = _rewardedResources.wood.Stick.ToString();
		lumberText.text = _rewardedResources.wood.Lumber.ToString();
		ironText.text = _rewardedResources.wood.Ironwood.ToString();
		bloodText.text = _rewardedResources.wood.Bloodwood.ToString();

		stickFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.wood.Stick, 0, 1, _rewardedResources.wood.Stick);
		lumberFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.wood.Lumber, 0, 1, _rewardedResources.wood.Lumber);
		ironFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.wood.Ironwood, 0, 1, _rewardedResources.wood.Ironwood);
		bloodFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.wood.Bloodwood, 0, 1, _rewardedResources.wood.Stick);
	}

	private void SetOreData(HeroResources _rewardedResources)
	{
		copperText.text = _rewardedResources.ore.Copper.ToString();
		silverText.text = _rewardedResources.ore.Silver.ToString();
		goldText.text = _rewardedResources.ore.Gold.ToString();
		platinumText.text = _rewardedResources.ore.Platinum.ToString();

		copperFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.ore.Copper, 0, 1, _rewardedResources.ore.Copper);
		silverFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.ore.Silver, 0, 1, _rewardedResources.ore.Silver);
		goldFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.ore.Gold, 0, 1, _rewardedResources.ore.Gold);
		platinumFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.ore.Platinum, 0, 1, _rewardedResources.ore.Platinum);
	}

	private void SetFoodData(HeroResources _rewardedResources)
	{
		wheatText.text = _rewardedResources.food.Wheat.ToString();
		riceText.text = _rewardedResources.food.Rice.ToString();
		cornText.text = _rewardedResources.food.Corn.ToString();
		potatoesText.text = _rewardedResources.food.Potatoes.ToString();

		WheatFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.food.Wheat, 0, 1, _rewardedResources.food.Wheat);
		riceFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.food.Rice , 0, 1, _rewardedResources.food.Rice);
		cornFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.food.Corn, 0, 1, _rewardedResources.food.Corn);
		potatoesFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.food.Potatoes, 0, 1, _rewardedResources.food.Potatoes);
	}

	private void SetHerbData(HeroResources _rewardedResources)
	{
		sageText.text = _rewardedResources.herbs.Sage.ToString();
		rosemaryText.text = _rewardedResources.herbs.Rosemary.ToString();
		chamomileText.text = _rewardedResources.herbs.Chamomile.ToString();
		valerianText.text = _rewardedResources.herbs.Valerian.ToString();

		sageFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.herbs.Sage, 0, 1, _rewardedResources.herbs.Sage);
		rosemaryFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.herbs.Rosemary, 0, 1, _rewardedResources.herbs.Rosemary);
		chamomileFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.herbs.Chamomile, 0, 1, _rewardedResources.herbs.Chamomile);
		valerianFiller.fillAmount = ClampRange.Clamp (0, _rewardedResources.herbs.Valerian, 0, 1, _rewardedResources.herbs.Valerian);
	}

	public void DisplayWonResources(HeroResources _rewardedResources)
	{
		SetWoodData (_rewardedResources);
		SetOreData (_rewardedResources);
		SetFoodData (_rewardedResources);
		SetHerbData (_rewardedResources);
	}
}
