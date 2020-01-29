using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public static HPBar instance;
    [SerializeField]
    GameObject pref;
    [SerializeField]
    Transform parent;

    private void Awake()
    {
        instance = this;
    }

    public void NodeCreate(int num)
    {
        var node = Instantiate(pref, parent);
        node.GetComponent<HPBarNode>().HPSet(num);
    }
}
