using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthSettings
{
    public bool pushingAir = false;
    public float volume = 1;
    public float pitch = 1;
    public string consonantKey = "";
    public string vowelKey = "A";

    public MouthSettings(bool pushingAir = false, float volume = 1, float pitch = 1, string consonantKey = "", string vowelKey = "A")
    {
        this.pushingAir = pushingAir;
        this.volume = volume;
        this.pitch = pitch;
        this.consonantKey = consonantKey;
        this.vowelKey = vowelKey;
    }

    public MouthSettings(MouthSettings settingsToCopy)
    {
        this.pushingAir = settingsToCopy.pushingAir;
        this.volume = settingsToCopy.volume;
        this.pitch = settingsToCopy.pitch;
        this.consonantKey = settingsToCopy.consonantKey;
        this.vowelKey = settingsToCopy.vowelKey;
    }
}
