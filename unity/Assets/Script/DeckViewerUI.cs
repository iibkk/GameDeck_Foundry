using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DeckViewerUI : MonoBehaviour
{
    public string baseUrl = "http://localhost:3000";
    public Transform deckViewerParent;
    public GameObject deckCardPrefab;
    public TMP_InputField deckIdInput;

    public void LoadDeckButton()
    {
        if (!int.TryParse(deckIdInput.text, out int deckId))
        {
            Debug.LogError("Deck ID must be a number.");
            return;
        }

        LoadCardsFromDeck(deckId);
    }

    public void LoadCardsFromDeck(int deckId)
    {
        StartCoroutine(LoadCardsRequest(deckId));
    }

    IEnumerator LoadCardsRequest(int deckId)
    {
        UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/api/cards?deck_id=" + deckId);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Load cards failed: " + request.downloadHandler.text);
            yield break;
        }

        CardListWrapper wrapper = JsonUtility.FromJson<CardListWrapper>(
            "{\"cards\":" + request.downloadHandler.text + "}"
        );

        ShowDeck(wrapper.cards);
    }

    public void ShowDeck(List<CardData> cards)
    {
        foreach (Transform child in deckViewerParent)
        {
            Destroy(child.gameObject);
        }

        foreach (CardData card in cards)
        {
            GameObject obj = Instantiate(deckCardPrefab, deckViewerParent);

            DeckCardItem item = obj.GetComponent<DeckCardItem>();
            item.Setup(card, this);
        }
    }

    [System.Serializable]
    public class CardListWrapper
    {
        public List<CardData> cards;
    }

    [System.Serializable]
    public class CardData
    {
        public int id;
        public int deck_id;
        public string card_name;
        public string card_text;
        public string description;
        public string front_image_url;
        public string back_image_url;
    }
}