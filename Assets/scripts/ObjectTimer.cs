using UnityEngine;
using UnityEngine.UI;

public class ObjectTimer : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonToFill;
    private Image fillImage;
    private float fillTime = 10;
    void Start()
    {
        fillImage = buttonToFill.GetComponent<Image>();
        fillImage.fillAmount = 0.0f;
    }


    public void resetTimer(){
        if (!isOnCooldown)
        {
            fillImage.fillAmount = 0f;
            isOnCooldown = true;
        }
    }

    private bool isOnCooldown = true;
    void Update()
    {
        if (isOnCooldown)
        {
            float val = 1.0f / fillTime * Time.deltaTime;
            if (fillImage.fillAmount + val <= 1.0f)
            {
                fillImage.fillAmount += val;
            }
            else
            {
                fillImage.fillAmount = 1.0f;
                isOnCooldown = false;
            }
        }
    }
}