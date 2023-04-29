using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Changes vowel key in the passed MouthSettings
public class VowelCommand : Command
{    
    private float scrollDelta;

    public VowelCommand(MouthSettings settings, float delta)
    {
        this.mouthSettings = settings;
        this.scrollDelta = delta;
    }

    public override MouthSettings Execute()
    {
        int currentIndex = MouthSounds.vowelList.FindIndex(x => x == this.mouthSettings.vowelKey);

        if (this.scrollDelta > 0)
        {            
            this.mouthSettings.vowelKey = MouthSounds.vowelList[currentIndex - 1];
        }
        else if (this.scrollDelta < 0)
        {
            this.mouthSettings.vowelKey = MouthSounds.vowelList[currentIndex + 1];
        }

        return this.mouthSettings;
    }
}