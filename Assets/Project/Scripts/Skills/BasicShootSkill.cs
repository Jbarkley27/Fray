using UnityEngine;
using UnityEngine.EventSystems;

public class BasicShootSkill : Skill, IPointerClickHandler
{
    public override void UseSkill(Vector3 direction)
    {
        Debug.Log("Shooting in direction: " + direction);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SkillManager.instance.AssignActiveSkill(this);
    }
}
