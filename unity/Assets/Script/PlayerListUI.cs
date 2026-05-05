using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListUI : MonoBehaviour
{
    public Transform playerListParent;
    public GameObject playerNamePrefab;

    private readonly List<GameObject> playerItems = new();


    void Start()                                    //TEST
    {
        SetPlayers(new List<string> { "Hao", "Alex", "Jenny", "Tom" });
    }

    public void SetPlayers(List<string> playerNames)
    {
        foreach (GameObject item in playerItems)
        {
            Destroy(item);
        }

        playerItems.Clear();

        foreach (string name in playerNames)
        {
            GameObject item = Instantiate(playerNamePrefab, playerListParent);
            item.GetComponent<TMP_Text>().text = name;
            playerItems.Add(item);
        }
    }
}