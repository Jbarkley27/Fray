using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBasicMoveSkill : EnemySkill   
{
    public override void UseSkill(Vector3 direction, Transform attackSource)
    {
        SkillManager.instance.movementSystem.Boost(direction);
    }
}
