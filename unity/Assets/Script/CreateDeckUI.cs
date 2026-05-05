using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
public class CreateDeckUI : MonoBehaviour
{
    public TMP_InputField deckNameInput;
    public TMP_InputField deckDescriptionInput;

    public string baseUrl = "http://localhost:3000";

    public void CreateDeck()
    {
        StartCoroutine(CreateDeckRequest());
    }

    IEnumerator CreateDeckRequest()
    {
        DeckData deck = new DeckData
        {
            name = deckNameInput.text,
            description = deckDescriptionInput.text
        };

        string json = JsonUtility.ToJson(deck);

        UnityWebRequest request = new UnityWebRequest(baseUrl + "/api/decks", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError("Create deck failed: " + request.downloadHandler.text);
        else
            Debug.Log("Deck created: " + request.downloadHandler.text);
    }

    [System.Serializable]
    public class DeckData
    {
        public string name;
        public string description;
    }
}