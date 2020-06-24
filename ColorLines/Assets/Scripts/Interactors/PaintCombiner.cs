using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PaintCombiner : Interactable
{
    [HideInInspector] public Rotations.Directions outDirection = Rotations.Directions.North;
    [SerializeField] float[] gameObjectRotations = new float[4];
    public LinePathSystem[] incomingLines = new LinePathSystem[3];
    public int[] incomingLinesDistance = new int[3];
    public LinePathSystem outGoingLine;
    LevelData levelData;

    public void Setup(LevelData levelData)
    {
        this.levelData = levelData;
        this.transform.localEulerAngles = new Vector3(0, 0, 90 * Random.Range(0, 4));
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

    public override void Interact()
    {
        Rotate();
        int count = interactedLines.Count;
        for (int i = 0; i < incomingLines.Length; i++)
        {
            if (incomingLines[i] != null)
            {
                incomingLines[i].UpdateLinePath(incomingLinesDistance[i]);
                incomingLines[i] = null;
            }
        }
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
        if ((int)outDirection == 3) outDirection = 0;
        else outDirection++;

        this.transform.rotation = Quaternion.Euler(0, 0, gameObjectRotations[(int)outDirection]);
    }

    public void CombinePaint(LinePathSystem inputLine, int distance)
    {
        for (int i = 0; i < incomingLines.Length; i++)
        {
            if (incomingLines[i] == null)
            {
                incomingLines[i] = inputLine;
                incomingLinesDistance[i] = distance;
                break;
            }
        }

        CombinePaint();
    }

    public void CombinePaint()
    {
        Color newColor = new Color(0, 0, 0, 1);
        foreach (var incomingLine in incomingLines)
        {
            if (incomingLine != null)
            {
                newColor += incomingLine.lineColor;
            }
        }

        newColor = new Color(Mathf.Clamp(newColor.r, 0, 1), Mathf.Clamp(newColor.g, 0, 1), Mathf.Clamp(newColor.b, 0, 1), 1);

        if (newColor != new Color(0,0,0,1))
        {
            if (outGoingLine != null)
            {
                outGoingLine.RemoveLine();
                outGoingLine = null;
            }

            int index = (int)outDirection;
            for (int i = 0; i < 2; i++)
            {
                if (index == 3) index = 0;
                else index++;
            }
            LinePathSystem line = new LinePathSystem(this.transform.position, (Rotations.Directions)index, levelData, newColor);
            for (int i = 0; i < incomingLines.Length; i++)
            {
                if (incomingLines[i] != null)
                {
                    line.lineParants.Add(incomingLines[i]);
                    incomingLines[i].lineChilds.Add(line, incomingLinesDistance[i]);
                }
            }
            levelData.lines.Add(line);
            outGoingLine = line;
        }
        else if (outGoingLine != null)
        {
            outGoingLine.RemoveLine();
            outGoingLine = null;
        }
    }
}
