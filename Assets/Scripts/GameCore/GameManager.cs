using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
public enum Mode
{
    Stay,
    Move
}

public enum MiniMode
{
    Classic,
    Chest,
    Pearl,
    Boss,
    Treasure,
    Night,
    Victim1,
    Victim2,
    Clone,
    Carnage,

}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static int MAX_LEVEL = 5;
    public static int CAM_SPEED = 3;
    public Mode mode;
    public MiniMode miniMode;
    [SerializeField] private GameObject level;
    [SerializeField] private int levelNum;
    [SerializeField] private List<Enemy> enemyList;
    [SerializeField] private List<Transform> enemyParents;
    [SerializeField] private Transform layers;
    [SerializeField] private int enemyCount;
    [SerializeField] private int curLayer;
    [SerializeField] private bool isPlay;
    public Camera cam;
    public CinemachineVirtualCamera virtualCamera;
    public Transform camTrans;
    private PlayerController player;
    private Vector3 camPos;
    private Vector3 lastPos;
    private void Awake()
    {
        instance = this;
        cam = Camera.main;
        camTrans = cam.transform;
        camPos = camTrans.position;
        lastPos = camPos;
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
            if (item != null)
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
                BoostItem boostItem = enemyParents[i].GetChild(j).GetComponent<BoostItem>();
                if (boostItem != null)
                {
                    boostItem.SetLayer(i);
                }
                DecreaseItem decreaseItem = enemyParents[i].GetChild(j).GetComponent<DecreaseItem>();
                if (decreaseItem != null)
                {
                    decreaseItem.SetLayer(i);
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
            player.transform.DOKill();
            camTrans.DOKill();
            foreach (Enemy item in enemyList)
            {
                item.transform.DOKill();
            }
            Destroy(level);
        }
        Camera.main.DOOrthoSize(5, 0);
        SetPlay(true);
        camTrans.position = camPos;
        if (DataGame.instance.GetLevel() < MAX_LEVEL)
            levelNum = DataGame.instance.GetLevel();
        else
            levelNum = DataGame.instance.GetLevel() % MAX_LEVEL;
        GameObject levelPool = Pooling.instance.GetLevel(levelNum);
        GameObject clone = Instantiate(levelPool, levelPool.transform.position, levelPool.transform.rotation);
        clone.name = "Level " + levelNum;
        level = clone;
        LevelManager levelManage = level.GetComponent<LevelManager>();
        SetMode(levelManage.GetMode());
        layers = level.transform.Find("Layers");
        player = level.transform.Find("Player").GetComponent<PlayerController>();
        virtualCamera.Follow = player.transform;
        if (GetMode() == Mode.Stay)
            virtualCamera.gameObject.SetActive(false);
        if (GetMode() == Mode.Move)
        {
            virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = levelManage.playZone;
            virtualCamera.gameObject.SetActive(true);
        }
        InitEnemy();

    }

    public int GetEnemyMax()
    {
        return enemyList.Count;
    }

    public void CheckLayer()
    {
        if (mode == Mode.Stay)
        {
            for (int i = 0; i < enemyParents[curLayer].childCount; i++)
            {
                GameObject child = enemyParents[curLayer].GetChild(i).gameObject;
                if (child.activeInHierarchy && child.tag == "Enemy")
                    return;
            }
            curLayer++;
            MoveCamera(mode);
        }
    }

    public void CheckWin()
    {
        CheckLayer();
        if (enemyCount >= GetEnemyMax() && mode == Mode.Stay)
        {
            CanvasManager.instance.OpenWinGame(true);
        }
        if (mode == Mode.Move)
        {
            LevelManager levelManage = level.GetComponent<LevelManager>();
            levelManage.currentEnemies++;
            if (levelManage.currentEnemies >= levelManage.targetEnemies)
            {
                CanvasManager.instance.OpenWinGame(true);
            }
        }

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

    public void MoveCamera(Mode mode)
    {
        if (mode == Mode.Move)
        {
            //camTrans.DOKill();

            //Vector3 convertPos = new Vector3(player.transform.position.x, player.transform.position.y, camTrans.position.z);
            //camTrans.DOMove(convertPos, (convertPos - camTrans.position).magnitude / CAM_SPEED).SetEase(Ease.Linear).OnComplete(()=> lastPos = camTrans.position);
        }
        if (mode == Mode.Stay)
        {
            float distance = 0;
            camTrans.DOKill();
            if (curLayer < layers.childCount)
                distance = enemyParents[curLayer - 1].position.y - enemyParents[curLayer].position.y;
            camTrans.DOMoveY(camTrans.position.y - distance, Mathf.Abs(distance) / CAM_SPEED).SetEase(Ease.Linear);

        }
    }

    public void StopCamera()
    {
        camTrans.DOKill();
        lastPos = camTrans.position;
    }

    public void ScaleCam(float scale, float time)
    {
        if (mode == Mode.Stay)
            cam.DOOrthoSize(scale, time);
        if (mode == Mode.Move)
        {
            StopAllCoroutines();
            StartCoroutine(scaleCam(scale, time));
        }
    }

    IEnumerator scaleCam(float scale, float time)
    {
        float unitPerFrame = (scale - virtualCamera.m_Lens.OrthographicSize) / time * Time.deltaTime;
        while (time >= 0)
        {
            virtualCamera.m_Lens.OrthographicSize += unitPerFrame;
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        virtualCamera.m_Lens.OrthographicSize = Mathf.Abs(scale);
    }

}
