using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGame : MonoBehaviour
{
    public static DataGame instance;

    private void Awake()
    {
        instance = this;
    }
    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt("LEVEL", level);
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt("LEVEL", 0);
    }
}
