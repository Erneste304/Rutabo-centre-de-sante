
// Keep your existing JavaScript functions for DOM manipulation
window.initializeDashboard = () => {
    // Re-initialize any dashboard widgets or event listeners if needed
    console.log("Dashboard initialized");
};

window.updateDateTime = () => {
    const now = new Date();
    const element = document.getElementById('currentDateTime');
    if (element) {
        element.innerText = now.toLocaleString();
    }
};

// Blazor calls this from C#
window.showToast = (message, type) => {
    // Simple toast implementation or use a library
    console.log(`[Toast ${type}]: ${message}`);
    // You can integrate a toast library here later
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.innerText = message;
    document.body.appendChild(toast);
    setTimeout(() => {
        toast.remove();
    }, 3000);
};

// LocalStorage helpers
window.appLocalStorage = {
    setItem: (key, value) => localStorage.setItem(key, value),
    getItem: (key) => localStorage.getItem(key),
    removeItem: (key) => localStorage.removeItem(key)
};