using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSys : MonoBehaviour
{
    public Enemy target;
    public Vector3 faceDirection;
    public BreathSys breathSys;

    public bool DetectEnemy(string direction)
    {
        if(direction == "W")
        {
            faceDirection = Vector3.up;
        }
        else if(direction == "S")
        {
            faceDirection = Vector3.down;
        }
        else if(direction == "A")
        {
            faceDirection = Vector3.left;

        }
        else if(direction == "D")
        {
            faceDirection = Vector3.right;

        }

        foreach(Enemy unit in GameMaster.Instance().enemySys.enemiesInScene)
        {
            if (unit.relativePosOfPL == faceDirection)
            {
                target = unit;
                return true;
            }
        }

        return false;
    }

    public void Action(string direction)
    {
        if (DetectEnemy(direction))
        {
            AttackWithBreath();
        }
    }

    public void AttackWithBreath()
    {
        if(breathSys.activeBreathIndex == 0)
        {
            target.GetAttackDmg(2);
            breathSys.breathDataSCO.firePt -= 1;
        }

        breathSys.DetectOutOfBreath();

        breathSys.UpdateBreathPtUI();
    }
}
