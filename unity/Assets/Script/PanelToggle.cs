using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel;
    public GameObject openButton;

    public void OpenPanel()
    {
        panel.SetActive(true);
        openButton.SetActive(false);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        openButton.SetActive(true);
    }
}