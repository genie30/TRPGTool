using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SkillFileLoader
{
    const string mpath = "Assets/Resources";
    const string spath = "SkillFile";

    public static List<string> GetFileName()
    {
        DirectoryInfo dir = new DirectoryInfo(mpath + "/" + spath);
        FileInfo[] info = dir.GetFiles("*.csv");
        List<string> list = new List<string>();
        foreach (var item in info)
        {
            var filename = Path.GetFileNameWithoutExtension(item.Name);
            list.Add(filename);
        }
        return list;
    }

    public static List<CharacterSkill> LoadSkill(string filename)
    {
        List<CharacterSkill> skilllist = new List<CharacterSkill>();
        TextAsset csvFile;
        List<string[]> csvdata = new List<string[]>();
        csvFile = Resources.Load(spath + "/" + filename) as TextAsset;
        StringReader sr = new StringReader(csvFile.text);

        while(sr.Peek() != -1)
        {
            string line = sr.ReadLine();
            csvdata.Add(line.Split(','));
        }

        for(var i =1; i < csvdata.Count; i++)
        {
            var item = csvdata[i];
            string name = item[0];
            int type = int.Parse(item[1]);
            int dam = int.Parse(item[2]);
            int cost = int.Parse(item[3]);
            int min = int.Parse(item[4]);
            int max = int.Parse(item[5]);
            bool exp = item[6] == "1";
            bool cut = item[7] == "1";
            bool oc = item[8] == "1";
            bool tc = item[9] == "1";
            bool aa = item[10] == "1";
            bool fd = item[11] == "1";
            string memo = item[12];
            int cor = int.Parse(item[13]);
            int san = int.Parse(item[14]);
            int mv = int.Parse(item[15]);

            CharacterSkill skill = new CharacterSkill(name, type, cost, min, max, dam, exp, cut, oc, tc, aa, fd, cor, memo, san, mv);
            skilllist.Add(skill);
        }
        return skilllist;
    }
}
