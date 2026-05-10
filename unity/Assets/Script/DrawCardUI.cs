using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DrawCardUI : MonoBehaviour
{
    public TMP_InputField deckIdInput;
    public handManager handManager;
    public string baseUrl = "http://localhost:3000";

    public void DrawCard()
    {
        StartCoroutine(DrawCardRequest());
    }

    IEnumerator DrawCardRequest()
    {
        if (!int.TryParse(deckIdInput.text, out int deckId))
        {
            Debug.LogError("Deck ID must be a number.");
            yield break;
        }

        string playerName = PlayerPrefs.GetString("playerName");
        int sessionId = PlayerPrefs.GetInt("sessionId");

        DrawRequest drawRequest = new DrawRequest
        {
            session_id = sessionId,
            deck_id = deckId,
            player_name = playerName
        };

        string json = JsonUtility.ToJson(drawRequest);

        UnityWebRequest request = new UnityWebRequest(baseUrl + "/api/game/draw-card", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Draw failed: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Card drawn: " + request.downloadHandler.text);

            DrawResponse response = JsonUtility.FromJson<DrawResponse>(request.downloadHandler.text);

            if (handManager != null && response.card != null)
            {
                handManager.CreateCardFromData(response.card);
            }
        }
    }

    [System.Serializable]
    public class DrawRequest
    {
        public int session_id;
        public int deck_id;
        public string player_name;
    }

    [System.Serializable]
    public class DrawResponse
    {
        public string message;
        public string player_name;
        public CardData card;
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