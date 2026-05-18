using UnityEngine;
using UnityEngine.InputSystem;

public class BoardObjectDraggable : MonoBehaviour
{
    public BoardEditorManager boardEditor;

    private bool dragging = false;
    private Vector3 offset;
    private Camera mainCam;
    private Collider2D myCollider;

    void Start()
    {
        mainCam = Camera.main;
        myCollider = GetComponent<Collider2D>();

        if (mainCam == null)
            Debug.LogError("No Main Camera found.");

        if (myCollider == null)
            Debug.LogError(gameObject.name + " has no Collider2D.");

        if (boardEditor == null)
            Debug.LogError(gameObject.name + " missing BoardEditorManager.");
    }

    void Update()
    {
        if (mainCam == null || Mouse.current == null || myCollider == null) return;
        if (boardEditor == null || !boardEditor.editMode) return;

        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (myCollider.OverlapPoint(mouseWorld))
            {
                boardEditor.SelectPile(GetComponent<midCardDrop>());      // students can select but cannot drag

                string role = PlayerPrefs.GetString("role", "student");

                if (role != "teacher")
                {
                    Debug.Log("Student cannot drag piles");
                    return;
                }

                dragging = true;

                offset = transform.position - new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z);

                Debug.Log("Start dragging: " + gameObject.name);
            }
        }

        if (Mouse.current.leftButton.isPressed && dragging)
        {
            transform.position =
                new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z) + offset;

            if (boardEditor.multiplayer != null)
            {
                boardEditor.multiplayer.SendMovePile(
                    gameObject.name,
                    transform.position
                );
            }
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (dragging)
            {
                Debug.Log("Stop dragging: " + gameObject.name);

                if (boardEditor.multiplayer != null)
                {
                    boardEditor.multiplayer.SendMovePile(gameObject.name, transform.position);
                }
            }

            dragging = false;
        }
    }
}