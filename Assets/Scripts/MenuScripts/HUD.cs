using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public MainMenu mainMenu;

    public GameObject GameOverScreen;
    public GameObject GameOverVictory;
    public GameObject GameOverDefeat;
    public Text       VictoryText;

    public void PlayNextLevel()
    {
        var currentLevel = CentralVariables.SELECTED_LEVEL;
        var currentDifficulty = CentralVariables.SELECTED_DIFFICULTY;
        if (currentDifficulty < 4)
        {
            CentralVariables.SELECTED_DIFFICULTY = currentDifficulty + 1;
        }
        else
        {
            CentralVariables.SELECTED_LEVEL = currentLevel + 1;
            CentralVariables.SELECTED_DIFFICULTY = 1;
        }

        GameOverVictory.SetActive(false);
        GameOverDefeat.SetActive(false);
        GameOverScreen.SetActive(false);
        mainMenu.PlayNextLevel();
    }

    public void ShowGameOverScreen()
    {
        GameOverScreen.SetActive(true);

        if (CentralVariables.IS_PLAYER_WIN)
        {
            VictoryText.text = "You Win";
            GameOverVictory.SetActive(true);

            var currentDifficulty = CentralVariables.SELECTED_DIFFICULTY;
            var currentLevel = CentralVariables.SELECTED_LEVEL;
            if (currentLevel == 12)
            {
                if (currentDifficulty == 4)
                    GameOverVictory.transform.Find("Button_Next").gameObject.SetActive(false);
                else
                    GameOverVictory.transform.Find("Button_Next").gameObject.SetActive(true);
            }
            else
            {
                GameOverVictory.transform.Find("Button_Next").gameObject.SetActive(true);
            }
        }
        else
        {
            VictoryText.text = "You Lost";
            GameOverDefeat.SetActive(true);
        }
    }
}