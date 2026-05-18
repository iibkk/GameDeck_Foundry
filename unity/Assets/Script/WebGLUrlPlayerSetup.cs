using UnityEngine;

public class WebGLUrlPlayerSetup : MonoBehaviour
{
    void Awake()
    {
        string url = Application.absoluteURL;

        if (url.Contains("role=teacher"))
            PlayerPrefs.SetString("role", "teacher");
        else if (url.Contains("role=student"))
            PlayerPrefs.SetString("role", "student");

        if (url.Contains("name=Sally"))
            PlayerPrefs.SetString("playerName", "Sally");
        else if (url.Contains("name=Nick"))
            PlayerPrefs.SetString("playerName", "Nick");

        PlayerPrefs.SetInt("sessionId", 1);
        PlayerPrefs.Save();

        Debug.Log("URL: " + url);
        Debug.Log("Role set to: " + PlayerPrefs.GetString("role"));
    }
}