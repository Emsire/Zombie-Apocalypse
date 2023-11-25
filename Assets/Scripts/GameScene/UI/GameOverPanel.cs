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
        txtWin.text = isWin ? "ͨ��" : "ʧ��";
        txtInfo.text = isWin ? "���ͨ�ؽ���" : "���ʧ�ܽ���";
        txtMoney.text = "��" + money;

        GameDataMgr.Instance.playerData.haveMoney += money;
        GameDataMgr.Instance.SavePlayerData();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;
    }
}
