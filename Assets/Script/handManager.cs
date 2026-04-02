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
        UpdateCardPosition(g);
    }
    

    public override void UpdateCardPosition(Card card)
    {
        currCards.RemoveAll(cards => cards == null);
        if (currCards.Count == 0) return;

        float cardSpacing = 1f / maxHandSize;
        float firstCardPosition = 0.5f - (currCards.Count - 1) * cardSpacing / 2f;
        Spline spline = splineContainer.Spline;


        float maxFanAngle = 12f;

        for (int i = 0; i < currCards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;

            Vector3 splinePosition = splineContainer.transform.TransformPoint(spline.EvaluatePosition(p));

            float centerOffset = i - (currCards.Count - 1) / 2f;
            float angle = -centerOffset * maxFanAngle;

            currCards[i].transform.DOKill();
            currCards[i].transform.DOMove(splinePosition, 0.25f);
            currCards[i].transform.DORotate(new Vector3(0f, 0f, angle), 0.25f);
            CardDisplay display = currCards[i].GetComponent<CardDisplay>();
            if (display != null)
            {
                display.SetSortingOrder(i * 2);
            }
        }
    }
}
