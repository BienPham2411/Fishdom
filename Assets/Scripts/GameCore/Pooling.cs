using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    public static Pooling instance;
    [SerializeField] private GameObject[] levels;

    private void Awake()
    {
        instance = this;
    }
    public GameObject GetLevel(int level)
    {
        if(level >= 0 && level < levels.Length)
            return levels[level];
        return null;
    }
}
