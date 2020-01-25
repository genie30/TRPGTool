using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDice : MonoBehaviour
{
    public static int Thirow()
    {
        return Random.Range(1,11);
    }
}
