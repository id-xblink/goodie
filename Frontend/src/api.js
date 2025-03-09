import axios from 'axios';
import router from '@/router'; // Подключаем роутер

const api = axios.create({
  baseURL: 'https://localhost:7145/api/', // Замените на реальный адрес backend
  headers: {
    'Content-Type': 'application/json'
  }
});

// const api = axios.create();
// api.defaults.baseURL = 'https://localhost:7145/api'; // <-- Задать отдельно

// Перехватчик запроса → автоматически добавляет токен
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, error => Promise.reject(error));

// Перехватчик ответа → проверка токена
api.interceptors.response.use(
  response => response, // Если всё в порядке, просто возвращаем ответ
  error => {
    if (error.response) {
      if (error.response.status === 401) {
        console.warn('Токен недействителен, разлогиниваем пользователя.');
        localStorage.removeItem('token'); // Удаляем токен
        //window.location.href = '/auth/login'; // Перенаправляем на страницу входа
        router.push('/auth/login'); // Используем Vue Router для перенаправления
      } else if (error.response.status === 403) {
        console.warn('Недостаточно прав для выполнения операции.');
      }
    }
    return Promise.reject(error);
  }
);

export default api;
