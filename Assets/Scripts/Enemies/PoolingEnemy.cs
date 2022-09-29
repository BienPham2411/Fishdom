using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PoolingEnemy : MonoBehaviour
{
    public List<GameObject> pooledEnemies;
    public GameObject enemyToPool;
    public int enemiesPerTime;
    public float rate;
    public Vector3 spawnPos;
    public int minPoint, maxPoint;
    public int minBossPoint, maxBossPoint;
    public float speed;
    private enum Direction
    {
        Left, 
        Right,
    }
    [SerializeField] private Direction direction;
    public int amountToPool;
    private void Start()
    {
        pooledEnemies = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(enemyToPool, transform);
            tmp.SetActive(false);
            pooledEnemies.Add(tmp);
        }
        MoveEnemies();
    }

    public GameObject GetPooledEnemy()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledEnemies[i].activeInHierarchy)
            {
                return pooledEnemies[i];
            }
        }
        return null;
    }

    public void MoveEnemies()
    {
        StartCoroutine(moveEnemies());
    }

    public int GetActiveEnemies()
    {
        int count = 0;
        for(int i = 0; i < pooledEnemies.Count; i++)
        {
            if (pooledEnemies[i].gameObject.activeInHierarchy)
                count++;
        }
        return count;
    }
    IEnumerator moveEnemies()
    {
        int count = 0;
        if(direction == Direction.Left)
        {
            GameObject item = null;
            Enemy enemy = null;
            while (true)
            {
                if (GetActiveEnemies() >= enemiesPerTime)
                {
                    yield return new WaitForSeconds(1 / rate);
                    continue;
                }
                count++;
                item = GetPooledEnemy();
                item.transform.DOKill();
                item.transform.localPosition = spawnPos;
                item.SetActive(true);
                enemy = item.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (count % enemiesPerTime != 0)
                        enemy.SetPoint(Random.Range(minPoint, maxPoint + 1));
                    else
                        enemy.SetPoint(Random.Range(minBossPoint, maxBossPoint + 1)) ;
                }
                item.transform.DOMoveX(-100f, 100f / speed);
                yield return new WaitForSeconds(1 / rate);
            }
        }
        if (direction == Direction.Right)
        {
            GameObject item = null;
            Enemy enemy = null;
            while (true)
            {
                if (GetActiveEnemies() >= enemiesPerTime)
                {
                    yield return new WaitForSeconds(1 / rate);
                    continue;
                }
                    count++;
                item = GetPooledEnemy();
                item.transform.DOKill();
                item.transform.localPosition = spawnPos;
                item.SetActive(true);
                enemy = item.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (count % enemiesPerTime != 0)
                        enemy.SetPoint(Random.Range(minPoint, maxPoint + 1));
                    else
                        enemy.SetPoint(Random.Range(minBossPoint, maxBossPoint + 1));
                }
                item.transform.DOMoveX(100f, 100f/speed);
                yield return new WaitForSeconds(1 / rate);
            }
        }
    }

}
