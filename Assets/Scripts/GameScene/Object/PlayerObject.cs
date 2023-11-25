using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private int atk;
    public int money;

    public float roundSpeed = 50;

    private Animator animator;

    public Transform gunPoint;
    //1、初始化玩家属性

    private void Start()
    {
        animator = this.transform.GetComponent<Animator>();
    }
    public void InitPlayerInfo(int atk, int money)
    {
        this.atk = atk;
        this.money = money;

        UpdateMoney();
    }
    //2、实现玩家对角色的操作
    private void Update()
    {
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));

        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * roundSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            animator.SetLayerWeight(animator.GetLayerIndex("Crouch Layer"), 1);
        else if(Input.GetKeyUp(KeyCode.LeftShift))
            animator.SetLayerWeight(animator.GetLayerIndex("Crouch Layer"), 0);

        if(Input.GetKeyDown(KeyCode.R))
            animator.SetTrigger("Roll");

        if (Input.GetMouseButtonDown(0))
            animator.SetTrigger("Fire");
    }
    //3、开火的事件

    public void KnifeEvent()
    {
        Collider[] collider = Physics.OverlapSphere(this.transform.position + this.transform.forward + Vector3.up, 1, 1 << LayerMask.NameToLayer("Monster"));

        GameDataMgr.Instance.PlaySound("Music/Knife");

        for(int i=0;i<collider.Length;i++)
        {
            MonsterObject obj = collider[i].GetComponent<MonsterObject>();
            if (obj != null && !obj.isDead)
            {
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                effObj.transform.position = collider[i].transform.position + collider[i].transform.up * 1;
                Destroy(effObj, 1);
   
                obj.Wound(this.atk);
                break;
            }
        }
    }

    public void ShootEvent()
    {
        RaycastHit[] hits= Physics.RaycastAll(new Ray(gunPoint.position, this.transform.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));

        GameDataMgr.Instance.PlaySound("Music/Gun");

        for(int i=0;i<hits.Length;i++)
        {
            MonsterObject obj = hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if(obj!=null && !obj.isDead)
            {
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1);

                obj.Wound(this.atk);
                break;
            }
        }
    }

    //4、更新当前钱数
    public void UpdateMoney()
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    public void AddMoney(int money)
    {
        this.money += money;
        UpdateMoney();
    }
}
