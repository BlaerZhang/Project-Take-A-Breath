using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool ifNextToPlayer = false;
    public GameObject player;
    public BreathSys breathSys;

    public void Awake()
    {
        GenerateInitialPositionRandomly();
    }

    public void GenerateInitialPositionRandomly()
    {
        int randomPosX = Random.Range(-3, 3);
        transform.position = new Vector3(randomPosX, 3, 0);
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
        breathSys.ChangeBreathPt(-1);
        breathSys.underAttack = true;
        breathSys.DetectOutOfBreath();
    }

    public void DetectPlayerAfterMove()
    {
        float verticalGap = Mathf.Abs(transform.position.y - player.transform.position.y);
        float horizontalGap = Mathf.Abs(transform.position.x - player.transform.position.x);

        if(verticalGap == 0 && horizontalGap == 1)
        {
            ifNextToPlayer = true;
        }
        else if(verticalGap == 1 && horizontalGap == 0)
        {
            ifNextToPlayer = true;
        }
    }

    public void Think()
    {
        if (ifNextToPlayer)
        {
            Attack();
        }
        else
        {
            Move();
        }
    }
}
