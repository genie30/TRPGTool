using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateText : MonoBehaviour
{
    [SerializeField] GameObject textObject;
    public static CreateText instance;
    [SerializeField]
    ContentSizeFitter fitter;
    
    private void Awake()
    {
        instance = this;
    }

    public void TextLog(string addText)
    {
        GameObject newText = Instantiate(textObject, transform);
        newText.GetComponent<Text>().text = addText;
        fitter.SetLayoutVertical();
        Canvas.ForceUpdateCanvases();
        newText.transform.SetSiblingIndex(0);
    }
}
