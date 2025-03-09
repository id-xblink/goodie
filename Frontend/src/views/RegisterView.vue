<template>
    <div class="register-container">
      <h2>Регистрация</h2>
      <!-- <input v-model="name" placeholder="Имя" />
      <input v-model="email" placeholder="Email" />
      <input v-model="password" type="password" placeholder="Пароль" /> -->


      <input v-model="newUser.name" placeholder="Имя" />
      <input v-model="newUser.login" placeholder="Логин" />
      <input v-model="userPassword" placeholder="Пароль" />
      <input v-model="newUser.address" placeholder="Адрес" />
      <button @click="handleRegister">Зарегистрироваться</button>
      <p>Уже есть аккаунт? <router-link to="/auth/login">Войти</router-link></p>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import { useAuthStore } from '@/store/authStore';
  import { useRouter } from 'vue-router';
  import api from '@/api';
  
  // const name = ref('');
  // const email = ref('');
  const userPassword = ref('');
  
  const authStore = useAuthStore();
  const router = useRouter();

  const newUser = ref({});
  
  const handleRegister = async () => {
    // const success = await authStore.register(name.value, email.value, password.value);
    // if (success) {
    //   router.push('/dashboard'); // После регистрации сразу переходим в личный кабинет
    // } else {
    //   alert('Ошибка регистрации');
    // }

    try {
      const payload = { ...newUser.value }; // Разворачиваем в новый объект



      const response = await api.post(`/users/registration?password=${encodeURIComponent(userPassword.value)}`, payload);
      //users.value.push(response.data); // Добавляем товар в список
      //showAddModal.value = false; // Закрываем модалку

      const success = await authStore.login(payload.login, userPassword.value);
      if (success) {
        switch (authStore.role) {
          case 'Admin':
            router.push('/dashboard'); // Страница для админа
            break;
          case 'Manager':
            router.push('/orders'); // Страница заказов для менеджера
            break;
          case 'Customer':
            router.push('/catalog'); // Страница товаров для покупателя
            break;
          default:
            router.push('/'); // Домашняя страница по умолчанию
        }
      } else {
        alert('Ошибка входа');
      }
    } catch (error) {
      console.error('Ошибка добавления товара:', error);
    }
  };


  
  </script>
  
  <style scoped>
  .register-container {
    max-width: 300px;
    margin: auto;
    text-align: center;
  }
  input {
    display: block;
    width: 100%;
    margin: 10px 0;
    padding: 8px;
  }
  button {
    width: 100%;
    padding: 10px;
    background-color: #27ae60;
    color: white;
    border: none;
    cursor: pointer;
  }
  button:hover {
    background-color: #2ecc71;
  }
  p {
    margin-top: 10px;
  }
  </style>
  