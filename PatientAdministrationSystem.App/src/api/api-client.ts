import axios, { AxiosInstance } from 'axios';

// Create an instance of Axios
const apiClient: AxiosInstance = axios.create({
  baseURL: 'https://localhost:7260/api', // Replace with your API's base URL
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor for adding authentication token or other logic
apiClient.interceptors.request.use(
  (config) => {
    // Add any request headers or logic here
    return config;
  },
  (error) => {
    // Handle request errors
    return Promise.reject(error);
  }
);

// Response interceptor for handling errors
apiClient.interceptors.response.use(
  (response) => {
    // Any status code in the range 2xx causes this function to trigger
    return response;
  },
  (error) => {
    // Any status codes that falls outside the range of 2xx cause this function to trigger
    if (axios.isAxiosError(error) && error.response) {
      // Handle different types of HTTP errors here
      console.error(`API Error: ${error.response.status} - ${error.response.statusText}`);
    } else {
      console.error('Unexpected Error:', error);
    }
    return Promise.reject(error);
  }
);

export default apiClient;
