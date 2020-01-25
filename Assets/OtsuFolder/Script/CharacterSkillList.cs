using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillList
{
    //public static CharacterSkillList instance;
    public List<CharacterSkill> skillList = new List<CharacterSkill>();

    //private void Awake()
    //{
    //    instance = this;
    //}

    public CharacterSkill SearchName(string name)
    {
        foreach (CharacterSkill skill in skillList)
        {
            if (skill.name.Contains(name))
            {
                return skill;
            }
        }
        return null;
    }
}
