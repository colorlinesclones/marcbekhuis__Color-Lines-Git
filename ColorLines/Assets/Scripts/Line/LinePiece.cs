using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LinePiece
{
    public LinePiece(LinePathSystem linePathSystem, Vector3 location, Rotations.Directions inDirection, Rotations.Directions outDirection, LevelData levelData, Color color, float rotationZ, Colliders collider = Colliders.None, Interactable interactor = null, EndPoint endPoint = null, LinePiece collidingLinePiece = null)
    {
        this.inDirection = inDirection;
        this.outDirection = outDirection;
        this.distance = linePathSystem.linePieces.Count;
        this.endPoint = endPoint;
        this.levelData = levelData;
        this.linePathSystem = linePathSystem;
        this.collider = collider;
        this.color = color;
        this.collidingLinePiece = collidingLinePiece;

        if (interactor != null)
        {
            if (interactor.interactedLines.Count > 0)
            {
                for (int i = 0; i < interactor.interactedLines.Count; i++)
                {
                    if (interactor.interactedLines.Values.ElementAt(i) == linePathSystem)
                    {
                        if (interactor.interactedLines.Keys.ElementAt(i).distance == distance - 1)
                        {
                            interactor.interactedLines.Remove(interactor.interactedLines.Keys.ElementAt(i));
                            break;
                        }
                    }
                }
            }
            if (!interactor.interactedLines.ContainsKey(this))
            {
                interactor.interactedLines.Add(this, linePathSystem);
                interactors.Add(interactor);
            }
        }

        GameObject justplaced = GameObject.Instantiate(levelData.lineGameObject, location, new Quaternion(0, 0, 0, 0), levelData.lineParent);

        if (levelData.levelLinePieces.ContainsKey(location))
        {
            levelData.levelLinePieces[location].Add(this);
        }
        else
        {
            levelData.levelLinePieces.Add(location, new List<LinePiece> {this });
        }

        justplaced.name = "Line " + linePathSystem.lineNumber.ToString() + " Piece " + distance.ToString() + " Color " + color.ToString();
        justplaced.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        spriteRenderer = justplaced.GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public SpriteRenderer spriteRenderer;
    public Rotations.Directions inDirection;
    public Rotations.Directions outDirection;
    public List<Interactable> interactors = new List<Interactable>();
    public EndPoint endPoint;
    public int distance;
    public Color color;
    public Colliders collider;
    public LinePathSystem linePathSystem;
    public LinePiece collidingLinePiece;
    LevelData levelData;

    public void UpdateSprite()
    {
        foreach (var linePieceSpriteRule in levelData.linePieceSpriteRules)
        {
            int index = (int)inDirection;
            int outdex = (int)outDirection;
            for (int i = 0; i < 4; i++)
            {
                if (linePieceSpriteRule.inDirection == (Rotations.Directions)index && linePieceSpriteRule.outDirection == (Rotations.Directions)outdex)
                {
                    if (collider == linePieceSpriteRule.collider)
                    {
                        spriteRenderer.color = color;
                        spriteRenderer.sprite = linePieceSpriteRule.sprite;
                        return;
                    }
                }
                if (index == 3) index = 0;
                else index++;
                if (outdex == 3) outdex = 0;
                else outdex++;
            }
        }
        spriteRenderer.color = color;
        spriteRenderer.sprite = levelData.linePieceSpriteRuleDefault.sprite;
        return;
    }

    public void UpdateLinePiece(Colliders collider, Interactable interactor = null, LinePiece collidingLinePiece = null)
    {
        this.collider = collider;
        if (this.interactors.Contains(interactor))
        {
            this.interactors[this.interactors.IndexOf(interactor)].interactedLines.Remove(this);
            this.interactors.Remove(interactor);
        }
        else if (interactor != null)
        {
            if (interactor.interactedLines.Count > 0)
            {
                for (int i = 0; i < interactor.interactedLines.Count; i++)
                {
                    if (interactor.interactedLines.Values.ElementAt(i) == linePathSystem)
                    {
                        if (interactor.interactedLines.Keys.ElementAt(i).distance == distance - 1)
                        {
                            interactor.interactedLines.Remove(interactor.interactedLines.Keys.ElementAt(i));
                            break;
                        }
                    }
                }
            }
            this.interactors.Add(interactor);
            interactor.interactedLines.Add(this, linePathSystem);
        }

        if (collidingLinePiece != null)
        {
            this.collidingLinePiece = collidingLinePiece;
            collidingLinePiece.collidingLinePiece = this;
        }

        UpdateSprite();
    }

    public enum Colliders 
    {
        None,
        Square,
        Circle,
        Begin,
        End,
        LineCollision
    }

}
