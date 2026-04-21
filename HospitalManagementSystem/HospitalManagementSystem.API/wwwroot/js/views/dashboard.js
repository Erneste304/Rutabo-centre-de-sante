import { el, loading, fmtMoney, badgeFor, fmtDate } from '../ui.js';
import { api } from '../api.js';

export async function dashboard(view, ctx) {
  view.replaceChildren(loading());
  const role = ctx.user.UserType;

  let stats, appointments = [], alerts = [];
  try {
    [stats, appointments, alerts] = await Promise.all([
      api.dashboard(role),
      api.appointments().catch(() => []),
      api.emergencyAlerts().catch(() => []),
    ]);
  } catch (e) {
    view.replaceChildren(el('div', { class: 'alert' }, e.message));
    return;
  }

  const cards = [
    { label: 'Total Patients', value: stats.totalPatients ?? 0, icon: 'fa-user-injured', tone: 't-primary' },
    { label: 'Total Staff', value: stats.totalStaff ?? 0, icon: 'fa-user-doctor', tone: 't-info' },
    { label: 'Departments', value: stats.totalDepartments ?? 0, icon: 'fa-hospital', tone: 't-accent' },
    { label: 'Active Shifts', value: stats.activeShifts ?? 0, icon: 'fa-user-clock', tone: 't-success' },
    { label: 'Avg. Wait', value: `${stats.avgWaitTime ?? 0} min`, icon: 'fa-clock', tone: 't-warning' },
    { label: 'Revenue', value: `${(stats.clinicRevenue ?? 0).toFixed(1)}k`, icon: 'fa-coins', tone: 't-success' },
    { label: 'Satisfaction', value: `${stats.satisfactionRate ?? 0}%`, icon: 'fa-face-smile', tone: 't-primary' },
    { label: 'Claims Paid', value: stats.claimsProcessed ?? 0, icon: 'fa-file-circle-check', tone: 't-info' },
  ];

  const recent = (Array.isArray(appointments) ? appointments : []).slice(0, 6);

  view.replaceChildren(
    el('div', { class: 'page-head' },
      el('div', {},
        el('h1', {}, `Welcome back, ${ctx.user.FullName?.split(' ')[0] || 'there'} 👋`),
        el('p', { class: 'muted' }, 'Here is what is happening at the hospital today.')
      ),
      el('span', { class: 'badge primary' }, role)
    ),
    el('div', { class: 'stats' },
      ...cards.map(c =>
        el('div', { class: 'stat' },
          el('div', { class: `icon ${c.tone}` }, el('i', { class: `fa-solid ${c.icon}` })),
          el('div', {},
            el('div', { class: 'label' }, c.label),
            el('div', { class: 'value' }, String(c.value)),
          )
        )
      )
    ),
    el('div', { class: 'grid-2' },
      el('div', { class: 'card' },
        el('div', { class: 'card-head' },
          el('div', {}, el('h3', {}, 'Recent Appointments'), el('div', { class: 'card-sub' }, 'Latest scheduled visits')),
          el('button', { class: 'btn btn-outline btn-sm', onclick: () => location.hash = '#/appointments' }, 'View all')
        ),
        recent.length === 0
          ? el('div', { class: 'empty' }, 'No appointments yet')
          : el('div', { class: 'list' },
              ...recent.map(a => el('div', { class: 'list-item' },
                el('div', { class: 'avatar small' }, (a.patientName || a.patient?.user?.fullName || 'P').slice(0, 1)),
                el('div', { class: 'meta' },
                  el('div', { class: 'title' }, a.appointmentNumber || `APT #${a.appointmentId}`),
                  el('div', { class: 'sub' }, `${a.appointmentType || 'Visit'} · ${fmtDate(a.appointmentDate)}`)
                ),
                el('span', { class: `badge ${badgeFor(a.status)}` }, a.status || '—'),
              ))
            )
      ),
      el('div', { class: 'card' },
        el('div', { class: 'card-head' },
          el('div', {}, el('h3', {}, 'Active Alerts'), el('div', { class: 'card-sub' }, 'Live emergency feed')),
          el('span', { class: `badge ${alerts.length ? 'danger' : 'success'}` }, `${alerts.length} active`)
        ),
        alerts.length === 0
          ? el('div', { class: 'empty' }, 'All clear right now')
          : el('div', { class: 'list' },
              ...alerts.slice(0, 6).map(x => el('div', { class: 'list-item' },
                el('div', { class: 'avatar small', style: 'background:linear-gradient(135deg,#ef4444,#f59e0b)' }, '!'),
                el('div', { class: 'meta' },
                  el('div', { class: 'title' }, x.alertType || 'Alert'),
                  el('div', { class: 'sub' }, x.location || x.description || '—')
                ),
                el('span', { class: `badge ${badgeFor(x.priority)}` }, x.priority || 'normal')
              ))
            )
      )
    )
  );
}
