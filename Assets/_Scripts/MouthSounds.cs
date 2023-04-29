using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouthSounds
{
    public static Dictionary<string, AudioClip> mouthDict;
    public static List<string> vowelList;

    public static void Setup(string characterKey)
    {
        LoadMouthSoundDict(characterKey);
        LoadVowelList();
    }

    public static void LoadMouthSoundDict(string characterKey)
    {
        mouthDict = new Dictionary<string, AudioClip>();
        
        //Consonants
        mouthDict.Add("A", Resources.Load<AudioClip>(characterKey + "/Consonants/C_K_G.wav"));
        mouthDict.Add("AW", Resources.Load<AudioClip>(characterKey + "/Consonants/CH_J_SH_ZE.wav"));
        mouthDict.Add("AS", Resources.Load<AudioClip>(characterKey + "/Consonants/F_V.wav"));
        mouthDict.Add("W", Resources.Load<AudioClip>(characterKey + "/Consonants/H.wav"));
        mouthDict.Add("WS", Resources.Load<AudioClip>(characterKey + "/Consonants/L.wav"));
        mouthDict.Add("SD", Resources.Load<AudioClip>(characterKey + "/Consonants/M_N.wav"));
        mouthDict.Add("S", Resources.Load<AudioClip>(characterKey + "/Consonants/P_B.wav"));
        mouthDict.Add("WD", Resources.Load<AudioClip>(characterKey + "/Consonants/R.wav"));
        mouthDict.Add("AD", Resources.Load<AudioClip>(characterKey + "/Consonants/S_Z.wav"));
        mouthDict.Add("D", Resources.Load<AudioClip>(characterKey + "/Consonants/T_TH_D.wav"));

        //Vowels
        mouthDict.Add("A", Resources.Load<AudioClip>(characterKey + "/Vowels/A.wav"));
        mouthDict.Add("AA", Resources.Load<AudioClip>(characterKey + "/Vowels/AA.wav"));
        mouthDict.Add("E", Resources.Load<AudioClip>(characterKey + "/Vowels/E.wav"));
        mouthDict.Add("EE", Resources.Load<AudioClip>(characterKey + "/Vowels/EE.wav"));
        mouthDict.Add("I", Resources.Load<AudioClip>(characterKey + "/Vowels/I.wav"));
        mouthDict.Add("II", Resources.Load<AudioClip>(characterKey + "/Vowels/II.wav"));
        mouthDict.Add("O", Resources.Load<AudioClip>(characterKey + "/Vowels/O.wav"));
        mouthDict.Add("OO", Resources.Load<AudioClip>(characterKey + "/Vowels/OO.wav"));
        mouthDict.Add("U", Resources.Load<AudioClip>(characterKey + "/Vowels/U.wav"));
        mouthDict.Add("UU", Resources.Load<AudioClip>(characterKey + "/Vowels/UU.wav"));
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
