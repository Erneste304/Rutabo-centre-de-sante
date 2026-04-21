// Generic list/table view used by Patients, Doctors, Appointments, Rooms, Inventory, Billing
import { el, loading, empty, badgeFor, fmtDate, fmtMoney } from '../ui.js';
import { api } from '../api.js';

const CONFIG = {
  patients: {
    title: 'Patients',
    sub: 'All registered patients',
    fetch: () => api.patients(),
    columns: [
      { label: 'MRN', get: r => r.medicalRecordNumber || r.MedicalRecordNumber || `#${r.patientId || r.PatientId}` },
      { label: 'Name', get: r => r.fullName || r.user?.fullName || 'Unknown' },
      { label: 'Blood', get: r => badge(r.bloodType, '') },
      { label: 'Insurance', get: r => r.insuranceProvider || '—' },
      { label: 'Admitted', get: r => fmtDate(r.admissionDate) },
      { label: 'Status', get: r => badge(r.isActive ? 'Active' : 'Inactive', r.isActive ? 'success' : '') },
    ],
  },
  doctors: {
    title: 'Doctors',
    sub: 'Medical staff directory',
    fetch: () => api.doctors(),
    columns: [
      { label: 'Name', get: r => r.fullName || r.user?.fullName || 'Doctor' },
      { label: 'Specialization', get: r => r.specialization || r.user?.specialization || '—' },
      { label: 'License', get: r => r.licenseNumber || '—' },
      { label: 'Experience', get: r => `${r.yearsOfExperience ?? '—'} yrs` },
      { label: 'Fee', get: r => fmtMoney(r.consultationFee) },
      { label: 'Available', get: r => badge(r.isAvailable ? 'Available' : 'Off', r.isAvailable ? 'success' : 'danger') },
    ],
  },
  appointments: {
    title: 'Appointments',
    sub: 'Scheduled patient visits',
    fetch: () => api.appointments(),
    columns: [
      { label: 'Number', get: r => r.appointmentNumber || `APT-${r.appointmentId}` },
      { label: 'Patient', get: r => r.patientName || r.patient?.user?.fullName || '—' },
      { label: 'Doctor', get: r => r.doctorName || r.doctor?.user?.fullName || '—' },
      { label: 'Type', get: r => r.appointmentType || '—' },
      { label: 'Date', get: r => fmtDate(r.appointmentDate) },
      { label: 'Status', get: r => badge(r.status, badgeFor(r.status)) },
    ],
  },
  rooms: {
    title: 'Rooms & Beds',
    sub: 'Room and bed availability',
    fetch: () => api.rooms(),
    columns: [
      { label: 'Room', get: r => r.roomNumber || `#${r.roomId}` },
      { label: 'Floor', get: r => r.floorNumber ?? '—' },
      { label: 'Beds', get: r => `${r.availableBeds ?? 0} / ${r.bedCount ?? 0}` },
      { label: 'Daily Rate', get: r => fmtMoney(r.dailyRate) },
      { label: 'Status', get: r => badge(r.roomStatus, badgeFor(r.roomStatus)) },
    ],
  },
  pharmacy: {
    title: 'Pharmacy & Inventory',
    sub: 'Medications and medical supplies',
    fetch: () => api.inventory(),
    columns: [
      { label: 'Code', get: r => r.itemCode },
      { label: 'Item', get: r => r.itemName },
      { label: 'Category', get: r => badge(r.category, 'info') },
      { label: 'Stock', get: r => r.currentStock ?? 0 },
      { label: 'Min', get: r => r.minimumStock ?? 0 },
      { label: 'Unit Price', get: r => fmtMoney(r.unitPrice) },
      { label: 'Status', get: r => {
          const low = (r.currentStock ?? 0) <= (r.minimumStock ?? 0);
          return badge(low ? 'Low Stock' : 'OK', low ? 'warning' : 'success');
        } },
    ],
  },
  billing: {
    title: 'Billing',
    sub: 'Patient invoices and payments',
    fetch: () => api.billings(),
    columns: [
      { label: 'Bill #', get: r => r.billNumber || `B-${r.billingId}` },
      { label: 'Patient', get: r => r.patientName || r.patient?.user?.fullName || '—' },
      { label: 'Date', get: r => fmtDate(r.billDate) },
      { label: 'Total', get: r => fmtMoney(r.totalAmount) },
      { label: 'Paid', get: r => fmtMoney(r.paidAmount) },
      { label: 'Balance', get: r => fmtMoney(r.balanceDue) },
      { label: 'Status', get: r => badge(r.paymentStatus, badgeFor(r.paymentStatus)) },
    ],
  },
  emergency: {
    title: 'Emergency Alerts',
    sub: 'Active and recent emergency situations',
    fetch: () => api.emergencyAlerts(),
    columns: [
      { label: 'Alert', get: r => r.alertType || 'Alert' },
      { label: 'Location', get: r => r.location || '—' },
      { label: 'Priority', get: r => badge(r.priority, badgeFor(r.priority)) },
      { label: 'Status', get: r => badge(r.status, badgeFor(r.status)) },
      { label: 'Created', get: r => fmtDate(r.createdAt) },
    ],
  },
};

function badge(text, kind = '') {
  if (text == null || text === '') return el('span', { class: 'muted' }, '—');
  if (typeof text === 'object') return text;
  return el('span', { class: `badge ${kind}` }, String(text));
}

function valueToNode(v) {
  if (v instanceof Node) return v;
  if (v == null) return document.createTextNode('—');
  return document.createTextNode(String(v));
}

export async function listView(view, key) {
  const cfg = CONFIG[key];
  if (!cfg) { view.replaceChildren(el('div', { class: 'empty' }, 'Unknown page')); return; }
  view.replaceChildren(loading());

  let rows;
  try { rows = await cfg.fetch(); } catch (e) {
    view.replaceChildren(el('div', { class: 'alert' }, e.message));
    return;
  }
  rows = Array.isArray(rows) ? rows : (rows?.items || rows?.data || []);

  view.replaceChildren(
    el('div', { class: 'page-head' },
      el('div', {}, el('h1', {}, cfg.title), el('p', { class: 'muted' }, cfg.sub)),
      el('div', { style: 'display:flex;gap:8px' },
        el('button', { class: 'btn btn-outline btn-sm' }, el('i', { class: 'fa-solid fa-filter' }), ' Filter'),
        el('button', { class: 'btn btn-primary btn-sm' }, el('i', { class: 'fa-solid fa-plus' }), ' Add new'),
      )
    ),
    el('div', { class: 'card', style: 'padding:0' },
      rows.length === 0 ? empty('Nothing here yet') :
      el('div', { class: 'table-wrap' },
        (() => {
          const tbl = el('table', { class: 'data' });
          const thead = el('thead', {}, el('tr', {}, ...cfg.columns.map(c => el('th', {}, c.label))));
          const tbody = el('tbody');
          for (const r of rows) {
            const tr = el('tr');
            for (const col of cfg.columns) {
              tr.appendChild(el('td', {}, valueToNode(col.get(r))));
            }
            tbody.appendChild(tr);
          }
          tbl.append(thead, tbody);
          return tbl;
        })()
      )
    )
  );
}
