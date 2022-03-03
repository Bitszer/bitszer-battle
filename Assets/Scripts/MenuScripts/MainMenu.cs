using UnityEngine;
using UnityEngine.UI;
using Utility.Logging;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
	private readonly ILog _log = LogManager.GetLogger<MainMenu>();
	
	// Stack to Navigate between UI Panels/Screen.
	private Stack<GameObject> UIstack;

	[Header("Dependencies")]
	[SerializeField] private GameManager gameManager = null;
	[SerializeField] private Levels levels = null;
	
	// Menu Screens
	[Header("MENU SCREENS")]
	public GameObject MainMenuScreen;
	public GameObject environmentSelectionScreen;
	public GameObject characterUpgardeScreen;
	public GameObject HUDScreen;
	public GameObject GameOverScreen;
	public GameObject GameOverVictory;
	public GameObject GameOverDefeat;
	public GameObject LevelSelectionScreen;
	public GameObject DifficultySelectionScreen;
	public GameObject PlayerResourcesScreen;
	public GameObject CharacterResourcesScreen;
	public GameObject EachHeroResourceScreen;
	public GameObject InsufficientScreen;
	public GameObject PauseScreen;
	public GameObject SettingScreen;
	public PlayerResourcesMenu PlayerResourcesMenu;
	//	public Dropdown LevelSelectionDropdown;
	//	public Dropdown difficultySelectionDropdown;

	public Button militiaButton;
	public Button warderButton;
	public Button infantryButton;
	public Button hunterButton;
	public Button cavalryButton;
	public Button wizardButton;
	public Button catapultButton;
	public Button dinosaurButton;
	public Button dragonHawkButton;
	public Button hippogryphyButton;
	public Button ardlizButton;
	public Button dragonSlayerButton;
	public Button titanButton;
	public Button beastLordButton;

	public GameObject SoundToggle;
	public GameObject AccountToggle;

	public Transform SoundPos;
	public Transform AccontPos;

	private Vector3 soundInitPos = new Vector3(0, 0, 0);
	private Vector3 accountInitPos = new Vector3(0, 0, 0);

	private bool IsSoundOff 
	{
		get => PlayerPrefs.GetInt("isSoundOff", 0) == 1;
		set => PlayerPrefs.SetInt("isSoundOff", value ? 1 : 0);
	}
	
	private bool isLoggedIn = false;

	private void Start()
	{
		if (gameManager == null)
			_log.Error("GameManager dependency not set.");
		
		if (levels == null)
			_log.Error("Levels dependency not set.");
		
		if (IsSoundOff) {
			CentralVariables.IS_SFX_ON = false;
			CentralVariables.IS_BG_MUSIC_ON = false;
			SoundManager.Instance.PlayBgMusic (false, SoundManager.BackGroundState.MainMenu);
		} else {
			CentralVariables.IS_SFX_ON = true;
			CentralVariables.IS_BG_MUSIC_ON = true;
			SoundManager.Instance.PlayBgMusic (true, SoundManager.BackGroundState.MainMenu);
		}
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		accountInitPos = AccountToggle.transform.position;
		soundInitPos = SoundToggle.transform.position;

		UIstack = new Stack<GameObject> ();
		PushInScreen (MainMenuScreen);
	}

	public void Back()
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		PopOutScreen ();
	}

	public void PauseGame()
	{
		
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		PushInScreen (PauseScreen);
		Time.timeScale = 0;
	}

	public void ResumeGame()
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		PopOutScreen ();
		Time.timeScale = 1;
	}

	// Calls When Select Level button Pressed
	public void SelectEnvironmentButtonClicked(int _val)
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		CentralVariables.SELECTED_ENVIRONMENT = _val;
	}

	public void OpenDifficultySelectionScreen(int _val)
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		CentralVariables.SELECTED_LEVEL = _val;
		levels.SetDifficultyButton (_val);
		PushInScreen (DifficultySelectionScreen);
	}

	public void OpenLevelSelectionScreen()
	{
		//GAManager.Instance.LogDesignEvent ("- Level Selection Screen -");

		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		levels.SetLevelProgressBar ();
		PushInScreen (LevelSelectionScreen);
	}

	public void SelectDifficulty(int _val)
	{
		CentralVariables.SELECTED_DIFFICULTY = _val;
		GoToGame ();
	}

	public void OpenPlayerResourcesScreen()
	{
		SoundManager.Instance.PlayOneShot(SoundManager.SoundState.Click, 1);
		PushInScreen(PlayerResourcesScreen);
		PlayerResourcesMenu.Refresh();
	}

	public void ShowInsufficientMessage()
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		PushInScreen (InsufficientScreen);
	}

	public void OpenCharacterResourcesScreen()
	{
		SoundManager.Instance.PlayOneShot(SoundManager.SoundState.Click, 1);
		gameManager.SetLevelProgression();
		PushInScreen(CharacterResourcesScreen);
	}

	public void OpenTempleResources()
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		PushInScreen (EachHeroResourceScreen);
		GetComponent<CharResourcesMenu>().ShowTempleResources();
	}

	public void OpenSelectedCharacterResources(int unitId)
	{
		SoundManager.Instance.PlayOneShot(SoundManager.SoundState.Click, 1);
		PushInScreen(EachHeroResourceScreen);
		GetComponent<CharResourcesMenu>().ShowCharacterResources(unitId);
	}

