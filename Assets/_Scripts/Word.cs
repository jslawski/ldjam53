using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Word structure in string:
//Seperate syllables with _
//Examples: wh_a_t 
//          s_t_raw_be_rey
//          c_rea_m
public class Word
{
    public string wordContents;
    public List<string> syllables;

    public Word(string contents)
    {
        this.wordContents = contents;
        this.syllables = Enumerable.ToList(contents.Split("_"));
    }

    public string GetParsedWord()
    {
        string parsedWord = "";

        for (int i = 0; i < syllables.Count; i++)
        {
            parsedWord += syllables[i];
        }

        return parsedWord;
    }
}
