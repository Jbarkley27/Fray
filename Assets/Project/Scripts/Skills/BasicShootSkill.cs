using UnityEngine;
using UnityEngine.EventSystems;


public class BasicShootSkill : Skill, IPointerClickHandler
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
    public float damage;
    public GameObject projectilePrefab;

    public override void UseSkill(Vector3 direction)
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
        ), GlobalDataStore.instance.projectileSource, direction);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SkillManager.instance.AssignActiveSkill(this);
    }


}
