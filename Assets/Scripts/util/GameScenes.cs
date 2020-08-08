// ReSharper disable once CheckNamespace

using System.Collections.Generic;

public static class GameScenes
{
    public const string MAIN_MENU = "MainMenu";
    public const string SANDBOX = "Sandbox";
    public const string LEVEL_01 = "Level_01";
    public const string LEVEL_02 = "Level_02";
    public const string LEVEL_03 = "Level_03";
    public const string LEVEL_04 = "Level_04";
    public const string LEVEL_05 = "Level_07";

    public static readonly List<string> LEVELS = new List<string>
        {LEVEL_01, LEVEL_02, LEVEL_03, LEVEL_04, LEVEL_05};
}