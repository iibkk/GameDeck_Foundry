# EnzeZm-login


老师注册接口： POST /api/auth/register

前端会发： {"email": "...", "username": "...", "password": "..."}

后端请返回： 成功给 200/201；如果邮箱重复，返回 {"message": "邮箱已存在"}。

老师登录接口： POST /api/auth/login

前端会发： {"email": "...", "password": "..."}

后端请返回： 成功请返回 {"token": "你的JWT串"}；失败返回 {"message": "密码错误"}。

学生加入房间接口： POST /api/game/join

前端会发： {"roomCode": "X7R2", "nickname": "Enze"}

后端请返回： 去数据库查有没有这个 code，如果有且状态是 active，返回 200 和 {"token": "临时sessionToken"}；没有请报错 {"message": "房间不存在"}。


三个URL分别 register login studentjoin
71 register
106 login
144 join
