using UnityEngine;
using UnityEngine.InputSystem;

public class SelectablePile : MonoBehaviour
{
    public BoardEditorManager boardEditor;
    private midCardDrop pile;
    private Collider2D myCollider;
    private Camera mainCam;

    void Start()
    {
        pile = GetComponent<midCardDrop>();
        myCollider = GetComponent<Collider2D>();
        mainCam = Camera.main;
    }

    void Update()
    {
        if (boardEditor == null || !boardEditor.editMode) return;
        if (mainCam == null || Mouse.current == null || myCollider == null) return;

        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (myCollider.OverlapPoint(mouseWorld))
            {
                boardEditor.SelectPile(pile);
                Debug.Log("Pile selected: " + gameObject.name);
            }
        }
    }
}