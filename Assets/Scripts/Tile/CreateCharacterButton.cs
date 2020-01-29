using UnityEngine;

public class CreateCharacterButton : MonoBehaviour
{
    [SerializeField]
    GameObject create;

    public void CharacterCreateClick()
    {
        create.SetActive(true);
    }
}
