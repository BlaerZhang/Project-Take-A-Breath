using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    public List<Enemy> enemiesInScene;

    public List<Enemy> enemyPrefabs;

    public void OrganizeEnemiesInScene()
    {
        for(int index = 0; index < enemiesInScene.Count; index++)
        {
            enemiesInScene[index].indexInScene = index;
        }
    }

    // Generate Enemy depends on the rate which is affected by the diffculty level
    public void RandomlyRespawnEenmy()
    {
        int index = Random.Range(0, 10);

        if(index < GameMaster.Instance().level)
        {
            GenerateNewEnemy();
        }
    }

    public void GenerateNewEnemy()
    {
        Debug.Log("A new enemy is generated");

        Enemy newEnemy = Instantiate(enemyPrefabs[0]);
        enemiesInScene.Add(newEnemy);
        OrganizeEnemiesInScene();
        newEnemy.transform.position = GetInitialPosition();
        newEnemy.DetectRelativePosPL();        
    }

    public void EnemiesThinking()
    {
        foreach(Enemy unit in enemiesInScene)
        {
            Debug.Log("enemies account  " + enemiesInScene.Count);
            unit.Think();
            
        }
    }

    public Vector3 GetInitialPosition()
    {
        Vector3 initialPos = Vector3.zero;  
        //First, determine which side it is coming from. (N E S W)
        int sideIndex = Random.Range(0, 4);
        //Then, determine which tile it is generated on that side from -3 to 3
        int posIndex = Random.Range(-3, 3);

        if(sideIndex == 0)
        {
            //Come from north
            initialPos += Vector3.up * 3;
            initialPos += Vector3.right * posIndex;
            Debug.Log("Comes from N");

        }
        else if(sideIndex == 1)
        {
            initialPos += Vector3.right * 3;
            initialPos += Vector3.up * posIndex;
            Debug.Log("Comes from E");

        }
        else if(sideIndex == 2)
        {
            initialPos += Vector3.down * 3;
            initialPos += Vector3.right * posIndex;
            Debug.Log("Comes from S");

        }
        else
        {
            initialPos += Vector3.left * 3;
            initialPos += Vector3.up * posIndex;
            Debug.Log("Comes from W");

        }

        return initialPos;
    }

    public void ClearEnemies()
    {
        foreach(Enemy unit in enemiesInScene)
        {
            Destroy(unit.gameObject);
        }
        enemiesInScene = new List<Enemy>();
    }
}
