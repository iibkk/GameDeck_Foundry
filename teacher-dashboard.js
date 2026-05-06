document.addEventListener('DOMContentLoaded', () => {

    const logoutBtn = document.getElementById('logoutBtn');
    const hostGameBtn = document.getElementById('hostGameBtn'); // 1. 抓取按钮

    // ==========================================
    // 老师创建房间逻辑
    // ==========================================
    if (hostGameBtn) {
        hostGameBtn.addEventListener('click', async () => {
            try {
                // 取出老师登录时存的 Token
                const token = localStorage.getItem('gameDeckToken');

                // 向后端发送创建房间的请求 (🚨 注意：请和后端队友确认这个 URL 是 /create 还是 /host)
                const response = await fetch('http://localhost:3000/api/session/create', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}` // 老师建房通常需要验证身份
                    }
                });

                if (response.ok) {
                    const data = await response.json();

                    // 把后端返回的房间号和 Session ID 存起来
                    localStorage.setItem('gameDeckRoomCode', data.session.join_code);
                    localStorage.setItem('gameDeckSessionId', data.session.session_id);
                    localStorage.setItem('gameDeckRole', 'Teacher'); // 极其重要：标记当前使用者是老师！

                    // 带着房间号跳转到 Unity 游戏页面
                    window.location.href = `unity-game.html?room=${data.session.join_code}`;
                } else {
                    alert("Failed to create room.");
                }
            } catch (error) {
                console.error(error);
                alert("Cannot connect to server.");
            }
        });
    }

    // --- 下面是你原有的登出逻辑保持不变 ---
    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            const confirmLogout = confirm("Are you sure you want to sign out?");
            if (confirmLogout) {
                localStorage.removeItem('gameDeckToken');
                window.location.href = 'Game.html';
            }
        });
    }
});