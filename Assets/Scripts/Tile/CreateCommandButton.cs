using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCommandButton : MonoBehaviour
{
    public void OnClick()
    {
        CreateSkillInfomation.instance.CreateSkillUiOn(GameManager.ci);
    }
}
