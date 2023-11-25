using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    public Text txtMoney;
    public Text txtName;
    public Text txtUnLock;

    public Button btnLeft;
    public Button btnRight;
    public Button btnStart;
    public Button btnBack;
    public Button btnUnLock;

    private Transform heroPos;

    private int nowIndex;
    private RoleInfo nowRoleData;
    private GameObject heroObj;
    public override void Init()
    {
        heroPos = GameObject.Find("HeroPos").transform;
        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();

        btnLeft.onClick.AddListener(() =>
        {
            nowIndex = (GameDataMgr.Instance.roleInfoList.Count + nowIndex - 1) % GameDataMgr.Instance.roleInfoList.Count;
            ChangeModel();
        });

        btnRight.onClick.AddListener(() =>
        {
            nowIndex = (GameDataMgr.Instance.roleInfoList.Count + nowIndex + 1) % GameDataMgr.Instance.roleInfoList.Count;
            ChangeModel();
        });

        btnUnLock.onClick.AddListener(() =>
        {
            PlayerData data = GameDataMgr.Instance.playerData;
            if(data.haveMoney>=nowRoleData.lockMoney)
            {
                data.haveMoney -= nowRoleData.lockMoney;
                txtMoney.text = data.haveMoney.ToString();
                data.buyHero.Add(nowRoleData.id);
                GameDataMgr.Instance.SavePlayerData();
                UpdateLockBtn();

                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("购买成功");
            }
            else
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("金钱不足");
            }
        });

        btnStart.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.nowSelRole = nowRoleData;
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });

        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            Camera.main.GetComponent<CameraAnimator>().TurnRight(()=>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });
        ChangeModel();
    }

    private void ChangeModel()
    {
        if (heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }

        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos.position, heroPos.rotation);

        //在开始界面不应该可以操纵角色-移除PlayerObject脚本
        Destroy(heroObj.GetComponent<PlayerObject>());

        txtName.text = nowRoleData.tips;
        txtUnLock.text = "￥" + nowRoleData.lockMoney;
        UpdateLockBtn();
    }

    private void UpdateLockBtn()
    {
        if(!GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id) && nowRoleData.lockMoney > 0)
        {
            btnUnLock.gameObject.SetActive(true);
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnLock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }

    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        if(heroObj!=null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
