using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCommandButton : MonoBehaviour
{
    public void OnClick()
    {
        if(GameManager.ci == null) return;
        CreateSkillInfomation.instance.CreateSkillUiOn(GameManager.ci);
    }
}
