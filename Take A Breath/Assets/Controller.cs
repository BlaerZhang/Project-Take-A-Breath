using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameMaster gM;
    private MMF_Player breathFeedback;
    private MMF_Player switchFeedback;
    private MMF_Player gotHitFeedback;



    private void Awake()
    {
        gM = GameMaster.Instance();
        breathFeedback = GameObject.Find("BreathFeedback").GetComponent<MMF_Player>();
        switchFeedback = GameObject.Find("SwitchFeedback").GetComponent<MMF_Player>();
        gotHitFeedback = GameObject.Find("GotHitFeedback").GetComponent<MMF_Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gotHitFeedback.IsPlaying)
        {
            breathFeedback.SkipToTheEnd();
            breathFeedback.StopFeedbacks();
        }
        
        if (!gM.ifTakingControl)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                gM.breathSys.ChangeBreath();
                switchFeedback.PlayFeedbacks();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gM.enemySys.EnemiesThinking();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (!breathFeedback.IsPlaying)
                {
                    if (!gotHitFeedback.IsPlaying)
                    {
                        breathFeedback.PlayFeedbacks();
                    }
                }
                if (gM.breathSys.breathTime < 1)
                {
                    gM.breathSys.breathTime += Time.deltaTime;
                }
                else
                {
                    gM.enemySys.EnemiesThinking();

                    gM.breathSys.breathTime = 0;
                    if (!gM.breathSys.underAttack)
                    {
                        gM.breathSys.ChangeBreathPt(1);
                    }
                    else
                    {
                        gM.breathSys.underAttack = false;
                    }

                    gM.enemySys.RandomlyRespawnEenmy();
                    gM.breathScore += 1;
                }
            }
            else
            {
                if (gM.breathSys.breathTime < 1)
                {
                    gM.breathSys.breathTime = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                gM.skillSys.Action("W");
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                gM.skillSys.Action("A");
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                gM.skillSys.Action("D");
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                gM.skillSys.Action("S");
            }
        }

    }
}

   
