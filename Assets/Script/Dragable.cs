using UnityEngine;
using UnityEngine.InputSystem;

public class Dragable : MonoBehaviour
{
    private Vector3 startDrag;
    public IDropArea colItem = null;


    public void StartDragging()
    {
        startDrag = transform.position;
    }


    public void Move(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, 0f);
    }


    public void StopDragging(IDropArea dropAreaFound)
    {
        colItem = dropAreaFound;

        if (dropAreaFound == null)
        {
            colItem = null;
            transform.position = startDrag;
        }
    }

    /*
    public void DragWithMouse()
    {
        if (Camera.main == null) return;
        if (Mouse.current == null) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, cardLayer);

            if (hit != null && hit.gameObject == gameObject)
            {
                isDragging = true;
                startDrag = transform.position;
                Debug.Log("Start dragging object");
            }
        }

        if (isDragging)
        {
            transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0f);
        }
        
        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;

            colli.enabled = false;
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(mouseWorldPos);
            IDropArea dropAreaFound = null;
            colli.enabled = true;

            foreach (Collider2D hit in hitColliders)
            { 
                if (hit.TryGetComponent(out IDropArea dropArea))
                {
                    dropAreaFound = dropArea;
                    break;
                }
            }
            
            if (dropAreaFound != null)
            {
                colItem = dropAreaFound;
            }
            else
            {
                colItem = null;
                transform.position = startDrag;
            }
        }
        
    }
    */
}
