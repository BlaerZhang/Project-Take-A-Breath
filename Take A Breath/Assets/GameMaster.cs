using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;

    public bool ifLose = false;
    public bool ifTakingControl = false;

    public GameObject loseUI;

    private void Awake()
    {
        instance = this;
    }

    public static GameMaster Instance()
    {
        if(instance == null)
        {
            instance = new GameMaster();
        }

        return instance;
    }

    public void GameOverLose()
    {
        ifLose = false;
        loseUI.SetActive(true);
        ifTakingControl = true;
    }

    public void OnClickRestart()
    {
        loseUI.SetActive(false);
    }

    private void Update()
    {
        if (ifLose)
        {
            GameOverLose();
        }
    }
}
