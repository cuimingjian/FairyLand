using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour {

    private Button btn_Start;
    private Button btn_Shop;
    private Button btn_Rank;
    private Button btn_Sound;
    private Button btn_Reset;
    private ManagerVars vars;
    private void Awake()
    {
        Init();
        vars = ManagerVars.GetManagerVars();
    }

    private void Init() {
        btn_Start = transform.Find("btn_Start").GetComponent<Button>();
        btn_Shop = transform.Find("Btns/btn_Shop").GetComponent<Button>();
        btn_Rank = transform.Find("Btns/btn_Rank").GetComponent<Button>();
        btn_Sound = transform.Find("Btns/btn_Sound").GetComponent<Button>();
        btn_Reset = transform.Find("Btns/btn_Reset").GetComponent<Button>();
        btn_Start.onClick.AddListener(OnStartButtonClick);
        btn_Shop.onClick.AddListener(OnShopButtonClick);
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Sound.onClick.AddListener(OnSoundButtonClick);
        btn_Reset.onClick.AddListener(OnResetButtonClick);
        EventCenter.AddListener(EventDefine.ShowMainPanel, Show);
        EventCenter.AddListener(EventDefine.ChangeSkin, ChangeShopSprite);
    }
    private void Start()
    {
        if (GameData.IsAgainGame)
        {
            EventCenter.Broadcast(EventDefine.ShowGamePanel);
            gameObject.SetActive(false);
        }
        //Sound();
        ChangeShopSprite();
    }

    /// <summary>
    /// Ons the start button click.
    /// </summary>
    private void OnStartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        GameManager._instance.IsGameStarted = true;
        EventCenter.Broadcast(EventDefine.ShowGamePanel);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Ons the shop button click.
    /// </summary>
    private void OnShopButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventDefine.ShowShopPanel);
    }
    /// <summary>
    /// Ons the rank button click.
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }
    /// <summary>
    /// Ons the sound button click.
    /// </summary>
    private void OnSoundButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        GameManager._instance.SetIsMusicOn(!GameManager._instance.GetIsMusicOn());
        Sound();
    }
    /// <summary>
    /// Sound this instance.
    /// </summary>
    private void Sound()
    {
        Debug.Log("Sound " + GameManager._instance.GetIsMusicOn());
        if (GameManager._instance.GetIsMusicOn())
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOn;
        }
        else
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOff;
        }
        EventCenter.Broadcast(EventDefine.IsMusicOn, GameManager._instance.GetIsMusicOn());
    }

    /// <summary>
    /// Ons the reset button click.
    /// </summary>
    private void OnResetButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        EventCenter.Broadcast(EventDefine.ShowResetPanel);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowMainPanel,Show);
        EventCenter.RemoveListener(EventDefine.ChangeSkin, ChangeShopSprite);
    }

    private void ChangeShopSprite()
    {
        btn_Shop.transform.GetChild(0).GetComponent<Image>().sprite = vars.skinSpriteList[GameManager._instance.GetSelectSkin()];
    }
}
