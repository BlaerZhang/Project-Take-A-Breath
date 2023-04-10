using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public BreathSys breathSys;
    public Enemy enemy;

    // Update is called once per frame
    void Update()
    {
        if (!GameMaster.Instance().ifTakingControl)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                breathSys.ChangeBreath();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                enemy.Think();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (breathSys.breathTime < 1)
                {
                    breathSys.breathTime += Time.deltaTime;
                }
                else
                {
                    breathSys.breathTime = 0;
                    if (!breathSys.underAttack)
                    {
                        breathSys.ChangeBreathPt(1);
                    }
                    enemy.Think();
                }
            }
            else
            {
                if (breathSys.breathTime < 1)
                {
                    breathSys.breathTime = 0;
                }
            }
        }
        
    }
}
