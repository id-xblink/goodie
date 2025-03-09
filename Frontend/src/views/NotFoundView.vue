<template>
    <div class="not-found">
      <h1>Ошибка 404</h1>
      <p>Страница не найдена</p>
      <button @click="goHome">На главную</button>
    </div>
  </template>
  
  <script setup>
  import { useAuthStore } from '@/store/authStore';
  import { useRouter } from 'vue-router';
  
  const authStore = useAuthStore();
  const router = useRouter();
  
  const goHome = () => {
    if (!authStore.token) {
      router.push('/guest'); // Гостевая страница для неавторизованных
    } else {
      switch (authStore.role) {
        case 'Admin':
          router.push('/dashboard');
          break;
        case 'Manager':
          router.push('/orders');
          break;
        case 'Customer':
          router.push('/products');
          break;
        default:
          router.push('/');
      }
    }
  };
  </script>
  
  <style scoped>
  .not-found {
    text-align: center;
    padding: 50px;
  }
  
  button {
    margin-top: 20px;
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
  </style>
  