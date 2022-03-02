using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Image[]  LevelButtons      = null;
    [SerializeField] private Button[] DifficultyButtons = null;
    [SerializeField] private Image[]  LevelProgress     = null;

    private int _level;
    private int _difficulty;

    /*
     * Mono Behavior.
     */

    public void Start()
    {
        if (!PlayerPrefs.HasKey("levelPrefs"))
        {
            SaveLevelsToPrefs(1);
            SaveDifficultyToPrefs(1, 1);
        }

        SetLevels();
        SetLevelProgressBar();
    }

    public void Unlock(int levelPlayed, int difficultyPlayed)
    {
        if (levelPlayed < 12)
        {
            if (difficultyPlayed == 1)
            {
                SaveDifficultyToPrefs(levelPlayed, difficultyPlayed + 1);
                SaveLevelsToPrefs(levelPlayed + 1);
                SaveDifficultyToPrefs(levelPlayed + 1, difficultyPlayed);
            }
            else
            {
                if (difficultyPlayed < 4)
                {
                    SaveDifficultyToPrefs(levelPlayed, difficultyPlayed + 1);
                }
            }
        }
        else
        {
            if (difficultyPlayed < 4)
                SaveDifficultyToPrefs(levelPlayed, difficultyPlayed + 1);
        }

        SetLevels();
    }

    public void SetLevels()
    {
        _level = GetLevelsFromPrefs();
        for (var i = 1; i <= _level; i++)
        {
            _difficulty = GetDifficultyFromPrefs(i);
            LevelButtons[i - 1].GetComponent<Button>().interactable = true;
            LevelButtons[i - 1].color = new Color(1, 1, 1, 0);
        }
    }

    public void SetLevelProgressBar()
    {
        _level = GetLevelsFromPrefs();
        for (var i = 1; i <= _level; i++)
        {
            _difficulty = GetDifficultyFromPrefs(i);
            LevelProgress[i - 1].fillAmount = ClampRange.Clamp(1, 4, 0, 1, _difficulty);
        }
    }

    public void SetDifficultyButton(int level)
    {
        for (var i = 0; i < 4; i++)
            DifficultyButtons[i].interactable = i < GetDifficultyFromPrefs(level);
    }

    /*
     * Player Prefs.
     */

    private void SaveLevelsToPrefs(int level)
    {
        if (level > GetLevelsFromPrefs())
            PlayerPrefs.SetInt("level", level);
    }

    private void SaveDifficultyToPrefs(int level, int difficulty)
    {
        if (difficulty > GetDifficultyFromPrefs(level))
            PlayerPrefs.SetInt("difficulty[" + level + "]", difficulty);
    }

    private int GetLevelsFromPrefs()
    {
        return PlayerPrefs.GetInt("level");
    }

    private int GetDifficultyFromPrefs(int level)
    {
        return PlayerPrefs.GetInt("difficulty[" + level + "]");
    }
}