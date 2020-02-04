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
        var ci = piece.GetComponent<CharacterItem>();
        ci.data = data;
        piece.name = data.name;
        piece.GetComponent<SpriteRenderer>().sprite = data.img;

        GameManager.instance.onBoardCharacterList.Add(ci);
        HPBar.instance.NodeCreate(GameManager.instance.onBoardCharacterList.Count -1);

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
            else if (num[i] >= 5)
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
        name.text = "";
        hhp.text = ahp.text = bhp.text = lhp.text = "4";
        sana.text = sanb.text = sanc.text = sand.text = sane.text = "3";
        memo.text = "";

        dd.ClearOptions();
        List<string> oplist = new List<string>();
        oplist.Add("なし");
        foreach(var item in CharacterDataList.characterList)
        {
            oplist.Add(item.name);
        }
        dd.AddOptions(oplist);
        dd.RefreshShownValue();
    }

    public void DropChange()
    {
        DataLoad(dd.value);
    }

    private void DataLoad(int num)
    {
        if (num == 0) return;
        num -= 1;
        var data = CharacterDataList.characterList[num];

        img.sprite = data.img;
        name.text = data.name;
        var tglChild = toggle.GetComponentsInChildren<Toggle>();
        switch (data.type)
        {
            case "ドール":
                tglChild[0].isOn = true;
                break;
            case "サヴァント":
                tglChild[1].isOn = true;
                break;
            case "ホラー":
                tglChild[2].isOn = true;
                break;
            case "レギオン":
                tglChild[3].isOn = true;
                break;
        }
        hhp.text = data.headhp.ToString();
        ahp.text = data.armhp.ToString();
        bhp.text = data.bodyhp.ToString();
        lhp.text = data.leghp.ToString();
        sana.text = data.sana.ToString();
        sanb.text = data.sanb.ToString();
        sanc.text = data.sanc.ToString();
        sand.text = data.sand.ToString();
        sane.text = data.sane.ToString();
        memo.text = data.memo;
    }
}
