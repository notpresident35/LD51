using System;
using UnityEngine;
using static PlayerPrefHandler;

// Stores the lookup name and default value of a player pref
[Serializable]
public class PlayerPrefHandler {
    
    // Class template
    public abstract class PlayerPrefData<T> {
        public string Name;
        
        public T DefaultValue;

        public PlayerPrefData (string name, T defaultVal) {
            Name = name;
            DefaultValue = defaultVal;
        }
    }

    // Inherited classes
    [Serializable]
    public class StringPlayerPrefData : PlayerPrefData<string> {
        public StringPlayerPrefData (string name, string defaultVal) : base (name, defaultVal) {
        }
    }
    [Serializable]
    public class IntPlayerPrefData : PlayerPrefData<int> {
        public IntPlayerPrefData (string name, int defaultVal) : base (name, defaultVal) {
        }
    }
    [Serializable]
    public class FloatPlayerPrefData : PlayerPrefData<float> {
        public FloatPlayerPrefData (string name, float defaultVal) : base (name, defaultVal) {
        }
    }

    [Serializable]
    public class KeycodePlayerPrefData : PlayerPrefData<KeyCode> {
        public KeycodePlayerPrefData (string name, KeyCode defaultVal) : base (name, defaultVal) {
        }
    }

    // Getters
    public static string GetString (StringPlayerPrefData PPD) {
        return PlayerPrefs.GetString (PPD.Name, PPD.DefaultValue);
    }
    public static int GetInt (IntPlayerPrefData PPD) {
        return PlayerPrefs.GetInt (PPD.Name, PPD.DefaultValue);
    }
    public static float GetFloat (FloatPlayerPrefData PPD) {
        return PlayerPrefs.GetFloat (PPD.Name, PPD.DefaultValue);
    }
    public static KeyCode GetKeycode (KeycodePlayerPrefData PPD) {
        string val = PlayerPrefs.GetString (PPD.Name, PPD.DefaultValue.ToString());
        return (KeyCode) Enum.Parse (typeof (KeyCode), val);
    }

    // Setters
    public static void SetString (StringPlayerPrefData PPD, string val) {
        PlayerPrefs.SetString (PPD.Name, val);
    }
    public static void SetInt (IntPlayerPrefData PPD, int val) {
        PlayerPrefs.SetInt (PPD.Name, val);
    }
    public static void SetFloat (FloatPlayerPrefData PPD, float val) {
        PlayerPrefs.GetFloat (PPD.Name, val);
    }
    public static void GetKeycode (KeycodePlayerPrefData PPD, KeyCode val) {
        PlayerPrefs.SetString (PPD.Name, val.ToString ());
    }
}

public class Statics
{
    // PlayerPref Lookup Names
    public static FloatPlayerPrefData AudioMasterVolumePPD = new FloatPlayerPrefData ("Master Volume", 1.0f);
    public static FloatPlayerPrefData AudioSFXVolumePPD = new FloatPlayerPrefData ("SFX Volume", 1.0f);
    public static FloatPlayerPrefData AudioMusicVolumePPD = new FloatPlayerPrefData ("Music Volume", 1.0f);
    
}
