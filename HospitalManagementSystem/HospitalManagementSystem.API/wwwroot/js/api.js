/**
 * ApiService Bridge for Hospital Management System
 * Handles API requests and authentication
 */
class ApiService {
    constructor() {
        this.baseUrl = '/api';
    }

    async request(url, options = {}) {
        const token = localStorage.getItem('hms_token');

        const defaultHeaders = {
            'Content-Type': 'application/json'
        };

        if (token) {
            defaultHeaders['Authorization'] = `Bearer ${token}`;
        }

        const config = {
            ...options,
            headers: {
                ...defaultHeaders,
                ...options.headers
            }
        };

        // Ensure URL starts with / if not present
        const fullUrl = url.startsWith('/') ? `${this.baseUrl}${url}` : `${this.baseUrl}/${url}`;

        try {
            const response = await fetch(fullUrl, config);

            if (!response.ok) {
                // Handle different error statuses
                if (response.status === 401) {
                    console.error('Unauthorized request');
                    // Optional: redirect to login
                }

                const errorData = await response.json().catch(() => ({}));
                throw new Error(errorData.message || `API request failed with status ${response.status}`);
            }

            // Return empty object for 204 No Content
            if (response.status === 204) return {};

            return await response.json();
        } catch (error) {
            console.error(`API Request Error [${fullUrl}]:`, error);
            throw error;
        }
    }
}

// Export for use in modules
window.ApiService = ApiService;
