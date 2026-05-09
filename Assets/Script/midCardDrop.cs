using Composition;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class midCardDrop : MonoBehaviour, IDropArea
{

    public List<Card> currCards = new();
    [SerializeField] public bool isFaceUp = false;

    public void dropArea(Card card)
    {
        if (card == null) return;
        if (card.currPile)
        {
            card.currPile.RemoveCard(card);
        }
        card.transform.SetParent(transform);
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


    public void ClearPile()
    {
        while (currCards.Count > 0) {
            RemoveCard(currCards[currCards.Count - 1]);
        }
    }


    public void AddCard(Card card)
    {
        if (card == null) { return; }
        card.currPile = this;
        currCards.Add(card);
        UpdateCardPosition(card);
        card.setCardFacing(isFaceUp);
    }


    public virtual void UpdateCardPosition(Card card)
    {

    }
}
