using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject cubePrefab;
    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material highlightedMaterial;

    private static GameObject startingPrefab;

    private GameObject draggingObject;
    private RaycastHit hit;
    private Ray ray;

    //Initing creation button
    void Start()
    {
        startingPrefab = cubePrefab;
    }

    void Update()
    {
        //Highlight docks with changing material to show merge options
        highlightDocks();
        //Drag selected object
        if (draggingObject != null)
        {
            dragObjectAtCursor(draggingObject);
        }

        //Reverse planting
        if (Input.GetMouseButtonDown(1) && draggingObject != null)
        {
            restoreObject(draggingObject);
        }

        //Cast ray
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //Planting and merging operations
            processRaycast(hit);
        }
    }


    //Change material of the docks according to dragging object and child blocks level
    private void highlightDocks()
    {
        for (int i = 0; i < DockGenerator.dockRenderer.GetLength(0); i++)
        {
            for (int j = 0; j < DockGenerator.dockRenderer.GetLength(1); j++)
            {
                MeshRenderer temp = DockGenerator.dockRenderer[i, j];
                if (draggingObject != null && DockGenerator.docks[i, j].transform.GetChild(0).transform.childCount != 0)
                {
                    Block current = DockGenerator.docks[i, j].transform.GetChild(0).transform.GetChild(0).GetComponent<Block>();
                    if (current != draggingObject.GetComponent<Block>() && current.level == draggingObject.GetComponent<Block>().level)
                    {
                        temp.material = highlightedMaterial;
                    }
                    else
                    {
                        temp.material = normalMaterial;
                    }
                }
                else
                {
                    temp.material = normalMaterial;
                }
            }
        }
    }


    //Insert new block if there is an empty dock available (Run through all if found place and return)
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
        draggingObject = temp.gameObject;
        draggingObject.GetComponent<BoxCollider>().enabled = false;
    }


    //Set dragging object to clicked docks child block
    void ifClickedOnDockCube(RaycastHit hit)
    {
        Transform temp = hit.transform.GetChild(0).transform;
        if (temp.childCount != 0)
        {
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
                        //If clicked on a empty dock place the cube
                        if (child.transform.childCount == 0)
                        {
                            placeDraggingObject(block);
                        }
                        //If clicked on a carrying dock merge if possible
                        else if (block.isMergeble(hit.transform.GetChild(0).transform.GetChild(0).gameObject))
                        {
                            mergeSelectedBlocks(child.transform.GetChild(0).gameObject, block);
                        }
                    }
                }
                //If clicked on a existing cube merge if possible
                else if (hit.transform.parent != null && hit.transform.parent.tag == "DockPoint")
                {
                    Block block = draggingObject.GetComponent<Block>();
                    GameObject child = hit.transform.gameObject;
                    if (block.isMergeble(hit.transform.gameObject))
                    {
                        mergeSelectedBlocks(child.transform.gameObject, block);
                    }
                }
            }
        }
    }

    void placeDraggingObject(Block block)
    {
        block.gameObject.transform.parent = hit.transform.GetChild(0).transform;
        block.setPosition();
        draggingObject = null;
    }

    //Merge blocks, enable box collider and reset dragging object
    void mergeSelectedBlocks(GameObject other, Block block)
    {
        block.merge(other);
        enableBoxCollider();
        draggingObject = null;
    }

    //Restore object to original position when right clicked
    void restoreObject(GameObject val)
    {
        val.GetComponent<Block>().setPosition();
        enableBoxCollider();
        draggingObject = null;
    }

    //Setup loaded blocks to docks
    public int[] placeLoadedBlocks(string data)
    {
        int val = 0;
        int maxVal = 1;
        string[] temp = data.Split('/');
        for (int i = 0; i < temp.Length - 1; i++)
        {
            string[] vals = temp[i].Split(',');
            if (int.Parse(vals[2]) > maxVal) maxVal = int.Parse(vals[2]);
            val += int.Parse(vals[2]);
            placeBlockWithLevel(int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2]));
        }
        return new int[] { val, maxVal };
    }

    //Place block with given level to given spawn point
    private static bool placeBlockWithLevel(int i, int j, int level)
    {
        if (DockGenerator.spawnPoints[i, j].transform.childCount == 0)
        {
            GameObject inst = Instantiate(startingPrefab, DockGenerator.spawnPoints[i, j].transform);
            inst.GetComponent<Block>().loadFromSave(level);
            PointGeneration.blocks.Add(inst.GetComponent<Block>());
            return true;
        }
        else return false;
    }

    //Place block when buyed if there is an empty dock else return false
    public static bool placeBuyedBlock(int level)
    {
        for (int i = 0; i < DockGenerator.spawnPoints.GetLength(0); i++)
        {
            for (int j = 0; j < DockGenerator.spawnPoints.GetLength(1); j++)
            {
                if (DockGenerator.spawnPoints[i, j].transform.childCount == 0)
                {
                    bool output = placeBlockWithLevel(i, j, level);
                    return true;
                }
            }
        }
        return false;
    }

    //Disable box collider for dragging
    void disableBoxCollider()
    {
        draggingObject.GetComponent<BoxCollider>().enabled = false;
    }


    //Enable box collider when dropped
    void enableBoxCollider()
    {
        draggingObject.GetComponent<BoxCollider>().enabled = true;
    }
}