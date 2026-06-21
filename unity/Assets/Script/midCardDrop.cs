using Composition;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class midCardDrop : MonoBehaviour, IDropArea
{

    [SerializeField] public bool isFaceUp = false;
    public List<Card> currCards = new List<Card>();
    public AnnouncementUI announcementUI;
    public MultiplayerWebSocket multiplayer;
    private HashSet<int> sentCards = new HashSet<int>();


    public void dropArea(Card card)
    {
        string playerName = PlayerPrefs.GetString("playerName", "User");

        if (card.currPile)
        {
            card.currPile.RemoveCard(card);
        }

        card.transform.SetParent(null, true);
        card.transform.localScale = Vector3.one;
        card.transform.rotation = Quaternion.identity;
        AddCard(card);

        if (!(this is handManager))
        {
            CardDisplay display = card.GetComponent<CardDisplay>();
            string cardName = display != null ? display.GetCardName() : "card";

            //    if (announcementUI != null)
            //    {
            //        announcementUI.Show(playerName + " has played " + cardName);
            //    }

            if (multiplayer != null)
            {
                string cardText = display != null && display.cardText != null
        ? display.cardText.text
        : "";

                multiplayer.SendPlayCard(cardName, cardText, card.GetInstanceID(), gameObject.name, display.frontImageUrl, display.backImageUrl, display.IsFaceUp());
            }
        }

        Debug.Log("Card Drop here");
    }


    public void RemoveCard(Card card)
    {
        if (currCards.Contains(card))
        {
            currCards.Remove(card);
            UpdateCardPosition(card);
        }
    }


    public virtual void AddCard(Card card)
    {
        card.currPile = this;
        currCards.Add(card);
        UpdateCardPosition(card);
    }


    public virtual void UpdateCardPosition(Card card)
    {
        float spacing = 2.2f;

        for (int i = 0; i < currCards.Count; i++)
        {
            Card c = currCards[i];

            float startX = -((currCards.Count - 1) * spacing) / 2f;
            Vector3 offset = new Vector3(startX + i * spacing, 0, 0);

            c.transform.position = transform.position + offset;
            c.transform.rotation = Quaternion.Euler(0, 0, 0);

            CardDisplay display = c.GetComponent<CardDisplay>();
            if (display != null)
            {
                display.SetSortingOrder(i * 10);
            }
        }
    }
}
