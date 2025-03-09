import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import Toast from 'vue-toastification';
import 'vue-toastification/dist/index.css';
import router from './router'; // 👈 Добавляем роутер

const app = createApp(App);

app.use(createPinia()); // ✅ Добавляем Pinia
app.use(router);
app.use(Toast);
app.mount('#app');
