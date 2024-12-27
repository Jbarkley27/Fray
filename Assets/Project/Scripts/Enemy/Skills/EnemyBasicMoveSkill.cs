using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBasicMoveSkill : EnemySkill   
{
    public override void UseSkill(Vector3 direction, Transform attackSource, EnemyBaseComponent enemyBaseComponent)
    {
        enemyBaseComponent.rotationModule.MoveToPlayer();
    }
}
