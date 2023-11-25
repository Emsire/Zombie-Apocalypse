using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    public MusicData musicData;

    public List<RoleInfo> roleInfoList;
    public List<SceneInfo> sceneInfoList;

    public PlayerData playerData;

    public RoleInfo nowSelRole;

    public List<MonsterInfo> monsterInfoList;

    public List<TowerInfo> towerInfoList;

    private GameDataMgr()
    {
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }

    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }

    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }

    public void PlaySound(string resName)
    {
        GameObject obj = new GameObject();
        AudioSource a = obj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(resName);
        a.volume = musicData.SoundValue;
        a.mute = !musicData.SoundOpen;
        a.Play();

        GameObject.Destroy(obj, 1);
    }
}
