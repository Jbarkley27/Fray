using UnityEngine;
using UnityEngine.EventSystems;

public class BasicMoveSkill : Skill, IPointerClickHandler   
{
    public override void UseSkill(Vector3 direction)
    {
        SkillManager.instance.movementSystem.Boost(direction);
    }

    
    public void OnPointerClick(PointerEventData eventData)
    {
        SkillManager.instance.AssignActiveSkill(this);
    }
}
