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
    GameObject toggle;

    [SerializeField]
    InputField name, hhp, ahp, bhp, lhp, sana, sanb, sanc, sand, sane, memo;

    private const string outputDir = "Assets/Resources/ItemData";

    public void CreateClick()
    {
        var group = toggle.GetComponent<ToggleGroup>();
        var type = group.ActiveToggles().FirstOrDefault().name;
        CharactorData data = new CharactorData(img.sprite, name.text, type,
            IPs(hhp), IPs(ahp), IPs(bhp), IPs(lhp), IPs(sana), IPs(sanb), IPs(sanc), IPs(sand), IPs(sane), memo.text);
        SafeCreateDirectory(outputDir);
        var path = Path.Combine(outputDir, name.text + ".asset");
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.Refresh();
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

    private int IPs(InputField s)
    {
        int num;
        var b = int.TryParse(s.text, out num);
        if (b || num < 0) num = 3;
        return num;
    }
}
