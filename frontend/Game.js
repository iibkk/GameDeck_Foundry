// Get panels and buttons
const roleSelectPanel = document.getElementById('roleSelectPanel');
const teacherWrapper = document.getElementById('teacherWrapper');
const studentPanel = document.getElementById('studentPanel');

const btnGoTeacher = document.getElementById('btnGoTeacher');
const btnGoStudent = document.getElementById('btnGoStudent');
const backToRoleBtns = document.querySelectorAll('.backToRole');

const logregBox = document.querySelector(".logreg-box");

const BASE_URL = 'http://localhost:3000';

// ==========================================
// 1. Role switching logic
// ==========================================
btnGoTeacher.addEventListener('click', () => {
  roleSelectPanel.classList.add('hidden');
  teacherWrapper.classList.remove('hidden');
});

btnGoStudent.addEventListener('click', () => {
  roleSelectPanel.classList.add('hidden');
  studentPanel.classList.remove('hidden');
});

// Go back to the role selection page when the "Back" button is clicked
backToRoleBtns.forEach(btn => {
  btn.addEventListener('click', () => {
    teacherWrapper.classList.add('hidden');
    studentPanel.classList.add('hidden');
    roleSelectPanel.classList.remove('hidden');

    // Reset the sliding state of the teacher login box to ensure it defaults to Login next time
    logregBox.classList.remove('active');
  });
});

// ==========================================
// 2. Teacher Login / Register sliding animation logic
// ==========================================
const loginLink = document.querySelector(".login-link");
const registerLink = document.querySelector(".register-link");

registerLink.addEventListener("click", (e) => {
  e.preventDefault();
  logregBox.classList.add("active");
});

loginLink.addEventListener("click", (e) => {
  e.preventDefault();
  logregBox.classList.remove("active");
});

// ==========================================
// 3. Teacher registration logic
// ==========================================
const registerForm = document.querySelector('.register');

registerForm.addEventListener('submit', async (e) => {
  e.preventDefault();

  // Based on current HTML order: Email, Name, Password
  const inputs = registerForm.querySelectorAll('input');
  const email = inputs[0].value.trim();
  const full_name = inputs[1].value.trim();
  const password = inputs[2].value;

  try {
    const response = await fetch(`${BASE_URL}/api/auth/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ full_name, email, password })
    });

    const data = await response.json();

    if (response.ok) {
      alert('Registration successful! Please log in.');
      logregBox.classList.remove("active");
      registerForm.reset();
    } else {
      alert('Registration failed: ' + (data.message || data.error || 'Unknown error'));
    }
  } catch (error) {
    console.error('Network Error:', error);
    alert('Cannot connect to the server. Is the backend running?');
  }
});

// ==========================================
// 4. Teacher login logic
// ==========================================
const loginForm = document.querySelector('.login');

loginForm.addEventListener('submit', async (e) => {
  e.preventDefault();

  const inputs = loginForm.querySelectorAll('input');
  const email = inputs[0].value.trim();
  const password = inputs[1].value;

  try {
    const response = await fetch(`${BASE_URL}/api/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });

    const data = await response.json();

    if (response.ok) {
      alert('Login successful!');

      localStorage.setItem('gameDeckToken', data.token);
      localStorage.setItem('teacherUser', JSON.stringify(data.user));

      // Change later if your real dashboard file name is different
      window.location.href = 'teacher-dashboard.html';
    } else {
      alert('Login failed: ' + (data.message || data.error || 'Wrong email or password'));
    }
  } catch (error) {
    console.error('Network Error:', error);
    alert('Cannot connect to the server. Is the backend running?');
  }
});

// ==========================================
// 5. Student join room logic
// ==========================================
document.getElementById('studentForm').addEventListener('submit', async function (e) {
  e.preventDefault();

  const join_code = document.getElementById('roomCodeInput').value.trim().toUpperCase();
  const player_name = document.getElementById('nicknameInput').value.trim();

  try {
    const response = await fetch(`${BASE_URL}/api/session/join`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ join_code, player_name })
    });

    const data = await response.json();

    if (response.ok) {
      alert(`Welcome ${player_name}! Room found. Loading game...`);

      // Save returned info for later use
      localStorage.setItem('studentPlayer', JSON.stringify(data.player));
      localStorage.setItem('studentSession', JSON.stringify(data.session));

      // Change later if your Unity file name is different
      window.location.href = `unity-game.html?room=${join_code}`;
    } else {
      alert("Error: " + (data.message || data.error || "Room code incorrect or game already started."));
    }
  } catch (error) {
    console.error('Network Error:', error);
    alert('Cannot connect to the server. Is the backend running?');
  }
});
