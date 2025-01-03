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
    public Image nextMoveIcon;
    public CanvasGroup nextMoveCanvasGroup;
    public EnemyMovementModule movementModule;


    private void Start()
    {
        movementModule = GetComponent<EnemyMovementModule>();
    }

    public void SetNextMove(EnemySkill move)
    {
        nextMove = move;
        nextMoveIcon.sprite = move.skillIcon;
        nextMoveCanvasGroup.DOFade(1, 0.5f)
        .SetEase(Ease.OutSine);
    }

    public void CalculateNextMove()
    {
        Debug.Log("Calculating next move for " + gameObject.name);

        // hide the last intention icon
        PrepareNextIntentionUI();

        // clear any current pathfinding happening in the movement module
        movementModule.StopCalculatingPath();

        // start the decision process - we may want to add a brain component so that we can have different types of enemies
        if(movementModule.IsWithinRange())
        {
            EnemySkill potentialNextMove = attackSkills[Random.Range(0, attackSkills.Count)];
            if(potentialNextMove != null) SetNextMove(potentialNextMove);
        } 
        else
        {
            EnemySkill potentialNextMove = moveSkills[Random.Range(0, moveSkills.Count)];

            if(potentialNextMove != null)
            {
                SetNextMove(potentialNextMove);
                movementModule.StartCalculatingPath();
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