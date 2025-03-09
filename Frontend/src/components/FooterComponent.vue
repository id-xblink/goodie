<template>
    <footer class="app-footer">
      <div class="status">
        <span>{{ apiStatus }}</span>
        <span v-if="currentSection"> | Раздел: {{ currentSection }}</span>
      </div>
      <div class="messages" v-if="message">
        <p>{{ message }}</p>
      </div>
    </footer>
  </template>
  
  <script setup>
  import { computed } from 'vue';
  import { useAuthStore } from '@/store/authStore';
  import { useRoute } from 'vue-router';
  import { storeToRefs } from 'pinia'
    import { useSystemStore } from '@/store/system.js'
  
  const authStore = useAuthStore();
  const route = useRoute();
  

  const systemStore = useSystemStore()
const { apiStatus } = storeToRefs(systemStore)
  // Состояние системы
  //const systemStatus = computed(() => (authStore.token ? 'Онлайн' : 'Оффлайн'));
  
  // Определение текущего раздела
  const currentSection = computed(() => {
    switch (route.path) {
      case '/dashboard': return 'Панель управления';
      case '/catalog': return 'Каталог';
      case '/orders': return 'Заказы';
      case '/cart': return 'Корзина';
      case '/user': return 'Пользователи'
      default: return null;
    }
  });
  
  // Сообщения (например, ошибки, уведомления)
  const message = computed(() => authStore.message);
  </script>
  
  <style scoped>
  .app-footer {
    position: fixed;
    bottom: 0;
    width: 100%;
    padding: 10px;
    background: #2c3e50;
    color: white;
    text-align: center;
    font-size: 14px;
  }
  
  .messages p {
    margin: 5px 0;
    color: #f39c12;
  }
  </style>
  