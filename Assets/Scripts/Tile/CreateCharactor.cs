﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharactorButton : MonoBehaviour
{
    [SerializeField]
    GameObject create;

    public void OnClick()
    {
        create.SetActive(true);
    }
}
