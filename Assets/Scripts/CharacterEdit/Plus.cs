using UnityEngine;
using UnityEngine.UI;

public class Plus : MonoBehaviour
{
    [SerializeField]
    Text selectstate;

    public void PlusClick()
    {
        int val;
        int.TryParse(selectstate.text, out val);
        val++;
        selectstate.text = val.ToString();
    }
}
