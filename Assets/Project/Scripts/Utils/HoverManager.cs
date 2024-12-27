using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoverManager : MonoBehaviour
{
    public float xOffset = 60f;

    public enum PanelType
    {
        EnemyStats,
        Deck,
        DrawPile,
        DiscardPile,
        ExhaustPile

    }

    public GameObject enemyStatsRoot;
    public GameObject deckRoot;
    public GameObject drawPileRoot;
    public GameObject discardPileRoot;
    public GameObject exhaustPileRoot;


    // Start is called before the first frame update
    void Start()
    {
        HideAllPanels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowEnemyStats(EnemyStatModule enemy)
    {
        HideAllPanels();
        enemyStatsRoot.transform.position = Input.mousePosition + new Vector3(xOffset, 0, 0);
        enemyStatsRoot.SetActive(true);
        enemyStatsRoot.GetComponent<EnemyStatUI>().UpdateUI(enemy);
    }

    public void HideEnemyStats()
    {
        if (enemyStatsRoot.activeSelf)
            enemyStatsRoot.SetActive(false);
    }

    public void HideAllPanels()
    {
        enemyStatsRoot.SetActive(false);
        deckRoot.SetActive(false);
        drawPileRoot.SetActive(false);
        discardPileRoot.SetActive(false);
        exhaustPileRoot.SetActive(false);
    }

    public void ShowPanel(string panelType)
    {
        switch (panelType)
        {
            case "Deck":
                deckRoot.SetActive(!deckRoot.activeSelf);
                // hide other panels
                drawPileRoot.SetActive(false);
                discardPileRoot.SetActive(false);
                exhaustPileRoot.SetActive(false);
                break;
            case "DrawPile":
                drawPileRoot.SetActive(!drawPileRoot.activeSelf);
                // hide other panels
                deckRoot.SetActive(false);
                discardPileRoot.SetActive(false);
                exhaustPileRoot.SetActive(false);
                break;
            case "DiscardPile":
                discardPileRoot.SetActive(!discardPileRoot.activeSelf);
                // hide other panels
                deckRoot.SetActive(false);
                drawPileRoot.SetActive(false);
                exhaustPileRoot.SetActive(false);
                break;
            case "ExhaustPile":
                exhaustPileRoot.SetActive(!exhaustPileRoot.activeSelf);
                // hide other panels
                deckRoot.SetActive(false);
                drawPileRoot.SetActive(false);
                discardPileRoot.SetActive(false);
                break;
        }
    }
}
