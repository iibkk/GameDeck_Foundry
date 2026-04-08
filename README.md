(Teacher Registration)
port address: `POST http://localhost:3000/api/auth/register`
frontend send (Body):
  ```json
  {
    "email": "teacher@adelaide.edu.au",
    "full_name": "Teacher Name",
    "password": "mySecurePassword"
  }

backend (success 200): {"message": "User registered successfully"}

backend (fail 400): {"message": "User already exists"}





Teacher Login：
POST http://localhost:3000/api/auth/login

frontend：
{
  "email": "teacher@adelaide.edu.au",
  "password": "mySecurePassword"
}

backend feedback (success 200): ```json
{
"token": "eyJhbGciOiJIUzI1...",
"user": { "user_id": 1, "role": "teacher" }
}
*(frontend had alreday take token save in localStorage of `gameDeckToken` )*





（Student Join)
POST http://localhost:3000/api/session/join


frontend send：
{
  "join_code": "X7R2",
  "player_name": "Student A"
}

The backend returned (success 200): JSON containing session and player objects.

(Note: The frontend has intercepted session_player_id and session_id and is preparing to send them to Unity.)







To Unity developers: When a student enters a room number on the webpage and is redirected to the game, the webpage frontend will automatically call your C# method.

You must create the receiving object: GameManager

You must write the method name: ReceivePlayerData(string data)

The data format I'm passing you is: "studentId, sessionId" (e.g., "5, 12")

Your task: Parse this string, obtain the ID, and then you can request the cards to be dealt from Node.js!

------------------------------------------
using UnityEngine;

// Have him attach this script to an object named GameManager
public class WebCommunication : MonoBehaviour
{
    // This method name must be exactly the same as the one written in the front-end JS.
    public void ReceivePlayerData(string dataFromWeb)
    {
        // The frontend sends "studentId, sessionId"
        string[] splitData = dataFromWeb.Split(',');
        
        int studentId = int.Parse(splitData[0]);
        int sessionId = int.Parse(splitData[1]);

        Debug.Log("Data received from the webpage was successfully received! Student ID: " + studentId + " | Session ID: " + sessionId);
        
        Once Unity obtains the ID, it can then request the student's card data from the Node.js backend.
    }
}
