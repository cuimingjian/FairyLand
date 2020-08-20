using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu(menuName = "CreateManagerVarsContainer")]
public class ManagerVars : ScriptableObject {

    public List<Sprite> bgThemeSpriteList = new List<Sprite>();
    public  float nextXPos = 0.554f;
    public  float nextYPos = 0.645f;
    public GameObject normalPlatformPre;
    public GameObject characterPre;
    public List<Sprite> platformThemeSpriteList = new List<Sprite>();
    public List<Sprite> skinSpriteList = new List<Sprite>();
    public List<Sprite> characterSkinSpriteList = new List<Sprite>();
    public List<GameObject> commonPlatformGoup = new List<GameObject>();
    public List<GameObject> WinterPlatformGoup = new List<GameObject>();
    public List<GameObject> GrassPlatformGoup = new List<GameObject>();
    public GameObject spikePlatformLeft;
    public GameObject spikePlatformRight;
    public GameObject deathEffect;
    public GameObject diamond;
    public GameObject chooseSkin;
    public List<string> skinNameList = new List<string>();
    public List<int> skinPrice = new List<int>();
    public Sprite musicOn, musicOff;

    public AudioClip jumpClip, fallClip, hitClip, diamondClip, buttonClip;

    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }
}
