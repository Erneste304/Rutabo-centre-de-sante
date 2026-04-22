import { el, toast } from '../ui.js';

export function wardBedManagement(view, ctx) {
  const openForm = (type) => {
    const titles = {
      assign: 'Assign Bed to Patient',
      transfer: 'Transfer Patient Bed',
      reserve: 'Reserve a Bed',
      discharge: 'Discharge Patient'
    };
    renderModal(type, titles[type]);
  };

  const renderModal = (type, title) => {
    const modal = el('div', { class: 'modal-overlay' },
      el('div', { class: 'modal-content' },
        el('div', { class: 'modal-header' },
          el('h3', {}, title),
          el('button', { class: 'btn-close', onclick: () => modal.remove() }, '×')
        ),
        el('div', { class: 'modal-body' },
          renderFormFields(type)
        ),
        el('div', { class: 'modal-footer' },
          el('button', { class: 'btn btn-ghost', onclick: () => modal.remove() }, 'Cancel'),
          el('button', { class: 'btn btn-primary', onclick: () => {
            toast('Form submitted successfully', 'success');
            modal.remove();
          }}, 'Submit')
        )
      )
    );
    document.body.appendChild(modal);
  };

  const renderFormFields = (type) => {
    const grid = el('div', { class: 'form-grid' });
    
    if (type === 'assign') {
      grid.append(
        field('Patient Name', el('input', { class: 'form-control', placeholder: 'Search patient...' }), true),
        field('Ward', select(['General Ward', 'ICU', 'Maternity', 'Pediatrics'])),
        field('Bed Number', select(['B-101', 'B-102', 'B-103'])),
        field('Admission Date & Time', el('input', { type: 'datetime-local', class: 'form-control', value: new Date().toISOString().slice(0, 16) }), true)
      );
    } else if (type === 'transfer') {
      grid.append(
        field('Current Patient/Bed', select(['John Doe (B-101)', 'Jane Smith (B-102)']), true),
        field('New Ward', select(['General Ward', 'ICU', 'Maternity'])),
        field('New Bed Number', select(['B-201', 'B-202'])),
        field('Reason for Transfer', el('textarea', { class: 'form-control', rows: '3' }), true)
      );
    } else if (type === 'reserve') {
      grid.append(
        field('Patient Name (Optional)', el('input', { class: 'form-control', placeholder: 'Enter name or ID' }), true),
        field('Ward', select(['General Ward', 'ICU'])),
        field('Bed Number', select(['B-105', 'B-106'])),
        field('Reservation Start', el('input', { type: 'date', class: 'form-control' })),
        field('Expected Duration (Days)', el('input', { type: 'number', class: 'form-control', min: '1', value: '1' }))
      );
    } else if (type === 'discharge') {
      grid.append(
        field('Patient to Discharge', select(['John Doe (B-101)', 'Jane Smith (B-102)']), true),
        field('Discharge Date & Time', el('input', { type: 'datetime-local', class: 'form-control', value: new Date().toISOString().slice(0, 16) }), true),
        field('Discharge Summary', el('textarea', { class: 'form-control', rows: '3', placeholder: 'Final notes...' }), true),
        field('Follow-up Required?', select(['No', 'Yes - 1 Week', 'Yes - 1 Month']))
      );
    }
    
    return grid;
  };

  const field = (label, input, full = false) => {
    return el('div', { class: `form-group ${full ? 'form-group-full' : ''}` },
      el('label', {}, label),
      input
    );
  };

  const select = (options) => {
    return el('select', { class: 'form-control' },
      ...options.map(o => el('option', {}, o))
    );
  };

  view.replaceChildren(
    el('div', { class: 'page-head' },
      el('div', {}, 
        el('h1', {}, 'Ward & Bed Management'),
        el('p', { class: 'muted' }, 'Manage patient bed assignments, transfers, and discharges')
      )
    ),
    el('div', { class: 'action-cards-grid' },
      actionCard('🛏️', 'Assign Bed', 'Assign a bed to a newly admitted patient', 'btn-primary', () => openForm('assign')),
      actionCard('🔄', 'Transfer Bed', 'Move a patient to a different bed or ward', 'btn-info', () => openForm('transfer')),
      actionCard('📅', 'Reserve Bed', 'Reserve a bed for an upcoming admission', 'btn-warning', () => openForm('reserve')),
      actionCard('🚪', 'Discharge Patient', 'Process patient discharge and free up bed', 'btn-danger', () => openForm('discharge'))
    )
  );
}

function actionCard(icon, title, desc, btnClass, onclick) {
  return el('div', { class: 'action-card', onclick },
    el('div', { class: `action-icon ${title.toLowerCase().split(' ')[0]}` }, icon),
    el('h3', {}, title),
    el('p', {}, desc),
    el('button', { class: `btn ${btnClass}`, style: 'width:100%' }, 'Open Form')
  );
}
