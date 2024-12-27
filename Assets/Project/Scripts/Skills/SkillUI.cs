using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillUI : MonoBehaviour, IPointerClickHandler
{
    public Skill skill;
    public Image skillImage;
    public Image skillBackground;
    public GameObject skillBorder;
    public CanvasGroup canvasGroup;

    private void Awake() {
        skillBorder.SetActive(false);
    }

    public void SetupUI(Skill skill)
    {
        this.skill = skill;
        skillImage.sprite = skill.skillIcon;
        skillBackground.color = skill.skillColor;
    }


    public void HideActiveBorder()
    {
        if(!DeckManager.instance.IsInHand(skill)) return;
        skillBorder.SetActive(false);
    }

    public void ShowActiveBorder()
    {
        if(!DeckManager.instance.IsInHand(skill)) return;
        skillBorder.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!DeckManager.instance.IsInHand(skill)) return;
        Debug.Log("Clicked on " + skill.skillName);
        DeckManager.instance.HideAllSkillBorders();
        SkillManager.instance.AssignActiveSkill(skill);
        ShowActiveBorder();
    }
}