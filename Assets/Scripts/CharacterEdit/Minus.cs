using UnityEngine;
using UnityEngine.UI;

public class Minus : MonoBehaviour
{
    [SerializeField]
    Text selectstate;

    public void MinusClick()
    {
        int val;
        int.TryParse(selectstate.text, out val);
        val--;
        if(val < 0)
        {
            val = 0;
        }
        selectstate.text = val.ToString();
    }
}
