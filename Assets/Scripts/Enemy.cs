using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int point;
    [SerializeField] private TextMeshPro textPoint;
    public int layer;

    private void Awake()
    {
        SetPoint(GetPoint());
    }

    private void OnMouseDown()
    {
        if(GameManager.instance.mode == Mode.Stay && GameManager.instance.GetPLay() && GetLayer() <= GameManager.instance.GetLayer())
            PlayerController.instance.MoveToEnemy(transform.position);
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
}
