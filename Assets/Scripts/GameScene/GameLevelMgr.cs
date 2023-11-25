using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;
    private GameLevelMgr() { }

    public PlayerObject player;

    private List<MonsterPoint> points = new List<MonsterPoint>();

    private int nowWaveNum = 0;
    private int maxWaveNum = 0;

    //private int nowMonsterNum = 0;
    private List<MonsterObject> monsterList = new List<MonsterObject>();

    //初始化人物，界面显示
    public void InitInfo(SceneInfo sceneInfo)
    {
        UIManager.Instance.ShowPanel<GamePanel>();
        //创建人物
        RoleInfo info = GameDataMgr.Instance.nowSelRole;
        Transform burnPoint = GameObject.Find("HeroBurnPos").transform;
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(info.res), burnPoint.position, burnPoint.rotation);
        //初始化人物
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(info.atk, sceneInfo.money);
        //设置摄像机跟随的目标
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);

        //初始化主塔
        MainTowerObject.Instance.UpdateHp(sceneInfo.towerHP, sceneInfo.towerHP);
    }

    public void AddMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
    }

    public void UpdateMaxNum(int num)
    {
        nowWaveNum += num;
        maxWaveNum = nowWaveNum;

        UIManager.Instance.ShowPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }

    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum += num;
        UIManager.Instance.ShowPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }

    //public void ChangeMonsterNum(int num)
    //{
    //    nowMonsterNum += num;
    //}
    public void AddMonster(MonsterObject obj)
    {
        monsterList.Add(obj);
    }
    public void RemoveMonster(MonsterObject obj)
    {
        monsterList.Remove(obj);
    }

    public MonsterObject FindMonster(Vector3 pos, int range)
    {
        for(int i=0;i<monsterList.Count;i++)
        {
            if ( !monsterList[i].isDead && Vector3.Distance(monsterList[i].transform.position, pos) <= range)
                return monsterList[i];
        }
        return null;
    }

    public List<MonsterObject> FindMonsters(Vector3 pos, int range)
    {
        List<MonsterObject> monsterObjs = new List<MonsterObject>();
        for(int i=0;i<monsterList.Count;i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(monsterList[i].transform.position, pos) <= range)
                monsterObjs.Add(monsterList[i]);
        }
        return monsterObjs;
    }

    //判断游戏是否结束
    public bool CheckOver()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (!points[i].CheckOver()) return false;
        }

        //if (nowMonsterNum == 0) Debug.Log("游戏胜利！");

        return monsterList.Count == 0;
    }

    /// <summary>
    /// 清空当前关卡记录的数据，避免影响下一次切关卡
    /// </summary>
    public void ClearInfo()
    {
        points.Clear();
        player = null;
        monsterList.Clear();
        nowWaveNum = maxWaveNum = 0;
    }
}
