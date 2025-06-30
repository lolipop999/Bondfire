using UnityEngine;
using TMPro;

public class SkillInfo : MonoBehaviour
{
    public string description;
    public string title;
    public TMP_Text skillTitle;
    public TMP_Text skillDescription;
    public TMP_Text skillCost;
    private CanvasGroup canvas;
    private SkillSO skillData;
    private SkillSlot skillSlot;
    private int cost;

    void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        if (canvas == null)
        {
            Debug.Log("missing canvas");
        }
        canvas.alpha = 0f;
        skillTitle.SetText(title);
        skillDescription.SetText(description);
        skillSlot = GetComponentInParent<SkillSlot>();
        skillData = skillSlot.skillSO;
        cost = skillData.cost;
        skillCost.SetText("Cost: " + cost.ToString());
    }

    public void ShowData()
    {
        canvas.alpha = 1f;
    }

    public void HideData()
    {
        canvas.alpha = 0f;
    }
}
