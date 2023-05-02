using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [SerializeField]
    private FadePanelManager fadePanel;

    private bool loading = false;

    [SerializeField]
    private AudioClip menuMusic;

    private AudioClip[] mouthFeel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        this.fadePanel.SetAlpha(1.0f);
        this.fadePanel.FadeFromBlack();
    }

    public void Start()
    {
        AudioChannelSettings menuAudioSettings = new AudioChannelSettings(true, 1, 1, 1, "BGM");
        AudioManager.instance.Play(this.menuMusic, menuAudioSettings);

        this.mouthFeel = Resources.LoadAll<AudioClip>("MouthFeel");

        AudioClip randomMouthFeel = this.mouthFeel[Random.Range(0, this.mouthFeel.Length)];

        AudioChannelSettings mouthFeelAudioSettings = new AudioChannelSettings(false, 1, 1, 1, "Voice");
        AudioManager.instance.Play(randomMouthFeel, mouthFeelAudioSettings);
    }

    public void PlayButtonClicked()
    {
        if (this.loading == false)
        {
            this.fadePanel.OnFadeSequenceComplete += this.LoadLevel;
            this.fadePanel.FadeToBlack();
            this.loading = true;
        }
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    private void LoadLevel()
    {
        this.fadePanel.OnFadeSequenceComplete -= this.LoadLevel;
        SceneManager.LoadScene("GameplayScene");
    }
}
