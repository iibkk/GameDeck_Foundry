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
