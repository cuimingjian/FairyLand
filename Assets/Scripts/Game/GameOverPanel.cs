using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameOverPanel : MonoBehaviour {

    public Button btn_Rank, btn_Home, btn_Reset;
    public Text txt_Score, txt_BestScore, txt_DiamondCount;
    public Image img_new;
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowGameOverPanel, Show);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGameOverPanel, Show);
    }

    private void Show()
    {
        img_new.gameObject.SetActive(false);
        txt_Score.text = GameManager._instance.GetGameScore().ToString();
        txt_DiamondCount.text = "+" + GameManager._instance.GetDiamondCount().ToString();
        GameManager._instance.UpdateAllDiamond(GameManager._instance.GetDiamondCount());
        gameObject.SetActive(true);
        btn_Rank.onClick.AddListener(OnRankBtnClick);
        btn_Home.onClick.AddListener(OnHomeBtnClick);
        btn_Reset.onClick.AddListener(OnRestartBtnClick);
        if(GameManager._instance.GetGameScore() > GameManager._instance.GetBestScoreByIndex(0))
        {
            ShowNewImg();
        }
        GameManager._instance.SetBestScore(GameManager._instance.GetGameScore());
        txt_BestScore.text ="最高分：" + GameManager._instance.GetBestScoreByIndex(0).ToString();
    }

    private void OnRankBtnClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }

    private void ShowNewImg()
    {
        img_new.gameObject.SetActive(true);
        img_new.transform.DOPunchScale(new Vector3(2, 2, 2), 0.5f, 1, 0.1f);
    }

    private void OnHomeBtnClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        GameData.IsAgainGame = false;
        Invoke("ReLoadScene", 1f);
    }

    private void OnRestartBtnClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        GameData.IsAgainGame = true;
        Invoke("ReLoadScene", 1f);
    }

    private void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
