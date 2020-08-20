using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour {
    public List<Text> txt_list = new List<Text>();
    private Button btn_Colse;
    private void Start()
    {
        EventCenter.AddListener(EventDefine.ShowRankPanel, Show);
        for (int i = 0; i < 3; i++)
        {
            //Debug.Log("GetBestScoreByIndex " + GameManager._instance.GetBestScoreByIndex(i).ToString());
            txt_list[i].text = GameManager._instance.GetBestScoreByIndex(i).ToString();
        }
        btn_Colse = transform.Find("btn_Colse").GetComponent<Button>();
        btn_Colse.onClick.AddListener(OnBtnColseClick);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankPanel, Show);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnBtnColseClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        gameObject.SetActive(false);
    }
}
