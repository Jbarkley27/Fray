using System.Collections;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {

    public float projectileMaxRange;


    public struct ProjectileType
    {
        
        public string name;
        [Tooltip("Time between each round")]
        public float fireRate;
        [Tooltip("How many rounds are in a burst (4 Round Burst)")]
        public int burstAmount;
        [Tooltip("How many projectiles are in each round")]
        public int burstsPerShot;
        public float spread;
        public float projectileSpeed;
        [Tooltip("How long between each projectile")]
        public float burstsPerShotFireRate;
        public float range;
        public int damage;
        public GameObject projectile;

        public ProjectileType(string name, float fireRate, int burstAmount, int burstsPerShot, float spread, float projectileSpeed, float burstsPerShotFireRate, float range, int damage, GameObject projectile)
        {
            this.name = name;
            this.fireRate = fireRate;
            this.burstAmount = burstAmount;
            this.burstsPerShot = burstsPerShot;
            this.spread = spread;
            this.projectileSpeed = projectileSpeed;
            this.burstsPerShotFireRate = burstsPerShotFireRate;
            this.range = range;
            this.damage = damage;
            this.projectile = projectile;
        }
    }


    public static ProjectileManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Blaster System, destroying new one.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShootProjectile(ProjectileType projectileType, Transform firePoint, Vector3 direction, bool isEnemy = false)
    {
        if (isEnemy)
        {
            StartCoroutine(ShootProjectileEnemyHelper(projectileType, firePoint, direction));
        }
        else
        {
            StartCoroutine(ShootProjectileHelper(projectileType, firePoint, direction));
        }
    }


    public IEnumerator ShootProjectileHelper(ProjectileType projectileType, Transform firePoint, Vector3 direction)
    {
        for (int i = 0; i < projectileType.burstAmount; i++)
        {
            for (int j = 0; j < projectileType.burstsPerShot; j++)
            {
                GameObject projectile = Instantiate(
                    projectileType.projectile, 
                    GlobalDataStore.instance.player.transform.position,
                    GlobalDataStore.instance.player.transform.rotation);

                projectile.GetComponent<Projectile>().SetupProjectile(direction, projectileType.projectileSpeed, projectileType.damage, projectileType.range);

                yield return new WaitForSeconds(projectileType.burstsPerShotFireRate);
            }

            yield return new WaitForSeconds(projectileType.fireRate);
        }
    }


    public IEnumerator ShootProjectileEnemyHelper(ProjectileType projectileType, Transform firePoint, Vector3 direction)
    {
        for (int i = 0; i < projectileType.burstAmount; i++)
        {
            for (int j = 0; j < projectileType.burstsPerShot; j++)
            {
                GameObject projectile = Instantiate(projectileType.projectile, firePoint.position, Quaternion.LookRotation(direction, Vector3.up));

                projectile.GetComponent<EnemyProjectile>().SetupProjectile(direction, projectileType.projectileSpeed, projectileType.damage, projectileType.range);

                yield return new WaitForSeconds(projectileType.burstsPerShotFireRate);
            }

            yield return new WaitForSeconds(projectileType.fireRate);
        }
    }
}