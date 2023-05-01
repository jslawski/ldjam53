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

    public RecordedSentence(RecordedSentence objectToCopy)
    {
        this.sentenceWords = objectToCopy.sentenceWords;
    }

    public void SaveWord(RecordedWord newWord)
    {
        this.sentenceWords.Add(newWord);
    }
}
