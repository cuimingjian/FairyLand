using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class GameData {
    public static bool IsAgainGame = false;

    private static bool IsFristGame;
    private static bool IsMusicOn;
    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    public void SetFristGame(bool isFristGame)
    {
        IsFristGame = isFristGame;
    }

    public void SetMusicOn(bool musicOn)
    {
        IsMusicOn = musicOn;
    }

    public void SetBestScoreArr(int[] bestArr)
    {
        bestScoreArr = bestArr;
    }

    public void SetSelectSkin(int select)
    {
        selectSkin = select;
    }

    public void SetSkinUnlocked(bool[] unlockedArr)
    {
        skinUnlocked = unlockedArr;
    }
     
    public void SetDiamondCount(int count)
    {
         diamondCount = count;
    }

    public bool GetFristGame()
    {
        return IsFristGame;
    }

    public bool GetMusicOn()
    {
        return IsMusicOn;
    }

    public int[] GetBestScoreArr()
    {
        return bestScoreArr;
    }

    public int GetSelectSkin()
    {
        return selectSkin;
    }

    public bool[] GetSkinUnlocked()
    {
        return skinUnlocked;
    }

    public int GetDiamondCount()
    {
        return diamondCount;
    }

}
