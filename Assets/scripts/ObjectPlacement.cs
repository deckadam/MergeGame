using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject cubePrefab;
    [SerializeField]
    private GameObject pSystem;

    private static GameObject startingPrefab;
    private ParticleControllerScript particleController;

    public static GameObject draggingObject;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 restorePoint;

    //Initing creation button
    void Start()
    {
        startingPrefab = cubePrefab;
        particleController = pSystem.GetComponent<ParticleControllerScript>();
    }

    void Update()
    {
        //Drag selected object
        if (draggingObject != null)
        {
            dragObjectAtCursor(draggingObject);
        }

        //Reverse planting
        if (Input.GetMouseButtonDown(1))
        {
            if (draggingObject != null)
            {
                Debug.Log(restorePoint);
                restoreObject(draggingObject, restorePoint);
            }
        }

        //Cast ray
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //Planting and merging operations
            processRaycast(hit);
        }


    }

    public static bool insertNewBlock()
    {
        for (int i = 0; i < DockGenerator.spawnPoints.GetLength(0); i++)
        {
            for (int j = 0; j < DockGenerator.spawnPoints.GetLength(1); j++)
            {
                if (DockGenerator.spawnPoints[i, j].transform.childCount == 0)
                {
                    GameObject newBlock = Instantiate(startingPrefab, DockGenerator.spawnPoints[i, j].transform);
                    newBlock.transform.rotation = Quaternion.identity;
                    Vector3 pos = new Vector3(0, 0.15f, 0);
                    newBlock.transform.position = newBlock.transform.parent.transform.position + pos;
                    PointGeneration.blocks.Add(newBlock.GetComponent<Block>());
                    return true;
                }
            }
        }
        return false;
    }

    //Drag the object
    void dragObjectAtCursor(GameObject obj)
    {
        Vector3 temp = Input.mousePosition;
        temp.z = 10.0f;
        obj.transform.position = Camera.main.ScreenToWorldPoint(temp);
    }

    //Set dragging object to current clicked placement object
    void ifClickedOnPlacementObject(RaycastHit hit)
    {
        Transform temp = hit.transform;
        restorePoint = temp.position;
        draggingObject = temp.gameObject;
        draggingObject.GetComponent<BoxCollider>().enabled = false;
    }

   
    //Set dragging object to clicked docks child block
    void ifClickedOnDockCube(RaycastHit hit)
    {
        Transform temp = hit.transform.GetChild(0).transform;
        if (temp.childCount != 0)
        {
            restorePoint = temp.position;
            draggingObject = temp.GetChild(0).gameObject;
            disableBoxCollider();
        }
    }

    //Raycasting operations
    void processRaycast(RaycastHit hit)
    {
        //Is mouse clicked and mouse not over any ui element
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //Is clicked on a cube when not dragging any object
            if (draggingObject == null && hit.transform.tag == "PlacementObject")
            {
                ifClickedOnPlacementObject(hit);
            }

            else if (draggingObject == null && hit.transform.tag == "DockCube")
            {
                ifClickedOnDockCube(hit);
            }

            else
            {
                //Is clicked to a cube
                if (hit.transform.tag == "DockCube")
                {
                    //When dragging an object
                    if (draggingObject != null)
                    {
                        Block block = draggingObject.GetComponent<Block>();
                        GameObject child = hit.transform.GetChild(0).transform.gameObject;
                        if (child.transform.childCount == 0)
                        {
                            placeDragging(block);
                        }
                        else if (block.isMergeble(hit.transform.GetChild(0).transform.GetChild(0).gameObject))
                        {
                            block.merge(child.transform.GetChild(0).gameObject);
                            particleController.playMergingParticlesAtPosition(block.transform.position);
                            placeDragging(block);
                        }
                    }
                }
                else if (hit.transform.parent != null && hit.transform.parent.tag == "DockPoint")
                {
                    Block block = draggingObject.GetComponent<Block>();
                    GameObject child = hit.transform.gameObject;
                    Debug.Log(block.isMergeble(hit.transform.gameObject));
                    if (block.isMergeble(hit.transform.gameObject))
                    {
                        block.merge(child.transform.gameObject);
                        placeDragging(block);
                    }
                }
            }
        }
    }

    //Place selected object
    void placeDragging(Block block)
    {
        Vector3 temp;
        if (hit.transform.tag == "DockCube")
        {
            draggingObject.transform.parent = hit.transform.GetChild(0).transform;
            temp = hit.transform.GetChild(0).transform.position;
        }
        else
        {
            draggingObject.transform.parent = hit.transform.parent;
            temp = hit.transform.parent.transform.position;
        }
        draggingObject.GetComponent<BoxCollider>().enabled = true;
        temp.y += 0.15f * block.level;
        Debug.Log(temp);
        draggingObject.transform.position = temp;
        draggingObject = null;
    }

    //Restore object to original position when right clicked
    void restoreObject(GameObject val, Vector3 restorePoint)
    {
        val.transform.position = restorePoint;
        val.GetComponent<BoxCollider>().enabled = true;
        draggingObject = null;
    }

    //Setup loaded blocks to docks
    public int placeLoadedBlocks(string data)
    {
        int val = 0;
        string[] temp = data.Split('/');
        for (int i = 0; i < temp.Length - 1; i++)
        {
            string[] vals = temp[i].Split(',');
            Vector3 pos = new Vector3(0, 0.15f * int.Parse(vals[2]), 0);
            GameObject inst = Instantiate(cubePrefab, DockGenerator.docks[int.Parse(vals[0]), int.Parse(vals[1])].transform.GetChild(0));
            inst.transform.position = inst.transform.parent.transform.position + pos;
            inst.GetComponent<Block>().loadFromSave(int.Parse(vals[2]));
            val += int.Parse(vals[2]);
            PointGeneration.blocks.Add(inst.GetComponent<Block>());
        }
        draggingObject = null;
        return val;
    }
    void disableBoxCollider()
    {
        draggingObject.GetComponent<BoxCollider>().enabled = false;
    }

    void enableBoxCollider()
    {
        draggingObject.GetComponent<BoxCollider>().enabled = true;
    }
}