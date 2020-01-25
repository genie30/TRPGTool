using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateText : MonoBehaviour
{
    [SerializeField] GameObject textObject;
    public static CreateText instance;
    
    private void Awake()
    {
        instance = this;
    }

    public void TextLog(string addText)
    {
        GameObject newText = Instantiate(textObject);
        newText.transform.SetParent(transform);
        newText.GetComponent<Text>().text = addText;
    }
}
