using UnityEngine;
using System;

public class WebGLUrlPlayerSetup : MonoBehaviour
{
    void Awake()
    {
        string url = Application.absoluteURL;

        string playerName = GetParam(url, "name");
        string role = GetParam(url, "role");
        string roomCode = GetParam(url, "roomCode");

        if (string.IsNullOrEmpty(playerName))
            playerName = "User";

        if (string.IsNullOrEmpty(role))
            role = "student";

        if (string.IsNullOrEmpty(roomCode))
            roomCode = "TESTROOM";

        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.SetString("role", role);
        PlayerPrefs.SetString("roomCode", roomCode);

        PlayerPrefs.Save();

        Debug.Log("URL: " + url);
        Debug.Log("PlayerName: " + playerName);
        Debug.Log("Role: " + role);
        Debug.Log("RoomCode: " + roomCode);
    }

    string GetParam(string url, string key)
    {
        Uri uri = new Uri(url);

        string query = uri.Query.TrimStart('?');

        string[] pairs = query.Split('&');

        foreach (string pair in pairs)
        {
            string[] parts = pair.Split('=');

            if (parts.Length == 2 && parts[0] == key)
            {
                return Uri.UnescapeDataString(parts[1]);
            }
        }

        return "";
    }
}