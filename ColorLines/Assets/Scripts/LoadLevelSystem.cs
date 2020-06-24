using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class LoadLevelSystem : MonoBehaviour
{
    [SerializeField] LevelData levelData;
    [SerializeField] string difficulty = "Easy";
    [SerializeField] int level = 1;
    [SerializeField] int ID = 0;

    [SerializeField] GameObject cornerpieceGameObject;
    [SerializeField] GameObject paintDividerGameObject;
    [SerializeField] GameObject paintCombinerGameObject;
    [SerializeField] GameObject startPointGameObject;
    [SerializeField] GameObject endPointGameObject;

    public void LoadLevel()
    {
        LevelDataSave levelDataSave;
        if (SelectedLevel.levelDataSave != null)
        {
            levelDataSave = SelectedLevel.levelDataSave;
        }
        else
        {
            levelDataSave = SaveSystem.LoadDataLevel("/Levels/" + difficulty, difficulty + ID.ToString());
            SelectedLevel.SelectLevel(levelDataSave);
        }

        for (int i = 0; i < levelDataSave.cornerPieces; i++)
        {
            GameObject justPlaced = Instantiate(cornerpieceGameObject, levelData.interactorsParent);

            justPlaced.transform.position = levelDataSave.cornerPiecesPosition[i];
            justPlaced.transform.rotation = levelDataSave.cornerPiecesRotation[i];

            CornerPiece cornerPiece = justPlaced.GetComponent<CornerPiece>();
            cornerPiece.Setup();

            levelData.interactables.Add(justPlaced.transform.position, cornerPiece);
        }

        for (int i = 0; i < levelDataSave.paintDiveders; i++)
        {
            GameObject justPlaced = Instantiate(paintDividerGameObject, levelData.interactorsParent);

            justPlaced.transform.position = levelDataSave.paintDivedersPosition[i];
            justPlaced.transform.rotation = levelDataSave.paintDivedersRotation[i];

            PaintDivider paintDivider = justPlaced.GetComponent<PaintDivider>();
            paintDivider.Setup();

            levelData.interactables.Add(justPlaced.transform.position, paintDivider);
        }

        for (int i = 0; i < levelDataSave.paintCombiners; i++)
        {
            GameObject justPlaced = Instantiate(paintCombinerGameObject, levelData.interactorsParent);

            justPlaced.transform.position = levelDataSave.paintCombinersPosition[i];
            justPlaced.transform.rotation = levelDataSave.paintCombinersRotation[i];

            PaintCombiner paintCombiner = justPlaced.GetComponent<PaintCombiner>();
            paintCombiner.Setup(levelData);

            levelData.interactables.Add(justPlaced.transform.position, paintCombiner);
        }

        for (int i = 0; i < levelDataSave.endPoints; i++)
        {
            GameObject justPlaced = Instantiate(endPointGameObject, levelData.endPointsParent);
            EndPoint endPoint = justPlaced.GetComponent<EndPoint>();
            endPoint.wantsColor = levelDataSave.endPointsColor[i];

            justPlaced.transform.position = levelDataSave.endPointsPosition[i];
            justPlaced.transform.rotation = levelDataSave.endPointsRotation[i];

            endPoint.Setup();
            levelData.endPoints.Add(justPlaced.transform.position, endPoint);
        }

        for (int i = 0; i < levelDataSave.startPoints; i++)
        {
            GameObject justPlaced = Instantiate(startPointGameObject, levelData.interactorsParent);
            StartPoint startPoint = justPlaced.GetComponent<StartPoint>();
            startPoint.emitingColor = levelDataSave.startPointsColor[i];

            justPlaced.transform.position = levelDataSave.startPointsPosition[i];
            justPlaced.transform.rotation = levelDataSave.startPointsRotation[i];

            startPoint.Setup();
            levelData.interactables.Add(justPlaced.transform.position, startPoint);
            levelData.startPoints.Add(justPlaced.transform.position, startPoint);
        }
    }
}
