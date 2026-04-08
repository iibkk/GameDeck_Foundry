using System.Collections.Generic;
using Composition;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class handManager : midCardDrop
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float hoverScale = 1.25f;
    [SerializeField] private float hoverLift = 1.2f;
    [SerializeField] private float pushDistance = 0.7f;
    private Card hoveredCard;
    private void Start()
    {

    }


    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DrawCard();
        }
    }


    private void DrawCard()
    {
        if (currCards.Count >= maxHandSize) return;

        Card g = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity, transform);

        int randomValue = Random.Range(1, 14);

        CardDisplay display = g.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.SetCardValue(randomValue);
        }

        AddCard(g);
    }
    public void SetHoveredCard(Card card)
    {
        hoveredCard = card;
        UpdateCardPosition(null);
    }

    public void ClearHoveredCard(Card card)
    {
        if (hoveredCard == card)
        {
            hoveredCard = null;
            UpdateCardPosition(null);
        }
    }


    public override void UpdateCardPosition(Card card)
    {
        currCards.RemoveAll(cards => cards == null);
        if (currCards.Count == 0) return;                       // stop if no cards in hand

        Spline spline = splineContainer.Spline;

        float handWidth = Mathf.Min(0.12f * (currCards.Count - 1), 0.5f);           // control total hand width
        float startP = 0.5f - handWidth / 2f;                                   
        float step = currCards.Count > 1 ? handWidth / (currCards.Count - 1) : 0f;  // spacing between cards

        float maxFanAngle = Mathf.Min(4f * (currCards.Count - 1), 12f);             // control fan spread
        int hoveredIndex = hoveredCard != null ? currCards.IndexOf(hoveredCard) : -1;   // find hovered card

        for (int i = 0; i < currCards.Count; i++)
        {
            float p = (currCards.Count == 1) ? 0.5f : startP + i * step;            // keep cards inside safe spline range
            p = Mathf.Clamp(p, 0.2f, 0.8f);

            Vector3 splinePosition =
                splineContainer.transform.TransformPoint(spline.EvaluatePosition(p));

            float centerOffset = i - (currCards.Count - 1) / 2f;
            float angle = -centerOffset * maxFanAngle;
            Vector3 finalPos = splinePosition;
            Vector3 finalScale = Vector3.one * normalScale;
            float finalAngle = angle;
            int order = i * 2;
            if (hoveredIndex != -1)
            {
                if (i < hoveredIndex)
                {
                    finalPos += Vector3.left * pushDistance;            // keep cards inside safe spline range
                }
                else if (i > hoveredIndex)
                {
                    finalPos += Vector3.right * pushDistance;           // push right cards away
                }
                else
                {
                    finalPos += Vector3.up * hoverLift;             //lift hovered card up
                    finalScale = Vector3.one * hoverScale;          // enlarge hovered card
                    finalAngle = 0f;                // make hovered card straight
                    order = 100;                    // draw hovered card on top
                }
            }
            finalPos.z = 0f;            // keep card on 2D plane

            currCards[i].transform.DOKill();                    // stop old tween before starting new one
            currCards[i].transform.DOMove(finalPos, 0.25f);             // move card into place
            currCards[i].transform.DORotate(new Vector3(0f, 0f, finalAngle), 0.25f);        // rotate card
            currCards[i].transform.DOScale(finalScale, 0.25f);  // rotate card

            CardDisplay display = currCards[i].GetComponent<CardDisplay>();
            if (display != null)
            {
                display.SetSortingOrder(order);         // update render orders
            }
        }
    }
}