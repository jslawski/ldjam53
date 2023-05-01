using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FullScript
{
    public static List<ScriptScene> allScenes;
    public static List<RecordedSentence> recordedSentences;

    public static void SetupFullScript()
    {
        allScenes = Enumerable.ToList(Resources.LoadAll<ScriptScene>("ScriptScenes"));
        recordedSentences = new List<RecordedSentence>();
        MouthSounds.Setup("Jared");
    }

    public static void SaveRecordedSentence(RecordedSentence newSentence)
    {
        recordedSentences.Add(newSentence);
    }    
}
