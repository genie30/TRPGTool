using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileLoad : MonoBehaviour
{
    [SerializeField]
    Dropdown dropdown;
    [SerializeField]
    Button loadbutton;
    [SerializeField]
    GameObject panel;

    private void Start()
    {
        var list = SkillFileLoader.GetFileName();
        if (list.Count == 0) 
        {
            loadbutton.interactable = false;
            return;
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(list);
        dropdown.RefreshShownValue();
    }

    public void LoadClick()
    {
        var filename = dropdown.options[dropdown.value].text;
        var skill = SkillFileLoader.LoadSkill(filename);
        GameManager.ci.skillList.skillList = skill;
        panel.SetActive(false);
    }
}
