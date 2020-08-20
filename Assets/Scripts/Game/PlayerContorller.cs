using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerContorller : MonoBehaviour {
    private bool isMoveLeft = false;
    private Vector3 nextPlatformRight;
    private Vector3 nextPlatformLeft;
    private bool isJumping = false;
    private ManagerVars vars;
    private Rigidbody2D my_body;
    public Transform rayDown, rayRight, rayLeft;
    public LayerMask platformLayer,obstacleLayer;
    private SpriteRenderer spriteRenderer;
    private GameObject lastHitGo = null;
    private AudioSource m_audioSource;

	// Use this for initialization
	void Start () {
        vars = ManagerVars.GetManagerVars();
        my_body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        EventCenter.AddListener(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        spriteRenderer.sprite = vars.characterSkinSpriteList[GameManager._instance.GetSelectSkin()];
        m_audioSource = GetComponent<AudioSource>();
	}
	/// <summary>
    ///  判断是否点击到UI
    /// </summary>
    /// <returns><c>true</c>, if pointer over game object was ised, <c>false</c> otherwise.</returns>
    /// <param name="mousePos">Mouse position.</param>
    private bool IsPointerOverGameObject(Vector2 mousePos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePos;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }

    // Update is called once per frame
    void Update () {
        if (IsPointerOverGameObject(Input.mousePosition)) return;
        if (GameManager._instance.IsGameStarted == false || GameManager._instance.IsGameOver || GameManager._instance.IsGamePause == true) return;
        if (Input.GetMouseButtonDown(0)&& !isJumping && nextPlatformLeft != Vector3.zero)
        {
            m_audioSource.PlayOneShot(vars.jumpClip);
            GameManager._instance.IsPlaying = true;
            EventCenter.Broadcast(EventDefine.DecidePath);
            isJumping = true;
            Vector3 mousePos = Input.mousePosition;
            if(mousePos.x <= Screen.width / 2)
            {
                isMoveLeft = true;
            }
            else if(mousePos.x > Screen.width/2)
            {
                isMoveLeft = false;
            }
            Jump();
        }

        if (my_body.velocity.y < 0 && IsRayPlatform() == false && GameManager._instance.IsGameOver == false)
        {
            Debug.Log("IsRayPlatform");
            m_audioSource.PlayOneShot(vars.fallClip);
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager._instance.IsGameOver = true;
            GameManager._instance.IsPlaying = false;
            //游戏结束调用结束界面 TODO
            StartCoroutine(DealyShowGameOverPanel());
        }
        if (isJumping && IsRayObstacle() && GameManager._instance.IsGameOver == false)
        {
            Debug.Log("IsRayObstacle");
            m_audioSource.PlayOneShot(vars.hitClip);
            GameObject deathEffect = ObjectPool._instance.GetDeatheffect();
            deathEffect.SetActive(true);
            deathEffect.transform.position = transform.position;
            GameManager._instance.IsGameOver = true;
            GameManager._instance.IsPlaying = false;
            spriteRenderer.enabled = false;
            //Destroy(gameObject);
            //游戏结束调用结束界面 TODO
            StartCoroutine(DealyShowGameOverPanel());

        }
        if (transform.position.y-Camera.main.transform.position.y < -6)
        {
            Debug.Log("fallClipfallClip");
            m_audioSource.PlayOneShot(vars.fallClip);
            GameManager._instance.IsGameOver = true;
            GameManager._instance.IsPlaying = false;
            spriteRenderer.enabled = false;
            StartCoroutine(DealyShowGameOverPanel());
        }
    }

    IEnumerator DealyShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.0f);
        EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
    }

    /// <summary>
    /// Ises the ray platform.
    /// </summary>
    /// <returns><c>true</c>, if ray platform was ised, <c>false</c> otherwise.</returns>
    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if (hit.collider != null)
        {
            //Debug.Log("hit tag  " + hit.collider.tag);
            if (hit.collider.tag == "Platform")
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Ises the ray obstacle.
    /// </summary>
    /// <returns><c>true</c>, if ray obstacle was ised, <c>false</c> otherwise.</returns>
    private bool IsRayObstacle()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);
        if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "Obstacle")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "Obstacle")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Jump()
    {
        if (isMoveLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.DOMoveX(nextPlatformLeft.x, 0.1f);
            transform.DOMoveY(nextPlatformLeft.y+0.8f, 0.08f);
        }
        else
        {
            transform.localScale = Vector3.one;
            transform.DOMoveX(nextPlatformRight.x, 0.1f);
            transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.08f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            isJumping = false;
            Vector3 currentPlatformPos = collision.gameObject.transform.position;
            nextPlatformLeft = new Vector3(currentPlatformPos.x - vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            nextPlatformRight = new Vector3(currentPlatformPos.x + vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            if (lastHitGo != collision.gameObject)
            {
                if (lastHitGo == null)
                {
                    lastHitGo = collision.gameObject;
                }
                else
                {
                    lastHitGo = collision.gameObject;
                    EventCenter.Broadcast(EventDefine.AddScore);
                }

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionEnter2D   " + collision.collider.tag);
        if (collision.collider.tag == "PickUp")
        {
            m_audioSource.PlayOneShot(vars.diamondClip);
            collision.gameObject.SetActive(false);
            EventCenter.Broadcast(EventDefine.AddDiamond);
        }
    }

    private void ChangeSkin()
    {
        spriteRenderer.sprite = vars.characterSkinSpriteList[GameManager._instance.GetSelectSkin()];
    }

    /// <summary>
    /// Ises the music on.
    /// </summary>
    /// <param name="value">If set to <c>true</c> value.</param>
    private void IsMusicOn(bool value)
    {
        m_audioSource.mute = !value;
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        EventCenter.RemoveListener(EventDefine.ChangeSkin, ChangeSkin);
    }
}
