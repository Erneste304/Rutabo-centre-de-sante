// Authentication Module
class Auth {
    constructor() {
        this.currentUser = null;
        this.token = localStorage.getItem('hms_token');
        this.init();
    }

    init() {
        // Check if user is already logged in
        if (this.token) {
            this.validateToken();
        }

        // Setup login form
        this.setupLoginForm();
    }

    setupLoginForm() {
        const loginBtn = document.getElementById('loginBtn');
        const userTypeBtns = document.querySelectorAll('.user-type-btn');

        if (loginBtn) {
            loginBtn.addEventListener('click', () => this.login());

            // Enter key login
            document.getElementById('password')?.addEventListener('keypress', (e) => {
                if (e.key === 'Enter') this.login();
            });
        }

        // User type selection
        userTypeBtns.forEach(btn => {
            btn.addEventListener('click', () => {
                userTypeBtns.forEach(b => b.classList.remove('active'));
                btn.classList.add('active');
            });
        });

        // Toggle password visibility
        document.querySelector('.toggle-password')?.addEventListener('click', (e) => {
            const passwordInput = document.getElementById('password');
            const icon = e.target.tagName === 'I' ? e.target : e.target.querySelector('i');

            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                icon.className = 'fas fa-eye-slash';
            } else {
                passwordInput.type = 'password';
                icon.className = 'fas fa-eye';
            }
        });

