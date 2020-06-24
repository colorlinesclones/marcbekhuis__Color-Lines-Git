using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectedLevel
{
    public static LevelDataSave levelDataSave;

    public static void SelectLevel(LevelDataSave levelDataSave)
    {
        SelectedLevel.levelDataSave = levelDataSave;
    }
}
