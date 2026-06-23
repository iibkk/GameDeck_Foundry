Prerequisites & Environment Setup
To evaluate and run this project locally, the host machine must have the following software installed. Please ensure these are configured before proceeding to the deployment steps.

1. Node.js (v20.0 or higher recommended)

Purpose: Required to run the backend microservices, manage packages via npm, and host the local static server via npx.

Download: https://nodejs.org/

Verification: Open a terminal and run node -v to ensure it is installed correctly.

2. PostgreSQL (v16.0 or higher recommended)

Purpose: Required for the local database to store user authentication and game session data.

Download: https://www.postgresql.org/download/

Important: Please keep track of the superuser (postgres) password you set during the installation process, as you will need to input it into the .env files in Phase 1.

3. Modern Web Browser

Purpose: Required for optimal Unity WebGL rendering and WebSocket real-time communication.

Requirement: Google Chrome, Microsoft Edge, or Mozilla Firefox (Latest Versions). Safari is not officially recommended for this WebGL build.

4. Visual Studio Code & Live Server (Optional but Recommended)

Purpose: Highly recommended for reviewing the codebase and utilizing the "Live Server" extension for a seamless frontend launch.

Download: https://code.visualstudio.com/



--------------------------------------------------------------------------------------------------------------------------------------

Configure Login Database Credentials (.env) Login and user data for this project are stored in a local PostgreSQL database. You must configure the database password in the backend folder.
Target Path: [Your unzipped directory]\GameDeck_Foundry-backend_api\backend.env Also "D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\backend.env"

Instructions:

Locate or create a file named .env in this directory.

Open it with a text editor and ensure it contains the correct database connection information (please modify it according to the actual PostgreSQL password on your test machine):

DB_HOST=localhost DB_PORT=5432 DB_NAME=gamedeck_db DB_USER=postgres DB_PASSWORD=”your password“ JWT_SECRET=gamedeck_super_secret_key_2026 PORT=3001

Install Project Dependencies
This project includes multiple Node environments. Before running it for the first time, please be sure to install the dependencies in the corresponding directories (skip this step if they are already installed):

Run npm install in the GameDeck_Foundry-backend_api\backend directory

Run npm install in the GameDeck_Foundry-Unity_backend_linked\backend directory

Phase Two: Core File Placement (Very Important!)
To ensure the front-end webpage loads the Unity game engine correctly, the Unity-exported Build folder must be placed in the correct path.

Instructions: Please move/copy the Build folder containing .wasm and .data files to the following front-end WebGL directory:

Plaintext [Your unzipped directory]\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\unity\WebGl\Build <- this copy put in to: [Your unzipped directory]\LoginAndJoinin[....here....] (Note: If this step is not performed, the game screen will not be displayed after clicking Host/Join Game.)

This project requires Four separate terminal windows (PowerShell / CMD) to run three microservices. Please keep these three windows running in the background.
->>>Terminal 1: Start the Login and Data Backend This is responsible for handling user registration, login authentication, and token distribution.

Open PowerShell and navigate to the following path:

PowerShell cd "D:\ICT_project1_private\GameDeck_Foundry-backend_api\backend" Start the service:

PowerShell npm start

->>>Terminal 2: Start the Unity Game Backend. This handles room creation, WebSocket connection logic, and in-game data synchronization.

Open a new PowerShell window and navigate to the following path:

PowerShell cd "D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\backend" Start the service:

PowerShell npm start

->>>Terminal 3: Start the Frontend WebGL Page This page hosts the user UI and runs the Unity game.

Open a new PowerShell window and navigate to the following path:

PowerShell cd "D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\unity\WebGl" Remove script execution restrictions (only needs to be executed once):

PowerShell Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass Start the static server:

PowerShell npx http-server -p 8080//8081 if 8080 in domain Start testing: After the service starts, open your browser and access http://127.0.0.1:8080 or http://localhost:8080 (or access your specific HTML file, such as Game.html) to begin testing!

