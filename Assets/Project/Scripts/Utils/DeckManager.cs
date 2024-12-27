using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

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
    public TMP_Text drawSizeText;

    [Header("Hand")]
    public List<SkillUI> hand;
    public Transform handUITransform;

    [Header("Discard")]
    public List<SkillUI> discard;
    public Transform discardUITransform;
    public TMP_Text discardSizeText;

    [Header("Exhaust")]
    public List<SkillUI> exhaust;
    public Transform exhaustUITransform;
    public TMP_Text exhaustSizeText;

    public int MaxHandSize = 3;
    public int StartHandSize = 3;

    public SkillUI coreSkillUI;



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
        UpdateAllPileCounts();
    }

    public void HideAllSkillBorders()
    {
        foreach (SkillUI skillUI in hand)
        {
            skillUI.HideActiveBorder();
        }

        coreSkillUI.HideActiveBorder();
    }

    public void UpdateAllPileCounts()
    {
        drawSizeText.text = draw.Count.ToString() + "/" + deck.Count.ToString();
        discardSizeText.text = discard.Count.ToString();
        exhaustSizeText.text = exhaust.Count.ToString();
    }

    public void InitiateBattle()
    {
        InitiateDeck();
        InitiateDraw();
        UpdateCoreSkill();
        for (int i = 0; i < StartHandSize; i++)
        {
            DrawCard();
        }
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

        // we want to copy the deck list so we don't modify the original deck
        foreach (SkillUI skillUI in deck)
        {
            SkillUI newSkillUI = Instantiate(skillUI, drawUITransform).GetComponent<SkillUI>();
            draw.Add(newSkillUI);
        }
    }

    public void UpdateCoreSkill()
    {
        coreSkillUI.SetupUI(SkillManager.instance.coreSkill);
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
        Debug.Log("Adding to hand");
        SkillUI skillUI = draw[0];
        skillUI.transform.SetParent(handUITransform);
        skillUI.HideActiveBorder();
        hand.Add(skillUI);
        draw.RemoveAt(0);
        skillUI.canvasGroup.alpha = 0;
        skillUI.canvasGroup.DOFade(1, Random.Range(0.4f, .6f))
        .SetEase(Ease.InSine)
        .OnComplete(() => {
        });
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
        skillUI.HideActiveBorder();
        skillUI.transform.SetParent(discardUITransform);
        skillUI.canvasGroup.DOFade(0, .1f)
        .SetEase(Ease.OutSine)
        .OnComplete(() => {
            DrawCard();
        });
        
    }

    public void DiscardActiveSkill()
    {
        if (SkillManager.instance.activeSkill == null) return;
        foreach (SkillUI skillUI in hand)
        {
            if (skillUI.skill == SkillManager.instance.activeSkill
                && skillUI != coreSkillUI)
            {
                DiscardCard(skillUI);
                return;
            }
        }
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
        Debug.Log(hand.Count + " -- " + MaxHandSize);
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

    public bool IsInHand(Skill skill)
    {
        foreach (SkillUI skillUI in hand)
        {
            if (skillUI.skill == skill)
            {
                return true;
            }
        }

        if (coreSkillUI.skill == skill)
        {
            return true;
        }

        return false;
    }
}
