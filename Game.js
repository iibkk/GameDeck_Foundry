const roleSelectPanel = document.getElementById('roleSelectPanel');
const teacherWrapper = document.getElementById('teacherWrapper');
const studentPanel = document.getElementById('studentPanel');

const btnGoTeacher = document.getElementById('btnGoTeacher');
const btnGoStudent = document.getElementById('btnGoStudent');
const backToRoleBtns = document.querySelectorAll('.backToRole');

const logregBox = document.querySelector(".logreg-box");

const BASE_URL = 'https://realaddress.com';//(not use yet)

// 1. Role switching logic
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

// 2. Teacher Login / Register sliding animation logic
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

// 3. [NEW] Teacher real registration logic (Register)
const registerForm = document.querySelector('form.register');

registerForm.addEventListener('submit', async (e) => {
  e.preventDefault(); // Prevent default form submission and page refresh

  // Grab values from input fields (Order based on HTML: Email, Name, Password)
  const inputs = registerForm.querySelectorAll('input');
  const email = inputs[0].value;
  const username = inputs[1].value;
  const password = inputs[2].value;

  try {
    // Send POST request to backend (assuming Node.js runs on localhost:3000)(register change here)
    const response = await fetch('http://localhost:3001/api/auth/register', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email: email, full_name: username, password: password })
    });

    // If successful (status code 200 or 201)
    // If successful (status code 200 or 201)
    if (response.ok) {
      // 1. succuess
      alert('Registration successful! Automatically logging you in...');

      try {
        // 2. 
        const loginResponse = await fetch('http://localhost:3001/api/auth/login', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ email: email, password: password })
        });

        if (loginResponse.ok) {
          const loginData = await loginResponse.json();

          // 3.  save Token loacl
          localStorage.setItem('gameDeckToken', loginData.token);

          registerForm.reset();
          window.location.href = 'teacher-dashboard.html';
        }
      } catch (loginErr) {
        console.error('Auto-login failed:', loginErr);
        logregBox.classList.remove("active");
        registerForm.reset();
      }
    } else {
      const errorData = await response.json();
      alert('Registration failed: ' + (errorData.message || 'Unknown error'));
    }
  } catch (error) {
    console.error('Network Error:', error);
    alert('Cannot connect to the server. Is the Node.js backend running?');
  }
});

// 4. [NEW] Teacher real login logic (Login)
const loginForm = document.querySelector('form.login');

loginForm.addEventListener('submit', async (e) => {
  e.preventDefault();

  const inputs = loginForm.querySelectorAll('input');
  const email = inputs[0].value;
  const password = inputs[1].value;

  try {//(teacher login URL change here)
    const response = await fetch('http://localhost:3001/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email: email, password: password })
    });

    if (response.ok) {
      const data = await response.json();
      // alert('Login successful!');

      localStorage.setItem('gameDeckToken', data.token);

      window.location.href = 'teacher-dashboard.html';
    } else {
      const errorData = await response.json();
      alert('Login failed: ' + (errorData.message || 'Wrong email or password'));
    }
  } catch (error) {
    console.error('Network Error:', error);
    alert('Cannot connect to the server. Is the Node.js backend running?');
  }
});

// 5 Student join room validation logic
document.getElementById('studentForm').addEventListener('submit', async function (e) {
  e.preventDefault();

  // Get input values and force room code to uppercase (e.g., x7r2 to X7R2, fool-proofing)
  const roomCode = document.getElementById('roomCodeInput').value.toUpperCase();
  const nickname = document.getElementById('nicknameInput').value;

  try {

    const response = await fetch('http://localhost:3000/api/session/join', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ join_code: roomCode, player_name: nickname })
    });

    if (response.ok) {
      // Room found successfully
      const data = await response.json();

      localStorage.setItem('gameDeckStudentId', data.player.session_player_id);
      localStorage.setItem('gameDeckSessionId', data.session.session_id);
      localStorage.setItem('gameDeckRole', 'student'); // 🚨 改为小写 student，保持前后端本地缓存数据一致性
      localStorage.setItem('gameDeckPlayerName', nickname);

      window.location.href = `unity-game.html?roomCode=${roomCode}&role=student&name=${encodeURIComponent(nickname)}`;

    } else {
      // Handle errors (e.g., room not found, full, or game already started)
      const errorData = await response.json();
      alert("Error: " + (errorData.message || "Room code incorrect or game already started."));
    }
  } catch (error) {
    console.error('Network Error:', error);
    alert('Cannot connect to the server. Is the Node.js backend running?');
  }
});