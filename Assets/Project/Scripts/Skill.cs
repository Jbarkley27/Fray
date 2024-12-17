using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// make this an interface
public class Skill : MonoBehaviour
{
    public float lineLengthMax;
    public string skillName;
    public Image activeBorder;

    public void Awake()
    {
        activeBorder.enabled = false;
    }

    public void Start()
    {
        SkillManager.instance.AddToList(this);
    }

    public void SetActive()
    {
        activeBorder.enabled = true;
    }

    public void SetInactive()
    {
        activeBorder.enabled = false;
    }

    public virtual void UseSkill(Vector3 direction)
    { }
}
