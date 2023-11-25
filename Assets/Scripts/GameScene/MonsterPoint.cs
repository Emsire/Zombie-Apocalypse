using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    public int maxWave;
    public int monsterNumOneWave;
    private int nowNum;

    public List<int> monsterIDs;
    private int nowID;

    public float createOffsetTime;//每只之间间隔时间
    public float delayTime;//每波之间间隔时间
    public float firstDelayTime;

    private void Start()
    {
        Invoke("CreateWave", firstDelayTime);
        GameLevelMgr.Instance.AddMonsterPoint(this);
        GameLevelMgr.Instance.UpdateMaxNum(maxWave);
    }

    private void CreateWave()
    {
        nowID = monsterIDs[Random.Range(0, monsterIDs.Count)];
        nowNum = monsterNumOneWave;
        CreateMonster();
        --maxWave;
        GameLevelMgr.Instance.ChangeNowWaveNum(-1);
    }

    private void CreateMonster()
    {
        MonsterInfo info = GameDataMgr.Instance.monsterInfoList[nowID - 1];
        GameObject obj = Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);
        MonsterObject monsterObj = obj.AddComponent<MonsterObject>();
        monsterObj.InitInfo(info);

        //GameLevelMgr.Instance.ChangeMonsterNum(1);
        GameLevelMgr.Instance.AddMonster(monsterObj);
        --nowNum;

        if (nowNum == 0)
        {
            if (maxWave > 0)
            {
                Invoke("CreateWave", delayTime);
            }
        }
        else
        {
            Invoke("CreateMonster", createOffsetTime);
        }
    }

    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}
