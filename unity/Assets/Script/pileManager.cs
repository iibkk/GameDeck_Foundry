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
        Card c = currCards[i];

        // all cards stack in same place
        c.transform.position = transform.position;
        c.transform.rotation = Quaternion.identity;
        c.transform.localScale = Vector3.one;

        CardDisplay display = c.GetComponent<CardDisplay>();
        if (display != null)
        {
            // later card gets higher sorting order
            display.SetSortingOrder(i * 20);
        }
    }
}
    public void ShufflePile()
    {
        for (int i = 0; i < currCards.Count; i++)
        {
            int randomIndex = Random.Range(i, currCards.Count);

            Card temp = currCards[i];
            currCards[i] = currCards[randomIndex];
            currCards[randomIndex] = temp;
        }

        UpdateCardPosition(null);
        Debug.Log("Pile shuffled");
    }
}