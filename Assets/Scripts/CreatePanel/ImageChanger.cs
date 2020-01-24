using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    Sprite[] spr;
    [SerializeField]
    Image image;
    int num = 0;

    public static Sprite CharactorImage { get; set; }

    private void OnEnable()
    {
        spr = Resources.LoadAll<Sprite>("Image");
        if (spr == null) return;
        image.sprite = spr[0];
    }

    public void OnClick()
    {
        num++;
        if (num >= spr.Length) num = 0;
        image.sprite = spr[num];
        CharactorImage = spr[num];
    }
}
