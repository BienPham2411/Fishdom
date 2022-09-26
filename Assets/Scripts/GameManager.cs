using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum Mode
{
    Stay,
    Move
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static int MAX_LEVEL = 2;
    public Mode mode;
    [SerializeField] private GameObject level;
    [SerializeField] private int levelNum;
    [SerializeField] private List<Enemy> enemyList;
    [SerializeField] private List<Transform> enemyParents;
    [SerializeField] private Transform layers;
    [SerializeField] private int enemyCount;
    [SerializeField] private int curLayer;
    [SerializeField] private bool isPlay;
    private PlayerController player;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }

    public void SetMode(Mode mode)
    {
        this.mode = mode;
    }

    public Mode GetMode()
    {
        return mode;
    }

    

    private void InitEnemy()
    {
        if (enemyParents.Count > 0)
        {
            enemyParents.Clear();
            enemyList.Clear();
        }
        for (int i = 0; i < layers.childCount; i++)
        {
            Transform item = layers.GetChild(i).Find("Enemies");
            if(item != null)
                enemyParents.Add(item);
        }
        for (int i = 0; i < enemyParents.Count; i++)
        {
            for (int j = 0; j < enemyParents[i].childCount; j++)
            {
                Enemy item = enemyParents[i].GetChild(j).GetComponent<Enemy>();
                if (item != null)
                {
                    enemyList.Add(item);
                    item.SetLayer(i);
                }
            }
        }
        enemyCount = 0;
        curLayer = 0;
    }

    public void Init()
    {
        if (level != null)
        {
            Destroy(level);
        }
        SetPlay(true);
        if(DataGame.instance.GetLevel() < MAX_LEVEL)
            levelNum = DataGame.instance.GetLevel();
        else
            levelNum = DataGame.instance.GetLevel() % MAX_LEVEL;
        GameObject levelPool = Pooling.instance.GetLevel(levelNum);
        GameObject clone = Instantiate(levelPool, levelPool.transform.position, levelPool.transform.rotation);
        clone.name = "Level " + levelNum;
        level = clone;
        layers = level.transform.Find("Layers");
        player = level.transform.Find("PLayer").GetComponent<PlayerController>();
        InitEnemy();

    }

    public int GetEnemyMax()
    {
        return enemyList.Count;
    }

    public void CheckLayer()
    {
        for(int i = 0; i < enemyParents[curLayer].childCount; i++)
        {
            GameObject child = enemyParents[curLayer].GetChild(i).gameObject;
            if (child.activeInHierarchy && child.tag == "Enemy")
                return;
        }
        curLayer++;
    }

    public void CheckWin()
    {
        CheckLayer();
        if (enemyCount >= GetEnemyMax()) 
            CanvasManager.instance.OpenWinGame(true);

    }

    public void NextLevel()
    {
        DataGame.instance.SetLevel(levelNum + 1);
        Init();
    }

    public int GetEnemy()
    {
        return enemyCount;
    }

    public void SetEnemy(int enemy)
    {
        enemyCount = enemy;
    }

    public void SetPlay(bool isPlay)
    {
        this.isPlay = isPlay;
    }

    public bool GetPLay()
    {
        return isPlay;
    }

    public void SetLayer(int curLayer)
    {
        this.curLayer = curLayer;
    }

    public int GetLayer()
    {
        return curLayer;
    }

    private void MoveCamera(Mode mode)
    {
        if (mode == Mode.Move)
        {
            //Camera.main.transform.DOKill();
            //Vector3 convertPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            //Camera.main.transform.DOMove(convertPos, (convertPos - Camera.main.transform.position).magnitude / speed).SetEase(Ease.Linear);
            //lastPos = convertPos;
        }
        if (mode == Mode.Stay)
        {

        }
    }
}
