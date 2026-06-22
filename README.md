


1. Configure Login Database Credentials (.env) Login and user data for this project are stored in a local PostgreSQL database. You must configure the database password in the backend folder.

Target Path: [Your unzipped directory]\GameDeck_Foundry-backend_api\backend\.env
Also "D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\backend\.env"

Instructions:

Locate or create a file named .env in this directory.

Open it with a text editor and ensure it contains the correct database connection information (please modify it according to the actual PostgreSQL password on your test machine):

DB_HOST=localhost
DB_PORT=5432
DB_NAME=gamedeck_db
DB_USER=postgres
DB_PASSWORD=”your password“
JWT_SECRET=gamedeck_super_secret_key_2026
PORT=3001


2. Install Project Dependencies

This project includes multiple Node environments. Before running it for the first time, please be sure to install the dependencies in the corresponding directories (skip this step if they are already installed):

Run `npm install` in the `GameDeck_Foundry-backend_api\backend` directory

Run `npm install` in the `GameDeck_Foundry-Unity_backend_linked\backend` directory


3. Phase Two: Core File Placement (Very Important!)

To ensure the front-end webpage loads the Unity game engine correctly, the Unity-exported Build folder must be placed in the correct path.

Instructions: Please move/copy the Build folder containing .wasm and .data files to the following front-end WebGL directory:

Plaintext
[Your unzipped directory]\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\unity\WebGl\Build <- this copy put in to:
[Your unzipped directory]\LoginAndJoinin\[....here....]
(Note: If this step is not performed, the game screen will not be displayed after clicking Host/Join Game.)

4. This project requires Four separate terminal windows (PowerShell / CMD) to run three microservices. Please keep these three windows running in the background.

->>>Terminal 1: Start the Login and Data Backend This is responsible for handling user registration, login authentication, and token distribution.

Open PowerShell and navigate to the following path:

PowerShell cd "D:\ICT_project1_private\GameDeck_Foundry-backend_api\backend" Start the service:

PowerShell npm start

->>>Terminal 2: Start the Unity Game Backend. This handles room creation, WebSocket connection logic, and in-game data synchronization.

Open a new PowerShell window and navigate to the following path:

PowerShell cd "D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\backend"
Start the service:

PowerShell npm start

->>>Terminal 3: Start the Frontend WebGL Page
This page hosts the user UI and runs the Unity game.

Open a new PowerShell window and navigate to the following path:

PowerShell cd "D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\unity\WebGl"
Remove script execution restrictions (only needs to be executed once):

PowerShell Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
Start the static server:

PowerShell npx http-server -p 8080//8081 if 8080 in domain
Start testing: After the service starts, open your browser and access http://127.0.0.1:8080 or http://localhost:8080 (or access your specific HTML file, such as Game.html) to begin testing!

->>>Last terminal !!!
Go to [Your unzipped directory]\LoginAndJoinin  >use->
 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass'
 'npx http-server -p 8082 -o /Game.html'







First important: move the Build file from unity_backend_linked to this paths
unity_backend_linked / unity / WebGl / Build

PS D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\unity\WebGl>  Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
PS D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\unity\WebGl> npx http-server -p 8080
This is for unity backend

PS D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\backend> npm start
This is for unity game backend


PS D:\ICT_project1_private\GameDeck_Foundry-backend_api\backend> npm start
this is for login data backend
