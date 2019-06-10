using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class BuyMenu : MonoBehaviour
{
    public static int maxLevelReached;
    [SerializeField]
    private Button prefabButton;
    private static Button staticButton;
    private static Image staticDock;
    [SerializeField]
    private Image buyMenuDock;
    [SerializeField]
    private int buyCostMultiplier = 10;
    private static int staticMultiplier;
    private static string template = "Add block : level";

    //For static functions setting non static variables to static equivalent
    private void Start()
    {
        staticMultiplier = buyCostMultiplier;
        staticDock = buyMenuDock;
        staticButton = prefabButton;
        Invoke("callOnStart", 1f);
    }

    //Invoked after a delay waiting for start functions to finish
    private void callOnStart()
    {
        addMenuButtons();
    }

    //Update buttons when new block level unlocked
    public static void updateButtons()
    {
        setButton(maxLevelReached.ToString());
    }

    //Set new button and add listener for instantiating new block
    private static void setButton(string val)
    {

        Button temp = Instantiate(staticButton, staticDock.transform);
        temp.transform.GetChild(0).GetComponent<Text>().text = template + val.ToString();
        temp.name = val;
        temp.onClick.AddListener(delegate { buyMenuButtonClickEvent(int.Parse(temp.name)); });
    }

    //Set buttons according to loaded data maxlevelreached is set by datesave function with maximum readed value
    public static void addMenuButtons()
    {
        for (int i = 0; i < maxLevelReached; i++)
        {
            setButton((i+1).ToString());
        }
    }

    //Event for buy menu buttons to add new blocks
    private static void buyMenuButtonClickEvent(int newBlockLevel)
    {
        if (PointGeneration.totalPoint >= newBlockLevel * staticMultiplier)
        {
            bool output = ObjectPlacement.placeBuyedBlock(newBlockLevel);
            if (output)
            {
                PointGeneration.totalPoint -= newBlockLevel * staticMultiplier;
                PointGeneration.immediateUpdate();
            }
        }
    }
    [SerializeField]
    private RectTransform panelAndScrollBarRect;
    private bool isOpen = true;

    //Button event for hiding or showing the menu panel
    public void hideOrShow()
    {
        if (!isOpen)
        {
            panelAndScrollBarRect.DOAnchorPos(new Vector2(400, 0), 0.4f);
            isOpen = true;
        }
        else
        {
            panelAndScrollBarRect.DOAnchorPos(new Vector2(0, 0), 0.4f);
            isOpen = false;
        }
    }
}
