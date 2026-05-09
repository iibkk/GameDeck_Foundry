using Composition;
using System.Collections.Generic;
using UnityEngine;

public class objSpawner : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private pileManager pilePrefab;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] public List<Card> totalDeck = new();
    public pileManager drawPile = null;

    [SerializeField] public int totalCards = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        getDrawPile();
        ShuffleCards();
    }


    public void getDrawPile()
    {
        drawPile = Object.FindFirstObjectByType<pileManager>();
    }


    public void ShuffleCards()
    {
        /*
        midCardDrop[] allPiles = Object.FindObjectsByType<midCardDrop>(FindObjectsSortMode.None);
        foreach(midCardDrop pile in allPiles) {
            pile.ClearPile();
        }
        
        
        foreach (Card card in totalDeck)
        {
            if (card != null)
            {
                Destroy(card.gameObject);
                totalDeck.Remove(card);
            }
        }
        */
        if (drawPile == null)
        {
            getDrawPile();
            if (drawPile == null) { return; }
        }

        for (int i = 0; i < totalCards; i++)
        {
            SpawnRandomCard();
        }

        for (int i = 0; i < totalDeck.Count; i++)
        {
            totalDeck[i].DropCard(drawPile);
        }
    }


    private void SpawnRandomCard()
    {
        if (totalDeck.Count >= totalCards) { return; }

        Card g = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity, transform);

        int randomValue = Random.Range(1, 14);

        CardDisplay display = g.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.SetCardValue(randomValue);
        }

        totalDeck.Add(g);
    }


    public void SpawnPile()
    {
        pileManager newPile = Instantiate(pilePrefab, spawnPoint.position, Quaternion.identity, transform);
    }
}
