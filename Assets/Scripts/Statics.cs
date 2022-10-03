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

    [Serializable]
    public class BoolPlayerPrefData : PlayerPrefData<bool> {
        public BoolPlayerPrefData (string name, bool defaultVal) : base (name, defaultVal) {
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
    public static bool GetBool (BoolPlayerPrefData PPD) {
        int val = PlayerPrefs.GetInt (PPD.Name, PPD.DefaultValue ? 1 : 0);
        return val != 0;
    }

    // Setters
    public static void SetString (StringPlayerPrefData PPD, string val) {
        PlayerPrefs.SetString (PPD.Name, val);
    }
    public static void SetInt (IntPlayerPrefData PPD, int val) {
        PlayerPrefs.SetInt (PPD.Name, val);
    }
    public static void SetFloat (FloatPlayerPrefData PPD, float val) {
        PlayerPrefs.SetFloat (PPD.Name, val);
    }
    public static void SetKeycode (KeycodePlayerPrefData PPD, KeyCode val) {
        PlayerPrefs.SetString (PPD.Name, val.ToString ());
    }
    public static void SetBool (BoolPlayerPrefData PPD, bool val) {
        PlayerPrefs.SetInt (PPD.Name, val ? 1 : 0);
    }
}

public class Statics
{
    // PlayerPref Lookup Names
    public static FloatPlayerPrefData AudioMasterVolumePPD = new FloatPlayerPrefData ("Master Volume", 1.0f);
    public static FloatPlayerPrefData AudioSFXVolumePPD = new FloatPlayerPrefData ("SFX Volume", 1.0f);
    public static FloatPlayerPrefData AudioMusicVolumePPD = new FloatPlayerPrefData ("Music Volume", 0.2f);

    public static BoolPlayerPrefData DedicatedChargeP1PPD = new BoolPlayerPrefData ("Dedicated Charge P1", false);
    public static BoolPlayerPrefData DedicatedChargeP2PPD = new BoolPlayerPrefData ("Dedicated Charge P2", false);

    public static FloatPlayerPrefData AIDifficultyPPD = new FloatPlayerPrefData ("AI Difficulty", 0.5f);

    // Relative file path to Resources directory for audio files
    //public static string AudioFilePathPrefix = "SFX/";
    public static string VFXFilePathPrefix = "VFX/";
    public static string GameModeFilePathPrefix = "GameModes/";

    // scene names
    public static string MainSceneFileName = "Main";
    public static string MainAISceneFileName = "MainAI";
    public static string GameOverSceneFileName = "GameOver";

    public static int WinningScore = 8;
}
