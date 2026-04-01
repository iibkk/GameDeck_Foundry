using UnityEngine;

public class midCardDrop : MonoBehaviour, IDropArea
{
    [SerializeField] private handManager handManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void dropArea(Card card)
    {
        handManager.RemoveCard(card.gameObject);
        card.transform.SetParent(transform);
        card.transform.position = transform.position;
        card.transform.rotation = transform.rotation;
        Debug.Log("Card Drop here");
    }
}
