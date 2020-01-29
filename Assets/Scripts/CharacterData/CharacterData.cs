using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateData")]
public class CharacterData : ScriptableObject
{
    public Sprite img;
    public string name;
    public string type;
    public int headhp, armhp, bodyhp, leghp;
    public int sana, sanb, sanc, sand, sane;
    public int overDamage;
    public int overSan;
    public string memo;
    public int area;

    public CharacterData(Sprite img, string name, string type, int headhp, int armhp, int bodyhp, int leghp, int sana, int sanb, int sanc, int sand, int sane, string memo)
    {
        this.img = img;
        this.name = name;
        this.type = type;
        this.headhp = headhp;
        this.armhp = armhp;
        this.bodyhp = bodyhp;
        this.leghp = leghp;
        this.sana = sana;
        this.sanb = sanb;
        this.sanc = sanc;
        this.sand = sand;
        this.sane = sane;
        overDamage = 0;
        overSan = 0;
        this.memo = memo;
        area = 2;
    }
}
