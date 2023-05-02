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
    private MMF_Player gotHitFeedback;
    private MMF_Player fireHitEnemyFeedback;
    private MMF_Player iceHitEnemyFeedback;
    private MMF_Player windHitEnemyFeedback;
    private MMF_Player breakIceFeedback;

    public SpriteRenderer fireCircle;
    public SpriteRenderer iceSprite;
    public ParticleSystem fireParticle;

    public bool isIced;
    public bool isFired;
    public bool isFireProtected;
    public bool isFireParticlePlaying = false;

    public void Awake()
    {
        gM = GameMaster.Instance();
        player = gM.player;
        UpdateHealthUI();
        enemyWalkFeedback = GameObject.Find("EnemyWalkFeedback").GetComponent<MMF_Player>();
        gotHitFeedback = GameObject.Find("GotHitFeedback").GetComponent<MMF_Player>();
        fireHitEnemyFeedback = GameObject.Find("FireHitEnemyFeedback").GetComponent<MMF_Player>();
        iceHitEnemyFeedback = GameObject.Find("IceHitEnemyFeedback").GetComponent<MMF_Player>();
        windHitEnemyFeedback = GameObject.Find("WindHitEnemyFeedback").GetComponent<MMF_Player>();
        breakIceFeedback = GameObject.Find("BreakIceFeedback").GetComponent<MMF_Player>();
    }

    public void GetAttackDmg(int dmg)
    {
        if (isIced)
        {
            healthPt -= dmg * 2;
            isIced = false;
            
            breakIceFeedback.transform.position = gameObject.transform.position;
            breakIceFeedback.PlayFeedbacks();
        }
        else
        {
            healthPt -= dmg;
        }
        UpdateHealthUI();
        DeathCheck();
    }

    public void GetIced()
    {
        GetAttackDmg(1);
        isIced = true;
        
        iceHitEnemyFeedback.transform.position = gameObject.transform.position;
        iceHitEnemyFeedback.PlayFeedbacks();
    }

    public void GetFired()
    {
        GetAttackDmg(1);
        isFired = true;

        fireHitEnemyFeedback.transform.position = gameObject.transform.position;
        fireHitEnemyFeedback.PlayFeedbacks();
    }

    public void GetPushed()
    {

        if (!isIced)
        {
            PushCheck();
        }

        GetAttackDmg(1);

        windHitEnemyFeedback.transform.position = gameObject.transform.position;
        windHitEnemyFeedback.PlayFeedbacks();
    }

    public void PushCheck()
    {
        foreach(Enemy unit in gM.enemySys.enemiesInScene)
        {
            if(unit.transform.position == this.transform.position + relativePosOfPL)
            {
                //��⵽�������λ������������λ��˫�������˺�
                GetAttackDmg(1);
                unit.GetAttackDmg(1);
                return;
            }
        }

        transform.position += relativePosOfPL;
        gM.skillSys.DetectPreviousFacingEnemy();
    }

    public void DeathCheck()
    {
        if(healthPt <= 0 )
        {
            gM.enemySys.enemiesInScene.RemoveAt(indexInScene);
            gM.enemySys.OrganizeEnemiesInScene();
            DeathTargetCheck();
            Destroy(this.gameObject);
        }
    }

    public void DeathTargetCheck()
    {
        if(gM.skillSys.target == this)
        {
            gM.skillSys.target = null;
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
            enemyWalkFeedback.PlayFeedbacks();
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
        if (isFired && !isFireParticlePlaying)
        {
            fireParticle.Play();
            isFireParticlePlaying = true;
        }
        else if (!isFired)
        {
            fireParticle.Stop();
            isFireParticlePlaying = false;
        }

        if (isIced)
        {
            iceSprite.enabled = true;
        }
        else
        {
            iceSprite.enabled = false;
        }
    }
}

