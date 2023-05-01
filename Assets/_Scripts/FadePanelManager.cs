using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanelManager : MonoBehaviour
{
    [SerializeField]
    private Image fadePanel;
    private float fadeSpeed = 1.0f;

    [SerializeField]
    private bool defaultBlack = false;

    public delegate void FadeComplete();
    public event FadeComplete OnFadeSequenceComplete;

    private void Awake()
    {
        if (this.defaultBlack == true)
        {
            this.fadePanel.fillAmount = 1.0f;
        }
        else
        {
            this.fadePanel.fillAmount = 0.0f;
        }
    }

    public void FadeToBlack()
    {
        StopAllCoroutines();
        StartCoroutine(this.FadeToBlackCoroutine());
    }

    public void FadeFromBlack()
    {
        StopAllCoroutines();
        StartCoroutine(this.FadeFromBlackCoroutine());
    }

    public void SetAlpha(float amount)
    {
        Color updatedColor = new Color(this.fadePanel.color.r, this.fadePanel.color.g, this.fadePanel.color.b, amount);
        fadePanel.color = updatedColor;
    }

    private IEnumerator FadeToBlackCoroutine()
    {
        while (fadePanel.color.a < 1 || Time.deltaTime == 0.0f)
        {
            Color updatedColor = new Color(this.fadePanel.color.r, this.fadePanel.color.g, this.fadePanel.color.b, this.fadePanel.color.a + (this.fadeSpeed * Time.fixedDeltaTime));
            fadePanel.color = updatedColor;
            yield return new WaitForFixedUpdate();
        }

        this.fadePanel.color = new Color(this.fadePanel.color.r, this.fadePanel.color.g, this.fadePanel.color.b, 1.0f);

        if (this.OnFadeSequenceComplete != null)
        {
            this.OnFadeSequenceComplete();
        }
    }

    private IEnumerator FadeFromBlackCoroutine()
    {
        while (fadePanel.color.a > 0 || Time.deltaTime == 0.0f)
        {
            Color updatedColor = new Color(this.fadePanel.color.r, this.fadePanel.color.g, this.fadePanel.color.b, this.fadePanel.color.a - (this.fadeSpeed * Time.fixedDeltaTime));
            fadePanel.color = updatedColor;
            yield return new WaitForFixedUpdate();
        }

        this.fadePanel.color = new Color(this.fadePanel.color.r, this.fadePanel.color.g, this.fadePanel.color.b, 0.0f);

        if (this.OnFadeSequenceComplete != null)
        {
            this.OnFadeSequenceComplete();
        }
    }
}
