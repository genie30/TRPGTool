using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataList : MonoBehaviour
{
    public static List<CharacterData> characterList = new List<CharacterData>();

    private void Start()
    {
        var data = Resources.LoadAll("ItemData");
        foreach(var item in data)
        {
            characterList.Add((CharacterData)item);
        }
    }

    public static void ListAdd(CharacterData data)
    {
        characterList.Add(data);
    }

    public static void RemoveData(string itemname)
    {
        for(var i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].name == itemname) characterList.RemoveAt(i);
        }
    }

    public static CharacterData ReturnItem(string itemname)
    {
        foreach(var item in characterList)
        {
            if (item.name == itemname) return item;
        }
        return null;
    }
}
