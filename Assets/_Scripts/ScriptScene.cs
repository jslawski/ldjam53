using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Script Scene", menuName = "ScriptableObjects/Script Scene")]
public class ScriptScene : ScriptableObject
{
    public string gameplayPromptCutsceneFileName;
    public string playbackPromptCutsceneFileName;   
    public string playbackResponseCutsceneFileName;
    public string responseSentence;

    public Vector3 responseCutsceneFacePosition;
    public Vector3 responseCutsceneFaceRotation;
    public Vector3 responseCutsceneFaceScale;

    public Sentence GetSentence()
    {
        return new Sentence(this.responseSentence);
    }
}
