using UnityEngine;

public class TeacherPanelAccess : MonoBehaviour
{
    public GameObject teacherAddCardPanel;

    public GameObject deckIdInput;

    public GameObject cardNameInput;
    public GameObject cardTextInput;
    public GameObject descriptionInput;
    public GameObject frontImageUrlInput;
    public GameObject backImageUrlInput;
    public GameObject addCardButton;
    public GameObject deckViewerPanel;
    public GameObject boardEditorPanel;
    public GameObject boardEditButton;

    void Start()
    {
        string role = PlayerPrefs.GetString("role", "student");




        teacherAddCardPanel.SetActive(true);
        deckIdInput.SetActive(true);

        bool isTeacher = role == "teacher";

        cardNameInput.SetActive(isTeacher);
        cardTextInput.SetActive(isTeacher);
        descriptionInput.SetActive(isTeacher);
        frontImageUrlInput.SetActive(isTeacher);
        backImageUrlInput.SetActive(isTeacher);
        addCardButton.SetActive(isTeacher);
        deckViewerPanel.SetActive(true);
        boardEditButton.SetActive(true);
        boardEditorPanel.SetActive(false);
        Debug.Log("Current role is: " + role);


    }
}