using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro numberText;

    public void SetCardValue(int value)
    {
        if (numberText != null)
        {
            numberText.text = value.ToString();
        }
    }
    public void SetSortingOrder(int order)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = order;
        }

        MeshRenderer mr = numberText.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.sortingOrder = order + 1;
        }
    }

    private void LateUpdate()
    {
        if (numberText != null)
        {
            numberText.transform.rotation = Quaternion.identity;
        }
    }
}