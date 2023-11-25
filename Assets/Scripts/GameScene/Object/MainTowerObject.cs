
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObject : MonoBehaviour
{
    private int hp;
    private int maxHp;

    private bool isDead = false;

    private static MainTowerObject instance;
    public static MainTowerObject Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateHp(int hp, int maxHp)
    {
        this.hp = hp;
        this.maxHp = maxHp;
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHP(hp, maxHp);
    }

    public void Wound(int dmg)
    {
        if (isDead) return;

        hp -= dmg;
        if(hp<=0)
        {
            hp = 0;
            isDead = true;
            //ÓÎÏ·½áÊø
            UIManager.Instance.ShowPanel<GameOverPanel>().InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);
        }

        UpdateHp(hp, maxHp);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
