using UnityEngine;
using UnityEngine.EventSystems;

public class SkillDataManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillInfo skillInfo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillInfo.ShowData();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillInfo.HideData();
    }
}
