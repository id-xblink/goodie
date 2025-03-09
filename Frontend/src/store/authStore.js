import { defineStore } from 'pinia';
import { useCartStore } from '@/store/cartStore';
import router from '@/router';
import api from '@/api'


export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || null,
    role: localStorage.getItem('role') || null,
    userId: localStorage.getItem('userId') || null, // ✅ Добавляем userId
    name: localStorage.getItem('name') || null,
    isAdmin: localStorage.getItem('isAdmin') || false,
  }),

  actions: {
    async login(login, password) {
      try {
        const administrativeRoles = [ 'Manager', 'Admin' ];
        console.log({login, password});
        const response = await api.post('/auth/login', { login, password });

        this.token = response.data.token;
        localStorage.setItem('token', this.token);

        // Расшифруем роль из токена
        const payload = JSON.parse(atob(this.token.split('.')[1]));
        this.role = payload.role;
        localStorage.setItem('role', this.role);
        console.log(this.role);
        this.userId = payload.guid; // ✅ Добавляем userId
        localStorage.setItem('userId', this.userId); // ✅ Сохраняем в LocalStorage
        this.name = payload.name; // ✅ Добавляем userId
        localStorage.setItem('name', this.name); // Имя
        this.isAdmin = administrativeRoles.includes(this.role);
        localStorage.setItem('isAdmin', this.isAdmin);

        const cartStore = useCartStore();
        cartStore.loadCart(); // Загружаем корзину при входе

        return true;
      } catch (error) {
        console.error('Ошибка авторизации:', error);
        return false;
      }
    },

    logout() {
      const cartStore = useCartStore();
      cartStore.clearCartOnLogout(); // Очищаем корзину
      this.token = null;
      this.role = null;
      this.userId = null; // ✅ Очищаем userId
      this.name = null; // ✅ Очищаем userId
      this.isAdmin = null; // ✅ Очищаем userId
      localStorage.removeItem('token');
      localStorage.removeItem('role');
      localStorage.removeItem('userId'); // ✅ Удаляем из LocalStorage
      localStorage.removeItem('name'); // ✅ Удаляем из LocalStorage
      localStorage.removeItem('isAdmin');
      router.push('/guest');
    }
  }
});
