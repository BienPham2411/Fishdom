using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private Vector3 position;
    [SerializeField] private int point;
    [SerializeField] private TextMeshPro textPoint;
    [SerializeField] private float speed = 2f, distance;
    [SerializeField] private bool isMove;
    private Vector3 lastPos;
    private void Awake()
    {
        lastPos = Camera.main.transform.position;
        distance = 0;
        instance = this;
        SetPoint(GetPoint());
        isMove = false;
    }
    public void MoveToTouchPosition()
    {
        //print(Input.touchCount);
        
        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;
        //isMove = true;
        position = Camera.main.ScreenToWorldPoint(touch.position);
        transform.DOKill();
        //transform.DOMove(new Vector3(position.x, position.y, 0), GetMoveTime(new Vector3(position.x, position.y, 0))).SetEase(Ease.Linear).OnComplete(() => isMove = false);
        transform.DOMove(new Vector3(position.x, position.y, 0), GetMoveTime(new Vector3(position.x, position.y, 0))).SetEase(Ease.Linear);
    }

    public void MoveToEnemy(Vector3 enemyPos)
    {
        if (isMove)
            return;
        isMove = true;
        transform.DOKill();
        transform.DOMove(enemyPos, GetMoveTime(enemyPos)).SetEase(Ease.Linear).OnComplete(() => isMove = false);
    }

    private void Update()
    {
        Vector3 convertPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        distance = (convertPos - lastPos).magnitude;
        if(distance >= 1.5f && GameManager.instance.mode == Mode.Move)
        {
            MoveCamera(Mode.Move);
        }
        if (Input.touchCount > 0 && GameManager.instance.GetMode() == Mode.Move && GameManager.instance.GetPLay())
        {
            //print(Input.touchCount);
            MoveToTouchPosition();
        }
    }

    private float GetMoveTime(Vector3 pos)
    {
        return Vector3.Magnitude(pos - transform.position) / speed;
    }

    public int GetPoint()
    {
        return point;
    }

    public void SetPoint(int point)
    {
        this.point = point;
        textPoint.text = point.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy.GetPoint() < GetPoint())
                EatEnemy(enemy);
            else
                GetEaten(enemy);
        }
        if(collision.tag == "Boost")
        {
            BoostItem item = collision.GetComponent<BoostItem>();
            SetPoint(GetPoint() * item.GetCoef());
            item.gameObject.SetActive(false);
        }
    }

    public void EatEnemy(Enemy enemy)
    {
        GameManager.instance.SetEnemy(GameManager.instance.GetEnemy() + 1);
        SetPoint(GetPoint() + enemy.GetPoint());
        enemy.GetEaten();
        GameManager.instance.CheckWin();
    }

    public void GetEaten(Enemy enemy)
    {
        CanvasManager.instance.OpenLoseGame(true);
        enemy.EatPlayer(this);
        gameObject.SetActive(false);
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
        if(mode == Mode.Stay)
        {
           
        }
    }
}
