using UnityEngine;
using System.Collections.Generic;
using Composition;
public class BoardEditorManager : MonoBehaviour
{
    public bool editMode = false;

    public GameObject boardEditorPanel;
    public GameObject handBar;
    public pileManager pilePrefab;
    public Transform spawnPoint;
    public Transform deckReturnPoint;

    private midCardDrop selectedPile;
    public GameObject boardEditButton;
    public MultiplayerWebSocket multiplayer;
    private int pileCounter = 0;



    void Start()
    {
        boardEditorPanel.SetActive(false);

        string role = PlayerPrefs.GetString("role", "student");
        bool isTeacher = role == "teacher";

        boardEditButton.SetActive(isTeacher);
        boardEditorPanel.SetActive(false);
    }
    bool IsTeacher()
    {
        return PlayerPrefs.GetString("role", "student") == "teacher";
    }

    public void ToggleEditMode()
    {
        editMode = !editMode;
        boardEditorPanel.SetActive(editMode);
    }

    public void AddPile()
    {
        if (!IsTeacher()) return;

        pileManager newPile =
            Instantiate(pilePrefab, spawnPoint.position, Quaternion.identity);

        newPile.gameObject.name = "Pile_" + pileCounter;
        pileCounter++;
        midCardDrop drop = newPile.GetComponent<midCardDrop>();
        if (drop != null)
        {
            drop.multiplayer = multiplayer;
            drop.announcementUI = multiplayer.announcementUI;
        }

        BoardObjectDraggable drag =
            newPile.GetComponent<BoardObjectDraggable>();

        if (drag != null)
            drag.boardEditor = this;

        SelectablePile select =
            newPile.GetComponent<SelectablePile>();

        if (select != null)
            select.boardEditor = this;

        if (multiplayer != null)
        {
            multiplayer.SendCreatePile(
                newPile.gameObject.name,
                newPile.transform.position
            );
        }
        else
        {
            Debug.LogError("Multiplayer is not assigned in BoardEditorManager");
        }
    }

    public void SelectPile(midCardDrop pile)
    {
        selectedPile = pile;
        Debug.Log("Selected pile: " + pile.name);
    }

    public void RemoveSelectedPile()
    {
        if (!IsTeacher()) return;
        if (selectedPile == null) return;

        string pileId = selectedPile.gameObject.name;

        if (multiplayer != null)
        {
            multiplayer.SendRemovePile(pileId);
        }

        Destroy(selectedPile.gameObject);
        selectedPile = null;
    }
    public void RemoveRemotePile(string pileId)
    {
        GameObject pile = GameObject.Find(pileId);

        if (pile != null)
        {
            Destroy(pile);
        }
    }

    public void ToggleHand()
    {
        if (!IsTeacher()) return;
        handBar.SetActive(!handBar.activeSelf);
    }
    public void CreateRemotePile(string pileId, Vector3 position)
    {
        GameObject existing = GameObject.Find(pileId);

        if (existing != null)
            return;

        pileManager newPile =
            Instantiate(pilePrefab, position, Quaternion.identity);

        newPile.gameObject.name = pileId;

        midCardDrop drop = newPile.GetComponent<midCardDrop>();
        if (drop != null)
        {
            drop.multiplayer = multiplayer;
            drop.announcementUI = multiplayer.announcementUI;
        }

        BoardObjectDraggable drag =
            newPile.GetComponent<BoardObjectDraggable>();

        if (drag != null)
            drag.boardEditor = this;

        SelectablePile select =
            newPile.GetComponent<SelectablePile>();

        if (select != null)
            select.boardEditor = this;
    }
    public void ShuffleSelectedPile()
    {
        if (!IsTeacher()) return;
        if (selectedPile == null)
        {
            Debug.LogError("No pile selected");
            return;
        }

        Debug.Log("Selected pile has cards: " + selectedPile.currCards.Count);

        List<Card> cardsToShuffle = new List<Card>(selectedPile.currCards);

        foreach (Card card in cardsToShuffle)
        {
            card.transform.SetParent(deckReturnPoint);
            card.transform.position = deckReturnPoint.position;
            card.transform.rotation = Quaternion.identity;
            card.currPile = null;
        }

        selectedPile.currCards.Clear();

        Debug.Log("Selected pile shuffled");
    }
    private bool cardsFaceUp = true;
    public void FlipCards()
    {
        if (!IsTeacher()) return;
        cardsFaceUp = !cardsFaceUp;

        Composition.Card[] allCards = FindObjectsByType<Composition.Card>(FindObjectsSortMode.None);

        foreach (Composition.Card card in allCards)
        {
            card.SetFaceUp(cardsFaceUp);
        }
    }
    public void ApplyMovePile(string pileId, Vector3 position)
    {
        GameObject pile = GameObject.Find(pileId);

        if (pile != null)
        {
            pile.transform.position = position;
        }
    }
}