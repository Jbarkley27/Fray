using UnityEngine;

public class EnemyIntentionModule : MonoBehaviour
{
    public EnemySkill nextMove;

    public void SetNextMove(EnemySkill move)
    {
        nextMove = move;
    }

    public void ExecuteNextMove(Vector3 direction)
    {
        nextMove.UseSkill(direction, gameObject.GetComponent<EnemyBaseComponent>().attackSource);
    }
}