using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayback : MonoBehaviour
{
    public MouthSettings previousSettings;
    private MouthSettings currentSettings;

    private int vowelChannelId = -1;
    private int consonantChannelId = -1;

    public bool isPlayingBack = false;

    public static AudioPlayback instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        this.previousSettings = new MouthSettings();
        this.currentSettings = new MouthSettings();

        this.vowelChannelId = -1;
        this.consonantChannelId = -1;
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
            
            if (AudioManager.instance.IsPlaying(this.consonantChannelId) == false)
            {
                this.PlayVowel();
            }            
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

        if (this.vowelChannelId != -1)
        {
            this.StopVowelAudio();
        }

        AudioChannelSettings audioSettings = new AudioChannelSettings(false, this.currentSettings.pitch, this.currentSettings.pitch, this.currentSettings.volume, "Voice");
        AudioClip consonantClip = MouthSounds.consonantDict[this.currentSettings.consonantKey];
        this.consonantChannelId = AudioManager.instance.Play(consonantClip, audioSettings);

        //Start coroutine to play Vowel
        StartCoroutine(this.PlayVowelAfterConsonent());
    }

    private IEnumerator PlayVowelAfterConsonent()
    {
        while (AudioManager.instance.IsPlaying(this.consonantChannelId))
        {
            yield return null;
        }

        if (this.currentSettings.pushingAir == true)
        {
            this.PlayVowel();
        }
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

            if (AudioManager.instance.IsPlaying(this.vowelChannelId))
            {
                if (this.previousSettings.vowelKey == this.currentSettings.vowelKey)
                {
                    return;
                }
                else
                {
                    AudioManager.instance.Stop(this.vowelChannelId);
                }
            }
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

    public void PlaybackRecordedWord(RecordedWord playbackWord)
    {
        StartCoroutine(this.PlaybackCoroutine(playbackWord));
    }

    private IEnumerator PlaybackCoroutine(RecordedWord playbackWord)
    {
        this.isPlayingBack = true;

        for (int i = 0; i < playbackWord.recordedSettings.Count; i++)
        {
            this.PlayAudio(playbackWord.recordedSettings[i]);

            yield return null;
        }

        this.isPlayingBack = false;
    }
}
