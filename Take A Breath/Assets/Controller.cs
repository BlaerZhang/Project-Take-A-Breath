using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameMaster gM;

    private void Awake()
    {
        gM = GameMaster.Instance();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gM.ifTakingControl)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                gM.breathSys.ChangeBreath();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gM.enemySys.EnemiesThinking();
            }

            if (Input.GetKey(KeyCode.Space))
            {
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
