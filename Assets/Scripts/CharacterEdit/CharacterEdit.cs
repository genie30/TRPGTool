using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEdit : MonoBehaviour
{
    public static CharacterEdit instance;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Image img;
    [SerializeField]
    Text name, overdamage, unpaidsan, headhp, armhp, bodyhp, leghp, sana, sanb, sanc, sand, sane;
    [SerializeField]
    InputField memo;

    private void Awake()
    {
        instance = this;
    }

    public void ValueSet()
    {
        img.sprite = GameManager.ci.data.img;
        name.text = GameManager.ci.data.name;
        overdamage.text = GameManager.ci.data.overDamage.ToString();
        unpaidsan.text = GameManager.ci.data.overSan.ToString();
        headhp.text = GameManager.ci.data.headhp.ToString();
        armhp.text = GameManager.ci.data.armhp.ToString();
        bodyhp.text = GameManager.ci.data.bodyhp.ToString();
        leghp.text = GameManager.ci.data.leghp.ToString();
        sana.text = GameManager.ci.data.sana.ToString();
        sanb.text = GameManager.ci.data.sanb.ToString();
        sanc.text = GameManager.ci.data.sanc.ToString();
        sand.text = GameManager.ci.data.sand.ToString();
        sane.text = GameManager.ci.data.sane.ToString();
        memo.text = GameManager.ci.data.memo.ToString();
        panel.SetActive(true);
    }

    public void CancelClick()
    {
        panel.SetActive(false);
    }

    public void SetClick()
    {
        GameManager.ci.data.overDamage = int.Parse(overdamage.text);
        GameManager.ci.data.overSan = int.Parse(unpaidsan.text);
        GameManager.ci.data.headhp = int.Parse(headhp.text);
        GameManager.ci.data.armhp = int.Parse(armhp.text);
        GameManager.ci.data.bodyhp = int.Parse(bodyhp.text);
        GameManager.ci.data.leghp = int.Parse(leghp.text);
        GameManager.ci.data.sana = int.Parse(sana.text);
        GameManager.ci.data.sanb = int.Parse(sanb.text);
        GameManager.ci.data.sanc = int.Parse(sanc.text);
        GameManager.ci.data.sand = int.Parse(sand.text);
        GameManager.ci.data.sane = int.Parse(sane.text);
        GameManager.ci.data.memo = memo.text;
        panel.SetActive(false);
    }
}
