using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSkillInfomation : MonoBehaviour
{
    public static CreateSkillInfomation instance;

    CharacterData Character;
    public GameObject panel;
    [SerializeField] Image CharacterImage;
    [SerializeField] Text CharacterName;
    [SerializeField] Dropdown skillListUi;
    [SerializeField] Text inputDamage;
    [SerializeField] Text inputCost;
    [SerializeField] Text inputName;
    [SerializeField] Dropdown dropType;
    [SerializeField] Text inputRangeMin;
    [SerializeField] Text inputRangeMax;
    [SerializeField] Toggle[] inputSan = new Toggle[2];
    [SerializeField] Toggle[] inputMove = new Toggle[5];
    [SerializeField] Toggle inputExplosion;
    [SerializeField] Toggle inputCut;
    [SerializeField] Toggle inputOneCombo;
    [SerializeField] Toggle inputTwoCombo;
    [SerializeField] Toggle inputAreaAttack;
    [SerializeField] Toggle inputFallDown;
    [SerializeField] Toggle[] inputCorrection = new Toggle[6];
    [SerializeField] Text inputMemo;
    [SerializeField] GameObject createButton;
    [SerializeField] GameObject updateButton;
    CharacterItem setItem;
    private void Awake()
    {
        instance = this;
    }

    ////スキル入力内容の整合性チェック
    public bool CheckIntegrity()
    {
        string errorMassage = null;
        int errorInt = 0;
        if (inputName.text == "")
        {
            errorMassage += "特技名が未記入です。";
        }
        if (!int.TryParse(inputDamage.text, out errorInt))
        {
            errorMassage += "威力が半角整数ではありません。";
        }
        if (!int.TryParse(inputCost.text, out errorInt))
        {
            errorMassage += "コストが半角整数ではありません。";
        }
        if (!int.TryParse(inputRangeMin.text, out errorInt))
        {
            errorMassage += "最小射程が半角整数ではありません。";
        }
        if (!int.TryParse(inputRangeMax.text, out errorInt))
        {
            errorMassage += "最大射程が半角整数ではありません。";
        }
        if (int.TryParse(inputRangeMin.text, out errorInt) && int.TryParse(inputRangeMax.text, out errorInt))
        {
            if (int.Parse(inputRangeMin.text) > int.Parse(inputRangeMax.text))
            {
                errorMassage += "最小射程≦最大射程にしてください。";
            }
        }

        if (errorMassage != null)
        {
            Debug.Log(errorMassage);
            return false;
        }
        else
        {
            //Debug.Log("スキル作成成功");
            return true;
        }

    } 

  
    public void CreateSkill()
    {
        if (!CheckIntegrity())
        {
            return;
        }
        CharacterSkill skill = CreateSkillInstance();
        setItem.skillList.skillList.Add(skill);
        SkillListUpdate();
        ResetAll();
    }
    //スキルのインスタンス作成

    CharacterSkill CreateSkillInstance()
    {
        int correction = 0;
        for (int i = 0; i < inputCorrection.Length; i++)
        {
            if (inputCorrection[i].isOn)
            {
                switch (i)
                {
                    case 0:
                        correction = -2;
                        break;
                    case 1:
                        correction = -1;
                        break;
                    case 2:
                        correction = 0;
                        break;
                    case 3:
                        correction = 1;
                        break;
                    case 4:
                        correction = 2;
                        break;
                    case 5:
                        correction = 3;
                        break;
                }
            }
        }
        int move = 0;
        for (int i = 0; i < inputMove.Length; i++)
        {
            if (inputMove[i].isOn)
            {
                switch (i)
                {
                    case 0:
                        move = -2;
                        break;
                    case 1:
                        move = -1;
                        break;
                    case 2:
                        move = 0;
                        break;
                    case 3:
                        move = 1;
                        break;
                    case 4:
                        move = 2;
                        break;
                }
            }
        }
        int san = 0;
        if (inputSan[1].isOn)
        {
            san = 1;
        }
        return new CharacterSkill(inputName.text, dropType.value, int.Parse(inputCost.text), int.Parse(inputRangeMin.text), int.Parse(inputRangeMax.text), int.Parse(inputDamage.text), inputExplosion.isOn, inputCut.isOn, inputOneCombo.isOn, inputTwoCombo.isOn, inputAreaAttack.isOn, inputFallDown.isOn, correction, inputMemo.text, san, move);
    }
    void ResetAll()
    {

        inputName.gameObject.transform.parent.GetComponent<InputField>().text = "";
        skillListUi.value = 0;
        inputDamage.gameObject.transform.parent.GetComponent<InputField>().text = "0";
        inputCost.gameObject.transform.parent.GetComponent<InputField>().text = "0";
        dropType.value = 0;
        inputRangeMin.gameObject.transform.parent.GetComponent<InputField>().text = "0";
        inputRangeMax.gameObject.transform.parent.GetComponent<InputField>().text = "0";
        inputSan[0].isOn = true;
        inputMove[2].isOn = true;
        inputExplosion.isOn = false;
        inputCut.isOn = false;
        inputOneCombo.isOn = false;
        inputTwoCombo.isOn = false;
        inputAreaAttack.isOn = false;
        inputCorrection[2].isOn = true;
        inputMemo.gameObject.transform.parent.GetComponent<InputField>().text = "";
    }
   
    public void CreateSkillUiOn(CharacterItem item)
    {
        panel.SetActive(true);
        setItem = item;
        CharacterImage.sprite = setItem.data.img;
        CharacterName.text = setItem.data.name;
        ResetAll();
        SkillListUpdate();
    }
    public void CreateSkillUiOff()
    {
        panel.SetActive(false);
    }
    public void SkillListUpdate()
    {
        skillListUi.ClearOptions();
        
        List<string> newList = new List<string>();
        newList.Add("新規作成");
        for (int i = 0; i < setItem.skillList.skillList.Count; i++)
        {
            newList.Add(setItem.skillList.skillList[i].name);
        }
        skillListUi.AddOptions(newList);
    }
    
    public void SkillListNumChange(Dropdown changeDrop)
    {
        int dropNum = changeDrop.value;
        if (dropNum == 0)
        {
            ResetAll();
            updateButton.SetActive(false);
            createButton.SetActive(true);
            return;
        }
        updateButton.SetActive(true);
        createButton.SetActive(false);
        CharacterSkill skill = setItem.skillList.skillList[dropNum - 1];
        inputName.gameObject.transform.parent.GetComponent<InputField>().text = skill.name;
        inputDamage.gameObject.transform.parent.GetComponent<InputField>().text = skill.damage.ToString();
        inputCost.gameObject.transform.parent.GetComponent<InputField>().text = skill.cost.ToString();
        dropType.value = skill.type;
        inputRangeMin.gameObject.transform.parent.GetComponent<InputField>().text = skill.rangeMin.ToString();
        inputRangeMax.gameObject.transform.parent.GetComponent<InputField>().text = skill.rangeMax.ToString();
        if (skill.addSan == 0)
        {
            inputSan[0].isOn = true;
            
        }
        else
        {
            inputSan[1].isOn = true;
        }
        inputExplosion.isOn = skill.explosion;
        inputCut.isOn = skill.cut;
        inputOneCombo.isOn = skill.oneCombo;
        inputTwoCombo.isOn = skill.twoCombo;
        inputAreaAttack.isOn = skill.areaAttack; 
        switch (skill.move)
        {
            case -2:
                inputMove[0].isOn = true;
                break;
            case -1:
                inputMove[1].isOn = true;
                break;
            case 0:
                inputMove[2].isOn = true;
                break;
            case 1:
                inputMove[3].isOn = true;
                break;
            case 2:
                inputMove[4].isOn = true;
                break;
        }
        switch (skill.correction)
        {
            case -2:
                inputCorrection[0].isOn = true;
                break;
            case -1:
                inputCorrection[1].isOn = true;
                break;
            case 0:
                inputCorrection[2].isOn = true;
                break;
            case 1:
                inputCorrection[3].isOn = true;
                break;
            case 2:
                inputCorrection[4].isOn = true;
                break;
            case 3:
                inputCorrection[5].isOn = true;
                break;
        }
        inputMemo.gameObject.transform.parent.GetComponent<InputField>().text = skill.memo;
    }

    public void UpdateButtonClick()
    {
        if (!CheckIntegrity())
        {
            return;
        }
        int dropNum = skillListUi.value-1;
        setItem.skillList.skillList.RemoveAt(dropNum);
        setItem.skillList.skillList.Insert(dropNum, CreateSkillInstance());
        ResetAll();
        SkillListUpdate();
        updateButton.SetActive(false);
        createButton.SetActive(true);
    }
//[SerializeField] Dropdown testDrop;
//public void TestDropCreate()
//{
//    setItem.skillList.CreateDropdown(testDrop);
//}
//public void GetSkill()
//{
//    Debug.Log(setItem.skillList.SearchName(inputName.text).name);
//}

}
