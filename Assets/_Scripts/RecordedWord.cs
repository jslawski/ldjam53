using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordedWord
{
    public List<MouthSettings> recordedSettings;

    public RecordedWord()
    {
        this.recordedSettings = new List<MouthSettings>();
    }

    public void SaveFrameSettings(MouthSettings frameSettings)
    {
        this.recordedSettings.Add(frameSettings);
    }

    /*
    public void Playback()
    {
        AudioPlayback.instance.PlaybackRecordedWord(this);
    } 
    */
}
