using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance { get; private set; }

    [Header("General")]
    public GameObject skillUIPrefab;

    [Header("Deck")]
    public List<SkillUI> deck;
    public Transform deckUITransform;

    [Header("Draw")]
    public List<SkillUI> draw;
    public Transform drawUITransform;

    [Header("Hand")]
    public List<SkillUI> hand;
    public Transform handUITransform;

    [Header("Discard")]
    public List<SkillUI> discard;
    public Transform discardUITransform;

    [Header("Exhaust")]
    public List<SkillUI> exhaust;
    public Transform exhaustUITransform;

    public int MaxHandSize = 3;



    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Deck Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitiateBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitiateBattle()
    {
        InitiateDeck();
        InitiateDraw();
    }

    // gets all skills and add them into the deck
    // should only be called when their are changes to the skill list
    public void InitiateDeck()
    {
        ClearAll();
        foreach (Skill skill in SkillManager.instance.skills)
        {
            GameObject skillUIObject = Instantiate(skillUIPrefab, deckUITransform);
            SkillUI skillUI = skillUIObject.GetComponent<SkillUI>();
            skillUI.SetupUI(skill);
            deck.Add(skillUI);
        }
    }

    public void InitiateDraw()
    {
        draw.Clear();
        foreach (SkillUI skillUI in deck)
        {
            SkillUI newSkillUI = Instantiate(skillUI, drawUITransform).GetComponent<SkillUI>();
            draw.Add(newSkillUI);
        }
    }

    public void DrawCard()
    {
        // check if we have the max hand size
        if (!CanDrawCard()) return;

        // check if we have cards in the draw pile
        if (draw.Count <= 0)
        {
            // shuffle the discard pile into the draw pile
            ShuffleDiscardIntoDraw();
        }

        // draw a card
        AddToHand();
    }

    public void AddToHand()
    {
        if (!CanDrawCard()) return;
        
        SkillUI skillUI = draw[0];
        hand.Add(skillUI);
        skillUI.transform.SetParent(handUITransform);
        draw.RemoveAt(0);
    }

    public void AddToExhaust(SkillUI skillUI)
    {
        exhaust.Add(skillUI);
        skillUI.transform.SetParent(exhaustUITransform);
    }

    public void DiscardCard(SkillUI skillUI)
    {
        hand.Remove(skillUI);
        discard.Add(skillUI);
        skillUI.transform.SetParent(discardUITransform);
    }

    public void ShuffleDiscardIntoDraw()
    {
        draw.AddRange(discard);
        discard.Clear();
        Shuffle(draw);
    }

    public void Shuffle(List<SkillUI> skills)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            int randomIndex = Random.Range(i, skills.Count);
            SkillUI temp = skills[i];
            skills[i] = skills[randomIndex];
            skills[randomIndex] = temp;
        }
    }

    public bool CanDrawCard()
    {
        return hand.Count < MaxHandSize;
    }

    public void ClearAll()
    {
        deck.Clear();
        hand.Clear();
        discard.Clear();
        draw.Clear();
        exhaust.Clear();

        foreach (Transform child in deckUITransform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in handUITransform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in discardUITransform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in drawUITransform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in exhaustUITransform)
        {
            Destroy(child.gameObject);
        }
    }
}
