using Composition;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class midCardDrop : MonoBehaviour, IDropArea
{

    public List<Card> currCards = new();
    
    
    public void dropArea(Card card)
    {
        if (card.currPile)
        {
            card.currPile.RemoveCard(card);
        }
        //card.transform.SetParent(transform);
        AddCard(card);

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


    public void AddCard(Card card)
    {
        card.currPile = this;
        currCards.Add(card);
        UpdateCardPosition(card);
    }


    public virtual void UpdateCardPosition(Card card)
    {

    }
}
