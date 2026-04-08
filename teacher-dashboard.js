document.addEventListener('DOMContentLoaded', () => {

    const logoutBtn = document.getElementById('logoutBtn');

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