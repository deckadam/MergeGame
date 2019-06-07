using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonToFill;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject cubePrefab;
    [SerializeField]
    private GameObject pSystem;

    private Image fillImage;
    [SerializeField]
    private float fillTime = 2;
    public static float fillAmount = 0;
    private bool isOnCooldown = true;
    private bool isCreatedDragging = false;
    private ParticleControllerScript particleController;

    GameObject draggingObject;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 restorePoint;

    //Initing creation button
    void Start()
    {
        fillImage = buttonToFill.GetComponent<Image>();
        fillImage.fillAmount = fillAmount;
        isCreatedDragging = true;
        particleController = pSystem.GetComponent<ParticleControllerScript>();
    }

    //Resting button to 0
    public void resetTimer()
    {
        if (!isOnCooldown&& draggingObject==null)
        {
            fillAmount = 0f;
            isOnCooldown = true;
        }
    }

    //Creating new block with level 1 aspect
    public void createNewObject()
    {
        if (draggingObject == null)
        {
            isCreatedDragging = true;
            draggingObject = Instantiate(cubePrefab);
            draggingObject.transform.rotation = Quaternion.identity;
            draggingObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void Update()
    {
        //Updating timed button
        if (isOnCooldown)
        {
            float val = 1.0f / fillTime * Time.deltaTime;
            if (fillAmount + val <= 1.0f)
            {
                fillAmount += val;
                fillImage.fillAmount = fillAmount;
            }
            else
            {
                fillImage.fillAmount = 1.0f;
                isOnCooldown = false;
            }
        }

        //Reverse planting
        if (Input.GetMouseButtonDown(1))
        {
            if (draggingObject != null)
            {
                if (!isCreatedDragging)
                {
                    restoreObject(draggingObject, restorePoint);
                }
                else
                {
                    Destroy(draggingObject);
                    fillImage.fillAmount = 1.0f;
                    isCreatedDragging = false;
                }
            }
        }

        //Drag selected object
        if (draggingObject != null)
        {
            dragObjectAtCursor(draggingObject);
        }

        //Cast ray
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //Planting and merging operations
            processRaycast(hit);
        }

    }


    //Drag the object
    void dragObjectAtCursor(GameObject obj)
    {
        Vector3 temp = Input.mousePosition;
        temp.z = 10.0f;
        obj.transform.position = Camera.main.ScreenToWorldPoint(temp);
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
                restorePoint = hit.transform.position;
                draggingObject = hit.transform.gameObject;
                draggingObject.GetComponent<BoxCollider>().enabled = false;
                isCreatedDragging = false;
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
                    //When not dragging an object and clicked on a carrying dock
                    else if (hit.transform.GetChild(0).transform.childCount != 0 && hit.transform.GetChild(0).transform.childCount != 0)
                    {
                        draggingObject = hit.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
                    }

                    //Else nothing to do
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
        if (isCreatedDragging)
        {
            PointGeneration.blocks.Add(block);
            isCreatedDragging = false;
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
            inst.transform.localScale = new Vector3(0.166f, 1f, 0.166f);
            inst.transform.position = inst.transform.parent.transform.position + pos;
            inst.GetComponent<Block>().loadFromSave(int.Parse(vals[2]));
            val += int.Parse(vals[2]);
            PointGeneration.blocks.Add(inst.GetComponent<Block>());
        }
        //GameObject parent = DockGenerator.docks[int.Parse(temp[0]), int.Parse(temp[1])].transform.GetChild(0).gameObject;
        draggingObject = null;
        return val;
    }
}