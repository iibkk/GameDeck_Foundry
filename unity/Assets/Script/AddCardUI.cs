using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AddCardUI : MonoBehaviour
{
    public TMP_InputField deckIdInput;
    public TMP_InputField cardNameInput;
    public TMP_InputField cardTextInput;
    public TMP_InputField descriptionInput;
    public TMP_InputField frontImageUrlInput;
    public TMP_InputField backImageUrlInput;
    public DeckViewerUI deckViewer;
    public string baseUrl = "http://localhost:3000";

    public void AddCard()
    {
        StartCoroutine(AddCardRequest());
    }

    IEnumerator AddCardRequest()
    {
        if (!int.TryParse(deckIdInput.text, out int deckId))
        {
            Debug.LogError("Deck ID must be a number.");
            yield break;
        }

        string token = PlayerPrefs.GetString("teacherToken");

        CardData card = new CardData
        {
            deck_id = deckId,
            card_name = cardNameInput.text,
            card_text = cardTextInput.text,
            description = descriptionInput.text,
            front_image_url = frontImageUrlInput.text,
            back_image_url = backImageUrlInput.text
        };

        string json = JsonUtility.ToJson(card);

        UnityWebRequest request = new UnityWebRequest(baseUrl + "/api/cards", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Add card failed: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Card added: " + request.downloadHandler.text);

            if (deckViewer != null)
            {
                deckViewer.LoadCardsFromDeck(deckId);
            }
        }
        if (cardNameInput.text.Length > 30)
        {
            Debug.LogError("Card name too long. Max 30 characters.");
            yield break;
        }

        if (cardTextInput.text.Length > 80)
        {
            Debug.LogError("Card text too long. Max 80 characters.");
            yield break;
        }

        if (descriptionInput.text.Length > 120)
        {
            Debug.LogError("Description too long. Max 120 characters.");
            yield break;
        }

        if (string.IsNullOrWhiteSpace(frontImageUrlInput.text))
        {
            Debug.LogError("Front image is required.");
            yield break;
        }

        if (string.IsNullOrWhiteSpace(backImageUrlInput.text))
        {
            Debug.LogError("Back image is required.");
            yield break;
        }
    }
    [System.Serializable]
    public class CardData
    {
        public int deck_id;
        public string card_name;
        public string card_text;
        public string description;
        public string front_image_url;
        public string back_image_url;
    }
}