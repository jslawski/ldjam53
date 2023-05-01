using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordDisplay : MonoBehaviour
{
    public static WordDisplay instance;

    [SerializeField]
    private Color highlightedColor;

    string highlightTagStart = "";
    string highlightTagEnd = "";

    private TextMeshProUGUI wordText;

    private Word currentWord;

    private void Awake()
    {
        this.highlightTagStart = "<b><u><color=" + ColorUtility.ToHtmlStringRGB(this.highlightedColor) + ">";
        this.highlightTagEnd = "</b></u></color>";

        this.wordText = GetComponent<TextMeshProUGUI>();

        if (instance == null)
        {
            instance = this;
        }
    }

    public void DisplayNewWord(Word newWord)
    {
        this.currentWord = newWord;
        this.UpdateWordDisplay(this.currentWord.GetParsedWord());
    }

    public void HighlightSyllable(int syllableIndex)
    {
        string highlightedWord = "";

        for (int i = 0; i < this.currentWord.syllables.Count; i++)
        {
            if (i == syllableIndex)
            {
                string highlightedSyllable = this.highlightTagStart + this.currentWord.syllables[i] + this.highlightTagEnd;
                highlightedWord += highlightedSyllable;
            }
            else
            {
                highlightedWord += this.currentWord.syllables[i];
            }
        }

        this.UpdateWordDisplay(highlightedWord);
    }

    private void UpdateWordDisplay(string wordText)
    {
        this.wordText.text = wordText;
    }
}
