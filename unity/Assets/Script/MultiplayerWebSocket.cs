using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using Composition;

public class MultiplayerWebSocket : MonoBehaviour
{
    public AnnouncementUI announcementUI;

    public Card cardPrefab;
    public Transform playZonePoint;

    private WebSocket websocket;
    private int sessionId;
    private string playerName;
    private string clientId;
    private Dictionary<int, Card> playedCards = new Dictionary<int, Card>();



    async void Start()
    {
        clientId = System.Guid.NewGuid().ToString();
        sessionId = PlayerPrefs.GetInt("sessionId", 1);
        playerName = PlayerPrefs.GetString("playerName", "User");

        websocket = new WebSocket("ws://127.0.0.1:3000");

        websocket.OnOpen += () =>
        {
            Debug.Log("WebSocket connected");

            SendJoinRoom();
        };

        websocket.OnMessage += (bytes) =>
        {
            string json = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("WebSocket message: " + json);

            WebSocketMessage msg = JsonUtility.FromJson<WebSocketMessage>(json);

            if (msg.type == "announcement")
            {
                announcementUI.Show(msg.message);
            }

            if (msg.type == "play_card")
            {
                if (msg.client_id == clientId)
                {
                    return;
                }

                announcementUI.Show(msg.player_name + " has played " + msg.card_name);
                SpawnCardInPlayZone(msg);
            }

            if (msg.type == "withdraw_card")
            {
                if (msg.client_id == clientId)
                {
                    return;
                }

                announcementUI.Show(msg.player_name + " has withdrawn " + msg.card_name);
                RemoveCardFromPlayZone(msg.card_id);
            }
        };

        websocket.OnError += (error) =>
        {
            Debug.LogError("WebSocket error: " + error);
        };

        websocket.OnClose += (code) =>
        {
            Debug.Log("WebSocket closed");
        };

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif
    }

    async void SendJoinRoom()
    {
        WebSocketMessage msg = new WebSocketMessage
        {
            type = "join_room",
            session_id = sessionId,
            player_name = playerName
        };

        await websocket.SendText(JsonUtility.ToJson(msg));
    }

    public async void SendPlayCard(string cardName, string cardText, int cardId)
    {
        if (websocket == null || websocket.State != WebSocketState.Open)
        {
            Debug.LogError("Cannot send play_card: WebSocket not connected");
            return;
        }

        WebSocketMessage msg = new WebSocketMessage
        {
            type = "play_card",
            session_id = sessionId,
            player_name = playerName,
            card_name = cardName,
            card_text = cardText,
            client_id = clientId,
            card_id = cardId

        };

        await websocket.SendText(JsonUtility.ToJson(msg));
    }

    public async void SendWithdrawCard(string cardName, int cardId)
    {
        if (websocket == null || websocket.State != WebSocketState.Open)
        {
            Debug.LogError("Cannot send withdraw_card: WebSocket not connected");
            return;
        }

        WebSocketMessage msg = new WebSocketMessage
        {
            type = "withdraw_card",
            session_id = sessionId,
            player_name = playerName,
            card_name = cardName,
            client_id = clientId,
            card_id = cardId
        };

        await websocket.SendText(JsonUtility.ToJson(msg));
    }
    void SpawnCardInPlayZone(WebSocketMessage msg)
    {
        if (playedCards.ContainsKey(msg.card_id))
        {
            return;
        }

        Card newCard = Instantiate(cardPrefab, playZonePoint.position, Quaternion.identity);

        CardDisplay display = newCard.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.SetCardInfo(msg.card_name, msg.card_text);
        }

        newCard.transform.localScale = Vector3.one;

        playedCards.Add(msg.card_id, newCard);
    }
    void RemoveCardFromPlayZone(int cardId)
    {
        if (!playedCards.ContainsKey(cardId))
        {
            return;
        }

        Card card = playedCards[cardId];

        if (card != null)
        {
            Destroy(card.gameObject);
        }

        playedCards.Remove(cardId);
    }

    async void OnApplicationQuit()
    {
        if (websocket != null)
        {
            await websocket.Close();
        }
    }

    [System.Serializable]
    public class WebSocketMessage
    {
        public string type;
        public int session_id;
        public string player_name;
        public string message;
        public int card_id;
        public string client_id;
        public string card_name;
        public string card_text;
    }
}