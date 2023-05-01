using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FullScript
{
    private int currentSceneId = 0;
    private int nextSceneId = 0;

    public List<ScriptScene> allScenes;
    public List<RecordedSentence> recordedSentences;

    public void SetupFullScript()
    {
        this.allScenes = Enumerable.ToList(Resources.LoadAll<ScriptScene>("ScriptScenes"));
        this.recordedSentences = new List<RecordedSentence>();
    }

    private void PlaybackSentence()
    {
        //Playback the recorded sentence based on the currentSceneId
    }

    public void StartNewScene()
    {
        this.currentSceneId = this.nextSceneId;
        this.nextSceneId++;

        if (this.currentSceneId >= this.allScenes.Count)
        {
            this.BeginFinalPlaybackSequence();
        }

        //Fade into GameplayPromptCutsceneFileName (likely an image)
        //Play audio of BGM
        //Play audio of prompt voiceline
        //Continue displaying/playing the above until voiceline completes
        //Play Player zoom-in cutscene (always the same)
        //Fade out, hide all cutscene assets, fade into gameplay        
        //Enable gameplay?
    }

    public void SaveRecordedSentence(RecordedSentence newSentence)
    {
        this.recordedSentences.Add(newSentence);
    }

    private void BeginFinalPlaybackSequence()
    {

    }
}
