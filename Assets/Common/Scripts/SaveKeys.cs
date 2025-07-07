using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveKeys {
    public const string MusicVolume = "MusicVolume";
    public const string SfxVolume = "SfxVolume";
    public const string VoiceVolume = "VoiceVolume";

    // -- DEBUG/EDITOR --
    public const string Editor_IsPrefabBeingEdited = "Editor_IsPrefabBeingEdited";

    // Cleaning Game
    public const string LastPlayedLevelIndex = "LastPlayedLevelIndex";
    public const string Wallet_NumCoins = "Wallet_NumCoins";
    public static string LevelPercentClean(string levelName) { return "LevelPercentClean_" + levelName; }

    public static string Debug_DoSkipPrePlaying = "Debug_DoSkipPrePlaying";
}
