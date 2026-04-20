using Composition;
using System.Collections.Generic;
using UnityEngine;

public class objSpawner : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] public List<Card> totalDeck = new();
    [SerializeField] public pileManager drawPile;
    [SerializeField] public int totalCards = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i  = 0; i < totalCards; i++)
        {
            SpawnRandomCard();
        }
        
        for (int i = 0; i < totalDeck.Count; i++)
        {
            drawPile.AddCard(totalDeck[i]);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    private void SpawnRandomCard()
    {
        Card g = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity, transform);

        int randomValue = Random.Range(1, 14);

        CardDisplay display = g.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.SetCardValue(randomValue);
        }

        totalDeck.Add(g);
    }
}
