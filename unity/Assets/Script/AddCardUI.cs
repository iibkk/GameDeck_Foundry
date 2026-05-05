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

    public string baseUrl = "http://localhost:3000";

    public void AddCard()
    {
        StartCoroutine(AddCardRequest());
    }

    IEnumerator AddCardRequest()
    {
        string token = PlayerPrefs.GetString("teacherToken");

        CardData card = new CardData
        {
            deck_id = int.Parse(deckIdInput.text),
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