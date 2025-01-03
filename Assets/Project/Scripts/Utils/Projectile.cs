using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour 
{
    public int damage;
    public float range;
    public Vector3 direction;
    public float force;
    public bool isSetup = false;

    public void SetupProjectile(Vector3 direction, float force, int damage, float range)
    {
        this.damage = damage;
        this.range = range;
        this.direction = GlobalDataStore.instance.player.transform.position - direction;
        this.force = force;

        // make projectile forward the same as the player forward
        // gameObject.transform.forward = GlobalDataStore.instance.player.transform.forward;

        // gameObject.GetComponent<Rigidbody>().velocity = transform.forward * force;

        // this.direction = TurnBasedManager.instance.movementSystem.gameObject.transform.position;

        gameObject.transform.rotation = GlobalDataStore.instance.player.transform.rotation;

        gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * force;

        isSetup = true;
    }

    // this controls the range
    public IEnumerator RangeLifeTime()
    {
        yield return new WaitForSeconds(range);

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (gameObject == null || !isSetup) return;

        ShouldDestroyItself();

        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * 40, Color.red);
        
        if (TurnBasedManager.instance.IsTimePaused())
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        } else
        {
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * force;
        }
    }

    public void ShouldDestroyItself()
    {

        if (Mathf.Abs(transform.position.x) > ProjectileManager.instance.projectileMaxRange 
            || 
            Mathf.Abs(transform.position.z) > ProjectileManager.instance.projectileMaxRange)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "enemy-visual")
        {
            Debug.Log("Hit enemy");

            EnemyStatModule enemyStatModule = collider.transform.parent.GetComponent<EnemyStatModule>();

            if (enemyStatModule != null)
            {
                enemyStatModule.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }


}