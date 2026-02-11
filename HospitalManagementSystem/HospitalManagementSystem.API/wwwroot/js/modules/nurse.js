// Nurse Dashboard Module
class NurseModule {
    constructor() {
        this.api = new ApiService();
        this.currentUser = JSON.parse(localStorage.getItem('user') || '{}');
        this.currentView = 'dashboard';
        this.nurseData = {
            patients: [],
            rooms: [],
            inventory: [],
            shiftReports: []
        };
    }

    init(container) {
        this.container = container;
        this.loadNurseDashboard();
        this.loadNurseData();
    }

    async loadNurseData() {
        try {
            // Load patients
            this.nurseData.patients = await this.api.request('/patients') || [];

            // Load rooms
            this.nurseData.rooms = await this.api.request('/rooms') || [];

            // Load inventory
            this.nurseData.inventory = await this.api.request('/inventory') || [];

            // Load shift reports
            this.nurseData.shiftReports = await this.api.request(`/nurse/shift-reports/${this.currentUser.id}`) || [];

        } catch (error) {
            console.error('Failed to load nurse data:', error);
            this.loadMockData();
        }
    }

    loadMockData() {
        // Mock patients
        this.nurseData.patients = [
            { patientId: 1, fullName: 'John Doe', roomNumber: '101', status: 'Stable' },
            { patientId: 2, fullName: 'Jane Smith', roomNumber: '102', status: 'Recovering' },
            { patientId: 3, fullName: 'Robert Johnson', roomNumber: 'ICU-01', status: 'Critical' }
        ];

        // Mock rooms
        this.nurseData.rooms = [
            { roomId: 1, roomNumber: '101', roomType: 'General', status: 'Occupied', availableBeds: 0, patientName: 'John Doe' },
            { roomId: 2, roomNumber: '102', roomType: 'General', status: 'Occupied', availableBeds: 0, patientName: 'Jane Smith' },
            { roomId: 3, roomNumber: '103', roomType: 'General', status: 'Available', availableBeds: 2 },
            { roomId: 4, roomNumber: 'ICU-01', roomType: 'ICU', status: 'Occupied', availableBeds: 0, patientName: 'Robert Johnson' }
        ];

        // Mock inventory
        this.nurseData.inventory = [
            { itemId: 1, itemName: 'Band-Aids', currentStock: 150, minimumStock: 200, status: 'Low Stock' },
            { itemId: 2, itemName: 'Gauze Pads', currentStock: 300, minimumStock: 250, status: 'Good' },
            { itemId: 3, itemName: 'Syringes (5ml)', currentStock: 500, minimumStock: 400, status: 'Good' }
        ];

        // Mock shift reports
        this.nurseData.shiftReports = [
            { reportId: 1, shiftDate: '2024-01-15', shiftType: 'Morning', isCompleted: true },
            { reportId: 2, shiftDate: '2024-01-16', shiftType: 'Evening', isCompleted: false }
        ];
    }

