// Role-based navigation definition
export const NAV = [
  { id: 'dashboard', label: 'Dashboard', icon: 'fa-gauge-high', roles: '*' },
  { id: 'patients', label: 'Patients', icon: 'fa-user-injured', roles: ['Admin', 'Doctor', 'Nurse', 'Receptionist'] },
  { id: 'doctors', label: 'Doctors', icon: 'fa-user-doctor', roles: ['Admin', 'Receptionist', 'Patient'] },
  { id: 'appointments', label: 'Appointments', icon: 'fa-calendar-check', roles: '*' },
  { id: 'rooms', label: 'Rooms & Beds', icon: 'fa-bed', roles: ['Admin', 'Doctor', 'Nurse'] },
  { id: 'ward-bed-management', label: 'Ward Management', icon: 'fa-hospital-user', roles: ['Admin', 'Doctor', 'Nurse'] },
  { id: 'pharmacy', label: 'Pharmacy & Inventory', icon: 'fa-pills', roles: ['Admin', 'Doctor', 'Nurse'] },
  { id: 'billing', label: 'Billing', icon: 'fa-file-invoice-dollar', roles: ['Admin', 'Accountant', 'Receptionist', 'Patient'] },
  { id: 'emergency', label: 'Emergency', icon: 'fa-triangle-exclamation', roles: ['Admin', 'Doctor', 'Nurse'] },
];

export function navFor(role) {
  return NAV.filter(n => n.roles === '*' || n.roles.includes(role));
}
