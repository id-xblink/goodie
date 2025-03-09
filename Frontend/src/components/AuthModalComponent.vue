<template>
    <div class="modal-overlay" v-if="visible" @click.self="close">
      <div class="modal-content">
        <div class="tabs">
          <button :class="{ active: isLogin }" @click="isLogin = true">Вход</button>
          <button :class="{ active: !isLogin }" @click="isLogin = false">Регистрация</button>
        </div>
        
        <div v-if="isLogin">
          <h2>Вход</h2>
          <input v-model="email" type="email" placeholder="Email">
          <input v-model="password" type="password" placeholder="Пароль">
          <button @click="login">Войти</button>
        </div>
        
        <div v-else>
          <h2>Регистрация</h2>
          <input v-model="name" type="text" placeholder="Имя">
          <input v-model="email" type="email" placeholder="Email">
          <input v-model="password" type="password" placeholder="Пароль">
          <button @click="register">Зарегистрироваться</button>
        </div>
        
        <button class="close-btn" @click="close">×</button>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  
  const visible = ref(false);
  const isLogin = ref(true);
  const email = ref('');
  const password = ref('');
  const name = ref('');
  
  const login = () => {
    console.log('Логин:', email.value, password.value);
  };
  
  const register = () => {
    console.log('Регистрация:', name.value, email.value, password.value);
  };
  
  const close = () => {
    visible.value = false;
  };
  
  defineExpose({ visible });
  </script>
  
  <style scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  width: 400px;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
  position: relative;
}

.tabs {
  display: flex;
  justify-content: space-around;
  margin-bottom: 15px;
}

.tabs button {
  flex: 1;
  padding: 10px;
  border: none;
  background: #f0f0f0;
  cursor: pointer;
}

.tabs button.active {
  background: #007bff;
  color: white;
}

input {
  width: calc(100% - 20px);
  padding: 8px;
  margin-bottom: 10px;
  border: 1px solid #ddd;
  border-radius: 5px;
}

button {
  width: 100%;
  padding: 10px;
  background: #007bff;
  color: white;
  border: none;
  cursor: pointer;
}

button.close-btn {
  position: absolute;
  top: 10px;
  right: 10px;
  background: transparent;
  font-size: 18px;
  color: #333;
}
</style>

  