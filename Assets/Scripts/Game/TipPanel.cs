using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipPanel : MonoBehaviour {
    public Text tipText;
    public Image tipBg;
    private void Awake()
    {
        EventCenter.AddListener<string>(EventDefine.ShowTip, Show);
    }
    // Use this for initialization
    void Start () {
        tipText.color = new Color(tipText.color.r, tipText.color.g, tipText.color.b, 0);
        tipBg.color = new Color(tipBg.color.r, tipBg.color.g, tipBg.color.b, 0);
	}
    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.ShowTip, Show);
    }

    private void Show(string text)
    {
        StopCoroutine("Dealy");
        tipText.text = text;
        transform.localPosition = new Vector3(0, -70, 0);
        transform.DOLocalMoveY(0, 0.3f).OnComplete(()=> {
            StartCoroutine("Dealy");
        });
        tipBg.DOColor(new Color(tipBg.color.r, tipBg.color.g, tipBg.color.b, 0.4f), 0.1f);
        tipText.DOColor(new Color(tipText.color.r, tipText.color.g, tipText.color.b, 1f),0.1f);
    }

    private IEnumerator Dealy()
    {
        yield return new WaitForSeconds(1f);
        transform.DOLocalMoveY(70, 0.3f);
        tipBg.DOColor(new Color(tipBg.color.r, tipBg.color.g, tipBg.color.b, 0), 0.1f);
        tipText.DOColor(new Color(tipText.color.r, tipText.color.g, tipText.color.b, 0), 0.1f);
    }
}