//	public void OpenTempleResources()
//	{
//		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
//		PushInScreen (EachHeroResourceScreen);
//		this.GetComponent<CharResourcesMenu> ().TempleResources ();
//	}

	public void OpenUltimateResources()
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		PushInScreen (EachHeroResourceScreen);
		this.GetComponent<CharResourcesMenu> ().ShowUltimateResources ();
	}

	public void PlayNextLevel()
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		gameManager.StopGame ();
		GoToGame ();
	}

	// Starts Game.
	// Initialize all the components for the game play.
	public void GoToGame()
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		EmptyStack ();
		PushInScreen (HUDScreen);
		gameManager.StartGame ();
	}

	// Restart Game From in the game play.
	public void RestartGame()
	{
		
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		GameOverScreen.SetActive (false);
		GameOverVictory.SetActive (false);
		GameOverDefeat.SetActive (false);
		Time.timeScale = 1;
		gameManager.StopGame ();
		GoToGame ();
	}

	// This Method open the Character upgrade screen from the gameover screen.
	public void GoToUpgradeFromGameEnd()
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		GoToMainMenu ();
		OpenCharacterResourcesScreen ();
	}

	// Go To Main Menu screen from the game play.
	// Stops the game. Reset all the components in the game.
	public void GoToMainMenu()
	{
		
		// Play BG Track For main menu.
		SoundManager.Instance.AdjustVolume(1);
		SoundManager.Instance.PlayBgMusic (true, SoundManager.BackGroundState.MainMenu);
		SoundManager.Instance.musicFader (4, SoundManager.Fade.In);

		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		gameManager.StopGame ();
		PopOutScreen ();
		PushInScreen (MainMenuScreen);
		GameOverScreen.SetActive (false);
		GameOverVictory.SetActive (false);
		GameOverDefeat.SetActive (false);
		Time.timeScale = 1;
	}

	public void GoToAuctionHouse()
	{
		SoundManager.Instance.PlayOneShot(SoundManager.SoundState.Click, 1);
		PopOutScreen();
		//AuctionHouse.Open();
	}

	// When Auction House Button Clicked From Main Menu Screen.
	// It sends the updated Resources of the Plyaer to Auction House.
	public void GoToAuctionHouseMainMenu()
	{
		SoundManager.Instance.PlayOneShot(SoundManager.SoundState.Click, 1);
		SceneManager.LoadScene(1);
		Screen.orientation = ScreenOrientation.Portrait;
		//AuctionHouse.Open();
	}
	
	public void OpenSettingsScreen()
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		PushInScreen(SettingScreen);
		CheckSound();
	}

	public void SoundOnOff()
	{
		if (!IsSoundOff) {
			IsSoundOff = true;

			LeanTween.move (SoundToggle, SoundPos.position, 0.1f);
			SoundToggle.GetComponentInChildren<Text> ().text = "OFF";
			CentralVariables.IS_SFX_ON = false;
			CentralVariables.IS_BG_MUSIC_ON = false;
			SoundManager.Instance.PlayBgMusic (false, SoundManager.BackGroundState.MainMenu);
		} else {
			IsSoundOff = false;

			LeanTween.move (SoundToggle, soundInitPos, 0.1f);
			SoundToggle.GetComponentInChildren<Text> ().text = "ON";

			CentralVariables.IS_SFX_ON = true;
			CentralVariables.IS_BG_MUSIC_ON = true;
			SoundManager.Instance.PlayBgMusic (true, SoundManager.BackGroundState.MainMenu);
		}
	}
	void CheckSound()
	{
		if (IsSoundOff) {
			

			LeanTween.move (SoundToggle, SoundPos.position, 0.1f);
			SoundToggle.GetComponentInChildren<Text> ().text = "OFF";
			CentralVariables.IS_SFX_ON = false;
			CentralVariables.IS_BG_MUSIC_ON = false;
			SoundManager.Instance.PlayBgMusic (false, SoundManager.BackGroundState.MainMenu);
		} else {
			

			LeanTween.move (SoundToggle, soundInitPos, 0.1f);
			SoundToggle.GetComponentInChildren<Text> ().text = "ON";

			CentralVariables.IS_SFX_ON = true;
			CentralVariables.IS_BG_MUSIC_ON = true;
			SoundManager.Instance.PlayBgMusic (true, SoundManager.BackGroundState.MainMenu);
		}
	}
	public void LogInOutToggle()
	{
		if (isLoggedIn) {
			isLoggedIn = false;
			LeanTween.move (AccountToggle, AccontPos.position, 0.1f);
		} else {
			isLoggedIn = true;
			LeanTween.move (AccountToggle, accountInitPos, 0.1f);
		}
	}

	public void CloseApplication()
	{
		SoundManager.Instance.PlayOneShot (SoundManager.SoundState.Click, 1);
		SceneManager.LoadScene("GamePlay");
	}

	// Make stack for UI Flow through out the game.
	// ****************** Stack Management Part *********************
	private void PushInScreen(GameObject _screen)
	{
		UIstack.Push (_screen);
		UIstack.Peek ().SetActive (true);
	}

	private void PopOutScreen()
	{
		if (UIstack.Count > 0) {
			UIstack.Peek ().SetActive (false);
			UIstack.Pop ();
			if(UIstack.Count > 0)
				UIstack.Peek ().SetActive (true);
		}

	}

	private void EmptyStack()
	{
		int count = UIstack.Count;
		for (int i = 0; i < count; i++) 
		{
			UIstack.Peek ().SetActive (false);
			UIstack.Pop ();
		}
	}
	// *****************************************************************
}
