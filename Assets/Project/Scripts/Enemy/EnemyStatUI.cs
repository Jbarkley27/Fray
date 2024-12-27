using System.Collections;
using UnityEngine;
using TMPro;

public class EnemyStatUI : MonoBehaviour
{

    public TMP_Text healthText;
    public TMP_Text blockText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI(EnemyStatModule enemy)
    {
        if (enemy == null) return;
        healthText.text = enemy.currentHealth + " / " + enemy.maxHealth;
        blockText.text = enemy.currentBlock.ToString();
    }
}