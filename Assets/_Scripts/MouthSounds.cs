using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouthSounds
{
    public static Dictionary<string, AudioClip> consonantDict;
    public static Dictionary<string, AudioClip> vowelDict;
    public static List<string> vowelList;

    public static void Setup(string characterKey)
    {
        LoadMouthSoundDict(characterKey);
        LoadVowelList();
    }

    public static void LoadMouthSoundDict(string characterKey)
    {
        consonantDict = new Dictionary<string, AudioClip>();
        vowelDict = new Dictionary<string, AudioClip>();

        //Consonants
        consonantDict.Add("A", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/C_K_G"));
        consonantDict.Add("AW", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/CH_J_SH_ZE"));
        consonantDict.Add("AS", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/F_V"));
        consonantDict.Add("W", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/H"));
        consonantDict.Add("WS", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/L"));
        consonantDict.Add("SD", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/M_N"));
        consonantDict.Add("S", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/P_B"));
        consonantDict.Add("WD", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/R"));
        consonantDict.Add("AD", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/S_Z"));
        consonantDict.Add("D", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/T_TH_D"));

        //Vowels
        vowelDict.Add("A", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/A"));
        vowelDict.Add("AA", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/AA"));
        vowelDict.Add("E", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/E"));
        vowelDict.Add("EE", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/EE"));
        vowelDict.Add("I", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/I"));
        vowelDict.Add("II", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/II"));
        vowelDict.Add("O", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/O"));
        vowelDict.Add("OO", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/OO"));
        vowelDict.Add("U", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/U"));
        vowelDict.Add("UU", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/UU"));
    }

    public static void LoadVowelList()
    {
        vowelList = new List<string>();

        vowelList.Add("A");
        vowelList.Add("AA");
        vowelList.Add("E");
        vowelList.Add("EE");
        vowelList.Add("I");
        vowelList.Add("II");
        vowelList.Add("O");
        vowelList.Add("OO");
        vowelList.Add("U");
        vowelList.Add("UU");
    }
}