->>>Last terminal !!! Go to [Your unzipped directory]\LoginAndJoinin >use-> 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass' 'npx http-server -p 8082 -o /Game.html'









Prerequisites & Environment Setup
To evaluate and run this project locally, the host machine must have the following software installed. Please ensure these are configured before proceeding to the deployment steps.

Node.js (v20.0 or higher recommended)

Purpose: Required to run the backend microservices, manage packages via npm, and host the local static server via npx.

Download: https://nodejs.org/

Verification: Open a terminal and run node -v to ensure it is installed correctly.

PostgreSQL (v16.0 or higher recommended)

Purpose: Required for the local database to store user authentication and game session data.

Download: https://www.postgresql.org/download/

Important: Please keep track of the superuser (postgres) password you set during the installation process, as you will need to input it into the .env files in Phase 1.

Modern Web Browser

Purpose: Required for optimal Unity WebGL rendering and WebSocket real-time communication.

Requirement: Google Chrome, Microsoft Edge, or Mozilla Firefox (Latest Versions). Safari is not officially recommended for this WebGL build.

Visual Studio Code & Live Server (Optional but Recommended)

Purpose: Highly recommended for reviewing the codebase and utilizing the "Live Server" extension for a seamless frontend launch.

Download: https://code.visualstudio.com/

⚙️ Phase 1: Database & Dependencies
1. Configure Login Database Credentials
Login and user data for this project are stored in a local PostgreSQL database. You must configure the database password in two backend folders.

Target Paths:

[Your unzipped directory]\GameDeck_Foundry-backend_api\backend\.env

[Your unzipped directory]\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\backend\.env

Instructions:
Locate or create a file named .env in both directories above. Open them with a text editor and paste the following configuration (modify DB_PASSWORD to your actual PostgreSQL password):

代码段
DB_HOST=localhost 
DB_PORT=5432 
DB_NAME=gamedeck_db 
DB_USER=postgres 
DB_PASSWORD="your password" 
JWT_SECRET=gamedeck_super_secret_key_2026 
PORT=3001
2. Install Project Dependencies
This project includes multiple Node environments. Install the dependencies in both backend directories:

For Login Data Backend:

PowerShell
cd "D:\ICT_project1_private\GameDeck_Foundry-backend_api\backend"
npm install
For Unity Game Backend:

PowerShell
cd "D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\backend"
npm install
📁 Phase 2: Core File Placement (CRITICAL)
To ensure the front-end webpage loads the Unity game engine correctly, the Unity-exported Build folder must be placed in the correct path.

Instructions: Move or copy the entire Build folder (containing .wasm and .data files) from the source path to the destination path:

Source:

Plaintext
[Your unzipped directory]\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\unity\WebGl\Build
Destination:

Plaintext
[Your unzipped directory]\LoginAndJoinin\Build
(Note: If this step is not performed, the game screen will not be displayed after clicking Host/Join Game.)

🚀 Phase 3: Services Startup
This project requires Four separate terminal windows (PowerShell / CMD) to run simultaneously. Please keep all windows running in the background.

🗄️ Terminal 1: Login and Data Backend
Responsible for handling user registration, login authentication, and token distribution.

PowerShell
cd "D:\ICT_project1_private\GameDeck_Foundry-backend_api\backend"
npm start
🎮 Terminal 2: Unity Game Backend
Handles room creation, WebSocket connection logic, and in-game data synchronization.

PowerShell
cd "D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\backend"
npm start
🌐 Terminal 3: Unity WebGL Static Server
Hosts the raw Unity WebGL files.

PowerShell
cd "D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\unity\WebGl"
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
npx http-server -p 8080
(Note: If port 8080 is in use, you can use -p 8081 instead).

🖥️ Terminal 4: Main Frontend UI (Launch Application)
This hosts the main user UI and connects to the game.

PowerShell
cd "[Your unzipped directory]\LoginAndJoinin"
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
npx http-server -p 8082 -o /Game.html
