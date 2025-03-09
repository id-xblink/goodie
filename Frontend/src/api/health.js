import api from '@/api.js';

const checkServerStatus = async () => {
    try {
      const response = await api.get('/health');
      return response.data;
    } catch (error) {
      console.error('Ошибка: ', error);
    }
  };
