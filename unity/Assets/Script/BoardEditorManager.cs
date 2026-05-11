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


    void Start()
    {
        boardEditorPanel.SetActive(false);

        string role = PlayerPrefs.GetString("role", "student");
        bool isTeacher = role == "teacher";

        boardEditButton.SetActive(isTeacher);
        boardEditorPanel.SetActive(false);
    }

    public void ToggleEditMode()
    {
        editMode = !editMode;
        boardEditorPanel.SetActive(editMode);
    }

    public void AddPile()
    {
        pileManager newPile = Instantiate(pilePrefab, spawnPoint.position, Quaternion.identity);

        BoardObjectDraggable drag = newPile.GetComponent<BoardObjectDraggable>();
        if (drag != null) drag.boardEditor = this;

        SelectablePile select = newPile.GetComponent<SelectablePile>();
        if (select != null) select.boardEditor = this;
    }

    public void SelectPile(midCardDrop pile)
    {
        selectedPile = pile;
        Debug.Log("Selected pile: " + pile.name);
    }

    public void RemoveSelectedPile()
    {
        if (selectedPile == null) return;

        Destroy(selectedPile.gameObject);
        selectedPile = null;
    }

    public void ToggleHand()
    {
        handBar.SetActive(!handBar.activeSelf);
    }
    public void ShuffleSelectedPile()
    {
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
        cardsFaceUp = !cardsFaceUp;

        Composition.Card[] allCards = FindObjectsByType<Composition.Card>(FindObjectsSortMode.None);

        foreach (Composition.Card card in allCards)
        {
            card.SetFaceUp(cardsFaceUp);
        }
    }

}