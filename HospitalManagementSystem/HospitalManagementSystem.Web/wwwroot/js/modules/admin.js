// Admin Dashboard Module
class AdminModule {
    constructor(container) {
        this.container = container;
        this.currentView = 'dashboard';
        this.data = {
            users: [],
            patients: [],
            doctors: [],
            departments: [],
            bills: [],
            payments: []
        };
    }
    
    init(container) {
        this.container = container || this.container;
        this.renderSidebar();
        this.loadDashboard();
        this.loadData();
    }
    
    renderSidebar() {
        const sidebarMenu = document.getElementById('sidebarMenu');
        if (!sidebarMenu) return;
        
        sidebarMenu.innerHTML = `
            <div class="menu-group">
                <div class="menu-group-title">Main</div>
                <a href="#" class="menu-item active" data-view="dashboard">
                    <i class="fas fa-tachometer-alt"></i>
                    <span>Dashboard</span>
                </a>
            </div>
            
            <div class="menu-group">
                <div class="menu-group-title">User Management</div>
                <a href="#" class="menu-item" data-view="users">
                    <i class="fas fa-users"></i>
                    <span>All Users</span>
                    <span class="menu-badge">${this.data.users.length}</span>
                </a>
                <a href="#" class="menu-item" data-view="addUser">
                    <i class="fas fa-user-plus"></i>
                    <span>Add New User</span>
                </a>
                <a href="#" class="menu-item" data-view="userActivity">
                    <i class="fas fa-history"></i>
                    <span>User Activity</span>
                </a>
            </div>
            
            <div class="menu-group">
                <div class="menu-group-title">Doctor Management</div>
                <a href="#" class="menu-item" data-view="doctors">
                    <i class="fas fa-user-md"></i>
                    <span>All Doctors</span>
                    <span class="menu-badge">${this.data.doctors.length}</span>
                </a>
                <a href="#" class="menu-item" data-view="addDoctor">
                    <i class="fas fa-stethoscope"></i>
                    <span>Add Doctor</span>
                </a>
                <a href="#" class="menu-item" data-view="doctorSchedule">
                    <i class="fas fa-calendar-alt"></i>
                    <span>Doctor Schedule</span>
                </a>
            </div>
            
            <div class="menu-group">
                <div class="menu-group-title">Patient Management</div>
                <a href="#" class="menu-item" data-view="patients">
                    <i class="fas fa-user-injured"></i>
                    <span>All Patients</span>
                    <span class="menu-badge">${this.data.patients.length}</span>
                </a>
                <a href="#" class="menu-item" data-view="addPatient">
                    <i class="fas fa-plus-circle"></i>
                    <span>Register Patient</span>
                </a>
                <a href="#" class="menu-item" data-view="patientRecords">
                    <i class="fas fa-file-medical"></i>
                    <span>Medical Records</span>
                </a>
            </div>
            
            <div class="menu-group">
                <div class="menu-group-title">Department & Rooms</div>
                <a href="#" class="menu-item" data-view="departments">
                    <i class="fas fa-building"></i>
                    <span>Departments</span>
                </a>
                <a href="#" class="menu-item" data-view="rooms">
                    <i class="fas fa-bed"></i>
                    <span>Room Management</span>
                </a>
                <a href="#" class="menu-item" data-view="inventory">
                    <i class="fas fa-boxes"></i>
                    <span>Inventory</span>
                </a>
            </div>
            
            <div class="menu-group">
                <div class="menu-group-title">Finance & Billing</div>
                <a href="#" class="menu-item" data-view="billing">
                    <i class="fas fa-file-invoice-dollar"></i>
                    <span>Billing</span>
                </a>
                <a href="#" class="menu-item" data-view="payments">
                    <i class="fas fa-credit-card"></i>
                    <span>Payments</span>
                </a>
                <a href="#" class="menu-item" data-view="financialReports">
                    <i class="fas fa-chart-line"></i>
                    <span>Financial Reports</span>
                </a>
            </div>
            
            <div class="menu-group">
                <div class="menu-group-title">System</div>
                <a href="#" class="menu-item" data-view="reports">
                    <i class="fas fa-chart-bar"></i>
                    <span>System Reports</span>
                </a>
                <a href="#" class="menu-item" data-view="auditLogs">
                    <i class="fas fa-clipboard-list"></i>
                    <span>Audit Logs</span>
                </a>
                <a href="#" class="menu-item" data-view="backup">
                    <i class="fas fa-database"></i>
                    <span>Backup & Restore</span>
                </a>
                <a href="#" class="menu-item" data-view="settings">
                    <i class="fas fa-cog"></i>
                    <span>Settings</span>
                </a>
            </div>
        `;
        
        // Add click events
        sidebarMenu.querySelectorAll('.menu-item').forEach(item => {
            item.addEventListener('click', (e) => {
                e.preventDefault();
                
                // Update active item
                sidebarMenu.querySelectorAll('.menu-item').forEach(i => i.classList.remove('active'));
                item.classList.add('active');
                
                // Load view
                const view = item.dataset.view;
                this.loadView(view);
            });
        });
    }
    
