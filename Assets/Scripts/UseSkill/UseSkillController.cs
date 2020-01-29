using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseSkillController : MonoBehaviour
{
    public static UseSkillController instance;

    private List<CharacterSkill> skillList;
    private List<CharacterItem> targetList;

    [SerializeField]
    Dropdown selectSkill, target, corjudge, cordam, corcost;
    [SerializeField]
    Text type, cost, range, pow, cor, memo;
    [SerializeField]
    Toggle explosion, cut, chain1, chain2, allatk;

    CharacterSkill UseSkill;
    CharacterItem TargetPiece;

    private void Awake()
    {
        instance = this;
    }

    public void SkillListLoad(List<CharacterSkill> list)
    {
        skillList = list;
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
        pow.text = loadSkill.damage.ToString();
        cor.text = loadSkill.correction.ToString();
        memo.text = loadSkill.memo;

        explosion.isOn = loadSkill.explosion;
        cut.isOn = loadSkill.cut;
        chain1.isOn = loadSkill.oneCombo;
        chain2.isOn = loadSkill.twoCombo;
        allatk.isOn = loadSkill.areaAttack;

        TargetSeach(loadSkill);
    }

    private void TargetSeach(CharacterSkill skill)
    {
        target.ClearOptions();
        targetList.Clear();

        var anum = GameManager.ci.areanum;
        targetList = GameManager.instance.inRangeCharacter(anum, skill.rangeMin, skill.rangeMax);
        List<string> opt = new List<string>();
        foreach(var item in targetList)
        {
            opt.Add(item.data.name);
        }
        target.AddOptions(opt);
        target.RefreshShownValue();
    }

    public void UseSkillButtonClick()
    {
        UseSkill = skillList[selectSkill.value];
        TargetPiece = targetList[target.value];
        TypeSpritter();
    }

    private void TypeSpritter()
    {
        switch (UseSkill.type)
        {
            case 0: // 攻撃
                StartCoroutine(Attack());
                break;
            case 1: // 防御
                GuardMethod();
                break;
            case 2: // 支援
                BuffMethod();
                break;
            case 3: // 移動
                MoveMethod();
                break;
            case 4: // その他
                break;
        }
    }

    IEnumerator Attack()
    {
        AttackMethod();
        while (GameManager.state == GameState.InterruptPhase)
        {
            yield return null;
        }
        AttackRecalc();
        while(GameManager.state == GameState.AttackReCulc)
        {
            yield return null;
        }
        thdice = 0;
        pname = "";
        DestroyCheck();
    }

    int thdice;
    string pname;
    private void AttackMethod()
    {
        pname = GameManager.ci.data.name;
        GameManager.state = GameState.AttackPhase;
        var dam = UseSkill.damage + (cordam.value - 3);
        var cost = UseSkill.cost + (corcost.value - 3);
        var cordice = corjudge.value - 3;

        var msg =
            pname + " : [" + UseSkill.name + "]  コスト" + cost + " / 射程" + UseSkill.rangeMin + "~" + UseSkill.rangeMax
            + " / ダメージ" + dam + "  出目補正" + cordice;
        if (UseSkill.explosion) msg += " +爆発";
        if (UseSkill.cut) msg += " +切断";
        if (UseSkill.oneCombo) msg += " +連撃1";
        if (UseSkill.twoCombo) msg += " + 連撃2";
        if (UseSkill.areaAttack) msg += " +全体攻撃";
        if (UseSkill.fallDowm) msg += " +転倒";
        CreateText.instance.TextLog(msg);
        CreateText.instance.TextLog("ターゲット：" + TargetPiece.data.name);
        thdice = ThrowDice.Thirow();
        CreateText.instance.TextLog("ダイススロー : " + thdice);
        GameManager.state = GameState.InterruptPhase;
        CreateText.instance.TextLog("支援・妨害・防御を行ってください");
    }

    private void AttackRecalc()
    {
        CreateText.instance.TextLog("補正後の攻撃判定");
        thdice += GameManager.DiceFix;
        var pos = GameManager.ci.gameObject.transform.position;
        var dam = UseSkill.damage + (cordam.value - 3) + GameManager.DamFix;
        var cost = UseSkill.cost + (corcost.value - 3);
        var cordice = corjudge.value - 3;

        var mpos = new Vector3(pos.x, pos.y - cost, pos.z);
        var jgnum = thdice + cordice;
        string judge = "判定値" + jgnum + " : ";

        if (jgnum <= 5 || (dam <= 0 && jgnum <= 10))
        {
            judge += "失敗";
            CreateText.instance.TextLog(judge);
            GameManager.state = GameState.PhaseEnd;
            return;
        }

        if (jgnum > 5 && jgnum <= 7)
        {
            judge += "成功(脚)";
            judge += " " + dam + "ダメージ";

            // areaattack
            if (UseSkill.areaAttack)
            {
                judge += " 全体攻撃";
                AreaAttack(jgnum, dam);
            }
            else
            {
                DamageSet(ref TargetPiece.data.leghp, TargetPiece.data, dam);
            }
            CreateText.instance.TextLog(judge);
            if (UseSkill.explosion) Explosion(dam);
            if (UseSkill.cut) Cut(ref TargetPiece.data.leghp);
            if (UseSkill.fallDowm) FallDown(TargetPiece.gameObject);
        }
        else if (jgnum == 8)
        {
            judge += "成功(胴)";
            judge += " " + dam + "ダメージ";

            // areaattack
            if (UseSkill.areaAttack)
            {
                judge += " 全体攻撃";
                AreaAttack(jgnum, dam);
            }
            else
            {
                DamageSet(ref TargetPiece.data.bodyhp, TargetPiece.data, dam);
            }
            CreateText.instance.TextLog(judge);
            if (UseSkill.explosion) Explosion(dam);
            if (UseSkill.cut) Cut(ref TargetPiece.data.bodyhp);
            if (UseSkill.fallDowm) FallDown(TargetPiece.gameObject);
        }
        else if (jgnum == 9)
        {
            judge += "成功(腕)";
            judge += " " + dam + "ダメージ";

            // areaattack
            if (UseSkill.areaAttack)
            {
                judge += " 全体攻撃";
                AreaAttack(jgnum, dam);
            }
            else
            {
                DamageSet(ref TargetPiece.data.armhp, TargetPiece.data, dam);
            }
            CreateText.instance.TextLog(judge);
            if (UseSkill.explosion) Explosion(dam);
            if (UseSkill.cut) Cut(ref TargetPiece.data.armhp);
            if (UseSkill.fallDowm) FallDown(TargetPiece.gameObject);
        }
        else if (jgnum > 9)
        {
            judge += "成功(頭)";

            var crit = 0;
            if (jgnum > 10)
            {
                crit += (jgnum - 10);
                judge += " クリティカル";
                dam += crit;
            }
            judge += " " + dam + "ダメージ";

            // areaattack
            if (UseSkill.areaAttack)
            {
                judge += " 全体攻撃";
                AreaAttack(jgnum, dam);
            }
            else
            {
                DamageSet(ref TargetPiece.data.headhp, TargetPiece.data, dam);
            }
            CreateText.instance.TextLog(judge);
            if (UseSkill.explosion) Explosion(dam);
            if (UseSkill.cut) Cut(ref TargetPiece.data.headhp);
            if (UseSkill.fallDowm) FallDown(TargetPiece.gameObject);
        }
        if (UseSkill.oneCombo) Combo();
        if (UseSkill.twoCombo) Combo();
        
        if (TargetPiece.data.overDamage > 0)
        {
            CreateText.instance.TextLog(TargetPiece.data.name + " : 超過ダメージ" + TargetPiece.data.overDamage);
        }
        if(UseSkill.addSan > 0)
        {
            GameManager.ci.data.overSan += UseSkill.addSan;
            CreateText.instance.TextLog(pname + " : 未精算狂気点" + GameManager.ci.data.overSan);
        }
        GameManager.ci.gameObject.transform.position = mpos;
        GameManager.state = GameState.PhaseEnd;
    }

    private void Explosion(int dam)
    {
        CreateText.instance.TextLog("爆発発動");
        if(TargetPiece.data.type == "ホラー" || TargetPiece.data.type == "レギオン")
        {
            TargetPiece.data.bodyhp -= dam;
        }
        else
        {
            TargetPiece.data.overDamage += dam;
        }
    }

    private void Cut(ref int hitPartsHP)
    {
        CreateText.instance.TextLog("切断判定");
        var dice = ThrowDice.Thirow();
        CreateText.instance.TextLog("ダイス：" + dice);
        if(dice > 5)
        {
            CreateText.instance.TextLog("切断成功");
            hitPartsHP = 0;
        }
        else
        {
            CreateText.instance.TextLog("切断失敗");
        }
    }

    private void Combo()
    {
        CreateText.instance.TextLog("連撃(手動で再攻撃してください)");
    }

    private void AreaAttack(int dice, int dam)
    {
        var tarea = TargetPiece.data.area;
        foreach(var item in GameManager.instance.onBoardCharacterList)
        {
            if(item.data.area == tarea)
            {
                if(dice > 5 && dice <= 7)
                {
                    if (item.data.type == "レギオン") dam += dam;
                    DamageSet(ref item.data.leghp, item.data, dam);
                }
                else if (dice == 8)
                {
                    if (item.data.type == "レギオン") dam += dam;
                    DamageSet(ref item.data.bodyhp, item.data, dam);
                }
                else if (dice == 9)
                {
                    if (item.data.type == "レギオン") dam += dam;
                    DamageSet(ref item.data.armhp, item.data, dam);
                }
                else
                {
                    if (item.data.type == "レギオン") dam += dam;
                    DamageSet(ref item.data.headhp, item.data, dam);
                }
            }
        }
    }

    private void FallDown(GameObject target)
    {
        CreateText.instance.TextLog("転倒発動");
        target.transform.localPosition = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y - 2, target.transform.localPosition.z);
    }

    private void DamageSet(ref int hitPartsHP, CharacterData data, int dam)
    {
        if(data.type == "ホラー" || data.type == "レギオン")
        {
            if (TargetPiece.data.bodyhp < 0)
            {
                TargetPiece.data.bodyhp = 0;
            }
            else
            {
                data.bodyhp -= dam;
            }
        }
        else
        {
            if (hitPartsHP - dam < 0)
            {
                data.overDamage = dam - hitPartsHP;
                hitPartsHP = 0;
            }
            else
            {
                hitPartsHP -= dam;
            }
        }
    }

    private void GuardMethod()
    {
        var pname = GameManager.ci.data.name;
        GameManager.DamFix = UseSkill.damage * -1;
        var msg =
            pname + " : [" + UseSkill.name + "]  コスト" + UseSkill.cost + " / ダメージ" + GameManager.DamFix;
        CreateText.instance.TextLog(msg);
    }

    private void BuffMethod()
    {
        var pname = GameManager.ci.data.name;
        GameManager.DiceFix = UseSkill.correction;
        var msg =
            pname + " : [" + UseSkill.name + "]  コスト" + UseSkill.cost + " / 補正" + GameManager.DiceFix;
        CreateText.instance.TextLog(msg);
    }

    private void MoveMethod()
    {
        var pname = GameManager.ci.data.name;
        var msg =
            pname + " : [" + UseSkill.name + "]  コスト" + UseSkill.cost + " / 移動" + UseSkill.move;
        CreateText.instance.TextLog(msg);
        var pos = TargetPiece.gameObject.transform.localPosition;
        pos = new Vector3(pos.x + (UseSkill.move * 5), pos.y - UseSkill.cost, pos.z);
    }

    private void DestroyCheck()
    {
        for(var i = 0; i < GameManager.instance.onBoardCharacterList.Count; i++)
        {
            var item = GameManager.instance.onBoardCharacterList[i];
            if ((item.data.leghp + item.data.armhp + item.data.bodyhp + item.data.headhp) - item.data.overDamage <= 0)
            {
                CreateText.instance.TextLog(item.data.name + "は残HP0の為削除");
                GameManager.instance.onBoardCharacterList.RemoveAt(i);
                Destroy(item.gameObject);
            }
        }
    }
}