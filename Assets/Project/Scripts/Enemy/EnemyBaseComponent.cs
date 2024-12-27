using UnityEngine;

public class EnemyBaseComponent : MonoBehaviour 
{
    public Transform attackSource;
    public EnemyIntentionModule intentionModule;
    public EnemyRotationModule rotationModule;

    public void Start()
    {
        EnemyManager.instance.allEnemies.Add(this);
    }
}