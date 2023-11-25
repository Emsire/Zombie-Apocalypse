using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;

    public override void Init()
    {
        MusicData data = GameDataMgr.Instance.musicData;
        togMusic.isOn = data.MusicOpen;
        togSound.isOn = data.SoundOpen;
        sliderMusic.value = data.MusicValue;
        sliderSound.value = data.SoundValue;

        btnClose.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.SaveMusicData();
            UIManager.Instance.HidePanel<SettingPanel>();
        });

        togMusic.onValueChanged.AddListener((v) =>
        {
            BKMusic.Instance.SetIsOpen(v);
            GameDataMgr.Instance.musicData.MusicOpen = v;
        });

        togSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.musicData.SoundOpen = v;
        });

        sliderMusic.onValueChanged.AddListener((v) =>
        {
            BKMusic.Instance.ChangeValue(v);
            GameDataMgr.Instance.musicData.MusicValue = v;
        });

        sliderSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.musicData.SoundValue = v;
        });
    }
}
