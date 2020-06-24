using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SaveLevel : MonoBehaviour
{
    [SerializeField] LevelDataSave.Difficulty difficulty = LevelDataSave.Difficulty.Easy;
    [SerializeField] int level = 1;
    [SerializeField] int ID = 0;
    [SerializeField] LevelStatusSave.Status levelStatus = LevelStatusSave.Status.Locked;
    [SerializeField] int lastUsedID = 0;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("lastUSedID"))
        {
            lastUsedID = PlayerPrefs.GetInt("LastUseID");
        }
    }

    public void SaveLevelData()
    {
        lastUsedID = ID;
        PlayerPrefs.SetInt("LastUsedID", ID);

        CornerPiece[] cornerPieces = FindObjectsOfType<CornerPiece>();
        Vector3[] cornerPiecesPosition = new Vector3[cornerPieces.Length];
        quaternion[] cornerPiecesRotation = new quaternion[cornerPieces.Length];

        for (int i = 0; i < cornerPieces.Length; i++)
        {
            cornerPiecesPosition[i] = cornerPieces[i].transform.position;
            cornerPiecesRotation[i] = cornerPieces[i].transform.rotation;
        }

        PaintDivider[] paintDividers = FindObjectsOfType<PaintDivider>();
        Vector3[] paintDividersPosition = new Vector3[paintDividers.Length];
        quaternion[] paintDividersRotation = new quaternion[paintDividers.Length];

        for (int i = 0; i < paintDividers.Length; i++)
        {
            paintDividersPosition[i] = paintDividers[i].transform.position;
            paintDividersRotation[i] = paintDividers[i].transform.rotation;
        }

        PaintCombiner[] paintCombiners = FindObjectsOfType<PaintCombiner>();
        Vector3[] paintCombinersPosition = new Vector3[paintCombiners.Length];
        quaternion[] paintCombinersRotation = new quaternion[paintCombiners.Length];

        for (int i = 0; i < paintCombiners.Length; i++)
        {
            paintCombinersPosition[i] = paintCombiners[i].transform.position;
            paintCombinersRotation[i] = paintCombiners[i].transform.rotation;
        }

        EndPoint[] endPoints = FindObjectsOfType<EndPoint>();
        Color[] endPointsColor = new Color[endPoints.Length];
        Vector3[] endPointsPosition = new Vector3[endPoints.Length];
        quaternion[] endPointsRotation = new quaternion[endPoints.Length];

        for (int i = 0; i < endPoints.Length; i++)
        {
            endPointsColor[i] = endPoints[i].wantsColor;
            endPointsPosition[i] = endPoints[i].transform.position;
            endPointsRotation[i] = endPoints[i].transform.rotation;
        }

        StartPoint[] startPoints = FindObjectsOfType<StartPoint>();
        Color[] startPointsColor = new Color[startPoints.Length];
        Vector3[] startPointsPosition = new Vector3[startPoints.Length];
        quaternion[] startPointsRotation = new quaternion[startPoints.Length];

        for (int i = 0; i < startPoints.Length; i++)
        {
            startPointsColor[i] = startPoints[i].emitingColor;
            startPointsPosition[i] = startPoints[i].transform.position;
            startPointsRotation[i] = startPoints[i].transform.rotation;
        }

        SaveSystem.SaveDataLevel("/Levels/" + difficulty,difficulty + ID.ToString(), 
            new LevelDataSave
            (difficulty,
            levelStatus,
            level,
            ID, 

            cornerPieces.Length,
            cornerPiecesPosition,
            cornerPiecesRotation,

            endPoints.Length,
            endPointsColor,
            endPointsPosition,
            endPointsRotation,

            paintDividers.Length,
            paintDividersPosition,
            paintDividersRotation,

            paintCombiners.Length,
            paintCombinersPosition,
            paintCombinersRotation,

            startPoints.Length,
            startPointsColor,
            startPointsPosition,
            startPointsRotation));
    }
}
