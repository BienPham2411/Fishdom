using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DecreaseItem : MonoBehaviour
{
    [SerializeField] private int coef;
    [SerializeField] private int layer;
    public enum DecreaseType
    {
        Divide,
        Subtract
    }
    public DecreaseType decreaseType;

    public TextMeshPro textCoef;

    private void Awake()
    {
        SetCoef(GetCoef());
    }

    private void SetCoef(int coef)
    {
        this.coef = coef;
        if (decreaseType == DecreaseType.Divide)
            textCoef.text = 1 + " / " + coef;
        if (decreaseType == DecreaseType.Subtract)
            textCoef.text = "- " + coef;

    }
    public int GetCoef()
    {
        return coef;
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.mode == Mode.Stay && GameManager.instance.GetPLay() && GetLayer() <= GameManager.instance.GetLayer())
            PlayerController.instance.MoveToEnemy(transform.position);
    }

    public void GetPoint(PlayerController player)
    {
        if (decreaseType == DecreaseType.Divide)
        {
            if (player.GetPoint() > GetCoef() && GetCoef() != 0)
                player.SetPoint(player.GetPoint() / GetCoef());
            else
            {
                CanvasManager.instance.OpenLoseGame(true);
                player.gameObject.SetActive(false);
            }
        }
        if (decreaseType == DecreaseType.Subtract)
        {
            if(player.GetPoint() > GetCoef())
                player.SetPoint(player.GetPoint() - GetCoef());
            else
            {
                CanvasManager.instance.OpenLoseGame(true);
                player.gameObject.SetActive(false);
            }
        }
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
