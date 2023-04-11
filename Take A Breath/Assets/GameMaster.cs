using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    public EnemySystem enemySys;
    public SkillSys skillSys;
    public BreathSys breathSys;
    public Controller control;

    public bool ifLose = false;
    public bool ifTakingControl = false;

    public int level;
    public int breathScore = 0;

    public GameObject loseUI;
    public TextMeshProUGUI scoreTMP;
    public GameObject player;

    private void Awake()
    {
        instance = this;
        GameStart();
    }

    public void GameStart()
    {
        level = 10;
        breathScore = 0;
        player.transform.position = Vector3.zero;
        enemySys.GenerateNewEnemy();
        breathSys.InitializeBreathPt();
        ifTakingControl = false;

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
        enemySys.ClearEnemies();
        GameStart();
    }

    private void Update()
    {
        if(breathScore < 10)
        {
            scoreTMP.text = "<link=\"pulse\">" + "0" + breathScore.ToString() + "</link>";
        }
        else
        {
            scoreTMP.text = "<link=\"pulse\">" + breathScore.ToString() + "</link>";
        }
        if (ifLose)
        {
            GameOverLose();
        }
    }
}
