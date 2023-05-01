using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Script Scene", menuName = "ScriptableObjects/Script Scene")]
public class ScriptScene : ScriptableObject
{
    public string gameplayPromptCutsceneFileName;
    public string playbackPromptCutsceneFileName;
    public string promptVoicelineFileName;
    public string playbackResponseCutsceneFileName;
    public string responseSentence;

    public Vector3 responseCutsceneFacePosition;
    public Vector3 responseCutsceneFaceRotation;
    public Vector3 responseCutsceneFaceScale;

    public Sentence GetSentence()
    {
        return new Sentence(this.responseSentence);
    }

    public Texture GetGameplayPromptCutscene()
    {
        return Resources.Load<Texture>("Cutscenes/" + this.gameplayPromptCutsceneFileName);
    }

    public string GetPlaybackPromptCutsceneURL()
    {
        return System.IO.Path.Combine(Application.streamingAssetsPath, this.playbackPromptCutsceneFileName + ".mp4");
    }

    public Texture GetPlaybackResponseCutscene()
    {
        return Resources.Load<Texture>("Cutscenes/" + this.playbackResponseCutsceneFileName);
    }

    public AudioClip GetPromptVoiceline()
    {
        return Resources.Load<AudioClip>("Voicelines/" + this.promptVoicelineFileName);
    }
}
