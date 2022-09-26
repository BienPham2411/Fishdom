using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BoostItem : MonoBehaviour
{
    [SerializeField] private int coef;

    public TextMeshPro textCoef;

    private void Awake()
    {
        SetCoef(GetCoef());
    }

    private void SetCoef(int coef)
    {
        this.coef = coef;
        textCoef.text = "x " + coef;
    }
    public int GetCoef()
    {
        return coef;
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.mode == Mode.Stay && GameManager.instance.GetPLay())
            PlayerController.instance.MoveToEnemy(transform.position);
    }
}
