using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResetPanel : MonoBehaviour {

    private Button btn_Yes;
    private Button btn_No;
    private Image img_Bg;
    private GameObject dialog;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowResetPanel, Show); 
        img_Bg = transform.Find("bg").GetComponent<Image>();
        btn_Yes = transform.Find("Dialog/btn_Yes").GetComponent<Button>();
        btn_No = transform.Find("Dialog/btn_No").GetComponent<Button>();
        dialog = transform.Find("Dialog").gameObject;

        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
        btn_No.onClick.AddListener(OnBtnNoClick);
        btn_Yes.onClick.AddListener(OnBtnYesClick);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowResetPanel, Show);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f), 0.3f);
        dialog.transform.DOScale(Vector3.one, 0.3f);
    }

    private void OnBtnYesClick() 
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        gameObject.SetActive(false);
        GameManager._instance.ResetData();
        Invoke("ReLoadScene", 1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBtnNoClick()
    {
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.3f);
        dialog.transform.DOScale(Vector3.zero, 0.3f).OnComplete(()=> {
            gameObject.SetActive(false);
        });
    }
    private void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
