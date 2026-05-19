document.addEventListener('DOMContentLoaded', () => {
    const joinGameBtn = document.getElementById('joinGameBtn');
    const playerNameInput = document.getElementById('playerName');
    const roomCodeInput = document.getElementById('roomCode');

    if (joinGameBtn) {
        joinGameBtn.addEventListener('click', async () => {
            // 1. 获取输入框里的内容
            const playerName = playerNameInput.value.trim();
            // 自动把学生输入的房间号变成大写字母，防止手滑
            const roomCode = roomCodeInput.value.trim().toUpperCase();

            // 2. 基础验证
            if (!playerName) {
                alert("Please enter your name!");
                return;
            }
            if (!roomCode || roomCode.length !== 6) {
                alert("Please enter a valid 6-digit Room Code!");
                return;
            }

            try {
                console.log(`🚀 正在向后端申请加入房间: ${roomCode}...`);

                // 3. 🚨 核心：向你后端的 /api/session/join 接口发送 POST 请求
                const response = await fetch('http://localhost:3000/api/session/join', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    // 带上后端急需的两个参数
                    body: JSON.stringify({
                        player_name: playerName,
                        join_code: roomCode
                    })
                });

                const data = await response.json();

                if (response.ok && data.success) {
                    console.log("🎉 成功加入房间！后端返回的数据：", data);

                    // 4. 把学生信息存入浏览器的本地缓存
                    localStorage.setItem('gameDeckRoomCode', data.join_code);
                    localStorage.setItem('gameDeckSessionId', data.session_id);
                    localStorage.setItem('gameDeckRole', 'student'); // 标记为学生！
                    localStorage.setItem('gameDeckPlayerName', playerName);

                    // 5. 🚨 终极闭环：跳转游戏，并拼出能让 Unity C# 脚本直接看懂的全小写 "role=student" 的完美网址暗号！
                    window.location.href = `unity-game.html?roomCode=${data.join_code}&role=student&name=${encodeURIComponent(playerName)}`;
                } else {
                    // 如果后端返回 404 (房间不存在) 或其他错误
                    alert(`Join Failed: ${data.message || 'Room not found.'}`);
                }

            } catch (error) {
                console.error("连接后端失败：", error);
                alert("Cannot connect to server. Make sure backend is running.");
            }
        });
    }
});