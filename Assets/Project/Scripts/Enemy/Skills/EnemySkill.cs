using UnityEngine;


// make this an interface
public class EnemySkill: MonoBehaviour
{
    public string skillName;
    public Sprite skillIcon;
    public SkillManager.EnemySkillID skillID;

    public virtual void UseSkill(Vector3 direction, Transform attackSource, EnemyBaseComponent enemyBaseComponent){}
}
