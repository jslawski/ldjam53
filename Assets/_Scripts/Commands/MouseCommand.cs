using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manipulate voice volume based on horizontal position
//Manipulate voice pitch based on vertical position
public class MouseCommand : Command
{
    float playSpacePositionX = 0f;
    float playSpacePositionY = 0f;

    //Constructor to assign player game object
    public MouseCommand(MouthSettings settings, float xPos, float yPos)
    {
        this.mouthSettings = settings;
        this.playSpacePositionX = xPos;
        this.playSpacePositionY = yPos;
    }

    public override MouthSettings Execute()
    {
        //Call function to update voice volume
        this.mouthSettings.volume = this.playSpacePositionX;
        //Call function to update voice pitch
        this.mouthSettings.pitch = this.playSpacePositionY;

        return this.mouthSettings;
    }
}
