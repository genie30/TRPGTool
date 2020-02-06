using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseSkillController : MonoBehaviour
{
    public static UseSkillController instance;

    public bool useskil = false;
    public CharacterData user;
    private List<CharacterSkill> skillList;
    private List<CharacterItem> targetList = new List<CharacterItem>();

    [SerializeField]
    GameObject panel;
    [SerializeField]
    Dropdown selectSkill, target, corjudge, cordam, corcost;
    [SerializeField]
    Text type, cost, range, pow, cor, memo, move, san;
    [SerializeField]
    Toggle explosion, cut, chain1, chain2, allatk, falldown;

    CharacterSkill UseSkill, SetSkill;
    CharacterItem TargetPiece, SetTarget;
    CharacterItem UserPiece;

    private void Awake()
    {
        instance = this;
    }

    public void SkillListLoad(List<CharacterSkill> list)
    {
        user = GameManager.ci.data;
        skillList = list;
        selectSkill.ClearOptions();
        List<string> opt = new List<string>();
        foreach(var item in skillList)
        {
            opt.Add(item.name);
        }
        selectSkill.AddOptions(opt);
        selectSkill.RefreshShownValue();
        SkillSelected();
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
                typestr = "ダメージ増加";
                break;
            case 3:
                typestr = "支援・妨害";
                break;
            case 4:
                typestr = "移動";
                break;
            case 5:
                typestr = "その他";
                break;
        }
        type.text = typestr;

        cost.text = loadSkill.cost.ToString();
        range.text = loadSkill.rangeMin + "~" + loadSkill.rangeMax;
        pow.text = loadSkill.damage.ToString();
        cor.text = loadSkill.correction.ToString();
        move.text = loadSkill.move.ToString();
        san.text = loadSkill.addSan.ToString();
        memo.text = loadSkill.memo;

        explosion.isOn = loadSkill.explosion;
        cut.isOn = loadSkill.cut;
        chain1.isOn = loadSkill.oneCombo;
        chain2.isOn = loadSkill.twoCombo;
        allatk.isOn = loadSkill.areaAttack;
        falldown.isOn = loadSkill.fallDowm;

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
        panel.SetActive(false);
    }

    public void CancelButton()
    {
        panel.SetActive(false);
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
            case 2: // ダメージ増加
                DamagePlusMethod();
                break;
            case 3: // 支援
                BuffMethod();
                break;
            case 4: // 移動
                MoveMethod();
                break;
            case 5: // その他
                break;
        }
    }

    IEnumerator Attack()
    {
        UserPiece = GameManager.ci;
        useskil = true;
        AttackMethod();
        while (GameManager.state == GameState.InterruptPhase)
        {
            yield return null;
        }
        useskil = false;
        AttackRecalc();
        while(GameManager.state == GameState.AttackReCulc)
        {
            yield return null;
        }
        GameManager.DiceFix = 0;
        GameManager.DamFix = 0;
        UserPiece = null;
        SetTarget = null;
        SetSkill = null;

        thdice = 0;
        pname = "";
        DestroyCheck();
    }

    public int thdice;
    string pname;
    private void AttackMethod()
    {
        SetSkill = UseSkill;
        SetTarget = TargetPiece;
        pname = GameManager.ci.data.name;
        GameManager.state = GameState.AttackPhase;
        var damage = SetSkill.damage + (cordam.value - 3);
        var cost = SetSkill.cost + (corcost.value - 3);
        var correction = (corjudge.value - 3) + SetSkill.correction;
        GameManager.DiceFix += correction;

        var msg =
            pname + " : [" + SetSkill.name + "]  コスト" + cost + " / 射程" + SetSkill.rangeMin + "~" + SetSkill.rangeMax
            + " / ダメージ" + damage + "  出目補正" + correction;
        if (SetSkill.explosion) msg += " +爆発";
        if (SetSkill.cut) msg += " +切断";
        if (SetSkill.oneCombo) msg += " +連撃1";
        if (SetSkill.twoCombo) msg += " + 連撃2";
        if (SetSkill.areaAttack) msg += " +全体攻撃";
        if (SetSkill.fallDowm) msg += " +転倒";
        CreateText.instance.TextLog(msg);
        CreateText.instance.TextLog("ターゲット：" + SetTarget.data.name);
        thdice = ThrowDice.Thirow();
        CreateText.instance.TextLog("ダイススロー : " + thdice);
        GameManager.state = GameState.InterruptPhase;
        CreateText.instance.TextLog("支援・妨害・防御を行ってください");
    }

    private void AttackRecalc()
    {
        CreateText.instance.TextLog("補正後の攻撃判定");
        thdice += GameManager.DiceFix;
        var pos = UserPiece.transform.position;
        var dam = SetSkill.damage + (cordam.value - 3) + GameManager.DamFix;

        var mpos = new Vector3(pos.x, pos.y - SetSkill.cost + (corcost.value - 3), pos.z);
        var jgnum = thdice;
        string judge = "判定値" + jgnum + " : ";

        if (jgnum <= 5 || (dam <= 0 && jgnum <= 10))
        {
            judge += "失敗";
            CreateText.instance.TextLog(judge);
            UserPiece.gameObject.transform.position = mpos;
            GameManager.state = GameState.PhaseEnd;
            return;
        }

        if (jgnum > 5 && jgnum <= 7)
        {
            judge += "成功(脚)";
            judge += " " + dam + "ダメージ";

            // areaattack
            if (SetSkill.areaAttack)
            {
                judge += " 全体攻撃";
                AreaAttack(jgnum, dam);
            }
            else
            {
                DamageSet(ref SetTarget.data.leghp, SetTarget.data, dam);
            }
            CreateText.instance.TextLog(judge);
            if (SetSkill.explosion) Explosion(dam);
            if (SetSkill.cut) Cut(ref SetTarget.data.leghp, dam);
            if (SetSkill.fallDowm) FallDown(SetTarget.gameObject);
        }
        else if (jgnum == 8)
        {
            judge += "成功(胴)";
            judge += " " + dam + "ダメージ";

            // areaattack
            if (SetSkill.areaAttack)
            {
                judge += " 全体攻撃";
                AreaAttack(jgnum, dam);
            }
            else
            {
                DamageSet(ref SetTarget.data.bodyhp, SetTarget.data, dam);
            }
            CreateText.instance.TextLog(judge);
            if (SetSkill.explosion) Explosion(dam);
            if (SetSkill.cut) Cut(ref SetTarget.data.bodyhp, dam);
            if (SetSkill.fallDowm) FallDown(SetTarget.gameObject);
        }
        else if (jgnum == 9)
        {
            judge += "成功(腕)";
            judge += " " + dam + "ダメージ";

            // areaattack
            if (SetSkill.areaAttack)
            {
                judge += " 全体攻撃";
                AreaAttack(jgnum, dam);
            }
            else
            {
                DamageSet(ref SetTarget.data.armhp, SetTarget.data, dam);
            }
            CreateText.instance.TextLog(judge);
            if (SetSkill.explosion) Explosion(dam);
            if (SetSkill.cut) Cut(ref SetTarget.data.armhp, dam);
            if (SetSkill.fallDowm) FallDown(SetTarget.gameObject);
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
            if (SetSkill.areaAttack)
            {
                judge += " 全体攻撃";
                AreaAttack(jgnum, dam);
            }
            else
            {
                DamageSet(ref SetTarget.data.headhp, SetTarget.data, dam);
            }
            CreateText.instance.TextLog(judge);
            if (SetSkill.explosion) Explosion(dam);
            if (SetSkill.cut) Cut(ref SetTarget.data.headhp, dam);
            if (SetSkill.fallDowm) FallDown(SetTarget.gameObject);
        }
        if (SetSkill.oneCombo || SetSkill.twoCombo) Combo();
        
        if (SetTarget.data.overDamage > 0)
        {
            CreateText.instance.TextLog(SetTarget.data.name + " : 超過ダメージ" + SetTarget.data.overDamage);
        }
        if(SetSkill.addSan > 0)
        {
            UserPiece.data.overSan += SetSkill.addSan;
            CreateText.instance.TextLog(pname + " : 未精算狂気点" + UserPiece.data.overSan);
        }
        UserPiece.gameObject.transform.position = mpos;
        GameManager.state = GameState.PhaseEnd;
    }

    private void Explosion(int dam)
    {
        CreateText.instance.TextLog("爆発発動");
        if(SetTarget.data.type == "ホラー" || SetTarget.data.type == "レギオン")
        {
            SetTarget.data.bodyhp -= dam;
        }
        else
        {
            SetTarget.data.overDamage += dam;
        }
    }

    private void Cut(ref int hitPartsHP, int dam)
    {
        CreateText.instance.TextLog("切断判定");
        var dice = ThrowDice.Thirow();
        CreateText.instance.TextLog("ダイス：" + dice);
        if(dice > 5)
        {
            CreateText.instance.TextLog("切断成功");
            if (TargetPiece.data.type == "ドール" || TargetPiece.data.type == "サヴァント") hitPartsHP = 0;
            else TargetPiece.data.bodyhp -= dam;
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
        var tarea = SetTarget.areanum;
        foreach(var item in GameManager.instance.onBoardCharacterList)
        {
            if(item.areanum == tarea)
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
            if (SetTarget.data.bodyhp < 0)
            {
                SetTarget.data.bodyhp = 0;
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
        var cost = UseSkill.cost + (corcost.value - 3);
        GameManager.ci.transform.position -= new Vector3(0, cost, 0);
        user.overSan += UseSkill.addSan;
        var msg =
            pname + " : [" + UseSkill.name + "]  コスト" + cost + " / ダメージ修正" + GameManager.DamFix;
        CreateText.instance.TextLog(msg);
    }

    private void DamagePlusMethod()
    {
        var pname = GameManager.ci.data.name;
        GameManager.DamFix += UseSkill.damage;
        var cost = UseSkill.cost + (corcost.value - 3);
        GameManager.ci.transform.position -= new Vector3(0, cost, 0);
        user.overSan += UseSkill.addSan;
        var msg =
            pname + " : [" + UseSkill.name + "]  コスト" + cost + " / ダメージ修正" + GameManager.DamFix;
        CreateText.instance.TextLog(msg);
    }

    private void BuffMethod()
    {
        var pname = GameManager.ci.data.name;
        GameManager.DiceFix += UseSkill.correction;
        var cost = UseSkill.cost + (corcost.value - 3);
        GameManager.ci.transform.position -= new Vector3(0, cost, 0);
        user.overSan += UseSkill.addSan;
        var msg =
            pname + " : [" + UseSkill.name + "]  コスト" + cost + " / 補正" + UseSkill.correction;
        CreateText.instance.TextLog(msg);
    }

    private void MoveMethod()
    {
        var pname = GameManager.ci.data.name;
        var cost = UseSkill.cost + (corcost.value - 3);
        var msg =
            pname + " : [" + UseSkill.name + "]  コスト" + cost + " / 移動" + UseSkill.move;
        CreateText.instance.TextLog(msg);
        var pos = TargetPiece.gameObject.transform.localPosition;
        TargetPiece.gameObject.transform.localPosition = new Vector3(pos.x + (UseSkill.move * 5), pos.y, pos.z);
        GameManager.ci.transform.position -= new Vector3(0, cost, 0);
        user.overSan += UseSkill.addSan;
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