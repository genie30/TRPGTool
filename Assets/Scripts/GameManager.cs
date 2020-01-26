using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool Stay{ get; set; }

    public static CharacterItem ci;

    private void Awake()
    {
        instance = this;
    }
}
