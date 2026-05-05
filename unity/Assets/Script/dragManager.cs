using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using Composition;

public class dragManager : MonoBehaviour
{
    [SerializeField] private LayerMask cardLayer;
    [SerializeField] private LayerMask pileLayer;

    private Dragable currDragObj = null;
    private Collider2D currCol = null;
    private Card currHoverCard = null;


    void Update()
    {
        if (Camera.main == null) return;
        if (Mouse.current == null) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Handle inputs
        HandleHover(mouseWorldPos);                 // check hover effect
        HandleMouseDown(mouseWorldPos);
        HandleDragging(mouseWorldPos);
        HandleMouseUp(mouseWorldPos);
    }

    void HandleHover(Vector2 mouseWorldPos)
    {
        if (currDragObj != null) return;            // do not hover while dragging

        Card newHoverCard = GetTopCardAtPoint(mouseWorldPos);           // get top visible card under mouse

        if (currHoverCard == newHoverCard) return;

        if (currHoverCard != null && currHoverCard.currPile is handManager oldHand)
        {
            oldHand.ClearHoveredCard(currHoverCard);        // remove old hover effect
        }

        currHoverCard = newHoverCard;

        if (currHoverCard != null && currHoverCard.currPile is handManager newHand)
        {
            newHand.SetHoveredCard(currHoverCard);       // apply hover effect to new card
        }
    }

    void HandleMouseDown(Vector2 mouseWorldPos)
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame || currDragObj != null) return;

        // Card drag
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, cardLayer);

        if (hit != null && hit.TryGetComponent(out Dragable obj))
        {
            currDragObj = obj;
            currCol = hit;

            currDragObj.StartDragging();
            Debug.Log("Start dragging object");
        }
    }


    void HandleDragging(Vector2 mouseWorldPos)
    {
        if (currDragObj == null) return;

        currDragObj.Move(mouseWorldPos);
    }


    void HandleMouseUp(Vector2 mouseWorldPos)
    {
        if (!Mouse.current.leftButton.wasReleasedThisFrame || currDragObj == null) return;

        currCol.enabled = false;
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mouseWorldPos, pileLayer);
        IDropArea dropAreaFound = null;
        currCol.enabled = true;

        foreach (Collider2D hit in hitColliders)
        {
            if (hit.TryGetComponent(out IDropArea dropArea))
            {
                dropAreaFound = dropArea;
                break;
            }
        }

        currDragObj.StopDragging(dropAreaFound);

        currDragObj = null;
        currCol = null;
    }
    Card GetTopCardAtPoint(Vector2 mouseWorldPos)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPos, cardLayer);
        if (hits == null || hits.Length == 0) return null;

        Card bestCard = null;
        int bestOrder = int.MinValue;

        foreach (Collider2D hit in hits)
        {
            Card card = hit.GetComponent<Card>();
            if (card == null) continue;

            SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
            int order = sr != null ? sr.sortingOrder : 0;

            if (bestCard == null || order > bestOrder)
            {
                bestCard = card;
                bestOrder = order;
            }
        }

        return bestCard;
    }
}
