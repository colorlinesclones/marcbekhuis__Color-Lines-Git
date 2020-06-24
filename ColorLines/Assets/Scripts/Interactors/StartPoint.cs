using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartPoint : Interactable
{
    public Color emitingColor;
    [HideInInspector] public Rotations.Directions outDirection = Rotations.Directions.North;
    [SerializeField] float[] gameObjectRotations = new float[4];

    public override void Interact()
    {
        Rotate();
        int count = interactedLines.Count;
        for (int i = 0; i < interactedLines.Count; i++)
        {
            interactedLines.Values.ElementAt(i).UpdateLinePath(interactedLines.Keys.ElementAt(i).distance, this.transform.position, outDirection);
            if (interactedLines.Count < count)
            {
                i--;
                count--;
            }
        }
    }

    public void Setup()
    {
        for (int i = 0; i < gameObjectRotations.Length; i++)
        {
            if (this.transform.localEulerAngles.z == gameObjectRotations[i])
            {
                int index = i;
                outDirection = (Rotations.Directions)index;
                break;
            }
        }
    }

    private void Rotate()
    {
        if ((int)outDirection == 3) outDirection = 0;
        else outDirection++;

        this.transform.rotation = Quaternion.Euler(0,0,gameObjectRotations[(int)outDirection]);
    }
}
