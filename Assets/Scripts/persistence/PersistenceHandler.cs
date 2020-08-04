using debug;
using UnityEngine;

// ReSharper disable once CheckNamespace
public static class PersistenceHandler
{
    private const string KEY_VOLUME_BASE = "basevolue";
    private const string KEY_VOLUME_MUSIC = "musicvolue";
    private const string KEY_VOLUME_MUTED = "volmuted";
    
    private const string KEY_LEVEL_CLEARED = "levelcleared";

    public static void resetGameProgress() => saveClearedLevel(-1);
    public static void saveClearedLevel(int level) => PlayerPrefs.SetInt(KEY_LEVEL_CLEARED, level);

    public static bool hasActiveGame()
    {
        if (PlayerPrefs.HasKey(KEY_LEVEL_CLEARED))
        {
            return PlayerPrefs.GetInt(KEY_LEVEL_CLEARED) > -1;
        }
        else
        {
            resetGameProgress();
            return false;
        }
    }

    public static int continueGame()
    {
        if (PlayerPrefs.HasKey(KEY_LEVEL_CLEARED))
        {
            return PlayerPrefs.GetInt(KEY_LEVEL_CLEARED);
        }
        else
        {
            resetGameProgress();
            return -1;
        }
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
    
    public static bool getVolumeMuted(bool defaultMuteState) => PlayerPrefs.HasKey(KEY_VOLUME_MUTED)
        ? (PlayerPrefs.GetInt(KEY_VOLUME_MUTED) == 0)
        : updateVolumeMuted(defaultMuteState);
    
    public static bool updateVolumeMuted(bool isMute)
    {
        PlayerPrefs.SetInt(KEY_VOLUME_MUTED, isMute ? 0 : 1);
        PlayerPrefs.Save();
        return isMute;
    }
}