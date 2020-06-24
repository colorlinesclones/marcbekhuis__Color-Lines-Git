using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaintDivider : Interactable
{
    [HideInInspector] public Rotations.Directions inDirection = Rotations.Directions.North;
    [SerializeField] float[] gameObjectRotations = new float[4];
    public LinePathSystem incomingLine;
    int incomingLineDistance;

    public void Setup()
    {
        this.transform.localEulerAngles = new Vector3(0, 0, 90 * Random.Range(0, 4));
        for (int i = 0; i < gameObjectRotations.Length; i++)
        {
            if (this.transform.localEulerAngles.z == gameObjectRotations[i])
            {
                int index = i;
                inDirection = (Rotations.Directions)index;
                break;
            }
        }
    }

    public override void Interact()
    {
        Rotate();
        int count = interactedLines.Count;
        if (incomingLine != null)
        {
            incomingLine.UpdateLinePath(incomingLineDistance);
            incomingLine = null;
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

    public void DividePaint(LinePathSystem inputLine, LevelData levelData, int distance)
    {
        incomingLine = inputLine;
        incomingLineDistance = distance;
        Color newColor = new Color(0, 0, 0, 1);

        int index = (int)inDirection;
        for (int i = 0; i < 2; i++)
        {
            if (index == 3) index = 0;
            else index++;
        }
        if (index == 3) index = 0;
        else index++;
        if (inputLine.lineColor.r > 0)
        {
            newColor = new Color(inputLine.lineColor.r, 0, 0, 1);
            LinePathSystem line = new LinePathSystem(this.transform.position, (Rotations.Directions)index, levelData, newColor);
            line.lineParants.Add(inputLine);
            inputLine.lineChilds.Add(line,distance);
            levelData.lines.Add(line);
        }

        if (index == 3) index = 0;
        else index++;
        if (inputLine.lineColor.g > 0)
        {
            newColor = new Color(0, inputLine.lineColor.g, 0, 1);
            LinePathSystem line = new LinePathSystem(this.transform.position, (Rotations.Directions)index, levelData, newColor);
            line.lineParants.Add(inputLine);
            inputLine.lineChilds.Add(line, distance);
            levelData.lines.Add(line);
        }

        if (index == 3) index = 0;
        else index++;
        if (inputLine.lineColor.b > 0)
        {
            newColor = new Color(0, 0, inputLine.lineColor.b, 1);
            LinePathSystem line = new LinePathSystem(this.transform.position, (Rotations.Directions)index, levelData, newColor);
            line.lineParants.Add(inputLine);
            inputLine.lineChilds.Add(line, distance);
            levelData.lines.Add(line);
        }
    }

    private void Rotate()
    {
        if ((int)inDirection == 3) inDirection = 0;
        else inDirection++;

        this.transform.rotation = Quaternion.Euler(0, 0, gameObjectRotations[(int)inDirection]);
    }
}
