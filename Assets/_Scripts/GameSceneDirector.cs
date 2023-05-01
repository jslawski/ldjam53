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

    [SerializeField]
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

    public bool muted = true;

    [SerializeField]
    private Texture gameBG;

    [SerializeField]
    private GameObject toBeContinuedPanel;

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

    private void Start()
    {
        Cursor.visible = false;
        this.StartNewScene();
    }

    #region Utility
    private void OrientPlayerFace()
    {
        Vector3 position = FullScript.allScenes[this.playbackSceneId].responseCutsceneFacePosition;
        Vector3 rotation = FullScript.allScenes[this.playbackSceneId].responseCutsceneFaceRotation;
        Vector3 scale = FullScript.allScenes[this.playbackSceneId].responseCutsceneFaceScale;

        this.faceMode.OrientFace(position, rotation, scale);
    }

    private void PlayBGM(string bgmFileName)
    {
        return;

        if (this.muted == true)
        {
            return;
        }

        AudioClip currentBGM = Resources.Load<AudioClip>("BGM/" + bgmFileName);

        if (this.bgmAudioID != -1)
        {
            this.bgmAudioID = AudioManager.instance.Crossfade(this.bgmAudioID, currentBGM, this.bgmAudioSettings, 0.3f);
        }
        else
        {
            this.bgmAudioID = AudioManager.instance.Play(currentBGM, this.bgmAudioSettings);
        }        
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
        if (this.muted == true)
        {
            StartCoroutine(this.DelayCallback(functionAfterComplete));
            return;
        }

        AudioClip voiceline = FullScript.allScenes[sceneId].GetPromptVoiceline();
        this.voicelineAudioId = AudioManager.instance.Play(voiceline, voicelineAudioSettings);

        StartCoroutine(this.WaitForVoicelineToComplete(functionAfterComplete));
    }

    private IEnumerator DelayCallback(VoicelineComplete functionAfterComplete)
    {
        yield return new WaitForSeconds(3.0f);

        functionAfterComplete();
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

    private void PlayAlertAnimation()
    {

    }

    #endregion

    #region Game Loop
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

        this.PlayVideoCutscene("Alien_GameplayTransition.mp4", false, this.PlayerZoomCutsceneFinish);
    }

    private void PlayerZoomCutsceneFinish()
    {
        this.fadePanel.SetAlpha(1.0f);

        this.PlayBGM("GameBGM");
        this.cutsceneImage.texture = this.gameBG;
        this.faceMode.OrientFace(this.gameplayFacePosition, this.gameplayFaceRotation, this.gameplayFaceScale);

        this.faceMode.EnableGameplayMode();

        this.fadePanel.OnFadeSequenceComplete += this.BeginGameplay;
        this.fadePanel.FadeFromBlack();
    }

    private void BeginGameplay()
    {
        this.fadePanel.OnFadeSequenceComplete -= this.BeginGameplay;

        //Play begin game animation here

        //Put this at the end of it
        this.player.ActivateGameplay();        
    }

    public void EndGameplay()
    {
        //Play endgame animation here
        //Copy/paste stuff from the coroutine at the end of it

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
    #endregion

    #region Final Playback
    private void BeginFinalPlaybackSequence()
    {
        this.playbackSceneId = 0;

        this.PlayBGM("cutsceneBGM");

        this.fadePanel.FadeFromBlack();

        //Show establishing cutscene 
        this.PlayVideoCutscene("EndCutScene_Intro.mp4", false, this.PlayNextPlaybackScene);                
    }

    private void PlayNextPlaybackScene()
    {
        this.faceMode.HideFace();

        this.fadePanel.SetAlpha(1.0f);

        this.fadePanel.OnFadeSequenceComplete += this.PlayPlaybackVoicelineCutscene;
        this.fadePanel.FadeFromBlack();              
    }

    private void PlayPlaybackVoicelineCutscene()
    {        
        this.fadePanel.OnFadeSequenceComplete -= this.PlayPlaybackVoicelineCutscene;
        this.PlayVideoCutscene(FullScript.allScenes[this.playbackSceneId].GetPlaybackPromptCutsceneURL(), true);
        this.PlayVoiceline(this.playbackSceneId, PlaybackVoicelineComplete);            
    }

    private void PlaybackVoicelineComplete()
    {
        this.fadePanel.OnFadeSequenceComplete += this.SetupPlaybackRecordedSentence;
        this.fadePanel.FadeToBlack();
    }

    private void SetupPlaybackRecordedSentence()
    {
        this.fadePanel.OnFadeSequenceComplete -= this.SetupPlaybackRecordedSentence;

        this.PlayImageCutscene(FullScript.allScenes[this.playbackSceneId].GetPlaybackResponseCutscene());

        this.faceMode.EnableCutsceneMode();
        this.OrientPlayerFace();

        this.fadePanel.OnFadeSequenceComplete += this.PlayPlaybackRecordedSentence;
        this.fadePanel.FadeFromBlack();
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
            this.DisplayToBeContinued();
        }
    }

    private void DisplayToBeContinued()
    {
        this.toBeContinuedPanel.gameObject.SetActive(true);
        this.fadePanel.FadeFromBlack();
    }
    #endregion
}
