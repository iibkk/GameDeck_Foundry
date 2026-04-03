using Composition;
using UnityEngine;

public class pileManager : midCardDrop
{
    public override void UpdateCardPosition(Card card)
    {
        card.Move(transform.position);
        card.transform.rotation = transform.rotation;
    }
}
