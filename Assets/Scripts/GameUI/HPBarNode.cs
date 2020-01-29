using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarNode : MonoBehaviour
{
    int hmax, amax, bmax, lmax;
    int shownum;
    [SerializeField]
    Text head, arm, body, leg, dam, san;
    [SerializeField]
    Slider hsl, asl, bsl, lsl;
    bool seton = false;

    public void HPSet(int num)
    {
        shownum = num;
        var data = GameManager.instance.onBoardCharacterList[shownum].data;
        hmax = data.headhp;
        amax = data.armhp;
        bmax = data.bodyhp;
        lmax = data.leghp;

        hsl.maxValue = hmax;
        asl.maxValue = amax;
        bsl.maxValue = bmax;
        lsl.maxValue = lmax;
        seton = true;
    }

    private void Update()
    {
        if (!seton) return;
        var data = GameManager.instance.onBoardCharacterList[shownum].data;
        hsl.value = data.headhp;
        asl.value = data.armhp;
        bsl.value = data.bodyhp;
        lsl.value = data.leghp;
        head.text = data.headhp + "/" + hmax;
        arm.text = data.armhp + "/" + amax;
        body.text = data.bodyhp + "/" + bmax;
        leg.text = data.leghp + "/" + lmax;
        dam.text = data.overDamage.ToString();
        san.text = data.overSan.ToString();
    }
}
