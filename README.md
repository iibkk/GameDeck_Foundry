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



cd "D:\ICT_project1_private\GameDeck_Foundry-backend_api\backend"
