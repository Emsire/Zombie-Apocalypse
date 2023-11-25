using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    public Text txtInfo;
    public Image imgScene;

    public Button btnLeft;
    public Button btnRight;
    public Button btnStart;
    public Button btnBack;

    private int nowIndex;
    private SceneInfo nowSceneInfo;

    public override void Init()
    {
        btnLeft.onClick.AddListener(() =>
        {
            nowIndex = (GameDataMgr.Instance.sceneInfoList.Count + nowIndex - 1) % GameDataMgr.Instance.sceneInfoList.Count;
            ChangeScene();
        });

        btnRight.onClick.AddListener(() =>
        {
            nowIndex = (GameDataMgr.Instance.sceneInfoList.Count + nowIndex + 1) % GameDataMgr.Instance.sceneInfoList.Count;
            ChangeScene();
        });

        btnStart.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();

            //¼ÓÔØ³¡¾°
            AsyncOperation ao = SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);
            ao.completed += ((obj) =>
            {
                GameLevelMgr.Instance.InitInfo(nowSceneInfo);
            });
        });

        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });

        ChangeScene();
    }

    public void ChangeScene()
    {
        nowSceneInfo = GameDataMgr.Instance.sceneInfoList[nowIndex];
        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);

        txtInfo.text = "Ãû³Æ£º\n" + nowSceneInfo.name + "\n" + "ÃèÊö£º\n" + nowSceneInfo.tips;
    }
}
