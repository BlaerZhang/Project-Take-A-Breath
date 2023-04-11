using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public GameMaster gM;
    public bool ifNextToPlayer = false;
    public GameObject player;
    public Vector3 relativePosOfPL;
    public int healthPt;
    public TextMeshProUGUI healthTMpro;
    public int indexInScene;

    public void Awake()
    {
        gM = GameMaster.Instance();
        player = gM.player;
    }

    public void GetAttackDmg(int dmg)
    {
        healthPt -= dmg;
        UpdateHealthUI();
        DeathCheck();
    }

    public void DeathCheck()
    {
        if(healthPt <= 0 )
        {
            gM.enemySys.enemiesInScene.RemoveAt(indexInScene);
            gM.enemySys.OrganizeEnemiesInScene();
            Destroy(this.gameObject);
        }
    }

    public void UpdateHealthUI()
    {
        healthTMpro.text = healthPt.ToString();
    }

    public void DetectRelativePosPL()
    {
        relativePosOfPL = transform.position - player.transform.position;
    }

    public void Move()
    {
        int randomIndex = Random.Range(0,2);
        if(randomIndex == 0)
        {
            Debug.Log("Enemy Move hor");
            float horizontalGap = transform.position.x - player.transform.position.x;

            if(horizontalGap > 0)
            {
                transform.position += Vector3.left;
            }
            else
            {
                transform.position += Vector3.right;
            }

        }
        else
        {
            Debug.Log("Enemy Move ver");

            float verticalGap = transform.position.y - player.transform.position.y;

            if(verticalGap > 0)
            {
                transform.position += Vector3.down;
            }
            else
            {
                transform.position += Vector3.up;
            }

        }
        
        DetectPlayerAfterMove();
    }

    public void Attack()
    {
        Debug.Log("Player lose 1 HP");
        gM.breathSys.ChangeBreathPt(-1);
        gM.breathSys.underAttack = true;
        gM.breathSys.DetectOutOfBreath();
    }

    public void DetectPlayerAfterMove()
    {
        DetectRelativePosPL();
        if(Mathf.Abs(relativePosOfPL.x) == 0 && Mathf.Abs(relativePosOfPL.y) == 1 || Mathf.Abs(relativePosOfPL.y) == 0 && Mathf.Abs(relativePosOfPL.x)==1)
        {
            ifNextToPlayer = true;
        }
    }

    public void Think()
    {
        if (ifNextToPlayer)
        {
            Debug.Log("Enemy is attacking");
            Attack();
        }
        else
        {
            Move();
        }
    }
}
