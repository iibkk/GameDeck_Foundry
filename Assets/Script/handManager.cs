using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class handManager : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    private List<GameObject> handCards = new();
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DrawCard();
        }
    }
    private void DrawCard()
    {
        if (handCards.Count >= maxHandSize) return;

        GameObject g = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity, transform);

        int randomValue = Random.Range(1, 14);

        CardDisplay display = g.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.SetCardValue(randomValue);
        }

        handCards.Add(g);
        UpdateCardPosition();
    }
    public void RemoveCard(GameObject card)
    {
        if (handCards.Contains(card))
        {
            handCards.Remove(card);
            UpdateCardPosition();
        }
    }
    public void AddCard(GameObject card)
    {
        if (!handCards.Contains(card) && handCards.Count < maxHandSize)
        {
            handCards.Add(card);
            UpdateCardPosition();
        }
    }
    private void UpdateCardPosition()
    {
        handCards.RemoveAll(card => card == null);
        if (handCards.Count == 0) return;

        float cardSpacing = 1f / maxHandSize;
        float firstCardPosition = 0.5f - (handCards.Count - 1) * cardSpacing / 2f;
        Spline spline = splineContainer.Spline;


        float maxFanAngle = 12f;

        for (int i = 0; i < handCards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;

            Vector3 splinePosition = splineContainer.transform.TransformPoint(spline.EvaluatePosition(p));

            float centerOffset = i - (handCards.Count - 1) / 2f;
            float angle = -centerOffset * maxFanAngle;

            handCards[i].transform.DOKill();
            handCards[i].transform.DOMove(splinePosition, 0.25f);
            handCards[i].transform.DORotate(new Vector3(0f, 0f, angle), 0.25f);
            CardDisplay display = handCards[i].GetComponent<CardDisplay>();
            if (display != null)
            {
                display.SetSortingOrder(i * 2);
            }
        }
    }
}
