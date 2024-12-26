using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour 
{
    public float damage;
    public float range;
    public Vector3 direction;
    public float force;
    public bool isSetup = false;

    public void SetupProjectile(Vector3 direction, float force, float damage, float range)
    {
        this.damage = damage;
        this.range = range;
        this.direction = direction;
        this.force = force;

        // apply force to begin moving
        // gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * force;

        // Debug.Log("Velocity: " + direction.normalized * force);

        gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * force;

        // StartCoroutine(RangeLifeTime());

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
        
        if (TurnBasedManager.instance.IsTimePaused())
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        } else
        {
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * force;
        }

        // Debug.Log("Velocity: " + gameObject.GetComponent<Rigidbody>().velocity);
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "enemy-visual")
        {
            Debug.Log("Hit enemy");
        }
    }


}