using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [HideInInspector] public Dictionary<Vector3,Interactable> interactables = new Dictionary<Vector3, Interactable>();
    [HideInInspector] public Dictionary<Vector3, StartPoint> startPoints = new Dictionary<Vector3, StartPoint>();
    [HideInInspector] public Dictionary<Vector3, EndPoint> endPoints = new Dictionary<Vector3, EndPoint>();
    [HideInInspector] public List<LinePathSystem> lines = new List<LinePathSystem>();
    [HideInInspector] public Dictionary<Vector3, List<LinePiece>> levelLinePieces = new Dictionary<Vector3, List<LinePiece>>();

    public float xPositiveBorder = 5;
    public float xNegativeBorder = 5;
    public float yPositiveBorder = 10;
    public float yNegativeBorder = 10;

    public GameObject lineGameObject;
    [Space]
    public LinePieceSpriteRule[] linePieceSpriteRules;
    public LinePieceSpriteRule linePieceSpriteRuleDefault;
    public float[] lineRotations = new float[4];
    [Space]
    public Transform lineParent;
    public Transform interactorsParent;
    public Transform endPointsParent;
    [Space]
    public GameObject victoryPanel;
}
