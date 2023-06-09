using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardCommand : Command
{
    private string consonantKey;
    public bool spacePressed;

    public KeyboardCommand(MouthSettings settings, string conKey, bool spacePressed)
    {
        this.mouthSettings = settings;
        this.consonantKey = conKey;
        this.spacePressed = spacePressed;
    }

    public KeyboardCommand(KeyboardCommand commandToCopy)
    {
        this.mouthSettings = commandToCopy.mouthSettings;
        this.consonantKey = commandToCopy.consonantKey;
        this.spacePressed = commandToCopy.spacePressed;
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

        UIManager.instance.UpdateConsonantUI(this.consonantKey);

        return this.mouthSettings;
    }
}