    async loadData() {
        try {
            // Mock API calls
            this.data = {
                users: await this.fetchUsers(),
                patients: await this.fetchPatients(),
                doctors: await this.fetchDoctors(),
                departments: await this.fetchDepartments(),
                bills: await this.fetchBills(),
                payments: await this.fetchPayments()
            };
            
            
            this.renderSidebar();
            
        } catch (error) {
            console.error('Error loading data:', error);
            Utils.showToast('Failed to load data', 'error');
        }
    }
    
    async fetchUsers() {
        // Mock data
        return [
            { id: 1, username: 'admin', fullName: 'System Admin', role: 'Admin', status: 'Active', email: 'admin@hospital.com', createdAt: '2024-01-01' },
            { id: 2, username: 'dr.smith', fullName: 'Dr. John Smith', role: 'Doctor', status: 'Active', email: 'smith@hospital.com', createdAt: '2024-01-02' },
            { id: 3, username: 'nurse.jane', fullName: 'Jane Williams', role: 'Nurse', status: 'Active', email: 'jane@hospital.com', createdAt: '2024-01-03' },
            { id: 4, username: 'reception', fullName: 'Sarah Johnson', role: 'Receptionist', status: 'Active', email: 'sarah@hospital.com', createdAt: '2024-01-04' },
            { id: 5, username: 'accountant', fullName: 'Mike Johnson', role: 'Accountant', status: 'Pending', email: 'mike@hospital.com', createdAt: '2024-01-05' }
        ];
    }
    
    async fetchPatients() {
        return [
            { id: 1, fullName: 'John Doe', age: 45, gender: 'Male', bloodType: 'O+', room: '101', status: 'Active' },
            { id: 2, fullName: 'Jane Smith', age: 32, gender: 'Female', bloodType: 'A-', room: '102', status: 'Active' },
            { id: 3, fullName: 'Robert Johnson', age: 58, gender: 'Male', bloodType: 'B+', room: 'ICU-01', status: 'Critical' }
        ];
    }
    
    // Similar methods for other data...
    
    loadView(view) {
        this.currentView = view;
        
        switch(view) {
            case 'dashboard':
                this.loadDashboard();
                break;
            case 'users':
                this.loadUsersView();
                break;
            case 'addUser':
                this.loadAddUserView();
                break;
            case 'doctors':
                this.loadDoctorsView();
                break;
            case 'patients':
                this.loadPatientsView();
                break;
            case 'departments':
                this.loadDepartmentsView();
                break;
            case 'rooms':
                this.loadRoomsView();
                break;
            case 'billing':
                this.loadBillingView();
                break;
            case 'payments':
                this.loadPaymentsView();
                break;
            case 'reports':
                this.loadReportsView();
                break;
            case 'auditLogs':
                this.loadAuditLogsView();
                break;
            case 'settings':
                this.loadSettingsView();
                break;
            default:
                this.loadDashboard();
        }
    }
    
