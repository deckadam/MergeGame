using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OfflineGeneratedPointDisplay : MonoBehaviour
{
    public Text displayText;
    private RectTransform panelRectTransform;
    public void setGeneratedPointText(int val)
    {
        if (DateSave.generatedPoint.ToString() != null)
        {
            displayText.text = val.ToString();
            panelRectTransform = gameObject.GetComponent<RectTransform>();
        }
    }

    public void PointGeneratedButton()
    {
        panelRectTransform.DOAnchorPos(new Vector2(-200,75),0.25f);
    }
}
