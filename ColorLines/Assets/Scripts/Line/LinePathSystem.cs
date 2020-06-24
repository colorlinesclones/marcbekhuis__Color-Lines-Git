using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class LinePathSystem
{
    public Dictionary<LinePathSystem, int> lineChilds = new Dictionary<LinePathSystem, int>();
    public List<LinePathSystem> lineParants = new List<LinePathSystem>();

    public List<LinePiece> linePieces = new List<LinePiece>();
    public Color lineColor;
    Vector3 startPosition;
    Rotations.Directions startDirection;
    LevelData levelData;
    public int lineNumber;

    //Rotations.Directions direction;
    [HideInInspector] public int timesCalledThisUpdate;

    public LinePathSystem(Vector3 startPosition, Rotations.Directions startDirection, LevelData levelData, Color lineColor)
    {
        this.startPosition = startPosition;
        this.startDirection = startDirection;
        this.levelData = levelData;
        this.lineColor = lineColor;
        lineNumber = levelData.lines.Count;

        UpdateLinePath(0, startPosition, startDirection);
    }

    public void UpdateLinePath(int firstPoint)
    {
        Vector3 location;
        Rotations.Directions direction;

        if (firstPoint > 0)
        {
            location = linePieces[firstPoint - 1].spriteRenderer.transform.position;
            direction = linePieces[firstPoint - 1].inDirection;
        }
        else
        {
            location = startPosition;
            direction = startDirection;
        }

        UpdateLinePath(firstPoint, location, direction);
    }

    public void UpdateLinePath(int firstPoint, Vector3 location, Rotations.Directions direction)
    {
        //if (timesCalledThisUpdate > 0) return;
        //timesCalledThisUpdate++;

        RemoveLinePieces(firstPoint - 1);

        while (location.x <= levelData.xPositiveBorder && location.x >= levelData.xNegativeBorder && location.y <= levelData.yPositiveBorder && location.y >= levelData.yNegativeBorder)
        {
            if (AddLinePiece(location, direction)) return;
            direction = linePieces[linePieces.Count - 1].outDirection;

            switch (direction)
            {
                case Rotations.Directions.North:
                    location = location + new Vector3(0,1,0);
                    break;
                case Rotations.Directions.East:
                    location = location + new Vector3(1, 0, 0);
                    break;
                case Rotations.Directions.South:
                    location = location - new Vector3(0, 1, 0);
                    break;
                case Rotations.Directions.West:
                    location = location - new Vector3(1, 0, 0);
                    break;
                default:
                    break;
            }
        }

        linePieces[linePieces.Count - 1].UpdateLinePiece(LinePiece.Colliders.Square);
    }

    public void RemoveLine()
    {
        RemoveLinePieces(0);
        levelData.lines.Remove(this);
        foreach (var lineChild in lineChilds)
        {
            lineChild.Key.lineParants.Remove(this);
            lineChild.Key.RemoveLine();
        }
        foreach (var lineParent in lineParants)
        {
            lineParent.lineChilds.Remove(this);
        }
    }

    private void RemoveLinePieces(int firstPoint)
    {
        if (linePieces.Count > 0)
        {
            if (linePieces[linePieces.Count - 1].endPoint)
            {
                linePieces[linePieces.Count - 1].endPoint.UpdateSprite(false);
            }
        }

        for (int i = 0; i < lineChilds.Count; i++)
        {
            if (lineChilds.Values.ElementAt(i) > firstPoint)
            {
                lineChilds.Keys.ElementAt(i).RemoveLine();
                i--;
            }
        }

        Dictionary<LinePathSystem, int> linesToUpdate = new Dictionary<LinePathSystem, int>();

        while(linePieces.Count > firstPoint && linePieces.Count > 0)
        {
            if (linePieces[linePieces.Count - 1].interactors.Count > 0)
            {
                for (int x = 0; x < linePieces[linePieces.Count - 1].interactors.Count; x++)
                {
                    if (linePieces[linePieces.Count - 1].interactors[x] is PaintDivider)
                    {
                        linePieces[linePieces.Count - 1].interactors[x].GetComponent<PaintDivider>().incomingLine = null;
                    }

                    else if (linePieces[linePieces.Count - 1].interactors[x] is PaintCombiner)
                    {
                        PaintCombiner paintCombiner = linePieces[linePieces.Count - 1].interactors[x].GetComponent<PaintCombiner>();
                        for (int i = 0; i < paintCombiner.incomingLines.Length; i++)
                        {
                            if (paintCombiner.incomingLines[i] == this)
                            {
                                paintCombiner.incomingLines[i] = null;
                                for (int y = 0; y < paintCombiner.incomingLines.Length; y++)
                                {
                                    if (paintCombiner.incomingLines[y] != null)
                                    {
                                        paintCombiner.CombinePaint();
                                        break;
                                    }
                                }
                                break;
                            }
                        }

                        if (paintCombiner.outGoingLine == this && linePieces.Count == 0)
                        {
                            paintCombiner.outGoingLine = null;
                        }
                    }

                    linePieces[linePieces.Count - 1].interactors[x].interactedLines.Remove(linePieces[linePieces.Count - 1]);
                    linePieces[linePieces.Count - 1].interactors.RemoveAt(x);
                    x--;
                }
            }

            if (levelData.levelLinePieces[linePieces[linePieces.Count - 1].spriteRenderer.transform.position].Count > 0)
            {
                if (linePieces[linePieces.Count - 1].collidingLinePiece != null)
                {
                    if (!linesToUpdate.ContainsKey(linePieces[linePieces.Count - 1].collidingLinePiece.linePathSystem))
                    {
                        linesToUpdate.Add(linePieces[linePieces.Count - 1].collidingLinePiece.linePathSystem, linePieces[linePieces.Count - 1].collidingLinePiece.distance);
                    }
                    linePieces[linePieces.Count - 1].collidingLinePiece.collidingLinePiece = null;
                    linePieces[linePieces.Count - 1].collidingLinePiece = null;
                }
                levelData.levelLinePieces[linePieces[linePieces.Count - 1].spriteRenderer.transform.position].Remove(linePieces[linePieces.Count - 1]);
            }
            else
            {
                levelData.levelLinePieces.Remove(linePieces[linePieces.Count - 1].spriteRenderer.transform.position);
            }

            GameObject.Destroy(linePieces[linePieces.Count - 1].spriteRenderer.gameObject);
            linePieces.RemoveAt(linePieces.Count - 1);
        }

        foreach (var line in linesToUpdate)
        {
            line.Key.UpdateLinePath(line.Value);
        }
    }

    private bool AddLinePiece(Vector3 location, Rotations.Directions direction)
    {
        // End point
        if (levelData.endPoints.ContainsKey(location))
        {
            // End point want this line.
            if (levelData.endPoints[location].wantsColor == lineColor)
            {
                linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], endPoint: levelData.endPoints[location], collider: LinePiece.Colliders.End));

                levelData.endPoints[location].UpdateSprite(true);
                return true;
            }
            // End point doesn't want this line.
            else
            {
                linePieces[linePieces.Count - 1].UpdateLinePiece(LinePiece.Colliders.Square);
                return true;
            }
        }
        // Interactable
        else if (levelData.interactables.ContainsKey(location))
        {
            // Start point
            if (levelData.interactables[location] is StartPoint)
            {
                // Hit other start point
                if (linePieces.Count > 0)
                {
                    linePieces[linePieces.Count - 1].UpdateLinePiece(LinePiece.Colliders.Circle);
                    return true;
                }
                // Lines start point.
                else
                {
                    linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], interactor: levelData.interactables[location], collider: LinePiece.Colliders.Begin));
                    return false;
                }
            }
            // Paint Divider
            else if (levelData.interactables[location] is PaintDivider)
            {
                PaintDivider paintDivider = levelData.interactables[location].GetComponent<PaintDivider>();
                if (linePieces.Count > 0)
                {
                    if (direction == paintDivider.inDirection)
                    {
                        linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], interactor: paintDivider, collider: LinePiece.Colliders.End));
                        paintDivider.DividePaint(this, levelData, linePieces.Count - 1);
                        return true;
                    }
                    else
                    {
                        linePieces[linePieces.Count - 1].UpdateLinePiece(LinePiece.Colliders.Circle,paintDivider);
                        return true;
                    }
                }
                else
                {
                    linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], interactor: paintDivider, collider: LinePiece.Colliders.Begin));
                    return false;
                }
            }
            // Paint Combiner
            else if (levelData.interactables[location] is PaintCombiner)
            {
                PaintCombiner paintCombiner = levelData.interactables[location].GetComponent<PaintCombiner>();
                if (linePieces.Count > 0)
                {
                    if (direction != paintCombiner.outDirection)
                    {
                        if (lineParants.Count > 0)
                        {
                            List<LinePathSystem> parentCheck = new List<LinePathSystem>();
                            parentCheck.AddRange(lineParants);
                            Debug.LogError("Outgoing line: " + paintCombiner.outGoingLine);
                            while (parentCheck.Count > 0)
                            {
                                if (parentCheck[0] == paintCombiner.outGoingLine)
                                {
                                    Debug.LogError("Line is now parent of its parent");
                                    linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], interactor: paintCombiner, collider: LinePiece.Colliders.Square));
                                    return true;
                                }
                                if (parentCheck[0].lineParants.Count > 0)
                                {
                                    parentCheck.AddRange(parentCheck[0].lineParants);
                                }
                                parentCheck.RemoveAt(0);
                            }
                        }

                        linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], interactor: paintCombiner, collider: LinePiece.Colliders.End));
                        paintCombiner.CombinePaint(this, linePieces.Count - 1);
                        return true;
                    }
                    else
                    {
                        linePieces[linePieces.Count - 1].UpdateLinePiece(LinePiece.Colliders.Circle, paintCombiner);
                        return true;
                    }
                }
                else
                {
                    linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], interactor: paintCombiner, collider: LinePiece.Colliders.Begin));
                    return false;
                }
            }
            // Corner piece
            else if (levelData.interactables[location] is CornerPiece)
            {
                CornerPiece cornerPiece = levelData.interactables[location].GetComponent<CornerPiece>();

                // Corner piece angled surface.
                if (direction == cornerPiece.inDirection || direction == cornerPiece.outDirection)
                {
                    int index = 0;
                    if (direction == cornerPiece.inDirection)
                    {
                        index = (int)cornerPiece.outDirection;
                        for (int i = 0; i < 2; i++)
                        {
                            if (index == 3) index = 0;
                            else index++;
                        }
                    }
                    else if (direction == cornerPiece.outDirection)
                    {
                        index = (int)cornerPiece.inDirection;
                        for (int i = 0; i < 2; i++)
                        {
                            if (index == 3) index = 0;
                            else index++;
                        }
                    }

                    linePieces.Add(new LinePiece(this, location, direction, (Rotations.Directions)index, levelData, lineColor, cornerPiece.transform.eulerAngles.z, interactor: cornerPiece));

                    return false;
                }
                // Corner piece flat surface.
                else
                {
                    linePieces[linePieces.Count - 1].UpdateLinePiece(LinePiece.Colliders.Square, cornerPiece);
                    return true;
                }
            }
            // Should never be called.
            return false;
        }
        // End line against map border.
        else if (location.x == levelData.xPositiveBorder && direction == Rotations.Directions.East)
        {
            linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], collider: LinePiece.Colliders.Square));
            return true;
        }
        else if (location.x == levelData.xNegativeBorder && direction == Rotations.Directions.West)
        {
            linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], collider: LinePiece.Colliders.Square));
            return true;
        }
        else if (location.y == levelData.yPositiveBorder && direction == Rotations.Directions.North)
        {
            linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], collider: LinePiece.Colliders.Square));
            return true;
        }
        else if (location.y == levelData.yNegativeBorder && direction == Rotations.Directions.South)
        {
            linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], collider: LinePiece.Colliders.Square));
            return true;
        }
        // Straight line.
        else
        {
            if (levelData.levelLinePieces.ContainsKey(location))
            {
                if (levelData.levelLinePieces[location].Count > 0)
                {
                    foreach (var levelLinePiece in levelData.levelLinePieces[location])
                    {
                        int index = (int)levelLinePiece.outDirection;
                        for (int i = 0; i < 2; i++)
                        {
                            if (index == 3) index = 0;
                            else index++;
                        }
                        if ((Rotations.Directions)index == direction)
                        {
                            linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], collider: LinePiece.Colliders.LineCollision, collidingLinePiece: levelLinePiece));
                            levelLinePiece.UpdateLinePiece(LinePiece.Colliders.LineCollision, collidingLinePiece: linePieces[linePieces.Count - 1]);
                            return true;
                        }
                    }
                }
                linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], collider: LinePiece.Colliders.None));
                return false;
            }
            else
            {
                linePieces.Add(new LinePiece(this, location, direction, direction, levelData, lineColor, levelData.lineRotations[(int)direction], collider: LinePiece.Colliders.None));
                return false;
            }
        }
    }
}
