using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    // 0 = play mode, 1 = edit mode
    public bool editMode = false;
    public bool isHand = true;

    private float zoom;
    private float minZoom = 2f;
    private float maxZoom = 8f;
    private float zoomVelocity = 0f;
    private float zoomTime = 0.25f;

    public midCardDrop selectedPile = null;

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject editorPanel;
    [SerializeField] private Button editButton;

    void Start()
    {
        minZoom = cam.orthographicSize;
        zoom = minZoom;
        editorPanel.SetActive(editMode);
        hand.SetActive(isHand);
    }

    void Update()
    {
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        Debug.Log(zoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref zoomVelocity, zoomTime);
    }

    public void ChangeMode()
    {
        editMode = !editMode;
        if (!editMode)
        {
            zoom = minZoom;
            editButton.GetComponentInChildren<TextMeshProUGUI>().text = "Edit Board";
        } else
        {
            zoom = maxZoom;
            editButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
        }
        editorPanel.SetActive(editMode);
        
    }

    public void ToggleHand()
    {
        if (!editMode)
        {
            return;
        }
        isHand = !isHand;

        //handManager[] allHands = Object.FindObjectsByType<handManager>(FindObjectsSortMode.None);
        hand.SetActive(isHand);
    }



    public void FlipCards()
    {
        if (selectedPile != null && editMode)
        {
            selectedPile.isFaceUp = !selectedPile.isFaceUp;
        }
    }


    public void RemovePile()
    {
        Destroy(selectedPile.gameObject);
        selectedPile = null;
    }
}
