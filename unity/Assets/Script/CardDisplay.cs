using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public TMP_Text cardNameText;
    public TMP_Text cardText;

    public void SetCardInfo(string name, string text)
    {
        if (cardNameText != null)
            cardNameText.text = name;

        if (cardText != null)
            cardText.text = text;
    }

    // Keep this because handManager still calls SetCardValue(randomValue)
    public void SetCardValue(int value)
    {
        SetCardInfo("Card " + value, "Value: " + value);
    }

    public void SetSortingOrder(int order)
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer r in renderers)
        {
            r.sortingOrder = order;
        }

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.sortingOrder = order + 1;
        }
    }
}