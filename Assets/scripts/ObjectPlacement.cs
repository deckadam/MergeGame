using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    private GameObject draggingObject;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 restorePoint;
    void Update()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (draggingObject != null)
        {
            Vector3 temp = Input.mousePosition;
            temp.z = 10.0f;
            draggingObject.transform.position = Camera.main.ScreenToWorldPoint(temp);
        }
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0)&&draggingObject==null)
            {
                if (hit.transform.tag == "PlacementObject")
                {
                    restorePoint = hit.transform.position;
                    draggingObject = hit.transform.gameObject;
                    draggingObject.GetComponent<BoxCollider>().enabled = false;
                }
            }
            else if (hit.transform.tag == "DockCube" && draggingObject != null && Input.GetMouseButtonDown(0))
            {
                Block block = draggingObject.GetComponent<Block>();
                GameObject child = hit.transform.GetChild(0).transform.gameObject;
                if (child.transform.childCount == 0)
                {
                    placeDragging(block);
                }
                else if (block.isMergeble(hit.transform.GetChild(0).transform.GetChild(0).gameObject)) {
                    block.merge(child.transform.GetChild(0).gameObject);
                    placeDragging(block);
                }
            }
        }
        if (draggingObject != null && Input.GetMouseButtonDown(1)) {
            draggingObject.transform.position = restorePoint;
            draggingObject.GetComponent<BoxCollider>().enabled = true;
            draggingObject = null;
        }
    }

    void placeDragging(Block block)
    {
        draggingObject.transform.parent = hit.transform.GetChild(0).transform;
        draggingObject.GetComponent<BoxCollider>().enabled = true;
        Vector3 temp = hit.transform.GetChild(0).transform.position;
        Debug.Log(temp.y);
        temp.y += 0.15f*block.level;
        Debug.Log(temp.y);
        draggingObject.transform.position = temp;
        draggingObject = null;
    }
}