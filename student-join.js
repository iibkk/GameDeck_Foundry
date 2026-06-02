document.addEventListener('DOMContentLoaded', () => {
    const joinGameBtn = document.getElementById('joinGameBtn');
    const playerNameInput = document.getElementById('playerName');
    const roomCodeInput = document.getElementById('roomCode');

    if (joinGameBtn) {
        joinGameBtn.addEventListener('click', async () => {

            const playerName = playerNameInput.value.trim();
            const roomCode = roomCodeInput.value.trim().toUpperCase();

            if (!playerName) {
                alert("Please enter your name!");
                return;
            }
            if (!roomCode || roomCode.length !== 6) {
                alert("Please enter a valid 6-digit Room Code!");
                return;
            }

            try {
                console.log(`🚀 applying join in room to backend: ${roomCode}...`);

                const response = await fetch('http://localhost:3000/api/session/join', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        player_name: playerName,
                        join_code: roomCode
                    })
                });

                const data = await response.json();

                if (response.ok && data.success) {
                    console.log("success join in to the room backend data back：", data);

                    localStorage.setItem('gameDeckRoomCode', data.join_code);
                    localStorage.setItem('gameDeckSessionId', data.session_id);
                    localStorage.setItem('gameDeckRole', 'student');
                    localStorage.setItem('gameDeckPlayerName', playerName);

                    window.location.href = `unity-game.html?roomCode=${data.join_code}&role=student&name=${encodeURIComponent(playerName)}`;
                } else {
                    alert(`Join Failed: ${data.message || 'Room not found.'}`);
                }

            } catch (error) {
                console.error("backend connect error：", error);
                alert("Cannot connect to server. Make sure backend is running.");
            }
        });
    }
});