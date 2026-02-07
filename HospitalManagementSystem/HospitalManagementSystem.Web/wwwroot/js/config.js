// Application Configuration
const CONFIG = {
    API_BASE_URL: 'http://localhost:5000/api',
    APP_VERSION: '2.0.0',
    APP_NAME: 'Hospital Management System',
    
    // User Types Configuration
    USER_TYPES: {
        ADMIN: 'Admin',
        DOCTOR: 'Doctor',
        NURSE: 'Nurse',
        PATIENT: 'Patient',
        RECEPTIONIST: 'Receptionist',
        ACCOUNTANT: 'Accountant'
    },
    
    // Status Colors
    STATUS_COLORS: {
        ACTIVE: '#28A745',
        PENDING: '#FFC107',
        INACTIVE: '#6C757D',
        SUSPENDED: '#DC3545',
        COMPLETED: '#17A2B8',
        CANCELLED: '#DC3545'
    },
    
    // Departments
    DEPARTMENTS: [
        'Cardiology', 'Neurology', 'Orthopedics', 'Pediatrics', 
        'Emergency', 'ICU', 'Surgery', 'Radiology', 'Pharmacy'
    ],
    
    // Specializations
    SPECIALIZATIONS: [
        'Cardiologist', 'Neurologist', 'Orthopedic Surgeon', 'Pediatrician',
        'General Physician', 'Surgeon', 'Radiologist', 'Psychiatrist'
    ],
    
    // Room Types
    ROOM_TYPES: [
        'General', 'ICU', 'Emergency', 'Operation',
        'Private', 'Semi-Private', 'Pediatric', 'Maternity'
    ]
};

// Utility Functions
const Utils = {
    formatDate: (date) => {
        return new Date(date).toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
    },
    
    formatDateTime: (date) => {
        return new Date(date).toLocaleString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    },
    
    formatCurrency: (amount) => {
        return new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD'
        }).format(amount);
    },
    
    generateId: (prefix = '') => {
        return prefix + Date.now().toString(36) + Math.random().toString(36).substr(2);
    },
    
    showToast: (message, type = 'success') => {
        const toast = document.createElement('div');
        toast.className = `toast toast-${type}`;
        toast.innerHTML = `
            <div class="toast-content">
                <i class="fas fa-${type === 'success' ? 'check-circle' : 'exclamation-circle'}"></i>
                <span>${message}</span>
            </div>
            <button class="toast-close">&times;</button>
        `;
        
        // Add to body
        document.body.appendChild(toast);
        
        // Auto remove after 5 seconds
        setTimeout(() => {
            toast.classList.add('hide');
            setTimeout(() => toast.remove(), 300);
        }, 5000);
        
        // Close button
        toast.querySelector('.toast-close').addEventListener('click', () => {
            toast.classList.add('hide');
            setTimeout(() => toast.remove(), 300);
        });
    },
    
    showLoading: () => {
        const loading = document.getElementById('loading');
        if (loading) loading.style.display = 'flex';
    },
    
    hideLoading: () => {
        const loading = document.getElementById('loading');
        if (loading) loading.style.display = 'none';
    },
    
    confirm: async (message) => {
        return new Promise((resolve) => {
            const modal = document.createElement('div');
            modal.className = 'confirm-modal';
            modal.innerHTML = `
                <div class="confirm-content">
                    <h3>Confirm Action</h3>
                    <p>${message}</p>
                    <div class="confirm-buttons">
                        <button class="btn btn-secondary" id="confirmCancel">Cancel</button>
                        <button class="btn btn-danger" id="confirmOk">Confirm</button>
                    </div>
                </div>
            `;
            
            document.body.appendChild(modal);
            
            modal.querySelector('#confirmOk').addEventListener('click', () => {
                modal.remove();
                resolve(true);
            });
            
            modal.querySelector('#confirmCancel').addEventListener('click', () => {
                modal.remove();
                resolve(false);
            });
        });
    }
};

// Export for use in other modules
window.CONFIG = CONFIG;
window.Utils = Utils;