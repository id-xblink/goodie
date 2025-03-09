<template>
    <header class="header">
      <nav class="left">
        <router-link to="/dashboard" active-class="active">🏠 Панель</router-link>
        <router-link v-if="authStore.isAdmin" to="/user" active-class="active">👤 Пользователи</router-link>
        <router-link to="/catalog" active-class="active">📦 Каталог</router-link>
        <router-link to="/orders" active-class="active">📜 Заказы</router-link>
        <router-link v-if="!authStore.isAdmin" to="/cart" active-class="active">🛒 Корзина ({{ cartStore.cartItems.length }})</router-link>
        <!-- <router-link v-if="authStore.role === 'Admin'" to="/admin" active-class="active">⚙️ Управление</router-link> -->
      </nav>
  
      <div class="right">
        <div class="user-info">
          <span>{{ userName }} ({{ userRole }})</span>
          <button class="logout" @click="authStore.logout">Выйти</button>
        </div>
      </div>
    </header>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import { useAuthStore } from '@/store/authStore';
  import { useCartStore } from '@/store/cartStore';
  
  const authStore = useAuthStore();
  const cartStore = useCartStore();
  const userName = ref('');
  const userRole = ref('');
  
  onMounted(() => {
    userName.value = localStorage.getItem('name') || 'Гость';
    userRole.value = localStorage.getItem('role') || 'Неизвестно';
  });
  </script>
  
  <style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: linear-gradient(135deg, #2c3e50, #34495e);
  color: white;
  padding: 12px 20px;
  border-radius: 10px;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
}

.left {
  display: flex;
  gap: 12px;
  flex-grow: 1;
}

.right {
  display: flex;
  align-items: center;
}

nav a {
  color: white;
  text-decoration: none;
  padding: 8px 15px;
  border-radius: 8px;
  transition: background 0.3s, transform 0.2s;
  font-size: 14px;
  font-weight: 500;
}

nav a:hover {
  background: rgba(255, 255, 255, 0.2);
  transform: translateY(-2px);
}

/* Активный раздел */
.router-link-active {
  background: #1abc9c;
  color: white;
  font-weight: 600;
  box-shadow: 0 0 8px rgba(26, 188, 156, 0.6);
}

.user-info {
  display: flex;
  align-items: center;
  gap: 12px;
  background: rgba(255, 255, 255, 0.15);
  padding: 8px 15px;
  border-radius: 8px;
}

.user-name {
  font-weight: 600;
  font-size: 14px;
}

.user-role {
  font-size: 12px;
  opacity: 0.8;
}

.logout {
  background: #e74c3c;
  color: white;
  border: none;
  padding: 8px 12px;
  cursor: pointer;
  border-radius: 6px;
  transition: background 0.3s, transform 0.2s;
  font-size: 14px;
}

.logout:hover {
  background: #c0392b;
  transform: scale(1.05);
}
</style>