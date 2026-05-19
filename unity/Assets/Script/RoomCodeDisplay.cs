using TMPro;
using UnityEngine;

public class RoomCodeDisplay : MonoBehaviour
{
    public TMP_Text roomCodeText;

    void Start()
    {
        string roomCode = PlayerPrefs.GetString("roomCode", "TESTROOM");
        roomCodeText.text = "Room Code: " + roomCode;
    }

    void Update()
    {
        if (roomCodeText == null)
        {
            Debug.LogError("RoomCodeText is not assigned");
            return;
        }

        string roomCode = PlayerPrefs.GetString("roomCode", "TESTROOM");
        roomCodeText.text = "Room Code: " + roomCode;
    }
}
