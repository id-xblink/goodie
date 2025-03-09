import api from '@/api.js';

const methodName = async () => {
  try {
    const response = await api.get('/categories');
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};
