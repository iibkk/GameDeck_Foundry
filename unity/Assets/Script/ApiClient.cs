using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    public string baseUrl = "http://localhost:3000";

    public void TestConnection()
    {
        StartCoroutine(TestConnectionRequest());
    }
    void Start()
    {
        TestConnection();
    }

    IEnumerator TestConnectionRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Backend connection failed: " + request.error);
        }
        else
        {
            Debug.Log("Backend connected: " + request.downloadHandler.text);
        }
    }
}