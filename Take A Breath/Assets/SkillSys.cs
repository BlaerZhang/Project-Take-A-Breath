using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillSys : MonoBehaviour
{
    public Enemy target;
    public Vector3 faceDirection;
    public BreathSys breathSys;
    public GameObject bloodstain;

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
            InstantiateBloodstain(direction);
        }
    }

    public void AttackWithBreath()
    {
        if(breathSys.activeBreathIndex == 0)
        {
            target.GetAttackDmg(2);
            breathSys.breathDataSCO.firePt -= 1;
        }
        else if(breathSys.activeBreathIndex == 1)
        {
            target.GetAttackDmg(2);
            breathSys.breathDataSCO.waterPt -= 1;
        }
        else if (breathSys.activeBreathIndex == 2)
        {
            target.GetAttackDmg(2);
            breathSys.breathDataSCO.bugPt -= 1;
        }

        breathSys.DetectOutOfBreath();

        breathSys.UpdateBreathPtUI();
    }
    
    void InstantiateBloodstain(string direction)
    {
        GameObject obj = new GameObject();
        float distance = Random.Range(2, 5);
        obj = Instantiate(this.bloodstain);

        switch (direction)
        {
            case "D":
                obj.transform.position = Vector3.right * distance;
                break;
            
            case"W":
                obj.transform.position = Vector3.up * distance;
                obj.transform.rotation = Quaternion.Euler(0,0,90);
                break;
            
            case"A":
                obj.transform.position = Vector3.left * distance;
                obj.transform.rotation = Quaternion.Euler(0,0,180);
                break;
            
            case"S":
                obj.transform.position = Vector3.down * distance;
                obj.transform.rotation = Quaternion.Euler(0,0,270);
                break;
        }
    }

    public void ClearBloods()
    {
        List<GameObject> bloodsInScene = GameObject.FindGameObjectsWithTag("Bloodstain").ToList();
        foreach (GameObject bloodstain in bloodsInScene)
        {
            Destroy(bloodstain);
        }
        bloodsInScene = new List<GameObject>();
    }
}
