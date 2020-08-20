using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {
    public static GameManager _instance;
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:GameManager"/> is game started.
    /// </summary>
    /// <value><c>true</c> if is game started; otherwise, <c>false</c>.</value>
    public bool IsGameStarted { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:GameManager"/> is game over.
    /// </summary>
    /// <value><c>true</c> if is game over; otherwise, <c>false</c>.</value>
    public bool IsGameOver { get; set; }

    public bool IsGamePause { get; set; }

    public bool IsPlaying { get; set; }
    private int gameScore = 0;
    private int gameDiamond = 0;

    private GameData data;
    private static bool IsFristGame;
    private static bool IsMusicOn;
    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    private ManagerVars vars;

    private void Awake()
    {
        _instance = this;
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.AddScore, AddScore);
        EventCenter.AddListener(EventDefine.AddDiamond, AddDiamond);
        if (GameData.IsAgainGame)
        {
            IsGameStarted = true;
            IsGameOver = false;
        }
        InitGameData();

    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddScore);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddDiamond);
    }

    private void AddScore()
    {
        if (IsGameStarted == false || IsGameOver || IsGamePause) return;
        gameScore++;
        EventCenter.Broadcast(EventDefine.UpdateScoreText, gameScore);
    }

    public int GetGameScore()
    {
        return gameScore;
    }

    private void AddDiamond()
    {
        gameDiamond++;
        EventCenter.Broadcast(EventDefine.UpdateDiamondText, gameDiamond);

    }

    public int GetDiamondCount()
    {
        return gameDiamond;
    }
    /// <summary>
    /// 游戏数据写入本地
    /// </summary>
    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.SetBestScoreArr(bestScoreArr);
                data.SetDiamondCount(diamondCount);
                Debug.Log("Save  " + IsMusicOn);
                data.SetMusicOn(IsMusicOn);
                data.SetFristGame(IsFristGame);
                data.SetSelectSkin(selectSkin);
                data.SetSkinUnlocked(skinUnlocked);
                bf.Serialize(fs, data);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    ///读取本地数据
    /// </summary>
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data",FileMode.Open))
            {
               data =(GameData)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    private void InitGameData()
    {
        Read();
        //读取本地数据，为空则是首次游戏
        if(data != null)
        {
            IsFristGame = data.GetFristGame();
        }
        else
        {
            IsFristGame = true;
        }
        //第一次游戏初始化数据
        if (IsFristGame)
        {
            IsFristGame = false;
            IsMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;
            diamondCount = 10;
            data = new GameData();
            Save();
        }
        else//不是第一次读取数据
        {
            Debug.Log("InitGameData  " + data.GetMusicOn());
            IsMusicOn = data.GetMusicOn();
            bestScoreArr = data.GetBestScoreArr();
            selectSkin = data.GetSelectSkin();
            skinUnlocked = data.GetSkinUnlocked();
            diamondCount = data.GetDiamondCount();
        }
    }
    /// <summary>
    /// 获取当前皮肤是否解锁
    /// </summary>
    /// <returns><c>true</c>, if skin unlocked was gotten, <c>false</c> otherwise.</returns>
    /// <param name="index">Index.</param>
    public bool GetSkinUnlocked(int index)
    {
        return skinUnlocked[index];
    }
    /// <summary>
    /// 设置解锁皮肤
    /// </summary>
    /// <param name="index">Index.</param>
    public void SetSkinUnlocked(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }

    public int GetAllDiamond()
    {
        return diamondCount;
    }

    public void UpdateAllDiamond(int count)
    {
        diamondCount += count;
        Save();
    }

    public int GetSelectSkin()
    {
        return selectSkin;
    }

    public void SetSelectSkin(int index)
    {
        selectSkin = index;
        Save();
    }

    public int GetBestScoreByIndex(int index)
    {
        return bestScoreArr[index];
    }

    public void SetBestScore(int score)
    {
        int replace=3;
        for(int i = 0; i < 3; i++)
        {
            if(score > bestScoreArr[i])
            {
                replace = i;
                break;
            }
        }
        if(replace < 2)
        {
            bestScoreArr[replace + 1] = bestScoreArr[replace];
            bestScoreArr[replace] = score;
        }else if(replace == 2)
        {
            bestScoreArr[replace] = score;
        }
        Save();
    }
    /// <summary>
    /// Sets the is music on.
    /// </summary>
    /// <param name="value">If set to <c>true</c> value.</param>
    public void SetIsMusicOn(bool value)
    {
        IsMusicOn = value;
        Save();
    }
    /// <summary>
    /// Gets the is music on.
    /// </summary>
    /// <returns><c>true</c>, if is music on was gotten, <c>false</c> otherwise.</returns>
    public bool GetIsMusicOn()
    {
        return data.GetMusicOn();
    }

    public void ResetData()
    {
        IsFristGame = false;
        IsMusicOn = true;
        bestScoreArr = new int[3];
        selectSkin = 0;
        skinUnlocked = new bool[vars.skinSpriteList.Count];
        skinUnlocked[0] = true;
        diamondCount = 10;
        Save();
    }
}
