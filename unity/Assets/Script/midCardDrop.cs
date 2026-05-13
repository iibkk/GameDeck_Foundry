using Composition;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class midCardDrop : MonoBehaviour, IDropArea
{

    [SerializeField] public bool isFaceUp = false;
    public List<Card> currCards = new List<Card>();
    public AnnouncementUI announcementUI;


    public void dropArea(Card card)
    {
        string playerName = PlayerPrefs.GetString("playerName", "User");

        if (card.currPile)
        {
            card.currPile.RemoveCard(card);
        }

        card.transform.SetParent(null, true);
        AddCard(card);

        if (!(this is handManager))
        {
            CardDisplay display = card.GetComponent<CardDisplay>();
            string cardName = display != null ? display.GetCardName() : "card";

            if (announcementUI != null)
            {
                announcementUI.Show(playerName + " has played " + cardName);
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

    }
}
