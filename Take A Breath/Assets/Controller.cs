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
    public Camera mainCamera;
    public Vector3 cameraTargetDirection;
    public Vector3 cameraOriginalPos;
    public bool onCameraMove = false;
    public float cameraTimer;



    private void Awake()
    {
        gM = GameMaster.Instance();
        breathFeedback = GameObject.Find("BreathFeedback").GetComponent<MMF_Player>();
        switchFeedback = GameObject.Find("SwitchFeedback").GetComponent<MMF_Player>();
        gotHitFeedback = GameObject.Find("GotHitFeedback").GetComponent<MMF_Player>();
        cameraOriginalPos = mainCamera.transform.position;
    }

    public void CameraMove(Vector3 direction)
    {
        cameraTargetDirection = direction;
        onCameraMove = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (onCameraMove)
        {
            cameraTimer += Time.deltaTime;
            mainCamera.transform.localPosition = cameraOriginalPos + cameraTargetDirection * Mathf.Lerp(0, 1, cameraTimer);
            if(cameraTimer >= 1)
            {
                onCameraMove = false;
                cameraTimer = 0;
                cameraOriginalPos = mainCamera.transform.localPosition;
                gM.enemySys.isAfterPLMove = true;
            }
        }
        else
        {

        }

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
            if (!onCameraMove)
            {
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
}

   
