using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhasePanel : MonoBehaviour
{
    [SerializeField]
    Text phase;
    [SerializeField]
    Button button, pccreatebutton, comcreatebutton, comselectbutton;

    GameState state;

    private void Update()
    {
        if (state == GameManager.state) return;
        state = GameManager.state;
        switch (GameManager.state)
        {
            case GameState.Ready:
                phase.text = "初期配置・再配置";
                CreateText.instance.TextLog("初期配置、再配置を行います。");
                break;
            case GameState.PieceSelect:
                phase.text = "駒作成、配置変更";
                CreateText.instance.TextLog("駒作成、配置変更を行ったらOKを押してください。");
                pccreatebutton.interactable = true;
                comcreatebutton.interactable = true;
                break;
            case GameState.AttackPhase:
                phase.text = "アクションフェイズ";
                CreateText.instance.TextLog("行動値の高い駒でアクションしてください。");
                button.interactable = false;
                comselectbutton.interactable = true;
                break;
            case GameState.InterruptPhase:
                phase.text = "割り込み処理";
                CreateText.instance.TextLog("ラピッド、ジャッジ、ダメージを行ってください。");
                button.interactable = true;
                break;
            case GameState.AttackReCulc:
                phase.text = "ダメージ判定";
                CreateText.instance.TextLog("最終ダメージ判定");
                button.interactable = false;
                comselectbutton.interactable = false;
                break;
            case GameState.PhaseEnd:
                phase.text = "ターン終了";
                CreateText.instance.TextLog("OKを押すと再配置に戻ります。");
                button.interactable = true;
                break;
        }
    }

    public void ButtonClick()
    {
        switch (GameManager.state)
        {
            case GameState.Ready:
                GameManager.state = GameState.PieceSelect;
                break;
            case GameState.PieceSelect:
                GameManager.state = GameState.AttackPhase;
                pccreatebutton.interactable = false;
                comcreatebutton.interactable = false;
                break;
            case GameState.InterruptPhase:
                break;
            case GameState.PhaseEnd:
                GameManager.state = GameState.Ready;
                break;
        }
    }
}
