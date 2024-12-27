using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyIntentionModule : MonoBehaviour
{
    public EnemySkill nextMove;
    public List<EnemySkill> attackSkills = new List<EnemySkill>();
    public List<EnemySkill> moveSkills = new List<EnemySkill>();
    public List<EnemySkill> supportSkills = new List<EnemySkill>();

    public bool justAttacked = false;
    public Image nextMoveIcon;
    public CanvasGroup nextMoveCanvasGroup;

    public void SetNextMove(EnemySkill move)
    {
        nextMove = move;
        nextMoveIcon.sprite = move.skillIcon;
        nextMoveCanvasGroup.DOFade(1, 0.3f)
        .SetEase(Ease.OutSine);

    }

    public void CalculateNextMove()
    {
        PrepareNextIntentionUI();
        if(GetComponent<EnemyRotationModule>().IsWithinRange())
        {
            EnemySkill potentialNextMove = attackSkills[Random.Range(0, attackSkills.Count)];

            if(potentialNextMove != null)
            {
                SetNextMove(potentialNextMove);
            }
        } else
        {
            EnemySkill potentialNextMove = moveSkills[Random.Range(0, moveSkills.Count)];

            if(potentialNextMove != null)
            {
                SetNextMove(potentialNextMove);
            }
        }
    }

    public void PrepareNextIntentionUI()
    {
        nextMoveCanvasGroup.DOFade(0, 0.2f)
        .SetEase(Ease.OutSine);
    }

    public void ExecuteNextMove(Vector3 direction)
    {
        nextMove.UseSkill(direction, gameObject.GetComponent<EnemyBaseComponent>().attackSource, gameObject.GetComponent<EnemyBaseComponent>());
    }
}