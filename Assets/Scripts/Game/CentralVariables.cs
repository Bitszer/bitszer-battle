using UnityEngine;
using System.Collections.Generic;

public static class CentralVariables
{
    public const string AuctionHousePoolId = "us-west-2:b9446e7c-86ab-4317-8a29-2b38366d89ba";
    
    public static bool IS_SFX_ON      = true;
    public static bool IS_BG_MUSIC_ON = true;

    public static int SELECTED_LEVEL       = 1; // Level Selected out of 12 Levels.
    public static int SELECTED_DIFFICULTY  = 1; // 1 = Novice  2 = Casual  3 = Medium  4 = Expert.
    public static int SELECTED_ENVIRONMENT = 1; // 1 = Day Environment ---- 2 = Night Environment.

    public static bool IS_PLAYER_WIN = false;
    public static int  EnemyMax      = 15;
    public static int  PlayerMax     = 15;

    public static bool IS_GAME_END     = false;
    public static bool IS_GAME_STOPPED = false;
    public static bool IS_GAME_STARTED = false;

    public static readonly List<HeroController> EnemyUnitInstances  = new List<HeroController>();
    public static readonly List<HeroController> PlayerUnitsInstances = new List<HeroController>();

    public static bool IsFirstTime()
    {
        if (PlayerPrefs.HasKey("is_first_time"))
            return false;
        PlayerPrefs.SetInt("is_first_time", 1);
        return true;
    }
}