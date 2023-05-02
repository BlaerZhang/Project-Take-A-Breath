using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
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
    private MMF_Player enemyWalkFeedback;
    private MMF_Player hitEnemyFeedback;
    private MMF_Player gotHitFeedback;

    public SpriteRenderer fireCircle;

    public bool isIced;
    public bool isFired;
    public bool isFireProtected;

    public void Awake()
    {
        gM = GameMaster.Instance();
        player = gM.player;
        UpdateHealthUI();
        enemyWalkFeedback = GameObject.Find("EnemyWalkFeedback").GetComponent<MMF_Player>();
        hitEnemyFeedback = GameObject.Find("HitEnemyFeedback").GetComponent<MMF_Player>();
        gotHitFeedback = GameObject.Find("GotHitFeedback").GetComponent<MMF_Player>();
    }

    public void GetAttackDmg(int dmg)
    {
        if (isIced)
        {
            healthPt -= dmg * 2;
            isIced = false;
        }
        else
        {
            healthPt -= dmg;
        }
        UpdateHealthUI();
        DeathCheck();
        hitEnemyFeedback.PlayFeedbacks();
    }

    public void GetIced()
    {
        GetAttackDmg(1);
        isIced = true;
    }

    public void GetFired()
    {
        GetAttackDmg(1);
        isFired = true;
    }

    public void GetPushed()
    {
        GetAttackDmg(1);
        PushCheck();

    }

    public void PushCheck()
    {
        foreach(Enemy unit in gM.enemySys.enemiesInScene)
        {
            if(unit.transform.position == this.transform.position + relativePosOfPL)
            {
                //检测到被推向的位置已有其他单位，双方承受伤害
                GetAttackDmg(1);
                unit.GetAttackDmg(1);
                return;
            }
        }

        transform.position += relativePosOfPL;
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
        float horizontalGap = transform.position.x - player.transform.position.x;
        float verticalGap = transform.position.y - player.transform.position.y;
        Vector3 destination = new Vector3();
        enemyWalkFeedback.PlayFeedbacks();


        if (horizontalGap == 0)
        {
            if (verticalGap > 0)
            {
                destination = transform.position + Vector3.down;
            }
            else
            {
                destination = transform.position + Vector3.up;
            }
        }
        else if(verticalGap == 0)
        {
            if (horizontalGap > 0)
            {
                destination = transform.position + Vector3.left;
            }
            else
            {
                destination = transform.position + Vector3.right;
            }
        }
        else
        {
            int randomIndex = Random.Range(0, 2);
            if (randomIndex == 0)
            {
                Debug.Log("Enemy Move hor");

                if (horizontalGap > 0)
                {
                    destination = transform.position + Vector3.left;
                }
                else
                {
                    destination = transform.position + Vector3.right;
                }

            }
            else
            {
                Debug.Log("Enemy Move ver");


                if (verticalGap > 0)
                {
                    destination = transform.position + Vector3.down;
                }
                else
                {
                    destination = transform.position + Vector3.up;
                }

            }
        }

        if (DetectIfOverlap(destination))
        {
            //Move();
        }
        else
        {
            transform.position = destination;
        }

        DetectPlayerAfterMove();
    }

    public bool DetectIfOverlap(Vector3 destination)
    {
        foreach(Enemy other in gM.enemySys.enemiesInScene)
        {
            if(other.transform.position == destination)
            {
                return true;
            }
        }

        return false;
    }

    public void Attack()
    {
        Debug.Log("Player lose 1 HP");
        gM.breathSys.ChangeBreathPt(-1);
        gM.breathSys.underAttack = true;
        gM.breathSys.DetectOutOfBreath();
        gotHitFeedback.PlayFeedbacks();
    }

    public void DetectPlayerAfterMove()
    {
        DetectRelativePosPL();
        if(Mathf.Abs(relativePosOfPL.x) == 0 && Mathf.Abs(relativePosOfPL.y) == 1 || Mathf.Abs(relativePosOfPL.y) == 0 && Mathf.Abs(relativePosOfPL.x)==1)
        {
            ifNextToPlayer = true;
        }
        else
        {
            ifNextToPlayer = false;
        }
    }

    public void Think()
    {
        if (!isIced)
        {
            if (isFired)
            {
                Burining();
            }
            else if (isFireProtected)
            {
                isFireProtected = false;
            }

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

    public void Burining()
    {
        foreach(Enemy unit in gM.enemySys.enemiesInScene)
        {
            Vector3 gap = unit.transform.position - this.transform.position;
            if(gap.magnitude == 1)
            {
                if (!unit.isFireProtected)
                {
                    unit.isFired = true;
                }
            }
        }

        GetAttackDmg(1);
        isFired = false;
        isFireProtected = true;
    }

    private void Update()
    {
        if (isFired)
        {
            fireCircle.enabled = true;
        }
        else
        {
            fireCircle.enabled = false;
        }
    }
}

