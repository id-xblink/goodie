<template>
    <div class="auth-layout">
      <main v-if="!authStore.isAuthenticated">
        <section class="welcome">
          <h1>Добро пожаловать!</h1>
          <p>Для продолжения вам нужно авторизоваться.</p>
          <button @click="goToLogin">Войти</button>
          <button @click="goToRegister">Регистрация</button>
        </section>
      </main>
      <footer>
        <p>&copy; 2025 My App</p>
      </footer>
    </div>
  </template>
  
  <script setup>
  import { useRouter } from 'vue-router';
  import { useAuthStore } from '@/store/authStore';
  
  const router = useRouter();
  const authStore = useAuthStore();
  
  const goToLogin = () => {
    router.push('/auth/login'); // Переход на страницу авторизации
  };
  
  const goToRegister = () => {
    router.push('/auth/register'); // Переход на страницу регистрации
  };
  </script>
  
  <style scoped>
  .auth-layout {
    display: flex;
    flex-direction: column;
    height: 100vh;
    justify-content: center;
    align-items: center;
    text-align: center;
    background: #f4f4f4;
  }
  
  .welcome {
    background: white;
    padding: 20px;
    border-radius: 10px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  }
  
  button {
    margin-top: 10px;
    padding: 10px 20px;
    background: #3498db;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
  }
  
  button:hover {
    background: #2980b9;
  }
  
  footer {
    position: absolute;
    bottom: 0;
    width: 100%;
    text-align: center;
    padding: 10px;
    background: #333;
    color: white;
  }
  </style>
  