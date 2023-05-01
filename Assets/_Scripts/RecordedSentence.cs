using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordedSentence
{
    public List<RecordedWord> sentenceWords;

    public RecordedSentence()
    {
        this.sentenceWords = new List<RecordedWord>();
    }

    public void RecordWord(RecordedWord newWord)
    {
        this.sentenceWords.Add(newWord);
    }
}
