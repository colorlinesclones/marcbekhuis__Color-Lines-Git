using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CornerPiece : Interactable
{
    [HideInInspector] public Rotations.Directions inDirection = Rotations.Directions.North;
    [HideInInspector] public Rotations.Directions outDirection = Rotations.Directions.North;
    [SerializeField] float[] gameObjectRotations = new float[4];

    public void Setup()
    {
        this.transform.localEulerAngles = new Vector3(0, 0, 90 * Random.Range(0,4));
        for (int i = 0; i < gameObjectRotations.Length; i++)
        {
            if (this.transform.localEulerAngles.z == gameObjectRotations[i])
            {
                int index = i;
                inDirection = (Rotations.Directions)index;
                break;
            }
        }

        if ((int)inDirection == 3)
        {
            outDirection = 0;
        }
        else outDirection = inDirection + 1;
    }

    public override void Interact()
    {
        Rotate();
        int count = interactedLines.Count;
        for (int i = 0; i < interactedLines.Count; i++)
        {
            interactedLines.Values.ElementAt(i).UpdateLinePath(interactedLines.Keys.ElementAt(i).distance);
            if (interactedLines.Count < count)
            {
                i--;
                count--;
            }
        }
    }

    private void Rotate()
    {
        if ((int)inDirection == 3) inDirection = 0;
        else inDirection++;
        if ((int)outDirection == 3) outDirection = 0;
        else outDirection++;

        this.transform.rotation = Quaternion.Euler(0, 0, gameObjectRotations[(int)inDirection]);
    }
}
