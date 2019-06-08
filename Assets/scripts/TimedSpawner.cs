using UnityEngine;
using UnityEngine.UI;

public class TimedSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject buttonToFill;
    // Start is called before the first frame update
    void Start()
    {
        fillImage = buttonToFill.GetComponent<Image>();
        fillImage.fillAmount = fillAmount;
    }
    private Image fillImage;
    [SerializeField]
    private float fillTime = 2;
    public static float fillAmount = 0;
    private bool isOnCooldown = true;

    //Resting button to 0
    public void resetTimer()
    {
        if (!isOnCooldown && ObjectPlacement.draggingObject == null)
        {
            fillAmount = 0f;
            isOnCooldown = true;
        }
    }
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
                isOnCooldown = true;
            }
            else
            {
                fillAmount = 1.0f;
                isOnCooldown = false;
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
