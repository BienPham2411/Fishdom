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
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool isMove;
    private void Awake()
    {
        oriScale = transform.lossyScale.x;
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
        float distX = convertPos.x - Camera.main.transform.position.x;
        float distY = convertPos.y - Camera.main.transform.position.y;
        if ((Mathf.Abs(distX) > 0.5f || Mathf.Abs(distY) > 2f) && GameManager.instance.mode == Mode.Move)
        {
            //GameManager.instance.MoveCamera(GameManager.instance.GetMode());
        }
        else if (GameManager.instance.mode == Mode.Move)
        {
            GameManager.instance.StopCamera();
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

    private int[] pointScale = { 1, 50, 200, 500, 1000, 5000 };
    private float oriScale = 1f;
    private float curScale = 1f;
    private Tween scaleTween;
    private void GetScale(int point)
    {
        if (scaleTween != null)
            scaleTween.Kill();
        curScale = oriScale;
        float camScale = 5;
        for (int i = 0; i < pointScale.Length; i++)
        {
            if (point >= pointScale[i])
            {
                camScale *= 1.1f;
                curScale *= 1.2f;
            }
            else
                break;
        }
        scaleTween = transform.DOScale(curScale, 0.5f);
        GameManager.instance.ScaleCam(camScale, 0.5f);
    }

    public void SetPoint(int point)
    {
        GetScale(point);
        this.point = point;
        textPoint.text = point.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy.GetPoint() < GetPoint())
                EatEnemy(enemy);
            else
                GetEaten(enemy);
        }
        if (collision.tag == "Boost")
        {
            BoostItem item = collision.GetComponent<BoostItem>();
            item.GetPoint(this);
            item.gameObject.SetActive(false);
        }
        if (collision.tag == "Decrease")
        {
            DecreaseItem item = collision.GetComponent<DecreaseItem>();
            item.GetPoint(this);
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



}
