using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillList : MonoBehaviour
{
    public List<CharacterSkill> skillList = new List<CharacterSkill>();

    //引数のドロップダウンにスキル名を表示
    public void CreateDropdown(Dropdown drop)
    {
        drop.ClearOptions();
        List<string> dropDownNameList = new List<string>();
        foreach (CharacterSkill skill in skillList)
        {
            dropDownNameList.Add(skill.name);
        }
        drop.AddOptions(dropDownNameList);
    }

    //引数のstringをスキルリストnameから検索、完全一致ではない？
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
