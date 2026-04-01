using UnityEngine;

public class cardDelete : MonoBehaviour, IDropArea
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private handManager handManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void dropArea(Card card)
    {
        handManager.RemoveCard(card.gameObject);
        Destroy(card.gameObject);
    }
}