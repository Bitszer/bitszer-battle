using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public HUD    hud;
    public Levels levels;

    private PlayerResourceManager playerResourcesManager;

    public void PlayGameOverAnimation()
    {
        var resourcesGameObject = GameObject.Find("ResourceManager");
        playerResourcesManager = resourcesGameObject.GetComponent<PlayerResourceManager>();

        StartCoroutine(GameOverDelay());
    }

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(3);

        hud.ShowGameOverScreen();
        SetAchievedResources();
    }

    void SetAchievedResources()
    {
        if (!CentralVariables.IS_PLAYER_WIN)
        {
            // Process game over flow here.
            return;
        }
        
        if (!PlayerPrefs.HasKey("levelPrefs"))
            PlayerPrefs.SetInt("levelPrefs", 1);

        playerResourcesManager.AddResourcesOnWin();
        levels.Unlock(CentralVariables.SELECTED_LEVEL, CentralVariables.SELECTED_DIFFICULTY);
    }
}