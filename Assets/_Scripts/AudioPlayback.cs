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

        AudioChannelSettings audioSettings = new AudioChannelSettings(false, this.currentSettings.pitch, this.currentSettings.pitch, this.currentSettings.volume + (0.8f * this.currentSettings.volume), "Voice");
        AudioClip consonantClip = MouthSounds.consonantDict[this.currentSettings.consonantKey];
        this.consonantChannelId = AudioManager.instance.Play(consonantClip, audioSettings);

        //Start coroutine to play Vowel
        StartCoroutine(this.PlayVowelAfterConsonent(consonantClip));
    }

    private IEnumerator PlayVowelAfterConsonent(AudioClip consonantClip)
    {
        float currentDuration = 0;

        float numIncrements = 3.0f;
        float increment = consonantClip.length / numIncrements;

        while (currentDuration < increment * (numIncrements - 1))
        {            
            yield return new WaitForFixedUpdate();
            currentDuration += Time.fixedDeltaTime;
        }
       
        if (this.currentSettings.pushingAir == true)
        {
            this.PlayVowel(true, increment);
        }        
    }
   
    //Vowels should only be played when:
    //The space bar has changed from pressed to unpressed
    //OR
    //The spacebar is currently pressed, and the previous vowel is different from the current vowel
    private void PlayVowel(bool crossfade = false, float crossfadeDuration = 0.0f)
    {
        AudioChannelSettings audioSettings = new AudioChannelSettings(true, this.currentSettings.pitch, this.currentSettings.pitch, this.currentSettings.volume, "Voice");

        this.SetTonedVowelKey();

        AudioClip vowelClip = MouthSounds.vowelDict[this.currentSettings.tonedVowelKey];

        if (this.vowelChannelId != -1)
        {
            AudioManager.instance.SetPitch(this.vowelChannelId, this.currentSettings.pitch);

            if (crossfade == false)
            {
                AudioManager.instance.SetVolume(this.vowelChannelId, this.currentSettings.volume);
            }

            if (AudioManager.instance.IsPlaying(this.vowelChannelId))
            {
                if (this.previousSettings.tonedVowelKey == this.currentSettings.tonedVowelKey)
                {
                    return;
                }
                else
                {
                    this.vowelChannelId = AudioManager.instance.Crossfade(this.vowelChannelId, vowelClip, audioSettings, 0.2f);
                    return;
                }
            }
        }        

        if (crossfade == true)
        {
            this.vowelChannelId = AudioManager.instance.Crossfade(this.consonantChannelId, vowelClip, audioSettings, crossfadeDuration);
        }
        else
        {
            this.vowelChannelId = AudioManager.instance.Play(vowelClip, audioSettings);
        }
    }

    private void SetTonedVowelKey()
    {
        string tonedVowel = this.currentSettings.vowelKey;

        if (this.currentSettings.volume <= 0.3f)
        {
            tonedVowel += "_1";
        }
        else if (this.currentSettings.volume <= 0.9f)
        {
            tonedVowel += "_2";
        }
        else
        {
            tonedVowel += "_3";
        }

        this.currentSettings.tonedVowelKey = tonedVowel;
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
