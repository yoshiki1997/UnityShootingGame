using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static int score {get; private set;}
    float lastEnemyKillTime;
    int streakCount;
    float streakExpiryTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        Enemy.OnDeathStatic += OnEnemyDestroy;
        FindObjectOfType<Player> ().OnDeath += OnPlayerDeath;
    }

    void OnEnemyDestroy()
    {
        if (Time.time < lastEnemyKillTime + streakExpiryTime)
        {
            streakCount++;
        }else {
            streakCount = 0;
        }

        lastEnemyKillTime = Time.time;

        score += 5 + (int)Mathf.Pow(2, streakCount);
    }

    void OnPlayerDeath()
    {
        Enemy.OnDeathStatic -= OnEnemyDestroy;
    }
}
