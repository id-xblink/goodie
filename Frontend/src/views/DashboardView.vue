<template>
  <div>
    <h1>Панель управления</h1>
    <p>{{ status }}</p>
    <button @click="checkServerStatus">Обновить статус</button>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import api from '@/api'; // Подставь правильный путь


const status = ref('Проверка...');


const checkServerStatus = async () => {
  try {
    console.log('Отправка запроса на:', api.defaults.baseURL + 'health');
    const response = await api.get(`/health`, { timeout: 5000,
      headers: { 'Cache-Control': 'no-cache' } // Отключаем кэширование
     });

     console.log('Заголовки ответа:', response.headers);
      console.log('Ответ сервера:', response.data);

     if (response.status === 200 && response.data.status === 'ok') {
      status.value = 'Система: онлайн';
    } else {
      status.value = 'Система: офлайн';
    }
  } catch (error) {
    console.error('Ошибка проверки сервера:', error.message);
    status.value = 'Система: офлайн';
  }
};

onMounted(() => {
  checkServerStatus();
});
</script>

<style scoped>
button {
  margin-top: 10px;
  padding: 10px 15px;
  background: #3498db;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}
button:hover {
  background: #2980b9;
}
</style>
