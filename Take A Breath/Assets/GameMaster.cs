using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    public EnemySystem enemySys;
    public SkillSys skillSys;
    public BreathSys breathSys;
    public Controller control;
    public Transform playerTrans;
    

    public bool ifLose = false;
    public bool ifTakingControl = false;

    public int level;
    public int breathScore = 0;
    public int stepScore = 0;

    public GameObject loseUI;
    public TextMeshProUGUI breathIndexTMP;
    public TextMeshProUGUI stepIndexTMP;
    public GameObject player;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GameStart();
    }
    public void GameStart()
    {
        level = 10;
        breathScore = 0;
        stepScore = 0;
        player.transform.position = Vector3.zero;
        control.mainCamera.transform.localPosition = Vector3.zero;
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
        control.ResetCam();
        // skillSys.ClearBloods();
        GameStart();
    }

    private void Update()
    {
        if(breathScore < 10)
        {
            breathIndexTMP.text = "<link=\"pulse\">" + "0" + breathScore.ToString() + "</link>";
        }
        else
        {
            breathIndexTMP.text = "<link=\"pulse\">" + breathScore.ToString() + "</link>";
        }

        if (stepScore < 10)
        {
            stepIndexTMP.text = "<link=\"pulse\">" + "0" + stepScore.ToString() + "</link>";
        }
        else
        {
            stepIndexTMP.text = "<link=\"pulse\">" + stepScore.ToString() + "</link>";
        }


        if (ifLose)
        {
            GameOverLose();
        }

    }
}