    loadDashboard() {
        this.container.innerHTML = `
            <div class="dashboard-header">
                <h2>Admin Dashboard</h2>
                <div class="dashboard-stats">
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-users"></i>
                        </div>
                        <div class="stat-content">
                            <h3>${this.data.users.length}</h3>
                            <p>Total Users</p>
                        </div>
                    </div>
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-user-injured"></i>
                        </div>
                        <div class="stat-content">
                            <h3>${this.data.patients.length}</h3>
                            <p>Active Patients</p>
                        </div>
                    </div>
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-user-md"></i>
                        </div>
                        <div class="stat-content">
                            <h3>${this.data.doctors.length}</h3>
                            <p>Doctors</p>
                        </div>
                    </div>
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-hospital-alt"></i>
                        </div>
                        <div class="stat-content">
                            <h3>${this.data.departments.length}</h3>
                            <p>Departments</p>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="dashboard-content">
                <div class="row">
                    <div class="col-md-8">
                        <div class="card">
                            <div class="card-header">
                                <h3>Recent Activity</h3>
                            </div>
                            <div class="card-body">
                                <div class="activity-list">
                                    ${this.getRecentActivity()}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card">
                            <div class="card-header">
                                <h3>Quick Actions</h3>
                            </div>
                            <div class="card-body">
                                <div class="quick-actions">
                                    <button class="btn-action" onclick="AdminModule.loadView('addUser')">
                                        <i class="fas fa-user-plus"></i>
                                        <span>Add New User</span>
                                    </button>
                                    <button class="btn-action" onclick="AdminModule.loadView('addPatient')">
                                        <i class="fas fa-plus-circle"></i>
                                        <span>Register Patient</span>
                                    </button>
                                    <button class="btn-action" onclick="AdminModule.loadView('addDoctor')">
                                        <i class="fas fa-stethoscope"></i>
                                        <span>Add Doctor</span>
                                    </button>
                                    <button class="btn-action" onclick="AdminModule.loadView('billing')">
                                        <i class="fas fa-file-invoice"></i>
                                        <span>Create Bill</span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="row mt-4">
                    <div class="col-md-6">
                        <div class="card">
                            <div class="card-header">
                                <h3>Recent Users</h3>
                            </div>
                            <div class="card-body">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Username</th>
                                            <th>Name</th>
                                            <th>Role</th>
                                            <th>Status</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        ${this.data.users.slice(0, 5).map(user => `
                                            <tr>
                                                <td>${user.username}</td>
                                                <td>${user.fullName}</td>
                                                <td>${user.role}</td>
                                                <td>
                                                    <span class="status-badge status-${user.status.toLowerCase()}">
                                                        ${user.status}
                                                    </span>
                                                </td>
                                            </tr>
                                        `).join('')}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col-md-6">
                        <div class="card">
                            <div class="card-header">
                                <h3>System Status</h3>
                            </div>
                            <div class="card-body">
                                <div class="system-status-list">
                                    <div class="status-item">
                                        <span class="status-label">Database</span>
                                        <span class="status-value online">Online</span>
                                    </div>
                                    <div class="status-item">
                                        <span class="status-label">API Server</span>
                                        <span class="status-value online">Online</span>
                                    </div>
                                    <div class="status-item">
                                        <span class="status-label">Backup System</span>
                                        <span class="status-value online">Online</span>
                                    </div>
                                    <div class="status-item">
                                        <span class="status-label">Security</span>
                                        <span class="status-value online">Active</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }
    
    loadUsersView() {
        this.container.innerHTML = `
            <div class="view-header">
                <h2>User Management</h2>
                <button class="btn btn-primary" onclick="AdminModule.loadView('addUser')">
                    <i class="fas fa-user-plus"></i> Add New User
                </button>
            </div>
            
            <div class="card">
                <div class="card-header">
                    <h3>All Registered Users</h3>
                    <div class="table-actions">
                        <div class="search-box">
                            <input type="text" placeholder="Search users..." id="userSearch">
                            <i class="fas fa-search"></i>
                        </div>
                        <button class="btn btn-secondary">
                            <i class="fas fa-filter"></i> Filter
                        </button>
                        <button class="btn btn-secondary">
                            <i class="fas fa-download"></i> Export
                        </button>
                    </div>
                </div>
                <div class="card-body">
                    <table class="table">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Username</th>
                                <th>Full Name</th>
                                <th>Email</th>
                                <th>Role</th>
                                <th>Department</th>
                                <th>Status</th>
                                <th>Created</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${this.data.users.map(user => `
                                <tr>
                                    <td>${user.id}</td>
                                    <td><strong>${user.username}</strong></td>
                                    <td>${user.fullName}</td>
                                    <td>${user.email}</td>
                                    <td>
                                        <span class="role-badge role-${user.role.toLowerCase()}">
                                            ${user.role}
                                        </span>
                                    </td>
                                    <td>${user.department || '-'}</td>
                                    <td>
                                        <span class="status-badge status-${user.status.toLowerCase()}">
                                            ${user.status}
                                        </span>
                                    </td>
                                    <td>${Utils.formatDate(user.createdAt)}</td>
                                    <td>
                                        <div class="action-buttons">
                                            <button class="btn-icon" title="Edit" onclick="AdminModule.editUser(${user.id})">
                                                <i class="fas fa-edit"></i>
                                            </button>
                                            <button class="btn-icon" title="View Details" onclick="AdminModule.viewUser(${user.id})">
                                                <i class="fas fa-eye"></i>
                                            </button>
                                            <button class="btn-icon" title="Reset Password" onclick="AdminModule.resetPassword('${user.username}')">
                                                <i class="fas fa-key"></i>
                                            </button>
                                            <button class="btn-icon btn-danger" title="Deactivate" onclick="AdminModule.toggleUserStatus('${user.username}', '${user.status}')">
                                                <i class="fas ${user.status === 'Active' ? 'fa-user-slash' : 'fa-user-check'}"></i>
                                            </button>
                                        </div>
                                    </td>
                                </tr>
                            `).join('')}
                        </tbody>
                    </table>
                    
                    <div class="table-footer">
                        <div class="pagination">
                            <button class="btn-pagination disabled">Previous</button>
                            <span class="page-info">Page 1 of 1</span>
                            <button class="btn-pagination disabled">Next</button>
                        </div>
                        <div class="table-summary">
                            Showing ${this.data.users.length} of ${this.data.users.length} users
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="row mt-4">
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-header">
                            <h3>User Statistics</h3>
                        </div>
                        <div class="card-body">
                            <div class="stats-grid">
                                <div class="stat-item">
                                    <div class="stat-value">${this.data.users.filter(u => u.role === 'Admin').length}</div>
                                    <div class="stat-label">Admins</div>
                                </div>
                                <div class="stat-item">
                                    <div class="stat-value">${this.data.users.filter(u => u.role === 'Doctor').length}</div>
                                    <div class="stat-label">Doctors</div>
                                </div>
                                <div class="stat-item">
                                    <div class="stat-value">${this.data.users.filter(u => u.role === 'Nurse').length}</div>
                                    <div class="stat-label">Nurses</div>
                                </div>
                                <div class="stat-item">
                                    <div class="stat-value">${this.data.users.filter(u => u.role === 'Patient').length}</div>
                                    <div class="stat-label">Patients</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-header">
                            <h3>Status Distribution</h3>
                        </div>
                        <div class="card-body">
                            <div class="status-distribution">
                                <div class="status-item">
                                    <span class="status-dot active"></span>
                                    <span class="status-label">Active</span>
                                    <span class="status-count">${this.data.users.filter(u => u.status === 'Active').length}</span>
                                </div>
                                <div class="status-item">
                                    <span class="status-dot pending"></span>
                                    <span class="status-label">Pending</span>
                                    <span class="status-count">${this.data.users.filter(u => u.status === 'Pending').length}</span>
                                </div>
                                <div class="status-item">
                                    <span class="status-dot inactive"></span>
                                    <span class="status-label">Inactive</span>
                                    <span class="status-count">${this.data.users.filter(u => u.status === 'Inactive').length}</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        // Add search functionality
        const searchInput = document.getElementById('userSearch');
        if (searchInput) {
            searchInput.addEventListener('input', (e) => {
                const searchTerm = e.target.value.toLowerCase();
                const rows = document.querySelectorAll('tbody tr');
                
                rows.forEach(row => {
                    const text = row.textContent.toLowerCase();
                    row.style.display = text.includes(searchTerm) ? '' : 'none';
                });
            });
        }
    }
    
