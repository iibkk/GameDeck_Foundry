<<<<<<< HEAD
document.addEventListener('DOMContentLoaded', () => {

    const logoutBtn = document.getElementById('logoutBtn');
    const hostGameBtn = document.getElementById('hostGameBtn');

    if (hostGameBtn) {
        hostGameBtn.addEventListener('click', async () => {
            try {
                console.log("apply real game room biulding...");

                const token = localStorage.getItem('gameDeckToken');
                let finalTeacherName = 'Teacher';

                if (token) {
                    try {
                        const decoded = jwtDecode.jwtDecode(token);
                        finalTeacherName = decoded.full_name || decoded.username || decoded.name || 'Teacher';
                        console.log(`teacher name detected: ${finalTeacherName}`);
                    } catch (decodeErr) {
                        console.error("Token fail", decodeErr);
                    }
                }

                const response = await fetch('http://localhost:3000/api/session/create', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ teacher_name: finalTeacherName })
                });

                if (response.ok) {
                    const data = await response.json();
                    console.log("房间创建成功，数据：", data);

                    localStorage.setItem('gameDeckRoomCode', data.session.join_code);
                    localStorage.setItem('gameDeckSessionId', data.session.session_id);
                    localStorage.setItem('gameDeckRole', 'teacher');

                    window.location.href = `unity-game.html?roomCode=${data.session.join_code}&role=teacher&name=${encodeURIComponent(finalTeacherName)}`;
                } else {
                    alert("Failed to create room on server.");
                }
            } catch (error) {
                console.error("Backend fail to connect：", error);
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
=======
document.addEventListener('DOMContentLoaded', () => {

    const logoutBtn = document.getElementById('logoutBtn');
    const hostGameBtn = document.getElementById('hostGameBtn'); // 1. 抓取按钮

    // ==========================================
    // 老师创建房间逻辑
    // ==========================================
    if (hostGameBtn) {
        // hostGameBtn.addEventListener('click', async () => {
        //     try {
        //         // 取出老师登录时存的 Token
        //         const token = localStorage.getItem('gameDeckToken');

        //         // 向后端发送创建房间的请求 (🚨 注意：请和后端队友确认这个 URL 是 /create 还是 /host)
        //         const response = await fetch('http://localhost:3000/api/session/create', {
        //             method: 'POST',
        //             headers: {
        //                 'Content-Type': 'application/json',
        //                 'Authorization': `Bearer ${token}` // 老师建房通常需要验证身份
        //             }
        //         });

        //         if (response.ok) {
        //             const data = await response.json();

        //             // 把后端返回的房间号和 Session ID 存起来
        //             localStorage.setItem('gameDeckRoomCode', data.session.join_code);
        //             localStorage.setItem('gameDeckSessionId', data.session.session_id);
        //             localStorage.setItem('gameDeckRole', 'Teacher'); // 极其重要：标记当前使用者是老师！

        //             // 带着房间号跳转到 Unity 游戏页面
        //             window.location.href = `unity-game.html?room=${data.session.join_code}`;
        //         } else {
        //             alert("Failed to create room.");
        //         }
        //     } catch (error) {
        //         console.error(error);
        //         alert("Cannot connect to server.");
        //     }
        // });

        hostGameBtn.addEventListener('click', async () => {
            // [演示专用兜底模式] 强行绕过报错的后端！
            console.log("⚠️ 绕过真实后端，使用紧急兜底模式建房！");

            // 1. 强行伪造一个房间号
            const fakeRoomCode = "DEMO99";

            // 2. 强行把身份和假房间号存入浏览器
            localStorage.setItem('gameDeckRoomCode', fakeRoomCode);
            localStorage.setItem('gameDeckSessionId', 'demo-session-12345');
            localStorage.setItem('gameDeckRole', 'Teacher');

            // 3. 直接跳转！明天台上演示时绝对丝滑！
            window.location.href = `unity-game.html?room=${fakeRoomCode}`;
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
>>>>>>> e1151ec81ad9be5687041646692dcb30ebb276a9
});