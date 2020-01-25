using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill
{
    public string name;
    public int type;
    public int cost;
    public int rangeMin; //射程 min ≦ max;
    public int rangeMax;
    public int damage;

    public bool explosion; //爆発
    public bool cut; //切断
    public bool oneCombo; //連撃1
    public bool twoCombo; //連撃2
    public bool areaAttack; //全体攻撃

    public int correction; //ダイス補正値
    public string memo;

    public int addSan; //狂気点
    public int move; //移動量

    public CharacterSkill(string name, int type, int cost, int rangeMin, int rangeMax, int damage, bool explosion, bool cut, bool oneCombo, bool twoCombo, bool areaAttack, int correction, string memo, int addSan, int move)
    {
        this.name = name;
        this.type = type;
        this.cost = cost;
        this.rangeMin = rangeMin;
        this.rangeMax = rangeMax;
        this.damage = damage;

        this.explosion = explosion;
        this.cut = cut;
        this.oneCombo = oneCombo;
        this.twoCombo = twoCombo;
        this.areaAttack = areaAttack;

        this.correction = correction;
        this.memo = memo;

        this.addSan = addSan;
        this.move = move;

    }

    //確認用
    public void CheckField()
    {
        Debug.Log("name" + this.name);
        Debug.Log("type" + this.type);
        Debug.Log("cost" + this.cost);
        Debug.Log("rangeMin" + this.rangeMin);
        Debug.Log("rangeMax" + this.rangeMax);
        Debug.Log("damage" + this.damage);

        Debug.Log("explosion" + this.explosion);
        Debug.Log("cut" + this.cut);
        Debug.Log("oneCombo" + this.oneCombo);
        Debug.Log("twoCombo" + this.twoCombo);
        Debug.Log("areaAttack" + this.areaAttack);

        Debug.Log("correction" + this.correction);
        Debug.Log("memo" + this.memo);

        Debug.Log("addSan" + this.addSan);
        Debug.Log("move" + this.move);
    }

}
