using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameSceneDirector : MonoBehaviour
{
    public static GameSceneDirector instance;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private bool playbackSentence = true;

    private RawImage cutsceneImage;

    public int currentSceneId = 0;
    private int nextSceneId = 0;

    
    [SerializeField]
    private RenderTexture videoCutsceneTexture;

    private Texture stillImageCutsceneTexture;

    [SerializeField]
    private FadePanelManager fadePanel;
    [SerializeField]
    private VideoPlayer cutscenePlayer;

    private int bgmAudioID;
    private int voicelineAudioId;

    private AudioChannelSettings bgmAudioSettings;
    private AudioChannelSettings voicelineAudioSettings;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        this.bgmAudioSettings = new AudioChannelSettings(true, 1.0f, 1.0f, 1.0f, "BGM");
        this.voicelineAudioSettings = new AudioChannelSettings(false, 1.0f, 1.0f, 1.0f, "Voice");

        FullScript.SetupFullScript();
    }

    public void StartNewScene()
    {
        this.fadePanel.OnFadeSequenceComplete -= this.StartNewScene;
        
        this.currentSceneId = this.nextSceneId;
        this.nextSceneId++;

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
        this.stillImageCutsceneTexture = FullScript.allScenes[this.currentSceneId].GetGameplayPromptCutscene();
        this.cutsceneImage.texture = this.stillImageCutsceneTexture;

        this.cutsceneImage.enabled = true;

        this.fadePanel.FadeFromBlack();

        //Play audio of BGM
        //Play audio of prompt voiceline

        StartCoroutine(this.WaitForEndOfVoiceline());
    }

    private IEnumerator WaitForEndOfVoiceline()
    {
        while (AudioManager.instance.IsPlaying(this.voicelineAudioId))
        {
            yield return null;
        }

        this.fadePanel.OnFadeSequenceComplete += this.PlayPlayerZoomCutscene;
        this.fadePanel.FadeToBlack();
    }

    private void PlayPlayerZoomCutscene()
    {
        this.cutsceneImage.texture = this.videoCutsceneTexture;

        this.fadePanel.OnFadeSequenceComplete -= this.PlayPlayerZoomCutscene;
        this.fadePanel.FadeFromBlack();

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "playerZoomCutscene.mp4");
        this.cutscenePlayer.url = filePath;

        this.cutscenePlayer.renderMode = VideoRenderMode.RenderTexture;
        this.cutscenePlayer.targetCameraAlpha = 1.0f;
        this.cutscenePlayer.Play();

        StartCoroutine(this.WaitForPlayerZoomCutsceneFinish());
    }

    private IEnumerator WaitForPlayerZoomCutsceneFinish()
    {
        while (this.cutscenePlayer.isPlaying == false)
        {
            yield return null;
        }

        while (this.cutscenePlayer.isPlaying)
        {
            yield return null;
        }

        this.fadePanel.SetFillAmount(1.0f);

        this.cutsceneImage.enabled = false;

        this.fadePanel.OnFadeSequenceComplete += this.BeginGameplay;
        this.fadePanel.FadeFromBlack();
    }

    private void BeginGameplay()
    {
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
            AudioPlayback.instance.PlaybackRecordedSentence(FullScript.recordedSentences[this.currentSceneId]);
            StartCoroutine(this.WaitForSceneSentencePlaybackComplete());
        }
        else
        {
            this.fadePanel.OnFadeSequenceComplete += this.StartNewScene;
            this.fadePanel.FadeToBlack();
        }
    }

    private IEnumerator WaitForSceneSentencePlaybackComplete()
    {
        while (AudioPlayback.instance.isSentencePlayingBack == true)
        {
            yield return null;
        }

        this.fadePanel.OnFadeSequenceComplete += this.StartNewScene;
        this.fadePanel.FadeToBlack();
    }
    
    private void BeginFinalPlaybackSequence()
    {

    }
}
