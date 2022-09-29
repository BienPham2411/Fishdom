using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class Enemy : MonoBehaviour
{
    [SerializeField] private int point;
    [SerializeField] private TextMeshPro textPoint;
    public int layer;

    private void Awake()
    {
        oriScale = transform.lossyScale.x;
        SetPoint(GetPoint());
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.mode == Mode.Stay && GameManager.instance.GetPLay() && GetLayer() <= GameManager.instance.GetLayer())
            PlayerController.instance.MoveToEnemy(transform.position);
    }

    public int GetPoint()
    {
        return point;
    }

    public void SetPoint(int point)
    {
        GetScale(point);
        this.point = point;
        textPoint.text = point.ToString();
    }

    public void GetEaten()
    {
        gameObject.SetActive(false);
    }

    public void EatPlayer(PlayerController player)
    {
        SetPoint(player.GetPoint() + GetPoint());
    }

    public void SetLayer(int layer)
    {
        this.layer = layer;
    }

    public int GetLayer()
    {
        return layer;
    }

    private int[] pointScale = { 1, 50, 200, 500, 1000, 5000 };
    private float oriScale = 1f;
    private float curScale = 1f;

    private void GetScale(int point)
    {
        curScale = oriScale;
        for (int i = 0; i < pointScale.Length; i++)
        {
            if (point >= pointScale[i])
            {
                curScale *= 1.2f;
            }
            else
                break;
        }
        transform.DOScale(curScale, 0.5f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayZone")
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        transform.DOKill();
    }
}