    loadAddUserView() {
        this.container.innerHTML = `
            <div class="view-header">
                <h2>Add New User</h2>
                <button class="btn btn-secondary" onclick="AdminModule.loadView('users')">
                    <i class="fas fa-arrow-left"></i> Back to Users
                </button>
            </div>
            
            <div class="card">
                <div class="card-header">
                    <h3>User Registration Form</h3>
                    <p>Fill in all required details to create a new user account</p>
                </div>
                <div class="card-body">
                    <form id="addUserForm" class="form-container">
                        <div class="form-row">
                            <div class="form-group">
                                <label for="userType">User Type *</label>
                                <select id="userType" class="form-control" required>
                                    <option value="">Select User Type</option>
                                    <option value="Admin">Administrator</option>
                                    <option value="Doctor">Doctor</option>
                                    <option value="Nurse">Nurse</option>
                                    <option value="Receptionist">Receptionist</option>
                                    <option value="Accountant">Accountant</option>
                                    <option value="Patient">Patient</option>
                                </select>
                            </div>
                            
                            <div class="form-group">
                                <label for="username">Username *</label>
                                <input type="text" id="username" class="form-control" required 
                                       placeholder="Enter unique username">
                                <small class="form-text">Must be 3-20 characters, letters and numbers only</small>
                            </div>
                        </div>
                        
                        <div class="form-row">
                            <div class="form-group">
                                <label for="fullName">Full Name *</label>
                                <input type="text" id="fullName" class="form-control" required 
                                       placeholder="Enter full name">
                            </div>
                            
                            <div class="form-group">
                                <label for="email">Email Address *</label>
                                <input type="email" id="email" class="form-control" required 
                                       placeholder="Enter email address">
                            </div>
                        </div>
                        
                        <div class="form-row">
                            <div class="form-group">
                                <label for="password">Password *</label>
                                <input type="password" id="password" class="form-control" required 
                                       placeholder="Enter password">
                                <small class="form-text">Minimum 8 characters with letters, numbers, and symbols</small>
                            </div>
                            
                            <div class="form-group">
                                <label for="confirmPassword">Confirm Password *</label>
                                <input type="password" id="confirmPassword" class="form-control" required 
                                       placeholder="Confirm password">
                            </div>
                        </div>
                        
                        <!-- Dynamic fields based on user type -->
                        <div id="dynamicFields"></div>
                        
                        <div class="form-row">
                            <div class="form-group">
                                <label for="phone">Phone Number</label>
                                <input type="tel" id="phone" class="form-control" 
                                       placeholder="Enter phone number">
                            </div>
                            
                            <div class="form-group">
                                <label for="status">Account Status</label>
                                <select id="status" class="form-control">
                                    <option value="Active">Active</option>
                                    <option value="Pending">Pending (Requires Approval)</option>
                                    <option value="Inactive">Inactive</option>
                                </select>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label for="notes">Notes</label>
                            <textarea id="notes" class="form-control" rows="3" 
                                      placeholder="Any additional notes..."></textarea>
                        </div>
                        
                        <div class="form-buttons">
                            <button type="button" class="btn btn-secondary" onclick="AdminModule.loadView('users')">
                                Cancel
                            </button>
                            <button type="submit" class="btn btn-primary">
                                <i class="fas fa-save"></i> Create User
                            </button>
                        </div>
                    </form>
                </div>
            </div>
            
            <div class="card mt-4">
                <div class="card-header">
                    <h3>User Creation Guidelines</h3>
                </div>
                <div class="card-body">
                    <ul class="guidelines">
                        <li><i class="fas fa-check-circle"></i> All fields marked with * are required</li>
                        <li><i class="fas fa-check-circle"></i> Usernames must be unique across the system</li>
                        <li><i class="fas fa-check-circle"></i> Email addresses will be used for notifications</li>
                        <li><i class="fas fa-check-circle"></i> Temporary passwords will be sent via email</li>
                        <li><i class="fas fa-check-circle"></i> Users can change their password after first login</li>
                    </ul>
                </div>
            </div>
        `;
        

        document.getElementById('userType').addEventListener('change', (e) => {
            this.updateDynamicFields(e.target.value);
        });
    
        document.getElementById('addUserForm').addEventListener('submit', (e) => {
            e.preventDefault();
            this.submitUserForm();
        });
    }
    
