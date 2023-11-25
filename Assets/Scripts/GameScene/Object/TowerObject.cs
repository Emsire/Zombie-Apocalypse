using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    private MonsterObject targetObj;
    private List<MonsterObject> targetObjs=new List<MonsterObject>();

    public Transform head;
    public Transform gunPos;

    private Vector3 targetPos;

    private float frontTime;
    private float roundSpeed = 20;

    private TowerInfo info;

    public void InitInfo(TowerInfo info)
    {
        this.info = info;
    }

    void Update()
    {
        if (info.atkType==1)
        {
            if (targetObj == null ||
               targetObj.isDead ||
               Vector3.Distance(this.transform.position, targetObj.transform.position) > info.atkRange)
            {
                targetObj = GameLevelMgr.Instance.FindMonster(this.transform.position, info.atkRange);
            }

            //���û���ҵ��κο��Թ����Ķ��� ��ô��̨�Ͳ�Ӧ����ת
            if (targetObj == null) return;

            targetPos = targetObj.transform.position;
            targetPos.y = head.position.y;

            head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(targetPos - head.position), roundSpeed * Time.deltaTime);

            if (Vector3.Angle(head.forward, targetPos - head.position) < 5 && (Time.time - frontTime) >= info.offsetTime)
            {
                targetObj.Wound(info.atk);
                GameDataMgr.Instance.PlaySound("Music/Tower");

                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPos.position, gunPos.rotation);
                Destroy(effObj, 0.2f);

                frontTime = Time.time;
            }
        }
        else
        {
            targetObjs = GameLevelMgr.Instance.FindMonsters(this.transform.position, info.atkRange);//ÿ��ȥ����������������Ŀ�꣬���幥��ֻ��һ��Ŀ����Խ������������ϸ�Ŀ�긴��

            if (targetObjs.Count>0 && (Time.time - frontTime) >= info.offsetTime)
            {
                GameDataMgr.Instance.PlaySound("Music/Tower");
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPos.position, gunPos.rotation);
                Destroy(effObj, 0.2f);

                for (int i = 0; i < targetObjs.Count; i++)
                    targetObjs[i].Wound(info.atk);

                frontTime = Time.time;
            }
        }
    }
}
