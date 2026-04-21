// Small UI helpers shared across views
export function el(tag, attrs = {}, ...children) {
  const node = document.createElement(tag);
  for (const [k, v] of Object.entries(attrs || {})) {
    if (k === 'class') node.className = v;
    else if (k === 'html') node.innerHTML = v;
    else if (k.startsWith('on') && typeof v === 'function') node.addEventListener(k.slice(2).toLowerCase(), v);
    else if (v !== false && v != null) node.setAttribute(k, v);
  }
  for (const c of children.flat()) {
    if (c == null || c === false) continue;
    node.appendChild(typeof c === 'string' ? document.createTextNode(c) : c);
  }
  return node;
}

export function toast(msg, kind = '') {
  let host = document.getElementById('toast');
  if (!host) { host = el('div', { id: 'toast' }); document.body.appendChild(host); }
  const t = el('div', { class: `toast ${kind}` }, msg);
  host.appendChild(t);
  setTimeout(() => { t.style.opacity = '0'; t.style.transform = 'translateY(8px)'; t.style.transition = 'all .2s'; }, 2400);
  setTimeout(() => t.remove(), 2700);
}

export function initials(name = '') {
  const parts = String(name).trim().split(/\s+/).filter(Boolean);
  if (!parts.length) return '?';
  const first = parts[0][0];
  const last = parts.length > 1 ? parts[parts.length - 1][0] : '';
  return (first + last).toUpperCase();
}

export function fmtMoney(n) {
  if (n == null || isNaN(n)) return '—';
  return new Intl.NumberFormat('en-US', { maximumFractionDigits: 0 }).format(Number(n)) + ' RWF';
}

export function fmtDate(d) {
  if (!d) return '—';
  const date = new Date(d);
  if (isNaN(date)) return String(d);
  return date.toLocaleDateString(undefined, { year: 'numeric', month: 'short', day: 'numeric' });
}

export function loading() {
  return el('div', { class: 'loading' }, el('div', { class: 'spinner' }), 'Loading…');
}

export function empty(msg = 'No data yet') {
  return el('div', { class: 'empty' }, el('i', { class: 'fa-regular fa-folder-open', style: 'font-size:28px;display:block;margin-bottom:8px;opacity:.5' }), msg);
}

export function badgeFor(status) {
  const s = String(status || '').toLowerCase();
  if (['active', 'completed', 'paid', 'available', 'approved', 'resolved'].includes(s)) return 'success';
  if (['pending', 'scheduled', 'partial', 'submitted', 'reorder'].includes(s)) return 'warning';
  if (['cancelled', 'failed', 'expired', 'critical', 'occupied', 'high'].includes(s)) return 'danger';
  if (['confirmed', 'in progress', 'low', 'info'].includes(s)) return 'info';
  return '';
}
