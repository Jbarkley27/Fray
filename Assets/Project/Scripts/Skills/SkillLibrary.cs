using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SkillLibrary : MonoBehaviour
{
    public static SkillLibrary instance;

    public static Dictionary<SkillManager.SkillID, Skill> skillDictionary = new Dictionary<SkillManager.SkillID, Skill>();

    [Header("Skills")]
    public Skill basicMoveSkill;
    public Skill basicShootSkill;

    private void Awake()
    {
        instance = this;
        PrepareLibrary();
    }

    public void PrepareLibrary()
    {
        skillDictionary.Clear();
        skillDictionary.Add(SkillManager.SkillID.BasicMoveSkill, basicMoveSkill);
        skillDictionary.Add(SkillManager.SkillID.BasicShootSkill, basicShootSkill);
    }

    public static Skill GetSkillFromID(SkillManager.SkillID skillID)
    {
        Skill newSkill = skillDictionary[skillID];

        if(newSkill != null)
        {
            return Instantiate(newSkill, GlobalDataStore.instance.skillParent);
        }

        return null;
    }
}