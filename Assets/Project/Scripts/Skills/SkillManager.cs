using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

    public static SkillManager instance;
    public List<SkillID> skillIDs;
    public List<Skill> skills;
    public Skill activeSkill;
    public Skill coreSkill;
    public int maxSkillSize = 20;

    public enum SkillID
    {
        BasicMoveSkill,
        BasicShootSkill
    }

    public enum EnemySkillID
    {
        BasicMoveSkill,
        BasicShootSkill
    }

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

    private void Start()
    {
        RefreshSkillList();
    }

    private void Update()
    {

    }

    public void AssignActiveSkill(Skill newActiveSkill)
    {
        if (newActiveSkill == activeSkill) return;
        activeSkill = newActiveSkill;
    }

    public void UseActiveSkill(Vector3 direction)
    {
        Debug.Log("Using active skill " + activeSkill.skillName);
        if (activeSkill != null)
        {
            activeSkill.UseSkill(direction);
            DeckManager.instance.HideAllSkillBorders();
        }
    }

    public void AddToList(Skill skill)
    {
        if (skills.Count >= maxSkillSize)
        {
            Debug.LogError("Skill list is full, cannot add more skills");
            return;
        }
        skills.Add(skill);
    }

    public void RefreshSkillList()
    {
        skills.Clear();
        foreach (SkillID skillID in skillIDs)
        {
            Skill skill = SkillLibrary.GetSkillFromID(skillID);
            if (skill != null)
            {
                skills.Add(skill);
            } else {
                Debug.LogError("Could not find skill with ID: " + skillID);
            }
        }
    }

    public void RemoveActiveSkill()
    {
        List<SkillUI> handAndCoreSkillsList = DeckManager.instance.hand;

        foreach(SkillUI skillUI in handAndCoreSkillsList)
        {
            skillUI.HideActiveBorder();
        }
        activeSkill = null;
    }
    
}