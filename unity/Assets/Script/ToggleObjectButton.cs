using UnityEngine;
using TMPro;

public class ToggleObjectButton : MonoBehaviour
{
    public GameObject targetObject;
    public TMP_Text buttonText;

    public void Toggle()
    {
        bool show = !targetObject.activeSelf;
        targetObject.SetActive(show);

        if (buttonText != null)
        {
            buttonText.text = show ? "Hide Deck" : "Show Deck";
        }
    }
}