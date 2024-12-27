using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyBaseComponent> allEnemies = new List<EnemyBaseComponent>();

    public static EnemyManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Enemy Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GetAllNextIntention()
    {
        foreach (EnemyBaseComponent enemy in allEnemies)
        {
            enemy.intentionModule.CalculateNextMove();
        }
    }

    public void ExecuteAllNextIntention()
    {
        foreach (EnemyBaseComponent enemy in allEnemies)
        {
            enemy.intentionModule.ExecuteNextMove(
                GlobalDataStore.instance.player.transform.position - enemy.transform.position);
        }
    }

    public void StopAllEnemyMovement()
    {
        foreach (EnemyBaseComponent enemy in allEnemies)
        {
            enemy.rotationModule.StopMovement();
        }
    }

    public void ResumeAllEnemyMovement()
    {
        foreach (EnemyBaseComponent enemy in allEnemies)
        {
            enemy.rotationModule.ResumeMovement();
        }
    }
}