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
            int newIndex = currentIndex - 1;

            if (newIndex < 0)
            {
                newIndex = MouthSounds.vowelList.Count - 1;
            }

            this.mouthSettings.vowelKey = MouthSounds.vowelList[newIndex];
        }
        else if (this.scrollDelta < 0)
        {
            int newIndex = currentIndex + 1;

            if (newIndex > (MouthSounds.vowelList.Count - 1))
            {
                newIndex = 0;
            }

            this.mouthSettings.vowelKey = MouthSounds.vowelList[newIndex];
        }

        return this.mouthSettings;
    }
}