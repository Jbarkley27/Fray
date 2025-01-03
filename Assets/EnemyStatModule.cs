using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatModule : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth = 100;

    [Header("Block")]
    public int maxBlock = 100;
    public int currentBlock = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        // take block into account
        if (currentBlock > 0)
        {
            currentBlock -= damage;
            if (currentBlock < 0)
            {
                currentHealth += currentBlock;
                currentBlock = 0;
            }
        }
        else
        {
            currentHealth -= damage;
        }

        Debug.Log("Enemy took damage: " + damage + " Current health: " + currentHealth);
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void GainBlock(int block)
    {
        currentBlock += block;
    }

    public void LostBlock(int amount)
    {
        currentBlock -= amount;

        if (currentBlock < 0)
        {
            currentBlock = 0;
        }
    }

    
}
