using System;
using UnityEngine;
using static PlayerPrefHandler;

[Serializable]
public class PlayerPrefHandler {
    // Stores the lookup name and default value of a player pref
    public abstract class PlayerPrefData<T> {
        public string ID;
        public T DefaultValue;

        public PlayerPrefData (string id, T defaultVal) {
            // TODO: Restrict T to string, int, and float
            ID = id;
            DefaultValue = defaultVal;
        }
    }

    [Serializable]
    public class StringPlayerPrefData : PlayerPrefData<string> {
        public StringPlayerPrefData (string id, string defaultVal) : base (id, defaultVal) {
        }
    }
    [Serializable]
    public class IntPlayerPrefData : PlayerPrefData<int> {
        public IntPlayerPrefData (string id, int defaultVal) : base (id, defaultVal) {
        }
    }
    [Serializable]
    public class FloatPlayerPrefData : PlayerPrefData<float> {
        public FloatPlayerPrefData (string id, float defaultVal) : base (id, defaultVal) {
        }
    }

    public static string GetString (StringPlayerPrefData PPD) {
        return PlayerPrefs.GetString (PPD.ID, PPD.DefaultValue);
    }
    public static int GetInt (IntPlayerPrefData PPD) {
        return PlayerPrefs.GetInt (PPD.ID, PPD.DefaultValue);
    }
    public static float GetFloat (FloatPlayerPrefData PPD) {
        return PlayerPrefs.GetFloat (PPD.ID, PPD.DefaultValue);
    }
}

public class Statics
{
    // PlayerPref Lookup Names
    public static FloatPlayerPrefData AudioMasterVolumePPD = new FloatPlayerPrefData ("Master Volume", 1.0f);
    public static FloatPlayerPrefData AudioSFXVolumePPD = new FloatPlayerPrefData ("SFX Volume", 1.0f);
    public static FloatPlayerPrefData AudioMusicVolumePPD = new FloatPlayerPrefData ("Music Volume", 1.0f);
    
}
