using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public int initSpawnCount = 5;
    private List<GameObject> normalPlatformList = new List<GameObject>();
    private List<GameObject> commonPlatformList = new List<GameObject>();
    private List<GameObject> winterPlatformList = new List<GameObject>();
    private List<GameObject> grassPlatformList = new List<GameObject>();
    private List<GameObject> spikePlatformLeftList = new List<GameObject>();
    private List<GameObject> spikePlatformRightList = new List<GameObject>();
    private List<GameObject> deathEffectList = new List<GameObject>();
    private List<GameObject> diamondList = new List<GameObject>();
    private ManagerVars vars;
    public static ObjectPool _instance;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        _instance = this;
        Init();
    }

    private void Init()
    {
        for(int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for(int j = 0; j < vars.commonPlatformGoup.Count; j++)
            {
                InstantiateObject(vars.commonPlatformGoup[j], ref commonPlatformList);
            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.WinterPlatformGoup.Count; j++)
            {
                InstantiateObject(vars.WinterPlatformGoup[j], ref winterPlatformList);
            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.GrassPlatformGoup.Count; j++)
            {
                InstantiateObject(vars.GrassPlatformGoup[j], ref grassPlatformList);
            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
        }
        for(int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.deathEffect, ref deathEffectList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.diamond, ref diamondList);
        }
    }

    private GameObject InstantiateObject(GameObject perfab , ref List<GameObject> addList)
    {
        GameObject go = Instantiate(perfab, transform);
        go.SetActive(false);
        addList.Add(go);
        return go;
    }
    /// <summary>
    /// 获得普通平台的方法
    /// </summary>
    /// <returns>The normal plat form.</returns>
    public GameObject GetNormalPlatform()
    {
        //Debug.Log("normalPlatformList  " + normalPlatformList.Count);
        for (int i = 0; i < normalPlatformList.Count; i++)
        {
            if(normalPlatformList[i].activeInHierarchy== false)
            {
                return normalPlatformList[i];
            }
        }
        return InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
    }

    public GameObject GetWinterGroupPlatform()
    {
        for (int i = 0; i < winterPlatformList.Count; i++)
        {
            if (winterPlatformList[i].activeInHierarchy == false)
            {
                return winterPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.WinterPlatformGoup.Count);
        return InstantiateObject(vars.WinterPlatformGoup[ran], ref winterPlatformList);
    }

    public GameObject GetCommonGroupPlatform()
    {
        for (int i = 0; i < commonPlatformList.Count; i++)
        {
            if (commonPlatformList[i].activeInHierarchy == false)
            {
                return commonPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.commonPlatformGoup.Count);
        return InstantiateObject(vars.commonPlatformGoup[ran], ref commonPlatformList);
    }

    public GameObject GetGrassGroupPlatform()
    {
        for (int i = 0; i < grassPlatformList.Count; i++)
        {
            if (grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.GrassPlatformGoup.Count);
        return InstantiateObject(vars.GrassPlatformGoup[ran], ref grassPlatformList);
    }

    public GameObject GetLeftSpikePlatform()
    {
        for (int i = 0; i < spikePlatformLeftList.Count; i++)
        {
            if (spikePlatformLeftList[i].activeInHierarchy == false)
            {
                return spikePlatformLeftList[i];
            }
        }
        return InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
    }
    public GameObject GetRightSpikePlatform()
    {
        for (int i = 0; i < spikePlatformRightList.Count; i++)
        {
            if (spikePlatformRightList[i].activeInHierarchy == false)
            {
                return spikePlatformRightList[i];
            }
        }
        return InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
    }
    public GameObject GetDeatheffect()
    {
        for (int i = 0; i < deathEffectList.Count; i++)
        {
            if (deathEffectList[i].activeInHierarchy == false)
            {
                return deathEffectList[i];
            }
        }
        return InstantiateObject(vars.deathEffect, ref deathEffectList);
    }

    public GameObject GetDiamond()
    {
        for (int i = 0; i < diamondList.Count; i++)
        {
            if (diamondList[i].activeInHierarchy == false)
            {
                return diamondList[i];
            }
        }
        return InstantiateObject(vars.diamond, ref diamondList);
    }
}
