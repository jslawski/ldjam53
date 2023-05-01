using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentence
{
    public List<Word> sentenceWords;

    public Sentence(string sentenceContents)
    {
        string[] sentenceArray = sentenceContents.Split(" ");

        this.sentenceWords = new List<Word>();

        for (int i = 0; i < sentenceArray.Length; i++)
        {
            this.sentenceWords.Add(new Word(sentenceArray[i]));
        }
    }
}
