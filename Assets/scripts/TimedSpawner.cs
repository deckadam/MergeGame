using UnityEngine;
using UnityEngine.UI;

public class TimedSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject buttonToFill;

    //For displaying dock status at buttons text
    private Text textToShow;
    // Start is called before the first frame update
    void Start()
    {
        fillImage = buttonToFill.GetComponent<Image>();
        fillImage.fillAmount = fillAmount;
        textToShow = buttonToFill.transform.GetChild(0).GetComponent<Text>();
        fillTimeShared = fillTime;
    }
    private Image fillImage;
    [SerializeField]
    private float fillTime = 2;
    public static float fillTimeShared;
    public static float fillAmount = 0;

    // Update is called once per frame
    void Update()
    {
        float val = 1.0f / fillTime * Time.deltaTime;
        if (fillAmount + val < 1.0f)
        {
            fillAmount += val;
            fillImage.fillAmount = fillAmount;
        }
        else
        {
            bool output = ObjectPlacement.insertNewBlock();
            if (output)
            {
                fillAmount = 0;
                textToShow.text = "New Building";
            }
            else
            {
                fillAmount = 1.0f;
                textToShow.text = "Docks are full";
            }
        }
        fillImage.fillAmount = fillAmount;
    }

    public static void refillButton()
    {
        fillAmount = 1.0f;
    }

    public void incrementFillAmount()
    {
        fillAmount += 0.1f;
    }
}
