using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Skill skill;
    public Image skillImage;
    public Image skillBackground;
    public GameObject skillBorder;

    private void Awake() {
        skillBorder.SetActive(false);
    }

    public void SetupUI(Skill skill)
    {
        this.skill = skill;
        skillImage.sprite = skill.skillIcon;
        skillBackground.color = skill.skillColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillBorder.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillBorder.SetActive(false);
    }

}