    updateDynamicFields(userType) {
        const container = document.getElementById('dynamicFields');
        let html = '';
        
        switch(userType) {
            case 'Doctor':
                html = `
                    <div class="form-row">
                        <div class="form-group">
                            <label for="specialization">Specialization *</label>
                            <select id="specialization" class="form-control" required>
                                <option value="">Select Specialization</option>
                                ${CONFIG.SPECIALIZATIONS.map(spec => 
                                    `<option value="${spec}">${spec}</option>`
                                ).join('')}
                            </select>
                        </div>
                        
                        <div class="form-group">
                            <label for="department">Department *</label>
                            <select id="department" class="form-control" required>
                                <option value="">Select Department</option>
                                ${CONFIG.DEPARTMENTS.map(dept => 
                                    `<option value="${dept}">${dept}</option>`
                                ).join('')}
                            </select>
                        </div>
                    </div>
                    
                    <div class="form-row">
                        <div class="form-group">
                            <label for="licenseNumber">License Number *</label>
                            <input type="text" id="licenseNumber" class="form-control" required 
                                   placeholder="Enter medical license number">
                        </div>
                        
                        <div class="form-group">
                            <label for="consultationFee">Consultation Fee</label>
                            <input type="number" id="consultationFee" class="form-control" 
                                   placeholder="Enter fee amount" step="0.01">
                        </div>
                    </div>
                `;
                break;
                
            case 'Nurse':
                html = `
                    <div class="form-row">
                        <div class="form-group">
                            <label for="nurseDepartment">Department *</label>
                            <select id="nurseDepartment" class="form-control" required>
                                <option value="">Select Department</option>
                                ${CONFIG.DEPARTMENTS.map(dept => 
                                    `<option value="${dept}">${dept}</option>`
                                ).join('')}
                            </select>
                        </div>
                        
                        <div class="form-group">
                            <label for="shift">Preferred Shift</label>
                            <select id="shift" class="form-control">
                                <option value="Morning">Morning Shift (7AM-3PM)</option>
                                <option value="Evening">Evening Shift (3PM-11PM)</option>
                                <option value="Night">Night Shift (11PM-7AM)</option>
                            </select>
                        </div>
                    </div>
                `;
                break;
                
            case 'Patient':
                html = `
                    <div class="form-row">
                        <div class="form-group">
                            <label for="patientBloodType">Blood Type</label>
                            <select id="patientBloodType" class="form-control">
                                <option value="">Select Blood Type</option>
                                <option value="A+">A+</option>
                                <option value="A-">A-</option>
                                <option value="B+">B+</option>
                                <option value="B-">B-</option>
                                <option value="O+">O+</option>
                                <option value="O-">O-</option>
                                <option value="AB+">AB+</option>
                                <option value="AB-">AB-</option>
                            </select>
                        </div>
                        
                        <div class="form-group">
                            <label for="emergencyContact">Emergency Contact</label>
                            <input type="text" id="emergencyContact" class="form-control" 
                                   placeholder="Emergency contact name">
                        </div>
                    </div>
                `;
                break;
        }
        
        container.innerHTML = html;
    }
    
