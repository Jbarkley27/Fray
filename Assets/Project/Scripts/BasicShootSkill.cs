using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShootSkill : Skill
{

    public override void UseSkill(Vector3 direction)
    {
        Debug.Log("Shooting in direction: " + direction);
    }

    public void OnPointerClick()
    {
        SkillManager.instance.AssignActiveSkill(this);
    }
}
