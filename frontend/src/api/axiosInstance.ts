import axios from 'axios';
export const API_URL = 'https://localhost:7286/api';


const instance = axios.create();

instance.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default instance;
