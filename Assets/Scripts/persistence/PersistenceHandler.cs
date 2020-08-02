using debug;
using UnityEngine;

// ReSharper disable once CheckNamespace
public static class PersistenceHandler
{
    private const string KEY_VOLUME_BASE = "basevolue";
    private const string KEY_VOLUME_MUSIC = "musicvolue";

    public static void resetGameProgress()
    {
        TODO.asLogWarning("Reset Game Not Implemented");
    }

    public static void startGame()
    {
        TODO.asLogWarning("Start Game Not Implemented");
    }

    public static bool hasActiveGame()
    {
        TODO.asLogWarning("Logic for acquiring active game not implemented");
        return !true;
    }

    public static void continueGame()
    {
        TODO.asLogWarning("Continue Game Not Implemented");
    }

    public static float getBaseVolume(float defaultVolume) => PlayerPrefs.HasKey(KEY_VOLUME_BASE)
        ? PlayerPrefs.GetFloat(KEY_VOLUME_BASE)
        : updateBaseVolume(defaultVolume);

    public static float updateBaseVolume(float volume)
    {
        PlayerPrefs.SetFloat(KEY_VOLUME_BASE, volume);
        PlayerPrefs.Save();
        return volume;
    }

    public static float getMusicVolume(float defaultVolume) => PlayerPrefs.HasKey(KEY_VOLUME_MUSIC)
        ? PlayerPrefs.GetFloat(KEY_VOLUME_MUSIC)
        : updateMusicVolume(defaultVolume);
    
    public static float updateMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat(KEY_VOLUME_MUSIC, volume);
        PlayerPrefs.Save();
        return volume;
    }
}