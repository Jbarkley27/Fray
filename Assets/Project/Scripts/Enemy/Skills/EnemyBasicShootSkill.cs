using UnityEngine;
using UnityEngine.EventSystems;


public class EnemyBasicShootSkill : EnemySkill
{
    [Header("Projectile Settings")]
    public float fireRate;
    public int burstAmount;
    public int burstsPerShot;
    public float burstTime;
    public float spread;
    public float projectileSpeed;
    public float burstsPerShotFireRate;
    public float range;
    public int damage;
    public GameObject projectilePrefab;

    public override void UseSkill(Vector3 direction, Transform attackSource, EnemyBaseComponent enemyBaseComponent)
    {
        ProjectileManager.instance.ShootProjectile(new ProjectileManager.ProjectileType(
            skillName,
            fireRate,
            burstAmount,
            burstsPerShot,
            spread,
            projectileSpeed,
            burstsPerShotFireRate,
            range,
            damage,
            projectilePrefab
        ), attackSource, direction, true);
    }
}
