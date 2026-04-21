import { api, session } from './api.js';
import { el, toast, initials } from './ui.js';
import { navFor } from './nav.js';
import { dashboard } from './views/dashboard.js';
import { listView } from './views/list.js';

const root = document.getElementById('root');

function mountTemplate(id) {
  const tpl = document.getElementById(id);
  root.replaceChildren(tpl.content.cloneNode(true));
}

// ====================== Login ======================
function renderLogin() {
  mountTemplate('tpl-login');

  const form = document.getElementById('loginForm');
  const errBox = document.getElementById('loginErr');

  // password toggle
  const toggle = form.querySelector('.pw-toggle');
  toggle.addEventListener('click', () => {
    const inp = form.querySelector('input[name=password]');
    const isPw = inp.type === 'password';
    inp.type = isPw ? 'text' : 'password';
    toggle.querySelector('i').className = isPw ? 'fa-regular fa-eye-slash' : 'fa-regular fa-eye';
  });

  // Quick demo login chips
  document.querySelectorAll('.chip').forEach(c => {
    c.addEventListener('click', () => {
      form.username.value = c.dataset.u;
      form.password.value = 'Admin@123';
      form.dispatchEvent(new Event('submit', { cancelable: true }));
    });
  });

  form.addEventListener('submit', async (e) => {
    e.preventDefault();
    errBox.classList.add('hidden');
    const btn = form.querySelector('button[type=submit]');
    const orig = btn.querySelector('.label').textContent;
    btn.disabled = true;
    btn.querySelector('.label').textContent = 'Signing in…';
    try {
      const data = await api.login(form.username.value.trim(), form.password.value);
      session.set(data);
      toast(`Welcome, ${data.FullName || data.Username}`, 'success');
      location.hash = '#/dashboard';
      renderApp();
    } catch (err) {
      errBox.textContent = err.message || 'Login failed';
      errBox.classList.remove('hidden');
    } finally {
      btn.disabled = false;
      btn.querySelector('.label').textContent = orig;
    }
  });
}

// ====================== App Shell ======================
function renderApp() {
  const user = session.get();
  if (!user) { renderLogin(); return; }
  mountTemplate('tpl-app');

  // user info
  const inits = initials(user.FullName || user.Username);
  document.getElementById('sideAvatar').textContent = inits;
  document.getElementById('topAvatar').textContent = inits;
  document.getElementById('sideName').textContent = user.FullName || user.Username;
  document.getElementById('sideRole').textContent = user.UserType;

  // nav
  const navHost = document.getElementById('sideNav');
  navHost.appendChild(el('div', { class: 'side-section' }, 'Workspace'));
  for (const item of navFor(user.UserType)) {
    const link = el('button', {
      class: 'side-link',
      'data-route': item.id,
      onclick: () => { location.hash = `#/${item.id}`; },
    }, el('i', { class: `fa-solid ${item.icon}` }), item.label);
    navHost.appendChild(link);
  }

  document.getElementById('logoutBtn').addEventListener('click', () => {
    session.clear();
    location.hash = '';
    renderLogin();
  });

  document.getElementById('menuToggle').addEventListener('click', () => {
    document.getElementById('sidebar').classList.toggle('open');
  });

  window.addEventListener('hashchange', router);
  router();
}

// ====================== Router ======================
async function router() {
  const user = session.get();
  if (!user) { renderLogin(); return; }

  const route = (location.hash.replace('#/', '') || 'dashboard').split('?')[0];
  const view = document.getElementById('view');
  if (!view) { renderApp(); return; }

  // active nav state
  document.querySelectorAll('.side-link').forEach(n => {
    n.classList.toggle('active', n.dataset.route === route);
  });

  // close mobile drawer on navigate
  document.getElementById('sidebar')?.classList.remove('open');

  const ctx = { user };
  try {
    if (route === 'dashboard') return dashboard(view, ctx);
    return listView(view, route);
  } catch (e) {
    view.replaceChildren(el('div', { class: 'alert' }, e.message || 'Failed to load page'));
  }
}

// ====================== Boot ======================
if (session.get()) renderApp();
else renderLogin();
