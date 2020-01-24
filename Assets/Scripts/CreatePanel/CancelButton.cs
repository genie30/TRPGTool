using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButton : MonoBehaviour
{
    [SerializeField]
    GameObject panel;

    public void OnClick()
    {
        panel.SetActive(false);
    }
}
