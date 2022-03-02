using UnityEditor;
using UnityEngine;

namespace Editor
{
    public sealed class PlayerPrefsMenu
    {
        [MenuItem("Bitszer/Set Max Level & Difficulty")]
        public static void SetMaxLevel()
        {
            PlayerPrefs.SetInt("level", 12);
            
            for (var i = 0; i <= 12; i++)
                PlayerPrefs.SetInt("difficulty[" + i + "]", 4);
        }

        [MenuItem("Bitszer/Reset Local Resources")]
        public static void ResetLocalResources()
        {
            PlayerPrefs.DeleteKey("player_resources");
        }

        [MenuItem("Bitszer/Reset Prefs")]
        public static void ResetSettings()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}