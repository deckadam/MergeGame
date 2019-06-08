﻿using UnityEngine;

public class DockGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject dockPrefab;
    [SerializeField]
    private GameObject parentObject;

    public static GameObject[,] docks;
    public static GameObject[,] spawnPoints;
    

    void Start()
    {
        docks = new GameObject[4, 4];
        spawnPoints = new GameObject[4, 4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                docks[i, j] = Instantiate(dockPrefab, new Vector3((i - 2) * 2, 0.15f, (j - 2) * 2), new Quaternion(0, 0, 0, 0), parentObject.transform);
                docks[i, j].gameObject.name = i + ","+j;
                spawnPoints[i, j] = docks[i, j].transform.GetChild(0).gameObject;
            }
        }
    }

    void Update()
    {

    }
}
