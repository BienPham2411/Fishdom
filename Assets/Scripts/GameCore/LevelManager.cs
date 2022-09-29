using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class LevelManager : MonoBehaviour
{
    public Mode mode;
    public MiniMode miniMode;
    public Collider2D playZone;

    public int targetEnemies;
    public int currentEnemies;
    public Mode GetMode()
    {
        return mode;
    }

    public MiniMode GetMiniMode()
    {
        return miniMode;
    }
}

//public class Client : MonoBehaviour
//{
//    public Group Group;
//    public bool GroupOneData;
//    public float GroupTwoData;
//    public string IndependentData;
//}

[CustomEditor(typeof(LevelManager))]
[CanEditMultipleObjects]
public class ClientEditor : Editor
{


    public override void OnInspectorGUI()
    {
        LevelManager level = (LevelManager)target;

        // Display dropdown
        level.mode = (Mode)EditorGUILayout.EnumPopup("Mode", level.mode);
        level.miniMode = (MiniMode)EditorGUILayout.EnumPopup("MiniMode", level.miniMode);
        // Display conditional for one    
        if (level.miniMode == MiniMode.Classic)
        {
            level.targetEnemies = EditorGUILayout.IntField("Target", level.targetEnemies);
            level.currentEnemies = EditorGUILayout.IntField("Current", level.currentEnemies);
        }
        // Display conditional for two
        //if (level.Group == Group.Two)
        //{
        //    level.GroupTwoData = EditorGUILayout.FloatField("Float", level.GroupTwoData);
        //}
        ////// Display always
        //level.playZone = (Collider2D) EditorGUILayout.ObjectField("PlayZone", level.playZone, level.playZone.GetType(), true);
    }
}
