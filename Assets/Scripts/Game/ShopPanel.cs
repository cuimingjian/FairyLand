using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopPanel : MonoBehaviour {

    private ManagerVars vars;
    private Transform parent;
    public Text txt_Name , txt_Diamond;
    public Button btn_Back , btn_Buy, btn_Select;
    private int selectIndex;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        parent = transform.Find("ScrollRect/Parent");
        EventCenter.AddListener(EventDefine.ShowShopPanel, Show);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.skinSpriteList.Count+2) * 160, 272);
        for (int i = 0; i < vars.skinSpriteList.Count; i++)
        {
            GameObject skinItem = Instantiate(vars.chooseSkin, parent);
            if (GameManager._instance.GetSkinUnlocked(i) == false)
            {
                skinItem.GetComponentInChildren<Image>().color = Color.gray;
            }
            skinItem.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            skinItem.transform.localPosition = new Vector3((i + 1) * 160, 0, 0);
        }
        btn_Back.onClick.AddListener(OnBackBtnClick);
        btn_Buy.onClick.AddListener(OnBuyBtnClick);
        btn_Select.onClick.AddListener(OnSelectBtnClick);
        parent.transform.DOLocalMoveX((GameManager._instance.GetSelectSkin()) * -160, 0.3f);

    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowShopPanel,Show);
    }

    private void Update()
    {
        int currentIndex = (int)Mathf.Round(parent.transform.localPosition.x / -160.0f);
        if (Input.GetMouseButtonUp(0))
        {
            parent.transform.DOLocalMoveX(currentIndex * -160, 0.3f);
        }
        SetItemSize(currentIndex);
        txt_Diamond.text = GameManager._instance.GetAllDiamond().ToString();
    }

    private void SetItemSize(int index)
    {
        //Debug.Log("SetItemSize index =  " + index);
        txt_Name.text = vars.skinNameList[index];
        selectIndex = index;

        for (int i = 0; i < parent.childCount; i++)
        {
            if (index == i)
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
                if (GameManager._instance.GetSkinUnlocked(index))
                {
                    btn_Buy.gameObject.SetActive(false);
                    btn_Select.gameObject.SetActive(true);
                    parent.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
                }
                else
                {
                    btn_Buy.gameObject.SetActive(true);
                    btn_Buy.GetComponentInChildren<Text>().text = vars.skinPrice[index].ToString();
                    btn_Select.gameObject.SetActive(false);
                    parent.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.gray;
                }
            }
            else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            }
        }
    }

    private void OnBackBtnClick()
    {
        //Debug.Log("onBackBtnClick  ");
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
    }

    private void OnSelectBtnClick( )
    {
        //Debug.Log("onSelectBtnClick  " + selectIndex );
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        GameManager._instance.SetSelectSkin(selectIndex);
        EventCenter.Broadcast(EventDefine.ChangeSkin);
    }

    private void OnBuyBtnClick()
    {
        //Debug.Log("onBuyBtnClick  "+selectIndex );
        EventCenter.Broadcast(EventDefine.ButtonClickAudio);
        int price = vars.skinPrice[selectIndex];
        if(price > GameManager._instance.GetAllDiamond())
        {
            EventCenter.Broadcast(EventDefine.ShowTip, "钻石不足， 不能购买");
            return;
        }
        GameManager._instance.UpdateAllDiamond(-price);
        GameManager._instance.SetSkinUnlocked(selectIndex);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
