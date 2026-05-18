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
    public string pile_id;
    private Dictionary<int, Card> playedCards = new Dictionary<int, Card>();
    private static MultiplayerWebSocket instance;



    async void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

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
            if (msg.type == "move_pile")
            {
                if (msg.client_id == clientId) return;

                boardEditor.ApplyMovePile(msg.pile_id, new Vector3(msg.x, msg.y, 0));
            }
            if (msg.type == "remove_pile")
            {
                if (msg.client_id != clientId)
                {
                    boardEditor.RemoveRemotePile(msg.pile_id);
                }
            }
            if (msg.type == "create_pile")
            {
                if (msg.client_id == clientId) return;

                Debug.Log("Received create pile: " + msg.pile_id);

                boardEditor.CreateRemotePile(
                    msg.pile_id,
                    new Vector3(msg.x, msg.y, 0)
                );
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

    public async void SendPlayCard(string cardName, string cardText, int cardId, string pileId)
    {
        WebSocketMessage msg = new WebSocketMessage
        {
            type = "play_card",
            session_id = sessionId,
            player_name = playerName,
            card_name = cardName,
            card_text = cardText,
            client_id = clientId,
            card_id = cardId,
            pile_id = pileId
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
            return;

        GameObject pileObj = GameObject.Find(msg.pile_id);

        if (pileObj == null)
        {
            Debug.LogError("Cannot find pile: " + msg.pile_id);
            return;
        }

        midCardDrop targetPile = pileObj.GetComponent<midCardDrop>();

        if (targetPile == null)
        {
            Debug.LogError("Target object has no midCardDrop: " + msg.pile_id);
            return;
        }

        Card newCard = Instantiate(cardPrefab, pileObj.transform.position, Quaternion.identity);

        CardDisplay display = newCard.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.SetCardInfo(msg.card_name, msg.card_text);
        }

        newCard.currPile = targetPile;
        targetPile.currCards.Add(newCard);
        targetPile.UpdateCardPosition(newCard);

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

    public BoardEditorManager boardEditor;

    public async void SendMovePile(string pileId, Vector3 position)
    {
        WebSocketMessage msg = new WebSocketMessage
        {
            type = "move_pile",
            session_id = sessionId,
            client_id = clientId,
            pile_id = pileId,
            x = position.x,
            y = position.y
        };

        await websocket.SendText(JsonUtility.ToJson(msg));
    }
    public async void SendCreatePile(string pileId, Vector3 position)
    {
        WebSocketMessage msg = new WebSocketMessage
        {
            type = "create_pile",
            session_id = sessionId,
            client_id = clientId,
            pile_id = pileId,
            x = position.x,
            y = position.y
        };

        await websocket.SendText(JsonUtility.ToJson(msg));
    }
    public async void SendRemovePile(string pileId)
    {
        if (websocket == null)
            return;

        WebSocketMessage msg = new WebSocketMessage();

        msg.type = "remove_pile";
        msg.session_id = sessionId;
        msg.client_id = clientId;
        msg.pile_id = pileId;

        string json = JsonUtility.ToJson(msg);

        await websocket.SendText(json);

        Debug.Log("Sent remove_pile: " + json);
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
        public string pile_id;
        public float x;
        public float y;
    }
}