const DashboardModule = {
    init: () => {
        console.log('Dashboard initialized');

        // Sidebar toggle
        const sidebarToggle = document.querySelector('.sidebar-toggle');
        const sidebar = document.querySelector('.sidebar');

        if (sidebarToggle && sidebar) {
            sidebarToggle.addEventListener('click', () => {
                sidebar.classList.toggle('collapsed');

                // Adjust main content margin if needed
                const mainContent = document.querySelector('.main-content');
                if (mainContent) {
                    mainContent.classList.toggle('expanded');
                }
            });
        }

        // Logout button
        const logoutBtn = document.getElementById('logoutBtn');
        if (logoutBtn) {
            logoutBtn.addEventListener('click', () => {
                if (confirm('Are you sure you want to logout?')) {
                    document.getElementById('dashboard').style.display = 'none';
                    document.getElementById('loginScreen').style.display = 'flex';

                    // Clear inputs
                    document.getElementById('username').value = '';
                    document.getElementById('password').value = '';
                }
            });
        }

        // Update current time
        DashboardModule.updateTime();
        setInterval(DashboardModule.updateTime, 1000);
    },

    updateTime: () => {
        const now = new Date();
        const timeElement = document.getElementById('currentTime');
        const dateElement = document.getElementById('currentDate');

        if (timeElement) {
            timeElement.textContent = now.toLocaleTimeString('en-US');
        }

        if (dateElement) {
            dateElement.textContent = now.toLocaleDateString('en-US', {
                weekday: 'long',
                year: 'numeric',
                month: 'long',
                day: 'numeric'
            });
        }
    }
};

window.DashboardModule = DashboardModule;
