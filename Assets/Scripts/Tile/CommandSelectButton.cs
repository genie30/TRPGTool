using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSelectButton : MonoBehaviour
{
    [SerializeField]
    GameObject usepanel;

    public void OnClick()
    {
        usepanel.SetActive(true);
        UseSkillController.instance.SkillListLoad(GameManager.ci.skillList.skillList);
    }
}
