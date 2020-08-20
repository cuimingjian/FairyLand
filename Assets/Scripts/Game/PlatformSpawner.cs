using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformGroupType
{
    Grass,
    Winter,

}

public class PlatformSpawner : MonoBehaviour {

    public Vector3 startSpwanPos;
    private ManagerVars vars;
    /// <summary>
    /// 下个平台的位置
    /// </summary>
    private Vector3 platformSpawnPosition;
    /// <summary>
    /// 是否朝左边生成
    /// </summary>
    private bool isLeftSpawn = false;

    /// <summary>
    /// 生成平台的数量
    /// </summary>
    private int spawnPlatformCount;

    private Sprite selectPlatformTheme;
    /// <summary>
    /// 组合平台的类型
    /// </summary>
    private PlatformGroupType groupType;
    /// <summary>
    /// 钉子组合平台是否生成在左边
    /// </summary>
    private bool spikeSpawnLeft = false;
    /// <summary>
    /// 钉子方向平台的位置
    /// </summary>
    private Vector3 spikeDirPlatformPos;
    /// <summary>
    /// 生成钉子平台之后需要在钉子方向生成的平台数量
    /// </summary>
    private int afterSpawnSpikeSpawnCount;

    private bool isSpikeSpawn;

    public int milestoneCount = 10;
    public float fallTime, minFallTime, multiple;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
        vars = ManagerVars.GetManagerVars();

    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath,DecidePath);
    }

    private void Start()
    {
        RandomPlatformTheme();
        platformSpawnPosition = startSpwanPos;
        spawnPlatformCount = 5;

        for (int i = 0; i < 5; i++)
        {
            DecidePath();
        }
        //生成人物
        GameObject player = Instantiate(vars.characterPre);
        player.transform.position = new Vector3(0, -1.8f, 0);
    }
    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        if (isSpikeSpawn)
        {
            AfterSpawnSpike();
            return;
        }
        if(spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }

    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {
        int ranObstacleDir = Random.Range(0, 2);
        if(spawnPlatformCount >= 1)
        {
            SpwanNomalPlatform(ranObstacleDir);
        }
        else if(spawnPlatformCount == 0)
        {
            int ran = Random.Range(0, 3);
            //生成通用组合平台
            if(ran == 0)
            {
                SpwamCommonPlatformGroup(ranObstacleDir);
            }
            //生成主题组合平台
            else if(ran == 1)
            {
                switch (groupType)
                {
                    case PlatformGroupType.Winter:
                        SpwamWinterPlatformGroup(ranObstacleDir);
                        break;
                    case PlatformGroupType.Grass:
                        SpwamGrassPlatformGroup(ranObstacleDir);
                        break;
                    default:
                        break;
                }
            }
            //生成钉子组合平台
            else
            {
                int value = -1;
                if (isLeftSpawn)
                {
                    value = 0;
                }else
                {
                    value = 1;
                }
                SpawnSpikePlatform(value,ranObstacleDir);
                afterSpawnSpikeSpawnCount = 6;
                isSpikeSpawn = true;
                if (spikeSpawnLeft)
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x - 1.65f, platformSpawnPosition.y + vars.nextYPos, 0);
                }
                else
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x + 1.65f, platformSpawnPosition.y + vars.nextYPos, 0);

                }
            }
        }

        int ranSpawnDiamond = Random.Range(0, 10);
        if(ranSpawnDiamond == 5 && GameManager._instance.IsPlaying)
        {
            GameObject go = ObjectPool._instance.GetDiamond();
            go.transform.position = new Vector3(platformSpawnPosition.x, platformSpawnPosition.y + 0.5f, 0);
            go.SetActive(true);
        }
        if (isLeftSpawn)//向左生成
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos);
        }
        else
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos);

        }
    }
    /// <summary>
    /// 生成普通的平台
    /// </summary>
    private void SpwanNomalPlatform(int ranObstacleDir)
    {
        GameObject go = ObjectPool._instance.GetNormalPlatform();
        go.SetActive(true);
        go.transform.position = platformSpawnPosition;
        go.transform.GetComponent<PlatformScript>().applySpriteRenderer(selectPlatformTheme, fallTime, ranObstacleDir);
    }
    /// <summary>
    /// Randoms the platform theme.
    /// </summary>
    private void RandomPlatformTheme()
    {
        int ran = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectPlatformTheme = vars.platformThemeSpriteList[ran];
        if(ran == 2)
        {
            groupType = PlatformGroupType.Winter;
        }
        else
        {
            groupType = PlatformGroupType.Grass;
        }
    }

    private void SpwamCommonPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool._instance.GetCommonGroupPlatform();
        go.SetActive(true);
        go.transform.position = platformSpawnPosition;
        go.transform.GetComponent<PlatformScript>().applySpriteRenderer(selectPlatformTheme, fallTime, ranObstacleDir);
    }
    private void SpwamWinterPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool._instance.GetWinterGroupPlatform();
        go.SetActive(true);
        go.transform.position = platformSpawnPosition;
        go.transform.GetComponent<PlatformScript>().applySpriteRenderer(selectPlatformTheme, fallTime, ranObstacleDir);
    }
    private void SpwamGrassPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool._instance.GetGrassGroupPlatform();
        go.SetActive(true);
        go.transform.position = platformSpawnPosition;
        go.transform.GetComponent<PlatformScript>().applySpriteRenderer(selectPlatformTheme, fallTime, ranObstacleDir);
    }

    private void SpawnSpikePlatform(int dir, int ranObstacleDir)
    {
        GameObject temp = null;
        if(dir == 0)
        {
            spikeSpawnLeft = false;
            temp = ObjectPool._instance.GetRightSpikePlatform();
        }
        else
        {
            spikeSpawnLeft = true;
            temp = ObjectPool._instance.GetLeftSpikePlatform();
        }
        temp.SetActive(true);
        temp.transform.position = platformSpawnPosition;
        temp.transform.GetComponent<PlatformScript>().applySpriteRenderer(selectPlatformTheme, fallTime, ranObstacleDir);
    }
    /// <summary>
    /// 生成钉子平台之后要生成的平台
    /// 包括钉子方向和原来方向
    /// </summary>
    private void AfterSpawnSpike()
    {
        if (afterSpawnSpikeSpawnCount > 0)
        {
            afterSpawnSpikeSpawnCount--;
            for(int i = 0; i < 2; i++)
            {
                GameObject temp = ObjectPool._instance.GetNormalPlatform();
                temp.SetActive(true);
                if (i == 0)//原来方向的平台
                {
                    if (isLeftSpawn)
                    {
                        temp.transform.position = platformSpawnPosition;
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        temp.transform.position = platformSpawnPosition;
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                }
                else//钉子方向的平台
                {
                    if (spikeSpawnLeft)
                    {
                        temp.transform.position = spikeDirPlatformPos;
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x - vars.nextXPos, spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        temp.transform.position = spikeDirPlatformPos;
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x + vars.nextXPos, spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                }
                temp.GetComponent<PlatformScript>().applySpriteRenderer(selectPlatformTheme,fallTime, 1); 
            }
        }
        else
        {
            isSpikeSpawn = false;
            DecidePath();
        }
    }
    private void Update()
    {
        if(GameManager._instance.IsGameStarted && GameManager._instance.IsGameOver == false)
        {
            UpdateFallDown();
        }
    }

    private void UpdateFallDown()
    {
        if(GameManager._instance.GetGameScore() > milestoneCount)
        {
            milestoneCount *= 2;
            fallTime *= multiple;
            if (fallTime < minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }
}
