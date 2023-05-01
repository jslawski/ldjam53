using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayback : MonoBehaviour
{
    public MouthSettings previousSettings;
    private MouthSettings currentSettings;

    private int vowelChannelId = -1;
    private int consonantChannelId = -1;

    private bool isWordPlayingBack = false;
    public bool isSentencePlayingBack = false;

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

        if (this.currentSettings.pushingAir == false)
        {
            if (this.vowelChannelId != -1)
            {
                this.StopVowelAudio();
            }
            
            FaceController.instance.UpdateMouthShapeConsonant(this.currentSettings.consonantKey, true, this.currentSettings.volume);
            FaceController.instance.ResetMouthSize();
            return;
        }
        
        if (this.currentSettings.pushingAir == true)
        {
            if (this.previousSettings.pushingAir == false && this.currentSettings.consonantKey != "")
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

        float numIncrements = 1.0f;
        float increment = consonantClip.length / numIncrements;
        while (AudioManager.instance.IsPlaying(this.consonantChannelId))
        //while (currentDuration < increment * (numIncrements - 1))
        {            
            yield return new WaitForFixedUpdate();
            currentDuration += Time.fixedDeltaTime;
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
    private void PlayVowel(bool crossfade = false, float crossfadeDuration = 0.0f)
    {
        AudioChannelSettings audioSettings = new AudioChannelSettings(true, this.currentSettings.pitch, this.currentSettings.pitch, this.currentSettings.volume, "Voice");

        this.SetTonedVowelKey();

        AudioClip vowelClip = MouthSounds.vowelDict[this.currentSettings.tonedVowelKey];
        
        if (this.vowelChannelId != -1)
        {
            FaceController.instance.UpdateMouthSize(this.currentSettings.volume);

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
                    FaceController.instance.UpdateMouthShapeVowel(this.currentSettings.vowelKey, this.currentSettings.volume);
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

        FaceController.instance.UpdateMouthShapeVowel(this.currentSettings.vowelKey, this.currentSettings.volume);
    }

    private void SetTonedVowelKey()
    {
        string tonedVowel = this.currentSettings.vowelKey;

        if (this.currentSettings.volume <= 0.3f)
        {
            tonedVowel += "_1";
        }
        else if (this.currentSettings.volume <= 0.7f)
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

    public void PlaybackRecordedSentence(RecordedSentence playbackSentence)
    {
        StartCoroutine(this.PlaybackSentenceCoroutine(playbackSentence));
    }

    private IEnumerator PlaybackSentenceCoroutine(RecordedSentence playbackSentence)
    {
        this.isSentencePlayingBack = true;

        for (int i = 0; i < playbackSentence.sentenceWords.Count; i++)
        {
            this.PlaybackRecordedWord(playbackSentence.sentenceWords[i]);

            while (this.isWordPlayingBack == true)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        this.isSentencePlayingBack = false;
    }

    private void PlaybackRecordedWord(RecordedWord playbackWord)
    {
        StartCoroutine(this.PlaybackCoroutine(playbackWord));
    }

    private IEnumerator PlaybackCoroutine(RecordedWord playbackWord)
    {
        this.isWordPlayingBack = true;

        int index = 0;

        while (playbackWord.recordedSettings[index].pushingAir == false)
        {
            index++;
            if (index + 10 < playbackWord.recordedSettings.Count)
            {
                if (playbackWord.recordedSettings[index + 10].pushingAir == true)
                {
                    break;
                }
            }
        }

        while (index < playbackWord.recordedSettings.Count - 1)
        {
            if (playbackWord.recordedSettings[index].pushingAir == false)
            {
                if (playbackWord.recordedSettings[index + 1].pushingAir == false)
                {
                    index++;

                    while (index < playbackWord.recordedSettings.Count - 1 &&
                           playbackWord.recordedSettings[index + 1].pushingAir == false)
                    {
                        index++;
                    }
                }
            }

            this.PlayAudio(playbackWord.recordedSettings[index]);

            yield return new WaitForFixedUpdate();

            index++;
        }

        this.PlayAudio(playbackWord.recordedSettings[playbackWord.recordedSettings.Count - 1]);

        this.isWordPlayingBack = false;
    }
}
