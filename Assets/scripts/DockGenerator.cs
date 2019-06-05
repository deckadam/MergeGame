using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject dockPrefab;
    [SerializeField]
    private GameObject parentObject;

    private GameObject[,] docks;

    public GameObject[,] Docks { get => docks; set => docks = value; }

    void Start()
    {
        docks = new GameObject[4,4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                docks[i, j] = Instantiate(dockPrefab, new Vector3((i - 2) * 2, 0.25f, (j - 2) * 2),new Quaternion(0,0,0,0),parentObject.transform);
            }
        }
    }

    void Update()
    {
        
    }
}