    async submitUserForm() {
        const form = document.getElementById('addUserForm');
        const formData = new FormData(form);
        
        // Validate passwords match
        const password = document.getElementById('password').value;
        const confirmPassword = document.getElementById('confirmPassword').value;
        
        if (password !== confirmPassword) {
            Utils.showToast('Passwords do not match', 'error');
            return;
        }
        
        try {
            Utils.showLoading();
            
            // Mock API call
            await new Promise(resolve => setTimeout(resolve, 1500));
            
            // Simulate successful creation
            const newUser = {
                id: this.data.users.length + 1,
                username: document.getElementById('username').value,
                fullName: document.getElementById('fullName').value,
                email: document.getElementById('email').value,
                role: document.getElementById('userType').value,
                status: document.getElementById('status').value,
                createdAt: new Date().toISOString().split('T')[0]
            };
            
            this.data.users.push(newUser);
            
            Utils.showToast(`User ${newUser.username} created successfully!`, 'success');
            
            // Update UI
            this.renderSidebar();
            
            // Go back to users list
            setTimeout(() => {
                this.loadUsersView();
            }, 1000);
            
        } catch (error) {
            Utils.showToast('Failed to create user: ' + error.message, 'error');
        } finally {
            Utils.hideLoading();
        }
    }
    
    // Add other view methods similarly...
    
    async editUser(userId) {
        // Implementation for editing user
        Utils.showToast('Edit user feature coming soon', 'info');
    }
    
