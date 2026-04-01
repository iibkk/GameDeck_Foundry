using UnityEngine;
using UnityEngine.InputSystem;

public class Card : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 startDrag;
    private Collider2D colli;

    void Start()
    {
        colli = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Camera.main == null) return;
        if (Mouse.current == null) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

            if (hit != null && hit.gameObject == gameObject)
            {
                isDragging = true;
                startDrag = transform.position;
                Debug.Log("Start dragging card");
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
            Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);
            colli.enabled = true;

            if (hitCollider != null && hitCollider.TryGetComponent(out IDropArea cardDropArea))
            {
                cardDropArea.dropArea(this);
            }
            else
            {
                transform.position = startDrag;
            }
        }
    }
}