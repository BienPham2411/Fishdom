using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    public Button btnReplay, btnNext;
    public GameObject dialogWin, dialogLose;

    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        btnReplay.onClick.AddListener(() => GameManager.instance.Init());
        btnNext.onClick.AddListener(() =>
        {
            GameManager.instance.NextLevel();
            OpenWinGame(false);
        });
    }
    public void OpenWinGame(bool isOpen)
    {
        GameManager.instance.SetPlay(!isOpen);
        dialogWin.SetActive(isOpen);
    }

    public void OpenLoseGame(bool isOpen)
    {
        GameManager.instance.SetPlay(!isOpen);
        dialogLose.SetActive(isOpen);
    }


}
