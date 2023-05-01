using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameSceneDirector : MonoBehaviour
{
    public static GameSceneDirector instance;

    private Vector3 gameplayFacePosition;
    private Vector3 gameplayFaceScale;
    private Vector3 gameplayFaceRotation;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private bool playbackSentence = true;

    private RawImage cutsceneImage;

    public int currentSceneId = 0;
    private int nextSceneId = 0;
    private int playbackSceneId = 0;
    
    [SerializeField]
    private RenderTexture videoCutsceneTexture;

    [SerializeField]
    private FadePanelManager fadePanel;
    [SerializeField]
    private VideoPlayer cutscenePlayer;

    [SerializeField]
    private ToggleFaceMode faceMode;

    private int bgmAudioID;
    private int voicelineAudioId;

    private AudioChannelSettings bgmAudioSettings;
    private AudioChannelSettings voicelineAudioSettings;

    private delegate void CutsceneComplete();
    private delegate void VoicelineComplete();
    private delegate void RecordedSentencePlaybackComplete();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        this.bgmAudioSettings = new AudioChannelSettings(true, 1.0f, 1.0f, 1.0f, "BGM");
        this.voicelineAudioSettings = new AudioChannelSettings(false, 1.0f, 1.0f, 1.0f, "Voice");

        FullScript.SetupFullScript();

        Transform initialFaceTransform = this.faceMode.GetFaceTransform();

        this.gameplayFacePosition = initialFaceTransform.position;
        this.gameplayFaceRotation = initialFaceTransform.rotation.eulerAngles;
        this.gameplayFaceScale = initialFaceTransform.localScale;
    }

    private void PlayBGM(string bgmFileName)
    {
        AudioClip currentBGM = Resources.Load<AudioClip>("BGM/" + bgmFileName);

        AudioManager.instance.Stop(this.bgmAudioID);

        this.bgmAudioID = AudioManager.instance.Play(currentBGM, this.bgmAudioSettings);
    }

    private void PlayImageCutscene(Texture imageTexture)
    {
        this.cutsceneImage.enabled = true;
        this.cutsceneImage.texture = imageTexture;
    }

    private void PlayVideoCutscene(string cutsceneFileName, bool looping = false, CutsceneComplete functionAfterComplete = null)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, cutsceneFileName);
        this.cutscenePlayer.url = filePath;

        this.cutsceneImage.enabled = true;
        this.cutsceneImage.texture = this.videoCutsceneTexture;

        this.cutscenePlayer.renderMode = VideoRenderMode.RenderTexture;
        this.cutscenePlayer.targetCameraAlpha = 1.0f;
        this.cutscenePlayer.Play();

        if (looping == false)
        {
            StartCoroutine(this.WaitForCutsceneToFinish(functionAfterComplete));
        }
    }

    private IEnumerator WaitForCutsceneToFinish(CutsceneComplete functionAfterComplete)
    {
        while (this.cutscenePlayer.isPlaying == false)
        {
            yield return null;
        }

        while (this.cutscenePlayer.isPlaying)
        {
            yield return null;
        }

        if (functionAfterComplete != null)
        {
            functionAfterComplete();
        }
    }

    private void PlayVoiceline(int sceneId, VoicelineComplete functionAfterComplete)
    {
        AudioClip voiceline = FullScript.allScenes[sceneId].GetPromptVoiceline();
        this.voicelineAudioId = AudioManager.instance.Play(voiceline, voicelineAudioSettings);

        StartCoroutine(this.WaitForVoicelineToComplete(functionAfterComplete));
    }

    private IEnumerator WaitForVoicelineToComplete(VoicelineComplete functionAfterComplete)
    {
        while (AudioManager.instance.IsPlaying(this.voicelineAudioId))
        {
            yield return null;
        }

        functionAfterComplete();
    }

    private void PlayRecordedSentence(int sceneId, RecordedSentencePlaybackComplete functionAfterComplete)
    {
        AudioPlayback.instance.PlaybackRecordedSentence(FullScript.recordedSentences[sceneId]);

        StartCoroutine(this.WaitForRecordedSentenceComplete(functionAfterComplete));
    }

    private IEnumerator WaitForRecordedSentenceComplete(RecordedSentencePlaybackComplete functionAfterComplete)
    {
        while (AudioPlayback.instance.isSentencePlayingBack == true)
        {
            yield return null;
        }

        functionAfterComplete();
    }

    public void StartNewScene()
    {
        this.fadePanel.OnFadeSequenceComplete -= this.StartNewScene;
        
        this.currentSceneId = this.nextSceneId;
        this.nextSceneId++;

        this.faceMode.HideFace();

        if (this.currentSceneId >= FullScript.allScenes.Count)
        {
            this.BeginFinalPlaybackSequence();
        }
        else
        {
            this.PlayGameplayPromptCutscene();
        }
    }

    private void PlayGameplayPromptCutscene()
    {
        this.PlayImageCutscene(FullScript.allScenes[this.currentSceneId].GetGameplayPromptCutscene());

        this.faceMode.HideFace();

        this.fadePanel.FadeFromBlack();

        this.PlayBGM("cutsceneBGM");
        this.PlayVoiceline(this.currentSceneId, this.PromptVoicelineComplete);        
    }

    private void PromptVoicelineComplete()
    {
        this.fadePanel.OnFadeSequenceComplete += this.PlayPlayerZoomCutscene;
        this.fadePanel.FadeToBlack();
    }

    private void PlayPlayerZoomCutscene()
    {        
        this.fadePanel.OnFadeSequenceComplete -= this.PlayPlayerZoomCutscene;
        this.fadePanel.FadeFromBlack();

        this.PlayVideoCutscene("playerZoomCutscene.mp4", false, this.PlayerZoomCutsceneFinish);
    }

    private void PlayerZoomCutsceneFinish()
    {
        this.fadePanel.SetFillAmount(1.0f);

        this.cutsceneImage.enabled = false;

        this.fadePanel.OnFadeSequenceComplete += this.BeginGameplay;
        this.fadePanel.FadeFromBlack();
    }

    private void BeginGameplay()
    {
        this.faceMode.EnableGameplayMode();
        this.faceMode.OrientFace(this.gameplayFacePosition, this.gameplayFaceRotation, this.gameplayFaceScale);

        this.fadePanel.OnFadeSequenceComplete -= this.BeginGameplay;

        this.player.ActivateGameplay();        
    }

    public void EndGameplay()
    {
        StartCoroutine(EndGameplayCoroutine());
    }

    private IEnumerator EndGameplayCoroutine()
    {
        float gameplayEndBuffer = 2.0f;

        while (gameplayEndBuffer > 0.0f)
        {
            gameplayEndBuffer -= Time.deltaTime;
            yield return null;
        }

        if (this.playbackSentence == true)
        {
            this.PlayRecordedSentence(this.currentSceneId, this.FadeToNextScene);
        }
        else
        {
            this.fadePanel.OnFadeSequenceComplete += this.StartNewScene;
            this.fadePanel.FadeToBlack();
        }
    }

    private void FadeToNextScene()
    {        
        this.fadePanel.OnFadeSequenceComplete += this.StartNewScene;
        this.fadePanel.FadeToBlack();
    }
    
    private void BeginFinalPlaybackSequence()
    {
        this.playbackSceneId = 0;

        this.PlayBGM("cutsceneBGM");
        
        //Show establishing cutscene 
        this.PlayVideoCutscene("establishingCutscene.mp4", false, this.PlayNextPlaybackScene);                
    }

    private void PlayNextPlaybackScene()
    {
        this.faceMode.HideFace();

        this.fadePanel.SetFillAmount(1.0f);

        this.fadePanel.OnFadeSequenceComplete += this.PlayPlaybackVoicelineCutscene;
        this.fadePanel.FadeFromBlack();              
    }

    private void PlayPlaybackVoicelineCutscene()
    {        
        this.fadePanel.OnFadeSequenceComplete -= this.PlayPlaybackVoicelineCutscene;
        this.PlayVoiceline(this.playbackSceneId, PlaybackVoicelineComplete);            
    }

    private void PlaybackVoicelineComplete()
    {
        this.fadePanel.OnFadeSequenceComplete += this.SetupPlaybackRecordedSentence;
        this.fadePanel.FadeToBlack();
    }

    private void SetupPlaybackRecordedSentence()
    {
        this.PlayImageCutscene(FullScript.allScenes[this.playbackSceneId].GetPlaybackResponseCutscene());

        this.faceMode.EnableCutsceneMode();
        this.OrientPlayerFace();

        this.fadePanel.OnFadeSequenceComplete += this.PlayPlaybackRecordedSentence;
        this.fadePanel.FadeFromBlack();
    }

    private void OrientPlayerFace()
    {
        Vector3 position = FullScript.allScenes[this.playbackSceneId].responseCutsceneFacePosition;
        Vector3 rotation = FullScript.allScenes[this.playbackSceneId].responseCutsceneFaceRotation;
        Vector3 scale = FullScript.allScenes[this.playbackSceneId].responseCutsceneFaceScale;

        this.faceMode.OrientFace(position, rotation, scale);
    }

    private void PlayPlaybackRecordedSentence()
    {
        this.fadePanel.OnFadeSequenceComplete -= this.PlayPlaybackRecordedSentence;

        this.PlayRecordedSentence(this.playbackSceneId, this.SetupNextPlaybackScene);        
    }

    private void SetupNextPlaybackScene()
    {
        this.fadePanel.OnFadeSequenceComplete += this.FadeIntoNextPlaybackScene;
        this.fadePanel.FadeToBlack();
    }

    private void FadeIntoNextPlaybackScene()
    {
        this.fadePanel.OnFadeSequenceComplete -= this.FadeIntoNextPlaybackScene;

        this.playbackSceneId++;

        if (this.playbackSceneId < FullScript.allScenes.Count)
        {
            this.PlayNextPlaybackScene();
        }
        else
        {
            //Do an endgame thing here!
        }
    }
}
