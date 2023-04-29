using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardCommand : Command
{
    private string consonantKey;
    private bool spacePressed;

    public KeyboardCommand(MouthSettings settings, string conKey, bool spacePressed)
    {
        this.mouthSettings = settings;
        this.consonantKey = conKey;
        this.spacePressed = spacePressed;
    }

    public override MouthSettings Execute()
    {
        if (this.spacePressed == true)
        {
            this.mouthSettings.pushingAir = true;
        }
        else
        {
            this.mouthSettings.pushingAir = false;
        }

        this.mouthSettings.consonantKey = this.consonantKey;
        
        return this.mouthSettings;
    }
}
