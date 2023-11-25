using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Text txtWin;
    public Text txtInfo;
    public Text txtMoney;
    public Button btnSure;
    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.HidePanel<GameOverPanel>();

            GameLevelMgr.Instance.ClearInfo();

            SceneManager.LoadScene("BeginScene");
        });
    }

    public void InitInfo(int money, bool isWin)
    {
        txtWin.text = isWin ? "通关" : "失败";
        txtInfo.text = isWin ? "获得通关奖励" : "获得失败奖励";
        txtMoney.text = "￥" + money;

        GameDataMgr.Instance.playerData.haveMoney += money;
        GameDataMgr.Instance.SavePlayerData();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;
    }
}
