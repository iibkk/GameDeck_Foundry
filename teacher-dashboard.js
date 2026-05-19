document.addEventListener('DOMContentLoaded', () => {

    const logoutBtn = document.getElementById('logoutBtn');
    const hostGameBtn = document.getElementById('hostGameBtn');

    if (hostGameBtn) {
        hostGameBtn.addEventListener('click', async () => {
            try {
                console.log("🚀 正在向后端发起真实的建房请求...");

                // 1. 获取 Token 用于解密名字
                const token = localStorage.getItem('gameDeckToken');
                let finalTeacherName = 'Teacher';

                if (token) {
                    try {
                        const decoded = jwtDecode.jwtDecode(token);
                        // 获取注册时的真实姓名
                        finalTeacherName = decoded.full_name || decoded.username || decoded.name || 'Teacher';
                        console.log(`🙋‍♂️ 识别到老师姓名: ${finalTeacherName}`);
                    } catch (decodeErr) {
                        console.error("Token 解析失败", decodeErr);
                    }
                }

                // 2. 发送创建房间请求
                const response = await fetch('http://localhost:3000/api/session/create', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ teacher_name: finalTeacherName })
                });

                if (response.ok) {
                    const data = await response.json();
                    console.log("房间创建成功，数据：", data);

                    // 3. 存储房间信息
                    localStorage.setItem('gameDeckRoomCode', data.session.join_code);
                    localStorage.setItem('gameDeckSessionId', data.session.session_id);
                    localStorage.setItem('gameDeckRole', 'teacher');

                    // 4. 跳转到游戏，把 finalTeacherName 传过去
                    window.location.href = `unity-game.html?roomCode=${data.session.join_code}&role=teacher&name=${encodeURIComponent(finalTeacherName)}`;
                } else {
                    alert("Failed to create room on server.");
                }
            } catch (error) {
                console.error("连接后端失败：", error);
                alert("Cannot connect to server.");
            }
        });
    }

    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            if (confirm("Are you sure you want to sign out?")) {
                localStorage.removeItem('gameDeckToken');
                localStorage.removeItem('gameDeckTeacherName');
                window.location.href = 'Game.html';
            }
        });
    }
});