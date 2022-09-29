using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BoostItem : MonoBehaviour
{
    [SerializeField] private int coef;
    [SerializeField] private int layer;
    public enum BoostType
    {
        Multiply,
        Add
    }
    public BoostType boostType;

    public TextMeshPro textCoef;

    private void Awake()
    {
        SetCoef(GetCoef());
    }

    private void SetCoef(int coef)
    {
        this.coef = coef;
        if (boostType == BoostType.Multiply)
            textCoef.text = "x " + coef;
        if (boostType == BoostType.Add)
            textCoef.text = "+ " + coef;

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

        if (boostType == BoostType.Multiply)
            player.SetPoint(player.GetPoint() * GetCoef());
        if (boostType == BoostType.Add)
            player.SetPoint(player.GetPoint() + GetCoef());
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
