using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MouthSounds
{
    public static Dictionary<string, AudioClip> consonantDict;
    public static Dictionary<string, AudioClip> vowelDict;
    public static List<string> vowelList;

    public static Dictionary<string, Sprite> consonantSpriteDict;
    public static List<Sprite> vowelUISprites;


    public static void Setup(string characterKey)
    {
        LoadMouthSoundDict(characterKey);
        LoadVowelList();

        LoadConsonantUISprites();
        LoadVowelUISprites();
    }

    private static void LoadMouthSoundDict(string characterKey)
    {
        consonantDict = new Dictionary<string, AudioClip>();
        vowelDict = new Dictionary<string, AudioClip>();

        //Consonants
        consonantDict.Add("A", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/S_Z"));
        consonantDict.Add("AW", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/C_K_G"));
        consonantDict.Add("AS", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/M_N"));
        consonantDict.Add("W", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/L"));
        consonantDict.Add("WS", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/T_TH_D"));
        consonantDict.Add("SD", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/P_B"));
        consonantDict.Add("S", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/F_V"));
        consonantDict.Add("WD", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/R"));
        consonantDict.Add("AD", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/CH_J_SH_ZE"));
        consonantDict.Add("D", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Consonants/H"));

        //Vowels
        vowelDict.Add("A_1", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/A_1"));
        vowelDict.Add("A_2", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/A_2"));
        vowelDict.Add("A_3", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/A_3"));
        vowelDict.Add("AA_1", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/AA_1"));
        vowelDict.Add("AA_2", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/AA_2"));
        vowelDict.Add("AA_3", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/AA_3"));
        vowelDict.Add("E_1", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/E_1"));
        vowelDict.Add("E_2", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/E_2"));
        vowelDict.Add("E_3", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/E_3"));
        vowelDict.Add("EE_1", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/EE_1"));
        vowelDict.Add("EE_2", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/EE_2"));
        vowelDict.Add("EE_3", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/EE_3"));
        vowelDict.Add("I_1", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/I_1"));
        vowelDict.Add("I_2", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/I_2"));
        vowelDict.Add("I_3", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/I_3"));
        vowelDict.Add("II_1", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/II_1"));
        vowelDict.Add("II_2", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/II_2"));
        vowelDict.Add("II_3", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/II_3"));
        vowelDict.Add("O_1", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/O_1"));
        vowelDict.Add("O_2", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/O_2"));
        vowelDict.Add("O_3", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/O_3"));
        vowelDict.Add("OO_1", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/OO_1"));
        vowelDict.Add("OO_2", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/OO_2"));
        vowelDict.Add("OO_3", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/OO_3"));
        vowelDict.Add("U_1", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/U_1"));
        vowelDict.Add("U_2", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/U_2"));
        vowelDict.Add("U_3", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/U_3"));
        vowelDict.Add("UU_1", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/UU_1"));
        vowelDict.Add("UU_2", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/UU_2"));
        vowelDict.Add("UU_3", Resources.Load<AudioClip>("MouthSounds/" + characterKey + "/Vowels/UU_3"));
    }

    private static void LoadVowelList()
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

    private static void LoadConsonantUISprites()
    {
        consonantSpriteDict = new Dictionary<string, Sprite>();

        consonantSpriteDict.Add("A", Resources.Load<Sprite>("UI/Consonants/A"));
        consonantSpriteDict.Add("AW", Resources.Load<Sprite>("UI/Consonants/AW"));
        consonantSpriteDict.Add("AS", Resources.Load<Sprite>("UI/Consonants/AS"));
        consonantSpriteDict.Add("W", Resources.Load<Sprite>("UI/Consonants/W"));
        consonantSpriteDict.Add("WS", Resources.Load<Sprite>("UI/Consonants/WS"));
        consonantSpriteDict.Add("SD", Resources.Load<Sprite>("UI/Consonants/SD"));
        consonantSpriteDict.Add("S", Resources.Load<Sprite>("UI/Consonants/S"));
        consonantSpriteDict.Add("WD", Resources.Load<Sprite>("UI/Consonants/WD"));
        consonantSpriteDict.Add("AD", Resources.Load<Sprite>("UI/Consonants/AD"));
        consonantSpriteDict.Add("D", Resources.Load<Sprite>("UI/Consonants/D"));
        consonantSpriteDict.Add("", Resources.Load<Sprite>("UI/Consonants/Default"));
    }

    private static void LoadVowelUISprites()
    {
        vowelUISprites = Enumerable.ToList(Resources.LoadAll<Sprite>("UI/Vowels"));
    }
}
