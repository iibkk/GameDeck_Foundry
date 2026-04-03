using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class dragManager : MonoBehaviour
{
    [SerializeField] private LayerMask cardLayer;
    [SerializeField] private LayerMask pileLayer;

    private Dragable currDragObj = null;
    private Collider2D currCol = null;


    void Update()
    {
        if (Camera.main == null) return;
        if (Mouse.current == null) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Handle inputs
        HandleMouseDown(mouseWorldPos);
        HandleDragging(mouseWorldPos);
        HandleMouseUp(mouseWorldPos);
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
}
