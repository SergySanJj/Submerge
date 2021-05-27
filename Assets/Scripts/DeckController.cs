using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public static DeckController current;

    static System.Random rnd = new System.Random();

    public GameObject cardContainer;

    public List<Deck> decks;

    [SerializeField] 
    public static DeckState deckState;

    public int spawnRaresRate = 3; // every n cards try spawn rare card

    public bool shuffleOnAdd = true;

    public GameObject cardPrefab;

    private void Awake()
    {
        current = this;

        deckState = new DeckState();

    }

    void Start()
    {
        GameEvents.current.onCreateNewCardTrigger += CreateCard;
        GameEvents.current.onSpawnDeathCard += DeathEvent;
        GameEvents.current.onUpdateCheckpointCards += UpdateCheckpointCards;
        GameEvents.current.onRemoveCardsWithTag += RemoveCardsWithTag;

        //if (DeckState.CanBeLoaded())
        //{
        //    deckState.Load();
        //}
        //else
        //{
        //    FormCardList();
        //}
        FormCardList();

        StartCoroutine(CreateCardAfter(0.5f));    
    }

    private void OnDestroy()
    {
        GameEvents.current.onCreateNewCardTrigger -= CreateCard;
        GameEvents.current.onSpawnDeathCard -= DeathEvent;
        GameEvents.current.onUpdateCheckpointCards -= UpdateCheckpointCards;
        GameEvents.current.onRemoveCardsWithTag -= RemoveCardsWithTag;
    }

    private IEnumerator CreateCardAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CreateCard();
    }

    private void FormCardList()
    {
        deckState.cardList = new List<Card>();
        deckState.currentDialogue = new List<Card>();
        deckState.tutorialCards = new List<Card>();
        deckState.neededCheckpoints = new List<Card>();

        deckState.rareCards = new List<Card>();

        foreach (Deck deck in decks)
        {
            AddDeck(deck);
        }
    }

    public void AddDeck(Deck deck)
    {
        foreach (DeckCard deckCard in deck.deckCards)
        {
            if (deckCard.card.isTutorial)
            {
                if (deckCard.card.neededCheckpointsToSpawn == null || deckCard.card.neededCheckpointsToSpawn.Count == 0)
                    deckState.tutorialCards.Add(deckCard.card);
                else
                {
                    deckState.neededCheckpoints.Add(deckCard.card);
                }
            }
            else if (deckCard.deckCardType.Equals(DeckCardType.RARE_EVENT))
            {
                deckState.rareCards.Add(deckCard.card);
            }
            else
            {
                if (deckCard.card.neededCheckpointsToSpawn == null || deckCard.card.neededCheckpointsToSpawn.Count == 0)
                {
                    //ShuffleToLast(deckCard.card);
                    ShuffleBetween(deckCard.card, 0f, 1f);
                }
                else
                {
                    deckState.neededCheckpoints.Add(deckCard.card);
                }
            }
        }

        if (this.shuffleOnAdd)
        {
            Reshuffle();
        }
    }

    public void Reshuffle()
    {
        Shuffle(deckState.cardList);
        Shuffle(deckState.neededCheckpoints);
        Shuffle(deckState.rareCards);
    }

    public static void Shuffle(List<Card> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            Card value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void RemoveCardsWithTag(CardTag tag)
    {
        RemoveIfHastag(deckState.cardList, tag);
        RemoveIfHastag(deckState.currentDialogue, tag);
        RemoveIfHastag(deckState.tutorialCards, tag);
        RemoveIfHastag(deckState.neededCheckpoints, tag);
        RemoveIfHastag(deckState.rareCards, tag);
    }

    private static void RemoveIfHastag(List<Card> list, CardTag tag)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].HasTag(tag))
                list.RemoveAt(i);
        }
    }
    private static void RemoveIfHastag(List<DeckCard> list, CardTag tag)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].card.HasTag(tag))
                list.RemoveAt(i);
        }
    }

    public void UpdateCheckpointCards()
    {
        for (int i = deckState.neededCheckpoints.Count - 1; i >= 0; i--)
        {
            Card card = deckState.neededCheckpoints[i];
            if (CheckpointsMet(card))
            {
                if (card.isTutorial)
                    deckState.tutorialCards.Add(card);
                else 
                    ShuffleToFirst(card);
                deckState.neededCheckpoints.RemoveAt(i);
            }
        }
    }

    public bool CheckpointsMet(Card card)
    {
        foreach(Checkpoint checkpoint in card.neededCheckpointsToSpawn)
        {
            if (!GameState.current.CheckpointPassed(checkpoint))
                return false;
        }
        return true;
    }

    public void ShuffleToLast(Card card)
    {
        deckState.cardList.Add(card);
    }

    public void ShuffleToFirst(Card card)
    {
        deckState.cardList.Insert(0, card);
    }

    public void ShuffleBetween(Card card, float start, float end)
    {
        int n = deckState.cardList.Count;
        int startPos = (int) (start * n);
        int endPos = (int) (end * n);


        int pos = rnd.Next(startPos, endPos);

        deckState.cardList.Insert(pos, card);

        Debug.Log("Shuffle between " + start + " " + end + " (" + startPos + ", " + endPos +") to   " + pos);
        string s = "";
        foreach (Card  c in deckState.cardList)
        {
            s += c.GetHashCode() + ", ";
        }
        Debug.Log(s);

        /*if (card.neededCheckpointsToSpawn == null || card.neededCheckpointsToSpawn.Count == 0)
            deckState.cardList.Insert(pos, card);
        else
            deckState.neededCheckpoints.Add(card);*/
    }

    public void ShuffleBegin(Card card)
    {
        ShuffleBetween(card, 0.0f, 0.33f);
    }

    public void ShuffleMiddle(Card card)
    {
        ShuffleBetween(card, 0.33f, 0.66f);
    }

    public void ShuffleEnd(Card card)
    {
        ShuffleBetween(card, 0.66f, 0.99f);
    }

    public void ShuffleDialogue(Card card)
    {
        deckState.currentDialogue.Add(card);
    }

    public void CreateCard()
    {
        
        Card nextCard = PullNextCard();
        if (nextCard == null)
        {
            Debug.Log("No more cards");
        } else {
            var cardObject = Instantiate(cardPrefab);

            try
            {
                cardObject.GetComponent<CardDisplay>().UpdateCardData(nextCard);

                string[] answers = cardObject.GetComponent<CardDisplay>().card.answers;
                string decision = cardObject.GetComponent<CardDisplay>().card.decisionQuestion;

                GameEvents.current.SetAnswerText(DropSides.LEFT, answers[0]);
                GameEvents.current.SetAnswerText(DropSides.RIGHT, answers[1]);
                GameEvents.current.SetDecisionText(decision);

                cardObject.transform.SetParent(cardContainer.transform, false);
                Vector3 scale = cardObject.transform.localScale;
                cardObject.transform.localScale = Vector3.zero;
                LeanTween.scale(cardObject, scale, 0.5f).setEaseOutQuad();
            } catch (Exception e)
            {
                Debug.Log(e);
                Debug.Log("Problem with " + nextCard);
            }
            
        }        
    }

    public void DeathEvent()
    {
        deckState.currentDialogue.Clear();
        deckState.tutorialCards.Clear();
        deckState.neededCheckpoints.Clear();
        deckState.cardList.Clear();
        deckState.rareCards.Clear();

        var deathCard = Resources.Load("Cards/Persons/PlayPersons/Death/Cards/Death", typeof(Card)) as Card;
        Debug.Log("Got death card " + deathCard.person.personName);
        ShuffleToFirst(deathCard);
    }

    private Card PullNextCard()
    {
        GameState.current.Save();
        deckState.Save();

        Card res = null;
        if (deckState.tutorialCards.Count > 0)
        {
            res = deckState.tutorialCards[0];
            deckState.tutorialCards.RemoveAt(0);
        }
        else if (deckState.currentDialogue.Count > 0)
        {
            res = deckState.currentDialogue[0];
            deckState.currentDialogue.RemoveAt(0);
        }
        else if (deckState.cardList.Count > 0)
        {
            res = deckState.cardList[0];
            deckState.cardList.RemoveAt(0);
            deckState.cardsPassed++;
            TryToSpawnRareCard();
        }
        
        return res;
    }

    private void TryToSpawnRareCard()
    {
        if (deckState.cardsPassed % spawnRaresRate == 0)
        {
            SpawnRareCard();
        }
    }

    private void SpawnRareCard()
    {
        Debug.Log("Spawning rare card");
        if (deckState.rareCards.Count > 0)
        {
            int rareIndex = rnd.Next(deckState.rareCards.Count);
            Card randomRare = deckState.rareCards[rareIndex];
            ShuffleMiddle(randomRare);
            deckState.rareCards.RemoveAt(rareIndex);
        }
    }
}
