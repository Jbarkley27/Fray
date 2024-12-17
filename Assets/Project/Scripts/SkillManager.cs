using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

    public static SkillManager instance;
    public List<Skill> skills;
    public Skill activeSkill;
    public Skill defaultMoveSkill;

    public MovementSystem movementSystem;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Skill Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (activeSkill == null)
            AssignActiveSkill(defaultMoveSkill);
    }

    public void AssignActiveSkill(Skill newActiveSkill)
    {
        foreach (Skill skill in skills)
        {
            skill.SetInactive();
        }

        activeSkill = newActiveSkill;
        newActiveSkill.SetActive();
    }

    public void UseActiveSkill(Vector3 direction)
    {
        Debug.Log("Using active skill " + activeSkill.skillName);
        if (activeSkill != null)
        {
            activeSkill.UseSkill(direction);
        }
    }

    public void AddToList(Skill skill)
    {
        skills.Add(skill);
    }
    
}