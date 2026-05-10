using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DeckCardItem : MonoBehaviour
{
    public TMP_Text cardNameText;
    public TMP_Text cardText;
    public Button deleteButton;

    private int cardId;
    private int deckId;
    private DeckViewerUI deckViewer;

    public void Setup(DeckViewerUI.CardData card, DeckViewerUI viewer)
    {
        cardId = card.id;
        deckId = card.deck_id;
        deckViewer = viewer;

        cardNameText.text = card.card_name;
        cardText.text = card.card_text;

        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(DeleteCard);
    }

    void DeleteCard()
    {
        StartCoroutine(DeleteRequest());
    }

    IEnumerator DeleteRequest()
    {
        UnityWebRequest request = UnityWebRequest.Delete(deckViewer.baseUrl + "/api/cards/" + cardId);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Delete failed: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Card deleted");
            deckViewer.LoadCardsFromDeck(deckId);
        }
    }
}