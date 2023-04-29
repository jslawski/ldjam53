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
        consonantDict.Add("A", Resources.Load<AudioClip>(characterKey + "/Consonants/C_K_G.wav"));
        consonantDict.Add("AW", Resources.Load<AudioClip>(characterKey + "/Consonants/CH_J_SH_ZE.wav"));
        consonantDict.Add("AS", Resources.Load<AudioClip>(characterKey + "/Consonants/F_V.wav"));
        consonantDict.Add("W", Resources.Load<AudioClip>(characterKey + "/Consonants/H.wav"));
        consonantDict.Add("WS", Resources.Load<AudioClip>(characterKey + "/Consonants/L.wav"));
        consonantDict.Add("SD", Resources.Load<AudioClip>(characterKey + "/Consonants/M_N.wav"));
        consonantDict.Add("S", Resources.Load<AudioClip>(characterKey + "/Consonants/P_B.wav"));
        consonantDict.Add("WD", Resources.Load<AudioClip>(characterKey + "/Consonants/R.wav"));
        consonantDict.Add("AD", Resources.Load<AudioClip>(characterKey + "/Consonants/S_Z.wav"));
        consonantDict.Add("D", Resources.Load<AudioClip>(characterKey + "/Consonants/T_TH_D.wav"));

        //Vowels
        vowelDict.Add("A", Resources.Load<AudioClip>(characterKey + "/Vowels/A.wav"));
        vowelDict.Add("AA", Resources.Load<AudioClip>(characterKey + "/Vowels/AA.wav"));
        vowelDict.Add("E", Resources.Load<AudioClip>(characterKey + "/Vowels/E.wav"));
        vowelDict.Add("EE", Resources.Load<AudioClip>(characterKey + "/Vowels/EE.wav"));
        vowelDict.Add("I", Resources.Load<AudioClip>(characterKey + "/Vowels/I.wav"));
        vowelDict.Add("II", Resources.Load<AudioClip>(characterKey + "/Vowels/II.wav"));
        vowelDict.Add("O", Resources.Load<AudioClip>(characterKey + "/Vowels/O.wav"));
        vowelDict.Add("OO", Resources.Load<AudioClip>(characterKey + "/Vowels/OO.wav"));
        vowelDict.Add("U", Resources.Load<AudioClip>(characterKey + "/Vowels/U.wav"));
        vowelDict.Add("UU", Resources.Load<AudioClip>(characterKey + "/Vowels/UU.wav"));
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
