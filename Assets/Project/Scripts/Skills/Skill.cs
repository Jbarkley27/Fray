using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// make this an interface
public class Skill: MonoBehaviour
{
    public float lineLengthMax;
    public string skillName;
    public Sprite skillIcon;
    public Color skillColor;
    public SkillManager.SkillID skillID;

    public virtual void UseSkill(Vector3 direction){}
}