    // Main Dashboard
    async loadNurseDashboard() {
        this.container.innerHTML = `
            <div class="nurse-dashboard">
                <div class="view-header">
                    <h2>Nurse Dashboard</h2>
                    <div class="user-info">
                        <span class="badge badge-primary">${this.currentUser.department || 'General'} Department</span>
                    </div>
                </div>

                <div class="welcome-message">
                    <h3>Welcome, Nurse ${this.currentUser.fullName}</h3>
                    <p>Your shift: <strong>Morning (7:00 AM - 3:00 PM)</strong></p>
                </div>

                <div class="quick-stats">
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-user-injured"></i>
                        </div>
                        <div class="stat-content">
                            <h3>${this.nurseData.patients.length}</h3>
                            <p>My Patients</p>
                        </div>
                    </div>
                    
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-bed"></i>
                        </div>
                        <div class="stat-content">
                            <h3>${this.nurseData.rooms.filter(r => r.status === 'Occupied').length}</h3>
                            <p>Occupied Rooms</p>
                        </div>
                    </div>
                    
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-exclamation-triangle"></i>
                        </div>
                        <div class="stat-content">
                            <h3>${this.nurseData.inventory.filter(i => i.status === 'Low Stock').length}</h3>
                            <p>Low Stock Items</p>
                        </div>
                    </div>
                    
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-clipboard-list"></i>
                        </div>
                        <div class="stat-content">
                            <h3>${this.nurseData.shiftReports.filter(r => !r.isCompleted).length}</h3>
                            <p>Pending Reports</p>
                        </div>
                    </div>
                </div>

                <div class="dashboard-content">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="card">
                                <div class="card-header">
                                    <h3>Quick Actions</h3>
                                </div>
                                <div class="card-body">
                                    <div class="quick-actions-grid">
                                        <button class="action-btn" onclick="NurseModule.showVitalSigns()">
                                            <i class="fas fa-heartbeat"></i>
                                            <span>Record Vital Signs</span>
                                        </button>
                                        <button class="action-btn" onclick="NurseModule.showMedicationAdmin()">
                                            <i class="fas fa-pills"></i>
                                            <span>Medication Administration</span>
                                        </button>
                                        <button class="action-btn" onclick="NurseModule.showCareTasks()">
                                            <i class="fas fa-tasks"></i>
                                            <span>Patient Care Tasks</span>
                                        </button>
                                        <button class="action-btn" onclick="NurseModule.showRoomManagement()">
                                            <i class="fas fa-bed"></i>
                                            <span>Room Management</span>
                                        </button>
                                        <button class="action-btn" onclick="NurseModule.showShiftReport()">
                                            <i class="fas fa-clipboard"></i>
                                            <span>Shift Report</span>
                                        </button>
                                        <button class="action-btn" onclick="NurseModule.showInventoryCheck()">
                                            <i class="fas fa-boxes"></i>
                                            <span>Inventory Check</span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="col-md-6">
                            <div class="card">
                                <div class="card-header">
                                    <h3>My Patients</h3>
                                    <button class="btn btn-sm btn-primary" onclick="NurseModule.showPatientList()">
                                        View All
                                    </button>
                                </div>
                                <div class="card-body">
                                    <div class="patient-list">
                                        ${this.nurseData.patients.slice(0, 3).map(patient => `
                                            <div class="patient-item">
                                                <div class="patient-info">
                                                    <h4>${patient.fullName}</h4>
                                                    <p>Room: ${patient.roomNumber || 'Not assigned'} | Status: 
                                                        <span class="status-${patient.status?.toLowerCase() || 'stable'}">
                                                            ${patient.status || 'Stable'}
                                                        </span>
                                                    </p>
                                                </div>
                                                <div class="patient-actions">
                                                    <button class="btn-icon" onclick="NurseModule.viewPatientDetails(${patient.patientId})">
                                                        <i class="fas fa-eye"></i>
                                                    </button>
                                                    <button class="btn-icon" onclick="NurseModule.recordVitals(${patient.patientId})">
                                                        <i class="fas fa-heartbeat"></i>
                                                    </button>
                                                </div>
                                            </div>
                                        `).join('')}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    // 6. SHIFT REPORT FEATURE
    async showShiftReport() {
        this.container.innerHTML = `
            <div class="shift-report-view">
                <div class="view-header">
                    <h2><i class="fas fa-clipboard-list"></i> Shift Report</h2>
                    <button class="btn btn-secondary" onclick="NurseModule.loadNurseDashboard()">
                        <i class="fas fa-arrow-left"></i> Back to Dashboard
                    </button>
                </div>

                <div class="card">
                    <div class="card-header">
                        <h3>Create Shift Report</h3>
                        <p>Document your shift activities and handover notes</p>
                    </div>
                    
                    <div class="card-body">
                        <form id="shiftReportForm">
                            <div class="form-row">
                                <div class="form-group">
                                    <label for="shiftDate">Shift Date *</label>
                                    <input type="date" id="shiftDate" class="form-control" 
                                           value="${new Date().toISOString().split('T')[0]}" required>
                                </div>
                                
                                <div class="form-group">
                                    <label for="shiftType">Shift Type *</label>
                                    <select id="shiftType" class="form-control" required>
                                        <option value="Morning">Morning (7:00 AM - 3:00 PM)</option>
                                        <option value="Evening">Evening (3:00 PM - 11:00 PM)</option>
                                        <option value="Night">Night (11:00 PM - 7:00 AM)</option>
                                        <option value="General">General Shift</option>
                                    </select>
                                </div>
                            </div>
                            
                            <div class="form-row">
                                <div class="form-group">
                                    <label for="startTime">Start Time *</label>
                                    <input type="time" id="startTime" class="form-control" 
                                           value="07:00" required>
                                </div>
                                
                                <div class="form-group">
                                    <label for="endTime">End Time *</label>
                                    <input type="time" id="endTime" class="form-control" 
                                           value="15:00" required>
                                </div>
                            </div>
                            
                            <div class="form-group">
                                <label for="patientsHandled">Patients Handled *</label>
                                <textarea id="patientsHandled" class="form-control" rows="3" 
                                          placeholder="List patients you cared for during this shift..." required></textarea>
                                <small class="form-text">Separate with commas or list one per line</small>
                            </div>
                            
                            <div class="form-group">
                                <label for="tasksCompleted">Tasks Completed</label>
                                <textarea id="tasksCompleted" class="form-control" rows="3" 
                                          placeholder="Document tasks completed during shift..."></textarea>
                            </div>
                            
                            <div class="form-group">
                                <label for="medicationsAdministered">Medications Administered</label>
                                <textarea id="medicationsAdministered" class="form-control" rows="3" 
                                          placeholder="List medications administered with dosages..."></textarea>
                            </div>
                            
                            <div class="form-group">
                                <label for="criticalIncidents">Critical Incidents</label>
                                <textarea id="criticalIncidents" class="form-control" rows="3" 
                                          placeholder="Any critical incidents, emergencies, or code situations..."></textarea>
                            </div>
                            
                            <div class="form-group">
                                <label for="issuesConcerns">Issues & Concerns</label>
                                <textarea id="issuesConcerns" class="form-control" rows="3" 
                                          placeholder="Equipment issues, supply shortages, or other concerns..."></textarea>
                            </div>
                            
                            <div class="form-group">
                                <label for="handoverNotes">Handover Notes *</label>
                                <textarea id="handoverNotes" class="form-control" rows="3" 
                                          placeholder="Important information for the next shift..." required></textarea>
                            </div>
                            
                            <div class="form-group">
                                <label for="nextShiftNotes">Notes for Next Shift</label>
                                <textarea id="nextShiftNotes" class="form-control" rows="2" 
                                          placeholder="Tasks pending or follow-up needed..."></textarea>
                            </div>
                            
                            <div class="form-check mb-3">
                                <input type="checkbox" id="isCompleted" class="form-check-input">
                                <label for="isCompleted" class="form-check-label">
                                    Mark as completed and submit
                                </label>
                            </div>
                            
                            <div class="form-buttons">
                                <button type="button" class="btn btn-secondary" onclick="NurseModule.loadNurseDashboard()">
                                    Cancel
                                </button>
                                <button type="submit" class="btn btn-primary">
                                    <i class="fas fa-save"></i> Save Shift Report
                                </button>
                            </div>
                        </form>
                    </div>
                </div>

                <div class="card mt-4">
                    <div class="card-header">
                        <h3>Previous Shift Reports</h3>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th>Date</th>
                                        <th>Shift Type</th>
                                        <th>Patients</th>
                                        <th>Status</th>
                                        <th>Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="previousReports">
                                    <!-- Will be populated by JavaScript -->
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        `;

        // Load previous reports
        await this.loadPreviousReports();

        // Setup form submission
        document.getElementById('shiftReportForm').addEventListener('submit', (e) => {
            e.preventDefault();
            this.submitShiftReport();
        });
    }

    async loadPreviousReports() {
        const reports = this.nurseData.shiftReports;

        const reportsHtml = reports.map(report => `
            <tr>
                <td>${new Date(report.shiftDate).toLocaleDateString()}</td>
                <td>${report.shiftType}</td>
                <td>${report.patientsHandled ? report.patientsHandled.split(',').length : 0}</td>
                <td>
                    <span class="status-badge ${report.isCompleted ? 'status-completed' : 'status-pending'}">
                        ${report.isCompleted ? 'Completed' : 'Draft'}
                    </span>
                </td>
                <td>
                    <button class="btn-icon" onclick="NurseModule.viewShiftReport(${report.reportId})" title="View">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn-icon" onclick="NurseModule.editShiftReport(${report.reportId})" title="Edit">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn-icon btn-danger" onclick="NurseModule.deleteShiftReport(${report.reportId})" title="Delete">
                        <i class="fas fa-trash"></i>
                    </button>
                </td>
            </tr>
        `).join('');

        document.getElementById('previousReports').innerHTML = reportsHtml;
    }

    async submitShiftReport() {
        try {
            const formData = {
                nurseId: this.currentUser.id,
                shiftDate: document.getElementById('shiftDate').value,
                shiftType: document.getElementById('shiftType').value,
                startTime: document.getElementById('startTime').value,
                endTime: document.getElementById('endTime').value,
                patientsHandled: document.getElementById('patientsHandled').value,
                tasksCompleted: document.getElementById('tasksCompleted').value,
                medicationsAdministered: document.getElementById('medicationsAdministered').value,
                criticalIncidents: document.getElementById('criticalIncidents').value,
                issuesConcerns: document.getElementById('issuesConcerns').value,
                handoverNotes: document.getElementById('handoverNotes').value,
                nextShiftNotes: document.getElementById('nextShiftNotes').value,
                isCompleted: document.getElementById('isCompleted').checked
            };

            // Show loading
            this.showLoading('Saving shift report...');

            // Send to API
            const response = await this.api.request('/nurse/shift-reports', {
                method: 'POST',
                body: JSON.stringify(formData)
            });

            this.showToast('Shift report saved successfully!', 'success');

            // Return to dashboard
            setTimeout(() => {
                this.loadNurseDashboard();
            }, 1500);

        } catch (error) {
            console.error('Failed to save shift report:', error);
            this.showToast('Failed to save shift report: ' + error.message, 'error');
        } finally {
            this.hideLoading();
        }
    }

    async viewShiftReport(reportId) {
        try {
            const report = await this.api.request(`/nurse/shift-reports/${reportId}`);

            if (report) {
                this.showReportModal(report);
            }
        } catch (error) {
            // Fallback to mock report
            const mockReport = {
                shiftDate: '2024-01-15',
                shiftType: 'Morning',
                startTime: '07:00',
                endTime: '15:00',
                patientsHandled: 'John Doe (101), Jane Smith (102), Robert Johnson (ICU-01)',
                tasksCompleted: 'Vital signs monitoring, medication administration, wound dressing changes',
                medicationsAdministered: 'Amoxicillin 500mg to John Doe, Ibuprofen 400mg to Jane Smith',
                criticalIncidents: 'None',
                issuesConcerns: 'Low stock of Band-Aids in Room 101 supply cabinet',
                handoverNotes: 'Patient John Doe requires pain medication at 4 PM. Jane Smith needs assistance with walking.',
                nextShiftNotes: 'Check Robert Johnson\'s vitals every hour.',
                isCompleted: true
            };

            this.showReportModal(mockReport);
        }
    }

    showReportModal(report) {
        const modal = document.createElement('div');
        modal.className = 'modal-overlay active';
        modal.innerHTML = `
            <div class="modal" style="max-width: 800px;">
                <div class="modal-header">
                    <h3>Shift Report Details</h3>
                    <button class="modal-close" onclick="this.closest('.modal-overlay').remove()">&times;</button>
                </div>
                <div class="modal-body">
                    <div class="report-details">
                        <div class="report-header">
                            <div class="report-info">
                                <h4>${report.shiftType} Shift</h4>
                                <p>Date: ${new Date(report.shiftDate).toLocaleDateString()} | 
                                   Time: ${report.startTime} - ${report.endTime}</p>
                            </div>
                            <span class="status-badge ${report.isCompleted ? 'status-completed' : 'status-pending'}">
                                ${report.isCompleted ? 'Completed' : 'Draft'}
                            </span>
                        </div>
                        
                        <div class="report-section">
                            <h5><i class="fas fa-user-injured"></i> Patients Handled</h5>
                            <p>${report.patientsHandled || 'Not specified'}</p>
                        </div>
                        
                        <div class="report-section">
                            <h5><i class="fas fa-tasks"></i> Tasks Completed</h5>
                            <p>${report.tasksCompleted || 'Not specified'}</p>
                        </div>
                        
                        <div class="report-section">
                            <h5><i class="fas fa-pills"></i> Medications Administered</h5>
                            <p>${report.medicationsAdministered || 'Not specified'}</p>
                        </div>
                        
                        <div class="report-section">
                            <h5><i class="fas fa-exclamation-triangle"></i> Critical Incidents</h5>
                            <p>${report.criticalIncidents || 'None reported'}</p>
                        </div>
                        
                        <div class="report-section">
                            <h5><i class="fas fa-exclamation-circle"></i> Issues & Concerns</h5>
                            <p>${report.issuesConcerns || 'None reported'}</p>
                        </div>
                        
                        <div class="report-section">
                            <h5><i class="fas fa-exchange-alt"></i> Handover Notes</h5>
                            <p>${report.handoverNotes || 'Not specified'}</p>
                        </div>
                        
                        <div class="report-section">
                            <h5><i class="fas fa-forward"></i> Notes for Next Shift</h5>
                            <p>${report.nextShiftNotes || 'Not specified'}</p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" onclick="this.closest('.modal-overlay').remove()">
                        Close
                    </button>
                    <button class="btn btn-primary" onclick="NurseModule.printReport(${JSON.stringify(report).replace(/"/g, '&quot;')})">
                        <i class="fas fa-print"></i> Print Report
                    </button>
                </div>
            </div>
        `;

        document.body.appendChild(modal);
    }

    // 4. ROOM MANAGEMENT FEATURE
    async showRoomManagement() {
        this.container.innerHTML = `
            <div class="room-management-view">
                <div class="view-header">
                    <h2><i class="fas fa-bed"></i> Room & Bed Management</h2>
                    <button class="btn btn-secondary" onclick="NurseModule.loadNurseDashboard()">
                        <i class="fas fa-arrow-left"></i> Back to Dashboard
                    </button>
                </div>

                <div class="card">
                    <div class="card-header">
                        <h3>Room Status Overview</h3>
                        <div class="room-stats">
                            <span class="stat-badge status-available">Available: 
                                ${this.nurseData.rooms.filter(r => r.status === 'Available').length}
                            </span>
                            <span class="stat-badge status-occupied">Occupied: 
                                ${this.nurseData.rooms.filter(r => r.status === 'Occupied').length}
                            </span>
                            <span class="stat-badge status-maintenance">Maintenance: 
                                ${this.nurseData.rooms.filter(r => r.status === 'Maintenance').length}
                            </span>
                        </div>
                    </div>
                    
                    <div class="card-body">
                        <div class="room-management-actions">
                            <div class="action-buttons">
                                <button class="btn btn-primary" onclick="NurseModule.showChangeRoomStatus()">
                                    <i class="fas fa-exchange-alt"></i> Change Room Status
                                </button>
                                <button class="btn btn-primary" onclick="NurseModule.showAssignPatientToRoom()">
                                    <i class="fas fa-user-plus"></i> Assign Patient to Room
                                </button>
                                <button class="btn btn-warning" onclick="NurseModule.showRequestCleaning()">
                                    <i class="fas fa-broom"></i> Request Cleaning
                                </button>
                                <button class="btn btn-info" onclick="NurseModule.showRoomHistory()">
                                    <i class="fas fa-history"></i> View Room History
                                </button>
                            </div>
                        </div>
                        
                        <div class="room-list mt-4">
                            <h4>Current Room Status</h4>
                            <div class="table-responsive">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Room #</th>
                                            <th>Type</th>
                                            <th>Status</th>
                                            <th>Beds (Available/Total)</th>
                                            <th>Patient</th>
                                            <th>Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody id="roomListTable">
                                        <!-- Will be populated by JavaScript -->
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;

        await this.loadRoomList();
    }

    async loadRoomList() {
        const rooms = this.nurseData.rooms;

        const roomsHtml = rooms.map(room => `
            <tr>
                <td><strong>${room.roomNumber}</strong></td>
                <td>${room.roomType}</td>
                <td>
                    <span class="status-badge status-${room.status?.toLowerCase() || 'available'}">
                        ${room.status || 'Available'}
                    </span>
                </td>
                <td>${room.availableBeds || 0} / ${room.bedCount || 1}</td>
                <td>${room.patientName || '-'}</td>
                <td>
                    <div class="action-buttons-small">
                        <button class="btn-icon" onclick="NurseModule.changeRoomStatus('${room.roomNumber}')" title="Change Status">
                            <i class="fas fa-exchange-alt"></i>
                        </button>
                        <button class="btn-icon" onclick="NurseModule.assignPatientToRoomModal('${room.roomNumber}')" title="Assign Patient">
                            <i class="fas fa-user-plus"></i>
                        </button>
                        <button class="btn-icon" onclick="NurseModule.viewRoomDetails('${room.roomNumber}')" title="View Details">
                            <i class="fas fa-eye"></i>
                        </button>
                    </div>
                </td>
            </tr>
        `).join('');

        document.getElementById('roomListTable').innerHTML = roomsHtml;
    }

    // Sub-feature: Assign Patient to Room
    async showAssignPatientToRoom() {
        const modal = document.createElement('div');
        modal.className = 'modal-overlay active';
        modal.innerHTML = `
            <div class="modal">
                <div class="modal-header">
                    <h3>Assign Patient to Room</h3>
                    <button class="modal-close" onclick="this.closest('.modal-overlay').remove()">&times;</button>
                </div>
                <div class="modal-body">
                    <form id="assignPatientForm">
                        <div class="form-group">
                            <label for="roomSelect">Select Room *</label>
                            <select id="roomSelect" class="form-control" required>
                                <option value="">Select a room...</option>
                                ${this.nurseData.rooms
                .filter(r => r.status === 'Available' && r.availableBeds > 0)
                .map(r => `<option value="${r.roomNumber}">${r.roomNumber} (${r.roomType}) - ${r.availableBeds} bed(s) available</option>`)
                .join('')}
                            </select>
                        </div>
                        
                        <div class="form-group">
                            <label for="patientSelect">Select Patient *</label>
                            <select id="patientSelect" class="form-control" required>
                                <option value="">Select a patient...</option>
                                ${this.nurseData.patients
                .map(p => `<option value="${p.patientId}">${p.fullName} (${p.roomNumber || 'No room'})</option>`)
                .join('')}
                            </select>
                        </div>
                        
                        <div class="form-group">
                            <label for="assignmentNotes">Assignment Notes</label>
                            <textarea id="assignmentNotes" class="form-control" rows="3" 
                                      placeholder="Any special requirements, equipment needed, or notes..."></textarea>
                        </div>
                        
                        <div class="form-check mb-3">
                            <input type="checkbox" id="updatePatientChart" class="form-check-input" checked>
                            <label for="updatePatientChart" class="form-check-label">
                                Update patient chart with room assignment
                            </label>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" onclick="this.closest('.modal-overlay').remove()">
                        Cancel
                    </button>
                    <button class="btn btn-primary" onclick="NurseModule.assignPatientToRoom()">
                        <i class="fas fa-check"></i> Assign Patient
                    </button>
                </div>
            </div>
        `;

        document.body.appendChild(modal);
    }

    async assignPatientToRoom() {
        try {
            const roomNumber = document.getElementById('roomSelect').value;
            const patientId = document.getElementById('patientSelect').value;
            const notes = document.getElementById('assignmentNotes').value;

            if (!roomNumber || !patientId) {
                this.showToast('Please select both room and patient', 'error');
                return;
            }

            this.showLoading('Assigning patient to room...');

            // API call
            const response = await this.api.request('/nurse/assign-patient', {
                method: 'POST',
                body: JSON.stringify({
                    patientId: parseInt(patientId),
                    roomNumber: roomNumber,
                    notes: notes
                })
            });

            this.showToast('Patient assigned to room successfully!', 'success');

            // Close modal and refresh
            document.querySelector('.modal-overlay.active').remove();

            // Refresh room list
            await this.loadNurseData();
            await this.showRoomManagement();

        } catch (error) {
            console.error('Failed to assign patient:', error);
            this.showToast('Failed to assign patient: ' + error.message, 'error');
        } finally {
            this.hideLoading();
        }
    }

    // Sub-feature: Change Room Status
    async showChangeRoomStatus() {
        const modal = document.createElement('div');
        modal.className = 'modal-overlay active';
        modal.innerHTML = `
            <div class="modal">
                <div class="modal-header">
                    <h3>Change Room Status</h3>
                    <button class="modal-close" onclick="this.closest('.modal-overlay').remove()">&times;</button>
                </div>
                <div class="modal-body">
                    <form id="changeRoomStatusForm">
                        <div class="form-group">
                            <label for="roomNumberSelect">Select Room *</label>
                            <select id="roomNumberSelect" class="form-control" required>
                                <option value="">Select a room...</option>
                                ${this.nurseData.rooms
                .map(r => `<option value="${r.roomNumber}">${r.roomNumber} - Current: ${r.status}</option>`)
                .join('')}
                            </select>
                        </div>
                        
                        <div class="form-group">
                            <label for="newStatus">New Status *</label>
                            <select id="newStatus" class="form-control" required>
                                <option value="">Select status...</option>
                                <option value="Available">Available</option>
                                <option value="Occupied">Occupied</option>
                                <option value="Cleaning">Cleaning</option>
                                <option value="Maintenance">Maintenance</option>
                                <option value="Reserved">Reserved</option>
                            </select>
                        </div>
                        
                        <div class="form-group">
                            <label for="statusReason">Reason for Status Change</label>
                            <textarea id="statusReason" class="form-control" rows="3" 
                                      placeholder="Why are you changing the room status?"></textarea>
                        </div>
                        
                        <div class="form-group" id="patientNameGroup" style="display: none;">
                            <label for="patientName">Patient Name (if occupied)</label>
                            <input type="text" id="patientName" class="form-control" 
                                   placeholder="Enter patient name">
                        </div>
                        
                        <div class="form-group" id="estimatedTimeGroup" style="display: none;">
                            <label for="estimatedTime">Estimated Duration</label>
                            <select id="estimatedTime" class="form-control">
                                <option value="">Select duration...</option>
                                <option value="30min">30 minutes</option>
                                <option value="1h">1 hour</option>
                                <option value="2h">2 hours</option>
                                <option value="4h">4 hours</option>
                                <option value="8h">8 hours (full shift)</option>
                            </select>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" onclick="this.closest('.modal-overlay').remove()">
                        Cancel
                    </button>
                    <button class="btn btn-primary" onclick="NurseModule.changeRoomStatusSubmit()">
                        <i class="fas fa-check"></i> Update Status
                    </button>
                </div>
            </div>
        `;

        document.body.appendChild(modal);

        // Show/hide additional fields based on status
        document.getElementById('newStatus').addEventListener('change', function () {
            const status = this.value;
            const patientGroup = document.getElementById('patientNameGroup');
            const timeGroup = document.getElementById('estimatedTimeGroup');

            patientGroup.style.display = status === 'Occupied' ? 'block' : 'none';
            timeGroup.style.display = (status === 'Cleaning' || status === 'Maintenance') ? 'block' : 'none';
        });
    }

    async changeRoomStatusSubmit() {
        try {
            const roomNumber = document.getElementById('roomNumberSelect').value;
            const newStatus = document.getElementById('newStatus').value;
            const reason = document.getElementById('statusReason').value;
            const patientName = document.getElementById('patientName')?.value || null;

            if (!roomNumber || !newStatus) {
                this.showToast('Please select room and status', 'error');
                return;
            }

            this.showLoading('Updating room status...');

            // API call
            const response = await this.api.request('/nurse/update-room-status', {
                method: 'PUT',
                body: JSON.stringify({
                    roomNumber: roomNumber,
                    newStatus: newStatus,
                    reason: reason,
                    patientName: patientName
                })
            });

            this.showToast('Room status updated successfully!', 'success');


            document.querySelector('.modal-overlay.active').remove();


            await this.loadNurseData();
            await this.showRoomManagement();

        } catch (error) {
            console.error('Failed to update room status:', error);
            this.showToast('Failed to update room status: ' + error.message, 'error');
        } finally {
            this.hideLoading();
        }
    }

    // Helper Methods
    showLoading(message = 'Loading...') {
        let loading = document.getElementById('loading');
        if (!loading) {
            loading = document.createElement('div');
            loading.id = 'loading';
            loading.className = 'loading-screen';
            loading.innerHTML = `
                <div class="loading-content">
                    <div class="spinner"></div>
                    <p>${message}</p>
                </div>
            `;
            document.body.appendChild(loading);
        }
        loading.style.display = 'flex';
    }

    hideLoading() {
        const loading = document.getElementById('loading');
        if (loading) {
            loading.style.display = 'none';
        }
    }

    showToast(message, type = 'success') {
        const toast = document.createElement('div');
        toast.className = `toast toast-${type}`;
        toast.innerHTML = `
            <div class="toast-content">
                <i class="fas fa-${type === 'success' ? 'check-circle' : 'exclamation-circle'}"></i>
                <span>${message}</span>
            </div>
            <button class="toast-close">&times;</button>
        `;

        document.body.appendChild(toast);

        setTimeout(() => {
            toast.classList.add('hide');
            setTimeout(() => toast.remove(), 300);
        }, 5000);

        toast.querySelector('.toast-close').addEventListener('click', () => {
            toast.classList.add('hide');
            setTimeout(() => toast.remove(), 300);
        });
    }

    printReport(report) {
        const printWindow = window.open('', '_blank');
        printWindow.document.write(`
            <html>
                <head>
                    <title>Shift Report</title>
                    <style>
                        body { font-family: Arial, sans-serif; padding: 20px; }
                        .report-header { text-align: center; margin-bottom: 30px; }
                        .report-section { margin: 20px 0; }
                        .report-section h4 { border-bottom: 2px solid #333; padding-bottom: 5px; }
                        @media print { button { display: none; } }
                    </style>
                </head>
                <body>
                    <div class="report-header">
                        <h1>Hospital Shift Report</h1>
                        <h3>${report.shiftType} Shift - ${new Date(report.shiftDate).toLocaleDateString()}</h3>
                        <p>Time: ${report.startTime} - ${report.endTime}</p>
                    </div>
                    
                    <div class="report-section">
                        <h4>Patients Handled</h4>
                        <p>${report.patientsHandled || 'Not specified'}</p>
                    </div>
                    
                    <div class="report-section">
                        <h4>Tasks Completed</h4>
                        <p>${report.tasksCompleted || 'Not specified'}</p>
                    </div>
                    
                    <div class="report-section">
                        <h4>Medications Administered</h4>
                        <p>${report.medicationsAdministered || 'Not specified'}</p>
                    </div>
                    
                    <div class="report-section">
                        <h4>Critical Incidents</h4>
                        <p>${report.criticalIncidents || 'None reported'}</p>
                    </div>
                    
                    <div class="report-section">
                        <h4>Issues & Concerns</h4>
                        <p>${report.issuesConcerns || 'None reported'}</p>
                    </div>
                    
                    <div class="report-section">
                        <h4>Handover Notes</h4>
                        <p>${report.handoverNotes || 'Not specified'}</p>
                    </div>
                    
                    <div class="report-section">
                        <h4>Notes for Next Shift</h4>
                        <p>${report.nextShiftNotes || 'Not specified'}</p>
                    </div>
                    
                    <div class="report-footer" style="margin-top: 50px; border-top: 1px solid #ccc; padding-top: 20px;">
                        <p><strong>Nurse:</strong> ${this.currentUser.fullName}</p>
                        <p><strong>Submitted:</strong> ${new Date().toLocaleString()}</p>
                    </div>
                    
                    <button onclick="window.print()" style="margin-top: 20px; padding: 10px 20px;">
                        Print Report
                    </button>
                </body>
            </html>
        `);
        printWindow.document.close();
    }
}


window.NurseModule = new NurseModule();