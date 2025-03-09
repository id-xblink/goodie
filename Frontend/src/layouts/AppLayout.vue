<!-- <template>
  <div class="app-layout">
    <AppSidebar />
    <div class="content">
      <AppHeader />
      <router-view />
    </div>
  </div>
</template>

<script setup>
import AppSidebar from '@/components/AppSidebar.vue';
import AppHeader from '@/components/AppHeader.vue';
</script> -->

<template>
  <div class="app-layout">
    <header>
      <AppHeader />
    </header>

    <main>
      <router-view /> <!-- Загружает вложенные страницы (Dashboard, Catalog и т. д.) -->
    </main>

    <footer>
      <AppFooter />
    </footer>
  </div>
</template>

<script setup>
import AppHeader from '@/components/HeaderComponent.vue';
import AppFooter from '@/components/FooterComponent.vue';
</script>

<!-- <style scoped>
.app-layout {
  display: flex;
  height: 100vh;
}
.content {
  flex-grow: 1;
  display: flex;
  flex-direction: column;
}
</style>

<style scoped>
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


<style scoped>
/* Основной контейнер */
.app-layout {
  display: flex;
  flex-direction: column;
  height: 100vh;
}

main {
  flex-grow: 1;
  display: flex;
  flex-direction: column;
}


/* Верхняя панель */
.header {
  height: 60px;
  background-color: #333;
  color: white;
  display: flex;
  align-items: center;
  padding: 0 20px;
}

/* Контент */
.content {
  flex: 1; /* Гарантированно занимает все оставшееся место */
  padding: 20px;
  overflow: auto;
}

/* Подвал */
.footer {
  height: 50px;
  background-color: #333;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Убираем лишние отступы */
html, body {
  margin: 0;
  padding: 0;
  height: 100%;
}
</style>