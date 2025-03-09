<script setup>
  import { useCartStore } from '@/store/cartStore';
  // import AppHeader from "@/components/AppHeader.vue";
  // import AppFooter from "@/components/AppFooter.vue";

  const cartStore = useCartStore();
  cartStore.loadCart(); // 🔄 Загружаем корзину при запуске приложения

</script>

<!-- <template>
  <router-view></router-view>
</template> -->

<!-- <template>
  <div class="layout">
    <header class="header">
      <AppHeader />
    </header>

    <main class="main">
      <router-view /> <!-- Контент из маршрутов -->
    <!-- </main>

    <footer class="footer">
      <AppFooter />
    </footer>
  </div>
</template> --> -->

<template>
  <router-view /> <!-- Vue сам подставит нужный Layout -->
</template>

<!-- <style scoped>
.layout {
  display: flex;
  flex-direction: column;
  height: 100vh;
}

.header, .footer {
  height: 60px;
  background-color: #333;
  color: white;
  text-align: center;
  line-height: 60px;
}

.main {
  flex-grow: 1;
  overflow: auto;
}
</style> -->