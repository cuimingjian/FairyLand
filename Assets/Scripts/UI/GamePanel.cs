using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {

    private Button btn_Pause;
    private Button btn_Play;
    private Text txt_Score;
    private Text txt_DiamondCount;
    private void Awake()
    {
        btn_Pause = transform.Find("btn_Pause").GetComponent<Button>();
        btn_Play = transform.Find("btn_Play").GetComponent<Button>();
        txt_Score = transform.Find("txt_Score").GetComponent<Text>();
        txt_DiamondCount = transform.Find("Diamond/txt_DiamondCount").GetComponent<Text>();
        gameObject.SetActive(false);
        btn_Play.gameObject.SetActive(false);
        EventCenter.AddListener(EventDefine.ShowGamePanel, Show);
        EventCenter.AddListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
        EventCenter.AddListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
        btn_Pause.onClick.AddListener(OnPuseButtonClick);
        btn_Play.onClick.AddListener(OnPlayButtonClick);
    }
    /// <summary>
    /// Ons the destroy.
    /// </summary>
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePanel, Show);
        EventCenter.RemoveListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
        EventCenter.RemoveListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
    }
    /// <summary>
    /// Show this instance.
    /// </summary>
    private void Show()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// Ons the puse button click.
    /// </summary>
    private void OnPuseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        if (GameManager._instance.IsGameOver) return;
        btn_Play.gameObject.SetActive(true);
        btn_Pause.gameObject.SetActive(false);
        GameManager._instance.IsGamePause = true;
        Time.timeScale = 0;
    }
    /// <summary>
    /// Ons the play button click.
    /// </summary>
    private void OnPlayButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        btn_Pause.gameObject.SetActive(true);
        btn_Play.gameObject.SetActive(false);
        GameManager._instance.IsGamePause = false;
        Time.timeScale = 1;
    }

    private void UpdateScoreText(int gameScore)
    {
        txt_Score.text = gameScore.ToString();
    }

    private void UpdateDiamondText(int diamondCount)
    {
        txt_DiamondCount.text = diamondCount.ToString();
    }
}
