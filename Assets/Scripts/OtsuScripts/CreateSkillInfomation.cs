using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSkillInfomation : MonoBehaviour
{
    public static CreateSkillInfomation instance;

    CharacterData Character;
    [SerializeField] Image CharacterImage;
    [SerializeField] Text CharacterName;
    [SerializeField] Dropdown skillList;
    [SerializeField] InputField inputDamage;
    [SerializeField] InputField inputCost;
    [SerializeField] InputField inputName;
    [SerializeField] Dropdown dropType;
    [SerializeField] InputField inputRangeMin;
    [SerializeField] InputField inputRangeMax;
    [SerializeField] Toggle[] inputSan = new Toggle[2];
    [SerializeField] Toggle[] inputMove = new Toggle[5];
    [SerializeField] Toggle inputExplosion;
    [SerializeField] Toggle inputOneCombo;
    [SerializeField] Toggle inputTwoCombo;
    [SerializeField] Toggle inputAreaAttack;
    [SerializeField] Toggle[] inputCorrection = new Toggle[6];
    [SerializeField] InputField inputMemo;

    private void Awake()
    {
        //Sprite
        instance = this;
    }

    ////スキル入力内容の整合性チェック
    public void CheckIntegrity()
    {
        string error;

    }

    ////デバッグ用
    //public void DebugOutPut()
    //{

    //}

}
