using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatusSave
{
    public enum Status
    {
        Locked,
        Unlocked,
        Finished
    }

    public LevelStatusSave(Status status)
    {
        this.status = status;
    }

    public Status status;
}
