<<<<<<< HEAD
# EnzeZm-login
First important: move the Build file from unity_backend_linked to this paths
unity_backend_linked / unity / WebGl / Build

PS D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\unity\WebGl>  Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
PS D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\unity\WebGl> npx http-server -p 8080
This is for unity backend

PS D:\ICT_project1_private\GameDeck_Foundry-Unity_backend_linked\GameDeck_Foundry-Unity_backend_linked\backend> npm start
This is for unity game backend


PS D:\ICT_project1_private\GameDeck_Foundry-backend_api\backend> npm start
this is for login data backend



teacher login port： POST /api/auth/register

fe send： {"email": "...", "username": "...", "password": "..."}

be feedback： succes: 200/201；if email duplicate，feedback {"message": "email execest"}。



teacher login port： POST /api/auth/login

fe send： {"email": "...", "password": "..."}

be feedback： sucess back {"token": "your JWT num"}；fail reply {"message": "wrong password"}。



student joining code： POST /api/game/join

fe send： {"roomCode": "X7R2", "nickname": "Enze"}

be feedback： go to database to search this code，had and active，reply 200 and {"token": "temporary sessionToken"}；if donest reply {"message": "room does not appear"}。


3 URL are: register login studentjoin
71 register
106 login
144 join
=======
# EnzeZm-login


teacher login port： POST /api/auth/register

fe send： {"email": "...", "username": "...", "password": "..."}

be feedback： succes: 200/201；if email duplicate，feedback {"message": "email execest"}。



teacher login port： POST /api/auth/login

fe send： {"email": "...", "password": "..."}

be feedback： sucess back {"token": "your JWT num"}；fail reply {"message": "wrong password"}。



student joining code： POST /api/game/join

fe send： {"roomCode": "X7R2", "nickname": "Enze"}

be feedback： go to database to search this code，had and active，reply 200 and {"token": "temporary sessionToken"}；if donest reply {"message": "room does not appear"}。


3 URL are: register login studentjoin
71 register
106 login
144 join
>>>>>>> e1151ec81ad9be5687041646692dcb30ebb276a9
