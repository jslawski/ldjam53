using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected MouthSettings mouthSettings;

    public abstract MouthSettings Execute();
}
