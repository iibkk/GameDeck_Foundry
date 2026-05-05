using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckViewerUI : MonoBehaviour
{
    public Transform deckViewerParent;
    public GameObject deckCardPrefab;

    void Start()
    {
        ShowDeck(new List<CardData>
    {
        new CardData { card_name = "Firewall", description = "Block attack" },
        new CardData { card_name = "Virus", description = "Deal damage" }
    });
    }
    public void ShowDeck(List<CardData> cards)
    {
        foreach (Transform child in deckViewerParent)
        {
            Destroy(child.gameObject);
        }

        foreach (CardData card in cards)
        {
            GameObject obj = Instantiate(deckCardPrefab, deckViewerParent);

            TMP_Text[] texts = obj.GetComponentsInChildren<TMP_Text>();

            texts[0].text = card.card_name;
            texts[1].text = card.description;
        }
    }
}

[System.Serializable]
public class CardData
{
    public string card_name;
    public string description;
}