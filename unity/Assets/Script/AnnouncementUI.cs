using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnnouncementUI : MonoBehaviour
{
    public TMP_Text announcementText;

    private List<string> history = new List<string>();
    private const int maxHistory = 5;

    public void Show(string message)
    {
        history.Add(message);

        if (history.Count > maxHistory)
        {
            history.RemoveAt(0);
        }

        announcementText.text = string.Join("\n", history);
        Debug.Log(message);
    }
}