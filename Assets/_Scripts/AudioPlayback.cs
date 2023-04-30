using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayback : MonoBehaviour
{
    public MouthSettings previousSettings;
    private MouthSettings currentSettings;

    private int vowelChannelId = -1;

    public static AudioPlayback instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        this.previousSettings = new MouthSettings();
        this.currentSettings = new MouthSettings();

        this.currentSettings.consonantKey = "EW";

        this.vowelChannelId = -1;
    }

    public void PlayAudio(MouthSettings latestSettings)
    {
        this.previousSettings = this.currentSettings;
        this.currentSettings = latestSettings;

        if (this.currentSettings.pushingAir == false && this.vowelChannelId != -1)
        {
            this.StopVowelAudio();
            return;
        }

        if (this.currentSettings.pushingAir == true)
        {
            if (this.currentSettings.consonantKey != "")
            {
                this.PlayConsonant();
            }

            this.PlayVowel();
        }        
    }

    //Consonants should only be played when:
    //The space bar has changed from unpressed to pressed
    //OR
    //The space bar is currently pressed, and the previous consonant is different from the current consonant
    private void PlayConsonant()
    {
        if (this.previousSettings.pushingAir == true)
        {
            if (this.previousSettings.consonantKey == this.currentSettings.consonantKey)
            {
                return;
            }            
        }

        AudioChannelSettings audioSettings = new AudioChannelSettings(false, this.currentSettings.pitch, this.currentSettings.pitch, this.currentSettings.volume, "Voice");
        AudioClip consonantClip = MouthSounds.consonantDict[this.currentSettings.consonantKey];
        AudioManager.instance.Play(consonantClip, audioSettings);       
    }

    //Vowels should only be played when:
    //The space bar has changed from pressed to unpressed
    //OR
    //The spacebar is currently pressed, and the previous vowel is different from the current vowel
    private void PlayVowel()
    {
        if (this.vowelChannelId != -1)
        {
            AudioManager.instance.SetPitch(this.vowelChannelId, this.currentSettings.pitch);
            AudioManager.instance.SetVolume(this.vowelChannelId, this.currentSettings.volume);
        }

        if (this.previousSettings.pushingAir == true && this.previousSettings.vowelKey == this.currentSettings.vowelKey)
        {            
            return;
        }

        if (this.vowelChannelId != -1)
        {
            AudioManager.instance.Stop(this.vowelChannelId);
        }

        AudioChannelSettings audioSettings = new AudioChannelSettings(true, this.currentSettings.pitch, this.currentSettings.pitch, this.currentSettings.volume, "Voice");
        AudioClip vowelClip = MouthSounds.vowelDict[this.currentSettings.vowelKey];

        this.vowelChannelId = AudioManager.instance.Play(vowelClip, audioSettings);
    }

    private void StopVowelAudio()
    {
        AudioManager.instance.Stop(this.vowelChannelId);
        this.vowelChannelId = -1;
    }
}
