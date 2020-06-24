using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinePieceSpriteRule
{
    public Sprite sprite;
    public Rotations.Directions inDirection;
    public Rotations.Directions outDirection;
    public LinePiece.Colliders collider;
}
