using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseSkillController : MonoBehaviour
{
    public static UseSkillController instance;

    private List<CharacterSkill> skillList;
    private Vector3Int charpos;

    [SerializeField]
    Dropdown selectSkill, target, corjudge, cordam, corcost;
    [SerializeField]
    Text type, cost, range, pow, cor, memo;
    [SerializeField]
    Toggle explosion, cut, chain1, chain2, allatk;

    private void Awake()
    {
        instance = this;
    }

    public void SkillListLoad(List<CharacterSkill> list, Vector3Int pos)
    {
        skillList = list;
        charpos = pos;
        selectSkill.ClearOptions();
        List<string> opt = new List<string>();
        foreach(var item in skillList)
        {
            opt.Add(item.name);
        }
        selectSkill.AddOptions(opt);
        selectSkill.RefreshShownValue();
    }

    public void SkillSelected()
    {
        var selectnum = selectSkill.value;
        var loadSkill = skillList[selectnum];

        string typestr = "";
        switch (loadSkill.type)
        {
            case 0:
                typestr = "攻撃";
                break;
            case 1:
                typestr = "防御";
                break;
            case 2:
                typestr = "支援・妨害";
                break;
            case 3:
                typestr = "移動";
                break;
            case 4:
                typestr = "その他";
                break;
        }
        type.text = typestr;

        cost.text = loadSkill.cost.ToString();
        range.text = loadSkill.rangeMin + "~" + loadSkill.rangeMax;
        pow.text = loadSkill.damage;
        cor.text = loadSkill.correction;
        memo.text = loadSkill.memo;

        explosion.isOn = loadSkill.explosion;
        cut.isOn = loadSkill.cut;
        chain1.isOn = loadSkill.oneCombo;
        chain2.isOn = loadSkill.twoCombo;
        allatk.isOn = loadSkill.areaAttack;
    }

    private void TargetSeach()
    {
        
    }
}
