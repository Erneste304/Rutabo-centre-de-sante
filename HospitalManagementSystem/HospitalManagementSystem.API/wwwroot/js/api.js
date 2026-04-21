// Centralized API client with JWT auth
const STORAGE_KEY = 'hms_session';

export const session = {
  get() {
    try { return JSON.parse(localStorage.getItem(STORAGE_KEY) || 'null'); }
    catch { return null; }
  },
  set(s) { localStorage.setItem(STORAGE_KEY, JSON.stringify(s)); },
  clear() { localStorage.removeItem(STORAGE_KEY); },
};

async function request(path, { method = 'GET', body, auth = true } = {}) {
  const headers = { 'Accept': 'application/json' };
  if (body !== undefined) headers['Content-Type'] = 'application/json';
  if (auth) {
    const s = session.get();
    if (s?.Token) headers['Authorization'] = `Bearer ${s.Token}`;
  }
  const res = await fetch(`/api${path}`, {
    method,
    headers,
    body: body !== undefined ? JSON.stringify(body) : undefined,
  });
  const text = await res.text();
  let data = null;
  if (text) { try { data = JSON.parse(text); } catch { data = text; } }
  if (!res.ok) {
    const msg = (data && (data.message || data.title)) || `Request failed (${res.status})`;
    const err = new Error(msg);
    err.status = res.status;
    err.data = data;
    throw err;
  }
  return data;
}

export const api = {
  // Auth
  login: (username, password) => request('/Auth/login', { method: 'POST', body: { username, password }, auth: false }),
  register: (payload) => request('/Auth/register', { method: 'POST', body: payload, auth: false }),

  // Dashboard
  dashboard: (type = 'Admin') => request(`/Dashboard/stats?type=${encodeURIComponent(type)}`),

  // Resources (lists)
  patients: () => request('/Patients'),
  doctors: () => request('/Doctors'),
  appointments: () => request('/Appointments'),
  rooms: () => request('/Rooms'),
  inventory: () => request('/Inventory'),
  billings: () => request('/Billings'),
  emergencyAlerts: () => request('/EmergencyAlert/active/list').catch(() => []),

  raw: request,
};
