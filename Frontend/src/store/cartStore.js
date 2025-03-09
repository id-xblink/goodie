import { defineStore } from 'pinia';
import { useAuthStore } from '@/store/authStore'; // 👈 Импорт авторизации
import { objectApiManager1 } from '@/api/catalog';
import Catalogi from '@/api/catalog';

export const useCartStore = defineStore('cart', {
  state: () => ({
    cartItems: [], // Загружаем из LocalStorage
    categories: [],
  }),

  actions: {
    async getCategories() {
      Catalogi('gi');
      this.categories = await objectApiManager1.fetchCategories1();
      console.log(this.categories);
    },

    loadCart() {
        const authStore = useAuthStore(); 
        const userCart = localStorage.getItem(`cart_${authStore.userId}`);
        this.cartItems = userCart ? JSON.parse(userCart) : [];
      },

    increaseQuantity(id) {
      const item = this.cartItems.find(item => item.id === id);
      item.quantity++;
      this.saveCart();
    },

    decreaseQuantity(id) {
      const item = this.cartItems.find(item => item.id === id);
      if (item.quantity > 1)
        item.quantity--;
      this.saveCart();
    },

    addToCart(product, quantity = 1) {
      const item = this.cartItems.find(item => item.id === product.id);
      if (item) {
        item.quantity = item.quantity + quantity;
      } else {
        this.cartItems.push({ ...product, quantity });
      }
      this.saveCart(); // Сохраняем после добавления
    },

    removeFromCart(id) {
      this.cartItems = this.cartItems.filter(item => item.id !== id);
      this.saveCart(); // Сохраняем после удаления
    },

    clearCart() {
      this.cartItems = [];
      this.saveCart();
    },

    saveCart() {
        const authStore = useAuthStore();
        if (authStore.userId) { 
            localStorage.setItem(`cart_${authStore.userId}`, JSON.stringify(this.cartItems));
        } 
    },

    clearCartOnLogout() {
        this.cartItems = [];
        //this.saveCart(); // Очищаем корзину, но не удаляем ключ
    }
  }
});