    async viewUser(userId) {
        // Implementation for viewing user details
        Utils.showToast('View user feature coming soon', 'info');
    }
    
    async resetPassword(username) {
        const confirmed = await Utils.confirm(`Reset password for user "${username}"?`);
        if (!confirmed) return;
        
        try {
            Utils.showLoading();
            
            // Mock API call
            await new Promise(resolve => setTimeout(resolve, 1000));
            
            // Generate temporary password
            const tempPassword = 'Temp@' + Math.random().toString(36).slice(2, 8);
            
            Utils.showToast(`Password reset successful! Temporary password: ${tempPassword}`, 'success');
            
            // Show password in modal
            const modal = document.createElement('div');
            modal.className = 'modal-overlay active';
            modal.innerHTML = `
                <div class="modal">
                    <div class="modal-header">
                        <h3>Password Reset Complete</h3>
                        <button class="modal-close">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="alert alert-info">
                            <i class="fas fa-info-circle"></i>
                            <p>The password has been reset for user <strong>${username}</strong>.</p>
                        </div>
                        
                        <div class="password-display">
                            <label>Temporary Password:</label>
                            <div class="password-box">
                                <code>${tempPassword}</code>
                                <button class="btn-copy" onclick="navigator.clipboard.writeText('${tempPassword}')">
                                    <i class="fas fa-copy"></i> Copy
                                </button>
                            </div>
                            <small>This password will expire in 24 hours. Please inform the user to change it immediately.</small>
                        </div>
                        
                        <div class="email-option">
                            <label>
                                <input type="checkbox" id="sendEmail" checked>
                                Send password reset email to ${username}
                            </label>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-secondary" onclick="this.closest('.modal-overlay').remove()">
                            Close
                        </button>
                        <button class="btn btn-primary" onclick="this.closest('.modal-overlay').remove()">
                            <i class="fas fa-envelope"></i> Send Email
                        </button>
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
            
        } catch (error) {
            Utils.showToast('Failed to reset password', 'error');
        } finally {
            Utils.hideLoading();
        }
    }
    
    async toggleUserStatus(username, currentStatus) {
        const newStatus = currentStatus === 'Active' ? 'Inactive' : 'Active';
        const action = currentStatus === 'Active' ? 'deactivate' : 'activate';
        
        const confirmed = await Utils.confirm(
            `Are you sure you want to ${action} user "${username}"?`
        );
        
        if (!confirmed) return;
        
        try {
            Utils.showLoading();
            
            // Mock API call
            await new Promise(resolve => setTimeout(resolve, 1000));
            
            // Update user in data
            const user = this.data.users.find(u => u.username === username);
            if (user) {
                user.status = newStatus;
            }
            
            Utils.showToast(`User ${username} ${action}d successfully!`, 'success');
            
            // Refresh the view
            this.loadUsersView();
            
        } catch (error) {
            Utils.showToast('Failed to update user status', 'error');
        } finally {
            Utils.hideLoading();
        }
    }
    
    getRecentActivity() {
        return `
            <div class="activity-item">
                <div class="activity-icon">
                    <i class="fas fa-user-plus"></i>
                </div>
                <div class="activity-content">
                    <p>New user registered: <strong>accountant.mike</strong></p>
                    <span class="activity-time">10 minutes ago</span>
                </div>
            </div>
            <div class="activity-item">
                <div class="activity-icon">
                    <i class="fas fa-file-invoice"></i>
                </div>
                <div class="activity-content">
                    <p>Bill #B2024001 created for John Doe</p>
                    <span class="activity-time">1 hour ago</span>
                </div>
            </div>
            <div class="activity-item">
                <div class="activity-icon">
                    <i class="fas fa-user-check"></i>
                </div>
                <div class="activity-content">
                    <p>Doctor account activated: Dr. Sarah Jones</p>
                    <span class="activity-time">2 hours ago</span>
                </div>
            </div>
            <div class="activity-item">
                <div class="activity-icon">
                    <i class="fas fa-database"></i>
                </div>
                <div class="activity-content">
                    <p>System backup completed successfully</p>
                    <span class="activity-time">Yesterday, 3:45 PM</span>
                </div>
            </div>
        `;
    }
}

// Make it available globally
window.AdminModule = new AdminModule();