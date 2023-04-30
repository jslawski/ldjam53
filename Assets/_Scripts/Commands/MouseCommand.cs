using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manipulate voice volume based on horizontal position
//Manipulate voice pitch based on vertical position
public class MouseCommand : Command
{
    private float playSpacePositionX = 0f;
    private float playSpacePositionY = 0f;

    private float minVolume = 0.5f;
    private float maxVolume = 3.0f;

    private float minPitch = 0.9f;
    private float maxPitch = 1.1f;

    //Constructor to assign player game object
    public MouseCommand(MouthSettings settings, float xPos, float yPos)
    {
        this.mouthSettings = settings;
        this.playSpacePositionX = xPos;
        this.playSpacePositionY = yPos;
    }

    public MouseCommand(MouseCommand commandToCopy)
    {
        this.mouthSettings = commandToCopy.mouthSettings;
        this.playSpacePositionX = commandToCopy.playSpacePositionX;
        this.playSpacePositionY = commandToCopy.playSpacePositionY;
    }

    public override MouthSettings Execute()
    {
        //Call function to update voice volume
        this.mouthSettings.volume = Mathf.Lerp(this.minVolume, this.maxVolume, Mathf.Abs(0.5f - this.playSpacePositionX));

        //Call function to update voice pitch
        this.mouthSettings.pitch = Mathf.Lerp(this.minPitch, this.maxPitch, this.playSpacePositionY);

        return this.mouthSettings;
    }
}
