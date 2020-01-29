using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameState state;
    public static bool Stay{ get; set; }

    public List<CharacterItem> onBoardCharacterList = new List<CharacterItem>();
    public static CharacterItem ci;

    public static int DiceFix, DamFix;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        state = GameState.Ready;
    }

    public List<CharacterItem> inRangeCharacter(int area, int minrange, int maxrange)
    {
        List<CharacterItem> target = new List<CharacterItem>();
        foreach(var item in onBoardCharacterList)
        {
            var itemarea = item.areanum;
            var inmin = area - minrange;
            var outmin = area - maxrange;
            var inmax = area + minrange;
            var outmax = area + maxrange;
            if(itemarea <= inmin && itemarea >= inmax)
            {
                if(itemarea >= outmin && itemarea <= outmax)
                {
                    target.Add(item);
                }
            }
        }
        return target;
    }
}

public enum GameState
{
    Ready, // 初期配置、再配置
    PieceSelect, // 手動
    AttackPhase,　// ダイス処理
    InterruptPhase,　// 攻撃に対する割り込み操作　手動
    AttackReCulc, // 割り込み処理後の計算　自動
    PhaseEnd　// フェイズエンド　最初に戻る
}
