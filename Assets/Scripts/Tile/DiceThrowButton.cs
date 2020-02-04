using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceThrowButton : MonoBehaviour
{
    [SerializeField]
    Button button;

    private void Update()
    {
        if (UseSkillController.instance.useskil)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void ButtonClick()
    {
        CreateText.instance.TextLog("ダイスを振り直します。（コスト処理は手動）");
        var dice = ThrowDice.Thirow();
        UseSkillController.instance.user.overSan += 1;
        CreateText.instance.TextLog("ダイス：" + dice);
        UseSkillController.instance.thdice = dice;
    }
}
