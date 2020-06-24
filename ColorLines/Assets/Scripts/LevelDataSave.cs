using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class LevelDataSave
{
    public Difficulty difficulty;
    public LevelStatusSave.Status beginStatus;
    public int level;
    public int ID;

    public int cornerPieces;
    public Vector3[] cornerPiecesPosition;
    public quaternion[] cornerPiecesRotation;

    public int paintDiveders;
    public Vector3[] paintDivedersPosition;
    public quaternion[] paintDivedersRotation;

    public int paintCombiners;
    public Vector3[] paintCombinersPosition;
    public quaternion[] paintCombinersRotation;

    public int endPoints;
    public Color[] endPointsColor;
    public Vector3[] endPointsPosition;
    public quaternion[] endPointsRotation;

    public int startPoints;
    public Color[] startPointsColor;
    public Vector3[] startPointsPosition;
    public quaternion[] startPointsRotation;

    public LevelDataSave(Difficulty difficulty,
        LevelStatusSave.Status beginStatus,
        int level, 
        int ID, 

        int cornerPieces, 
        Vector3[] cornerPiecesPosition, 
        quaternion[] cornerPiecesRotation, 

        int endPoints,
        Color[] endPointsColor,
        Vector3[] endPointsPosition,
        quaternion[] endPointsRotation,

        int paintDiveders,
    Vector3[] paintDivedersPosition,
    quaternion[] paintDivedersRotation,

    int paintCombiners,
    Vector3[] paintCombinersPosition,
    quaternion[] paintCombinersRotation,

    int startPoints,
        Color[] startPointsColor,
        Vector3[] startPointsPosition,
        quaternion[] startPointsRotation)
    {
        this.difficulty = difficulty;
        this.beginStatus = beginStatus;
        this.level = level;
        this.ID = ID;

        this.cornerPieces = cornerPieces;
        this.cornerPiecesPosition = cornerPiecesPosition;
        this.cornerPiecesRotation = cornerPiecesRotation;

        this.paintDiveders = paintDiveders;
        this.paintDivedersPosition = paintDivedersPosition;
        this.paintDivedersRotation = paintDivedersRotation;

        this.paintCombiners = paintCombiners;
        this.paintCombinersPosition = paintCombinersPosition;
        this.paintCombinersRotation = paintCombinersRotation;

        this.endPoints = endPoints;
        this.endPointsColor = endPointsColor;
        this.endPointsPosition = endPointsPosition;
        this.endPointsRotation = endPointsRotation;

        this.startPoints = startPoints;
        this.startPointsColor = startPointsColor;
        this.startPointsPosition = startPointsPosition;
        this.startPointsRotation = startPointsRotation;
    }

    public enum Difficulty 
    {
        Easy,
        Medium,
        Hard,
        Extreem
    }

}
