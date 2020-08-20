using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

    public SpriteRenderer[] renderers;
    private bool startTimer;
    private float m_fallTime;
    public GameObject obstacle;
    private Rigidbody2D m_body;
    public void applySpriteRenderer(Sprite sprite,float fallTime,int obstacleDir)
    {

        m_fallTime = fallTime;
        startTimer = true;
        m_body = GetComponent<Rigidbody2D>();
        m_body.bodyType = RigidbodyType2D.Static;
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sprite = sprite;
        }

        if(obstacleDir == 0)
        {
            if(obstacle!= null)
            {
                obstacle.transform.localPosition = new Vector3(-obstacle.transform.localPosition.x, obstacle.transform.localPosition.y, 0f);
            }
        }
    }

    private void Update()
    {
        if (GameManager._instance.IsGameStarted == false || GameManager._instance.IsGameOver || GameManager._instance.IsPlaying == false) return;
        if (startTimer)
        {
            m_fallTime -= Time.deltaTime;
            if (m_fallTime < 0)
            {
                //掉落 TODO
                if(m_body.bodyType != RigidbodyType2D.Dynamic)
                {
                    m_body.bodyType = RigidbodyType2D.Dynamic;
                    StartCoroutine(DeathHide());
                }
            }
        }
        if (transform.position.y - Camera.main.transform.position.y < -6)
        {
            StartCoroutine(DeathHide());
        }
    }
    private IEnumerator DeathHide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
