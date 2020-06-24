using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Dictionary<LinePiece, LinePathSystem> interactedLines = new Dictionary<LinePiece, LinePathSystem>();

    public abstract void Interact();
}