        // Forgot password link
        const forgotLink = document.getElementById('forgotPasswordLink');
        if (forgotLink) {
            forgotLink.addEventListener('click', (e) => {
                e.preventDefault();
                const username = prompt('Enter your username to request a password reset:');
                if (username) this.requestPasswordReset(username);
            });
        }
    }

    async requestPasswordReset(username) {
        try {
            const response = await fetch(`${CONFIG.API_URL}/Auth/forgot-password`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username })
            });
            const data = await response.json();
            Utils.showToast(data.message, 'info');
        } catch (err) {
            Utils.showToast('Failed to send request', 'error');
        }
    }


    async login() {
        const username = document.getElementById('username').value.trim();
        const password = document.getElementById('password').value.trim();
        const rememberMe = document.getElementById('rememberMe').checked;

        // Get selected user type
        const selectedBtn = document.querySelector('.user-type-btn.active');
        const userType = selectedBtn?.dataset.type || 'admin';

        if (!username || !password) {
            Utils.showToast('Please enter username and password', 'error');
            return;
        }

        Utils.showLoading();

        try {
            const response = await fetch(`${CONFIG.API_URL}/Auth/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    username: username,
                    password: password
                })
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Invalid credentials');
            }

            const data = await response.json();

            // Backend returns { token, username, userType, fullName, ... }
            // Verify user type matches selection (if required by business logic)
            if (data.userType.toLowerCase() !== userType.toLowerCase()) {
                // Warning: Redirecting anyway but logging the mismatch
                console.warn(`User Type Mismatch: Expected ${userType}, got ${data.userType}`);
            }

            // Login successful
            this.currentUser = {
                username: data.username,
                userType: data.userType,
                fullName: data.fullName || data.username,
                token: data.token
            };
            this.token = data.token;

            // Save to localStorage
            localStorage.setItem('hms_token', this.token);
            localStorage.setItem('hms_user', JSON.stringify(this.currentUser));

            Utils.showToast(`Welcome back, ${this.currentUser.fullName}!`, 'success');
            Utils.hideLoading();
            this.showDashboard();

        } catch (error) {
            Utils.hideLoading();
            Utils.showToast(error.message || 'Login failed. Please try again.', 'error');
        }
    }

    async validateToken() {
        // Validate token with API
        try {
            const userData = localStorage.getItem('hms_user');
            if (userData) {
                this.currentUser = JSON.parse(userData);
                this.showDashboard();
            }
        } catch (error) {
            this.logout();
        }
    }

    showDashboard() {
        // Hide login screen
        document.getElementById('loginScreen').style.display = 'none';

        // Show dashboard
        const dashboard = document.getElementById('dashboard');
        dashboard.style.display = 'flex';

        // Update user info
        if (this.currentUser) {
            document.getElementById('userFullName').textContent = this.currentUser.fullName;
            document.getElementById('userRole').textContent = this.currentUser.userType;
            document.getElementById('userRole').className = `user-role ${this.currentUser.userType.toLowerCase()}-role`;
            document.getElementById('userDepartment').textContent = this.currentUser.department || '';

            // Update dashboard title
            document.getElementById('dashboardTitle').textContent = `${this.currentUser.userType} Dashboard`;

            // Load appropriate dashboard module
            this.loadDashboardModule();
        }

        // Initialize dashboard
        this.initDashboard();
    }

    loadDashboardModule() {
        const userType = this.currentUser?.userType?.toLowerCase();
        console.log(`Loading dashboard for user type: ${userType}`);

        // Remove existing module if any
        const oldModule = document.querySelector('.dashboard-module');
        if (oldModule) oldModule.remove();

        // Create module container
        const moduleContainer = document.createElement('div');
        moduleContainer.className = 'dashboard-module';
        moduleContainer.id = `${userType}Module`;

        // Add to content area
        const contentArea = document.getElementById('contentArea');
        contentArea.innerHTML = '';
        contentArea.appendChild(moduleContainer);

        // Load module based on user type
        try {
            switch (userType) {
                case 'admin':
                    this.loadAdminModule(moduleContainer);
                    break;
                case 'doctor':
                    this.loadDoctorModule(moduleContainer);
                    break;
                case 'nurse':
                    this.loadNurseModule(moduleContainer);
                    break;
                case 'receptionist':
                    this.loadReceptionistModule(moduleContainer);
                    break;
                case 'accountant':
                    this.loadAccountantModule(moduleContainer);
                    break;
                case 'patient':
                    this.loadPatientModule(moduleContainer);
                    break;
                default:
                    console.warn(`Unknown user type: ${userType}, loading admin module`);
                    this.loadAdminModule(moduleContainer);
            }
        } catch (error) {
            console.error('Error loading dashboard module:', error);
            this.showErrorModule(moduleContainer, userType);
        }
    }

    loadAdminModule(container) {
        if (window.AdminModule) {
            window.AdminModule.init(container);
        } else {
            this.showPlaceholderModule(container, 'Admin', 'admin', [
                { icon: 'fa-users', title: 'User Management', desc: 'Manage all system users, roles, and permissions' },
                { icon: 'fa-user-md', title: 'Doctor Management', desc: 'Manage doctors and their specializations' },
                { icon: 'fa-procedures', title: 'Patient Management', desc: 'View and manage patient records' },
                { icon: 'fa-building', title: 'Department Management', desc: 'Manage hospital departments' },
                { icon: 'fa-bed', title: 'Room Management', desc: 'Manage hospital rooms and beds' },
                { icon: 'fa-file-invoice-dollar', title: 'Billing Management', desc: 'Manage billing and payments' },
                { icon: 'fa-chart-line', title: 'System Reports', desc: 'View comprehensive system reports' },
                { icon: 'fa-cog', title: 'System Configuration', desc: 'Configure system settings' }
            ]);
        }
    }

    loadDoctorModule(container) {
        if (window.DoctorModule) {
            window.DoctorModule.init(container);
        } else {
            this.showPlaceholderModule(container, 'Doctor', 'doctor', [
                { icon: 'fa-calendar-check', title: "Today's Schedule", desc: 'View your appointments for today' },
                { icon: 'fa-prescription', title: 'Write Prescription', desc: 'Create prescriptions for patients' },
                { icon: 'fa-notes-medical', title: 'Medical Records', desc: 'Update patient medical records' },
                { icon: 'fa-clock', title: 'Set Availability', desc: 'Manage your availability schedule' },
                { icon: 'fa-users', title: 'My Patients', desc: 'View your assigned patients' }
            ]);
        }
    }

    loadNurseModule(container) {
        if (window.NurseModule) {
            window.NurseModule.init(container);
        } else {
            this.showPlaceholderModule(container, 'Nurse', 'nurse', [
                { icon: 'fa-heartbeat', title: 'Record Vital Signs', desc: 'Record patient vital signs' },
                { icon: 'fa-pills', title: 'Administer Medication', desc: 'Manage medication administration' },
                { icon: 'fa-tasks', title: 'Patient Care Tasks', desc: 'View and complete care tasks' },
                { icon: 'fa-bed', title: 'Manage Rooms', desc: 'Manage room assignments' },
                { icon: 'fa-file-medical', title: 'Shift Reports', desc: 'Create and view shift reports' },
                { icon: 'fa-boxes', title: 'Inventory Check', desc: 'Check medical inventory' }
            ]);
        }
    }

    loadReceptionistModule(container) {
        if (window.ReceptionistModule) {
            window.ReceptionistModule.init(container);
        } else {
            this.showPlaceholderModule(container, 'Receptionist', 'receptionist', [
                { icon: 'fa-user-plus', title: 'Patient Registration', desc: 'Register new patients' },
                { icon: 'fa-calendar-alt', title: 'Appointment Scheduling', desc: 'Schedule patient appointments' },
                { icon: 'fa-sign-in-alt', title: 'Check-In/Check-Out', desc: 'Manage patient check-ins' },
                { icon: 'fa-money-bill-wave', title: 'Billing & Payments', desc: 'Process payments' },
                { icon: 'fa-user-clock', title: 'Doctor Schedule', desc: 'View doctor schedules' }
            ]);
        }
    }

    loadAccountantModule(container) {
        if (window.AccountantModule) {
            window.AccountantModule.init(container);
        } else {
            this.showPlaceholderModule(container, 'Accountant', 'accountant', [
                { icon: 'fa-file-invoice', title: 'View All Bills', desc: 'View all billing records' },
                { icon: 'fa-money-check-alt', title: 'View Payments', desc: 'View payment history' },
                { icon: 'fa-exchange-alt', title: 'Transactions', desc: 'View all transactions' },
                { icon: 'fa-check-circle', title: 'Approve Transactions', desc: 'Approve pending transactions' },
                { icon: 'fa-chart-pie', title: 'Financial Reports', desc: 'Generate financial reports' }
            ]);
        }
    }

    loadPatientModule(container) {
        if (window.PatientModule) {
            window.PatientModule.init(container);
        } else {
            this.showPlaceholderModule(container, 'Patient', 'patient', [
                { icon: 'fa-calendar', title: 'My Appointments', desc: 'View your appointments' },
                { icon: 'fa-calendar-plus', title: 'Book Appointment', desc: 'Schedule a new appointment' },
                { icon: 'fa-file-medical-alt', title: 'Medical Records', desc: 'View your medical records' },
                { icon: 'fa-file-invoice-dollar', title: 'My Bills', desc: 'View your bills and payments' },
                { icon: 'fa-user-edit', title: 'Update Profile', desc: 'Update your personal information' },
                { icon: 'fa-user-md', title: 'View Doctors', desc: 'Browse available doctors' }
            ]);
        }
    }

    showPlaceholderModule(container, roleName, roleClass, features) {
        container.innerHTML = `
            <div class="welcome-message">
                <h2>Welcome to ${roleName} Dashboard</h2>
                <p class="subtitle">Hospital Management System - ${roleName} Portal</p>
                <div class="dashboard-grid">
                    ${features.map(feature => `
                        <div class="dashboard-card ${roleClass}-card">
                            <div class="card-icon">
                                <i class="fas ${feature.icon}"></i>
                            </div>
                            <div class="card-content">
                                <h3>${feature.title}</h3>
                                <p>${feature.desc}</p>
                            </div>
                            <button class="card-action-btn" onclick="alert('Feature coming soon!')">
                                <i class="fas fa-arrow-right"></i>
                            </button>
                        </div>
                    `).join('')}
                </div>
            </div>
        `;
    }

    showErrorModule(container, userType) {
        container.innerHTML = `
            <div class="error-message">
                <i class="fas fa-exclamation-triangle"></i>
                <h2>Error Loading Dashboard</h2>
                <p>Unable to load the ${userType} dashboard module.</p>
                <button onclick="location.reload()" class="btn btn-primary">Reload Page</button>
            </div>
        `;
    }

    initDashboard() {
        // Update date and time
        this.updateDateTime();
        setInterval(() => this.updateDateTime(), 1000);

        // Setup sidebar toggle
        document.querySelector('.sidebar-toggle')?.addEventListener('click', () => {
            document.querySelector('.sidebar').classList.toggle('collapsed');
        });

        // Setup logout
        document.getElementById('logoutBtn')?.addEventListener('click', () => this.logout());

        // Setup notifications panel
        document.getElementById('notificationsBtn')?.addEventListener('click', () => {
            document.getElementById('notificationsPanel').classList.toggle('active');
        });

        // Setup search
        document.getElementById('searchBtn')?.addEventListener('click', () => {
            this.showSearchModal();
        });
    }

    updateDateTime() {
        const now = new Date();
        const dateStr = now.toLocaleDateString('en-US', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
        const timeStr = now.toLocaleTimeString('en-US', {
            hour12: true,
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
        });

        document.getElementById('currentDate').textContent = dateStr;
        document.getElementById('currentTime').textContent = timeStr;
    }

    logout() {
        // Clear storage
        localStorage.removeItem('hms_token');
        localStorage.removeItem('hms_user');


        this.currentUser = null;
        this.token = null;


        document.getElementById('dashboard').style.display = 'none';
        document.getElementById('loginScreen').style.display = 'flex';


        document.getElementById('username').value = '';
        document.getElementById('password').value = '';

        Utils.showToast('Logged out successfully', 'success');
    }

    showSearchModal() {
        const modal = document.createElement('div');
        modal.className = 'modal-overlay active';
        modal.innerHTML = `
            <div class="modal">
                <div class="modal-header">
                    <h3>Search Hospital System</h3>
                    <button class="modal-close">&times;</button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <input type="text" id="globalSearch" placeholder="Search patients, doctors, appointments..." autocomplete="off">
                    </div>
                    <div id="searchResults"></div>
                </div>
            </div>
        `;

        document.body.appendChild(modal);

        modal.querySelector('.modal-close').addEventListener('click', () => {
            modal.remove();
        });

        modal.addEventListener('click', (e) => {
            if (e.target === modal) {
                modal.remove();
            }
        });

        const searchInput = modal.querySelector('#globalSearch');
        searchInput.focus();

        searchInput.addEventListener('input', (e) => {
            this.performSearch(e.target.value);
        });
    }

    performSearch(query) {
        const results = [
            { type: 'Patient', name: 'John Doe', id: 'P1001', details: 'Cardiology - Room 101' },
            { type: 'Doctor', name: 'Dr. Smith', id: 'D101', details: 'Cardiology Department' },
            { type: 'Appointment', name: 'Follow-up', id: 'A2001', details: 'Tomorrow, 10:00 AM' }
        ];

        const filtered = results.filter(item =>
            item.name.toLowerCase().includes(query.toLowerCase()) ||
            item.id.toLowerCase().includes(query.toLowerCase())
        );

        const resultsContainer = document.querySelector('#searchResults');
        if (resultsContainer) {
            if (query.length < 2) {
                resultsContainer.innerHTML = '<p>Type at least 2 characters to search</p>';
                return;
            }

            if (filtered.length === 0) {
                resultsContainer.innerHTML = '<p>No results found</p>';
                return;
            }

            resultsContainer.innerHTML = filtered.map(item => `
                <div class="search-result-item">
                    <div class="result-type">${item.type}</div>
                    <div class="result-content">
                        <h4>${item.name}</h4>
                        <p>${item.id} • ${item.details}</p>
                    </div>
                    <button class="btn btn-primary btn-sm">View</button>
                </div>
            `).join('');
        }
    }
}


document.addEventListener('DOMContentLoaded', () => {
    window.Auth = new Auth();
});