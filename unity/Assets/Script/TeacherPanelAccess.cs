using UnityEngine;

public class TeacherPanelAccess : MonoBehaviour
{
    
    public GameObject controlPanel;
    public GameObject openTeacherPanelButton;

    void Start()                                                
    {
        PlayerPrefs.SetString("role", "teacher");                       //TEST
        controlPanel.SetActive(false);
        openTeacherPanelButton.SetActive(false);

        string role = PlayerPrefs.GetString("role");

        if (role == "teacher")
        {
            openTeacherPanelButton.SetActive(true);
        }
    }
}