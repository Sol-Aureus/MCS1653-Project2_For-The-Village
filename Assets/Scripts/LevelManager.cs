using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Path")]
    public Transform[] points;

    private void Awake()
    {
        instance = this;
    }
}
