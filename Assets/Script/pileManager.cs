using Composition;
using UnityEngine;
using DG.Tweening;
public class pileManager : midCardDrop
{
    [SerializeField] private float boardScale = 1f;
    public override void UpdateCardPosition(Card card)
    {
        for (int i = 0; i < currCards.Count; i++)
        {
            currCards[i].transform.DOKill();                                // stop any previous tween on this card

            Vector3 pos = transform.position;
            pos.z = 0f;                                                     // keep card on the 2D plane

            currCards[i].transform.DOMove(pos, 0.2f);                           // move card to board position
            currCards[i].transform.DORotate(Vector3.zero, 0.2f);                // reset rotation to normal
            currCards[i].transform.DOScale(Vector3.one * boardScale, 0.2f);     // reset size for board

            CardDisplay display = currCards[i].GetComponent<CardDisplay>();
            if (display != null)
            {
                display.SetSortingOrder(i * 2);                             // control which card draws on top
            }
        }
    }
}