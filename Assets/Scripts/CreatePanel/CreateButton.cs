using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using System.IO;

public class CreateButton : MonoBehaviour
{
    [SerializeField]
    Image img;
    [SerializeField]
    GameObject toggle, panel, pref, preffolder;

    [SerializeField]
    InputField name, hhp, ahp, bhp, lhp, sana, sanb, sanc, sand, sane, memo;
    [SerializeField]
    Dropdown dd;

    private const string outputDir = "Assets/Resources/ItemData";

    public void CreateClick()
    {
        var nums = IPs();
        var group = toggle.GetComponent<ToggleGroup>();
        var type = group.ActiveToggles().FirstOrDefault().name;
        CharacterData data = new CharacterData(img.sprite, name.text, type,
            nums[0], nums[1], nums[2], nums[3], nums[4], nums[5], nums[6], nums[7], nums[8], memo.text);
        SafeCreateDirectory(outputDir);
        var path = Path.Combine(outputDir, name.text + ".asset");
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.Refresh();

        var piece = Instantiate(pref, preffolder.transform);
        CharacterDataList.RemoveData(data.name);
        CharacterDataList.ListAdd(data);
        piece.GetComponent<CharacterItem>().data = data;
        piece.name = data.name;
        panel.SetActive(false);
    }

    private void SafeCreateDirectory(string path)
    {
        var currentPath = "";
        var splitChar = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        foreach (var dir in path.Split(splitChar))
        {
            var parent = currentPath;
            currentPath = Path.Combine(currentPath, dir);
            if (!AssetDatabase.IsValidFolder(currentPath))
            {
                AssetDatabase.CreateFolder(parent, dir);
            }
        }
    }

    private int[] IPs()
    {
        int[] num = new int[9];
        InputField[] s = new InputField[9] { hhp, ahp, bhp, lhp, sana, sanb, sanc, sand, sane };
        for(var i = 0; i < s.Length; i++)
        {
            var b = int.TryParse(s[i].text, out num[i]);
            if(i <= 4)
            {
                if (!b || num[i] < 0) num[i] = 3;
            }
            if (num[i] >= 5)
            {
                if (!b) num[i] = 3;
                else if (num[i] < 0) num[i] = 0;
                else if (num[i] > 4) num[i] = 4;
            }

        }
        return num;
    }

    private void OnEnable()
    {
        dd.ClearOptions();
        foreach(var item in CharacterDataList.characterList)
        {
        }
    }
}
