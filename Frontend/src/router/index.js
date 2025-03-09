import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/store/authStore';
import AppLayout from '@/layouts/AppLayout.vue';
import AuthLayout from '@/layouts/AuthLayout.vue';
import Login from '@/views/LoginView.vue';
import Dashboard from '@/views/DashboardView.vue';
import Catalog from '@/views/CatalogView.vue';
import CartView from '@/views/CartView.vue';
import Orders from '@/views/OrdersView.vue';
import UserManagement from '@/views/UserView.vue';
import GuestHome from '@/views/HomeView.vue';
import Register from '@/views/RegisterView.vue';

// import HomeView from "@/views/HomeView.vue";
// import AnotherView from "@/views/AnotherView.vue";

const routes = [
  {
    path: '/',
    component: AppLayout,
    children: [
      { path: 'dashboard', component: Dashboard, meta: { requiresAuth: true } },
      { path: 'user', component: UserManagement, meta: { requiresAuth: true } },
      { path: 'catalog', component: Catalog, meta: { requiresAuth: true } },
      { path: 'orders', component: Orders, meta: { requiresAuth: true } },
      { path: 'cart', component: CartView, meta: { requiresAuth: true } }
    ]
  },
  {
    path: '/auth',
    component: AuthLayout, // Создадим AuthLayout.vue
    children: [
      { path: 'login', component: Login },
      { path: 'register', component: Register } // <--- ДОБАВИЛИ РЕГИСТРАЦИЮ
    ]
  },
  {
    path: '/guest', component: GuestHome
  },
  {
    path: '/:pathMatch(.*)*', // Обработчик 404
    component: () => import('@/views/NotFoundView.vue')
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Глобальная проверка маршрутов
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  if (to.meta.requiresAuth && !authStore.token) {
    next('/auth/login'); // Если токена нет, перенаправляем на логин
  } else {
    next();
  }
});

export default router;
