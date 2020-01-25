using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItem : MonoBehaviour
{
    public CharacterData data;
    public CharacterSkillList skillList;

    public int areanum; // 奈落0　地獄1　煉獄2　花園3　楽園4

    private void Update()
    {
        var pos = transform.position;
        if(pos.x > 6)
        {
            areanum = 4;
        }
        else if(pos.x > 1)
        {
            areanum = 3;
        }
        else if(pos.x > -4)
        {
            areanum = 2;
        }
        else if(pos.x > -9)
        {
            areanum = 1;
        }
        else
        {
            areanum = 0;
        }
    }
}
