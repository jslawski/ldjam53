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

    public RecordedWord(RecordedWord objectToCopy)
    {
        this.recordedSettings = objectToCopy.recordedSettings;
    }

    public void SaveFrameSettings(MouthSettings frameSettings)
    {
        this.recordedSettings.Add(frameSettings);
    }
}
