using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance;


    
    [Header("Health")]
    public int maxHealth = 100;
    public int playerMaxHealth = 10;
    public int currentHealth = 10;
    public TMP_Text healthText;

    [Header("Energy")]
    public int maxEnergy = 100;
    public int playerMaxEnergy = 10;
    public int currentEnergy = 10;
    public TMP_Text energyText;

    [Header("Block")]
    public int maxBlock = 100;
    public int playerMaxBlock = 10;
    public int currentBlock = 10;
    public TMP_Text blockText;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Player Stat Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateAllStatText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAllStatText()
    {
        healthText.DOText(currentHealth + "/" + playerMaxHealth, 0.2f, true, ScrambleMode.Numerals);
        energyText.DOText(currentEnergy.ToString(), 0.2f, true, ScrambleMode.Numerals);
        blockText.DOText(currentBlock.ToString(), 0.2f, true, ScrambleMode.Numerals);
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
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > playerMaxHealth)
        {
            currentHealth = playerMaxHealth;
        }
    }

    public void GainBlock(int block)
    {
        currentBlock += block;
    }

    public void GainEnergy(int energy)
    {
        currentEnergy += energy;
    }

    public void LoseEnergy(int energy)
    {
        currentEnergy -= energy;
    }

    public bool CanUseSkill(int energyCost)
    {
        return currentEnergy >= energyCost;
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
