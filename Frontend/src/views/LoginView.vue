<template>
  <div class="login-container">
    <h2>Авторизация</h2>
    <input v-model="login" placeholder="Логин" />
    <input v-model="password" type="password" placeholder="Пароль" />
    <button @click="handleLogin">Войти</button>
    <p>Нет аккаунта? <router-link to="/auth/register">Зарегистрироваться</router-link></p>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useAuthStore } from '@/store/authStore';
import { useRouter } from 'vue-router';

const login = ref('');
const password = ref('');
const authStore = useAuthStore();
const router = useRouter();

const handleLogin = async () => {
  const success = await authStore.login(login.value, password.value);
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
};
</script>

<style scoped>
.login-container {
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
  background-color: #2c3e50;
  color: white;
  border: none;
  cursor: pointer;
}
button:hover {
  background-color: #34495e;
}
p {
  margin-top: 10px;
}
</style>
