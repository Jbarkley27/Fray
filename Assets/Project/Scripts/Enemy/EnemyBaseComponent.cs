using UnityEngine;

public class EnemyBaseComponent : MonoBehaviour 
{
    public Transform attackSource;
    public EnemyIntentionModule intentionModule;
    public EnemyMovementModule rotationModule;
    public bool MovementWithTracking = false;

    public void Start()
    {
        EnemyManager.instance.allEnemies.Add(this);
    }
}