using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private Image vowelUI;
    [SerializeField]
    private Image consonantUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
        
    public void UpdateVowelUI(int vowelIndex)
    {
        this.vowelUI.sprite = MouthSounds.vowelUISprites[vowelIndex];
    }

    public void UpdateConsonantUI(string consonantKey)
    {
        this.consonantUI.sprite = MouthSounds.consonantSpriteDict[consonantKey];
    }
}
