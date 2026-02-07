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
        
        // For demo purposes, use mock authentication
        // In real app, this would call your API
        Utils.showLoading();
        
        try {
            // Mock API call
            await new Promise(resolve => setTimeout(resolve, 1000));
            
            // Mock users for demo
            const mockUsers = {
                'admin': { 
                    username: 'admin', 
                    password: 'Admin@123', 
                    userType: 'Admin',
                    fullName: 'System Administrator',
                    department: 'System Administration',
                    email: 'admin@hospital.com'
                },
                'doctor': { 
                    username: 'dr.smith', 
                    password: 'Doctor@123', 
                    userType: 'Doctor',
                    fullName: 'Dr. John Smith',
                    specialization: 'Cardiology',
                    department: 'Cardiology',
                    email: 'smith@hospital.com'
                },
                'reception': { 
                    username: 'reception', 
                    password: 'Reception@123', 
                    userType: 'Receptionist',
                    fullName: 'Sarah Johnson',
                    department: 'Reception',
                    email: 'reception@hospital.com'
                },
                'accountant': { 
                    username: 'accountant', 
                    password: 'Accountant@123', 
                    userType: 'Accountant',
                    fullName: 'Mike Johnson',
                    department: 'Finance',
                    email: 'accountant@hospital.com'
                }
            };
            
            // Check credentials
            const user = Object.values(mockUsers).find(u => 
                u.username === username && u.password === password
            );
            
            if (user && user.userType.toLowerCase() === userType.toLowerCase()) {
                // Login successful
                this.currentUser = user;
                this.token = this.generateToken();
                
                // Save to localStorage if remember me is checked
                if (rememberMe) {
                    localStorage.setItem('hms_token', this.token);
                    localStorage.setItem('hms_user', JSON.stringify(user));
                }
                
                // Show success message
                Utils.showToast(`Welcome back, ${user.fullName}!`, 'success');
                
                // Hide loading and show dashboard
                Utils.hideLoading();
                this.showDashboard();
                
            } else {
                throw new Error('Invalid credentials or user type mismatch');
            }
            
        } catch (error) {
            Utils.hideLoading();
            Utils.showToast(error.message || 'Login failed. Please try again.', 'error');
        }
    }
    
    generateToken() {
        return 'mock_token_' + Date.now();
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
        switch(userType) {
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
                this.loadReceptionModule(moduleContainer);
                break;
            case 'accountant':
                this.loadAccountantModule(moduleContainer);
                break;
            case 'patient':
                this.loadPatientModule(moduleContainer);
                break;
            default:
                this.loadAdminModule(moduleContainer);
        }
    }
    
    loadAdminModule(container) {
        // Admin module will be loaded from admin.js
        if (window.AdminModule) {
            window.AdminModule.init(container);
        } else {
            // Fallback content
            container.innerHTML = `
                <div class="welcome-message">
                    <h2>Welcome, Admin</h2>
                    <p>Hospital Management System Admin Dashboard</p>
                    <div class="card-grid">
                        <div class="card">
                            <div class="card-header">
                                <i class="fas fa-users"></i>
                                <h3>User Management</h3>
                            </div>
                            <div class="card-body">
                                <p>Manage all system users, roles, and permissions.</p>
                            </div>
                        </div>
                        <!-- Add more cards -->
                    </div>
                </div>
            `;
        }
    }
    
    // Similar methods for other user types...
    
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