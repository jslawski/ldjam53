using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBeContinuedMenu : MonoBehaviour
{
    [SerializeField]
    private FadePanelManager fadePanel;
    
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(this.ReturnToMenu());            
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(5.0f);

        this.fadePanel.OnFadeSequenceComplete += this.ChangeToMenuScene;
        this.fadePanel.FadeToBlack();
    }

    // Update is called once per frame
    void ChangeToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
