document.addEventListener('DOMContentLoaded', () => {

    const logoutBtn = document.getElementById('logoutBtn');
    const hostGameBtn = document.getElementById('hostGameBtn');

    if (hostGameBtn) {
        hostGameBtn.addEventListener('click', async () => {
            try {
                console.log("start to build room...");

                const token = localStorage.getItem('gameDeckToken');
                let finalTeacherName = 'Teacher';

                if (token) {
                    try {
                        const decoded = jwtDecode.jwtDecode(token);
                        finalTeacherName = decoded.full_name || decoded.username || decoded.name || 'Teacher';
                        console.log(`teacher name deteced: ${finalTeacherName}`);
                    } catch (decodeErr) {
                        console.error("Token fail compile", decodeErr);
                    }
                }

                const response = await fetch('http://localhost:3000/api/session/create', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ teacher_name: finalTeacherName })
                });

                if (response.ok) {
                    const data = await response.json();
                    console.log("success", data);

                    localStorage.setItem('gameDeckRoomCode', data.session.join_code);
                    localStorage.setItem('gameDeckSessionId', data.session.session_id);
                    localStorage.setItem('gameDeckRole', 'teacher');

                    window.location.href = `unity-game.html?roomCode=${data.session.join_code}&role=teacher&name=${encodeURIComponent(finalTeacherName)}`;
                } else {
                    alert("Failed to create room on server.");
                }
            } catch (error) {
                console.error("backend fail connect：", error);
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