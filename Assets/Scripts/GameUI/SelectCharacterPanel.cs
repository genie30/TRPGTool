using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterPanel : MonoBehaviour
{
    public static SelectCharacterPanel instance;
    [SerializeField]
    Image img;
    [SerializeField]
    Text nametext;

    private void Awake()
    {
        instance = this;
    }

    public void SelectChange()
    {
        img.sprite = GameManager.ci.data.img;
        nametext.text = GameManager.ci.data.name;
    }
}
