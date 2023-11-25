using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private MonsterInfo monsterInfo;

    public int hp;
    public bool isDead = false;

    private float frontTime;

    public RuntimeAnimatorController Resource { get; private set; }

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
    }

    //初始化
    public void InitInfo(MonsterInfo info)
    {
        monsterInfo = info;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        hp = info.hp;
        agent.speed = agent.acceleration = info.moveSpeed;
        agent.angularSpeed = info.roundSpeed;
    }
    //受伤
    public void Wound(int dmg)
    {
        if (isDead) return;

        hp -= dmg;
        animator.SetTrigger("Wound");

        if(hp<=0)
        {
            Dead();
        }
        else
        {
            GameDataMgr.Instance.PlaySound("Music/Wound");
        }
    }

    //死亡
    public void Dead()
    {
        isDead = true;
        //agent.isStopped = true;
        agent.enabled = false;
        animator.SetBool("Dead", true);

        GameDataMgr.Instance.PlaySound("Music/Dead");
        GameLevelMgr.Instance.player.AddMoney(20);
    }

    public void DeadEvent()
    {
        //GameLevelMgr.Instance.ChangeMonsterNum(-1);
        GameLevelMgr.Instance.RemoveMonster(this);

        Destroy(this.gameObject);
        if(GameLevelMgr.Instance.CheckOver())
            UIManager.Instance.ShowPanel<GameOverPanel>().InitInfo(GameLevelMgr.Instance.player.money, true);
    }

    //出生后开始运动

    public void BornOver()
    {
        animator.SetBool("Run", true);
        agent.SetDestination(MainTowerObject.Instance.transform.position);
    }

    //运动 停止
    private void Update()
    {
        if (isDead) return;

        animator.SetBool("Run", agent.velocity != Vector3.zero);

        if(Vector3.Distance(this.transform.position, MainTowerObject.Instance.transform.position) < 4 &&
            Time.time-frontTime>=monsterInfo.atkOffset)
        {
            animator.SetTrigger("Atk");
            frontTime = Time.time;
        }
    }

    //伤害检测
    private void AtkEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.up + this.transform.forward, 1, 1 << LayerMask.NameToLayer("MainTower"));

        GameDataMgr.Instance.PlaySound("Music/Eat");
        
        for(int i=0;i<colliders.Length;i++)
        {
            if(MainTowerObject.Instance.gameObject==colliders[i].gameObject)
            {
                MainTowerObject.Instance.Wound(monsterInfo.atk);
            }
        }
    }
}
