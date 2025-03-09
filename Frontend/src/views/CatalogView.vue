<template>
  <div>
    <h1>📦 Каталог товаров</h1>
    <div class="filters">
      <input v-model="searchQuery" placeholder="🔍 Поиск по названию..." />
      <input v-model.number="minPrice" type="number" placeholder="💰 Мин. цена" />
      <input v-model.number="maxPrice" type="number" placeholder="💰 Макс. цена" />
      <label>Фильтр по категориям:</label>
      <select v-model="selectedCategory">
        <option value="">Все</option>
        <option v-for="category in categories" :key="category.id" :value="category.id">
          {{ category.name }}
        </option>
      </select>
      <button @click="applyFilter" >Применить фильтры</button>
    </div>

    <!-- <button v-if="authStore.role === 'Manager' || authStore.role === 'Admin'" @click="showAddModal = true"> -->
    <button v-if="authStore.isAdmin" @click="showAddModal = true">
      ➕ Добавить товар
    </button>

    <div v-if="isLoading" class="spinner"></div>
    <!-- <DataTable v-else :columns="columns" :data="filteredProducts" :role="authStore.role" @addToCart="cartStore.addToCart" @edit="openEditModal" @delete="deleteProduct"/> -->
    <DataTable v-else :columns="columns" :data="filteredProducts" :role="authStore.role" @addToCart="openCartModal" @edit="openEditModal" @delete="deleteProduct"/>
    <div class="pagination">
      <button @click="prevPage" :disabled="currentPage === 1">⬅️ Назад</button>
      <span>Страница {{ currentPage }} из {{ totalPages }}</span>
      <button @click="nextPage" :disabled="currentPage === totalPages">Вперёд ➡️</button>
    </div>
  </div>
  <!-- Модальное окно для добавления товара -->
  <transition name="modal">
    <div v-if="showAddModal" class="modal">
      <div class="modal-content">
        <h3>Добавить товар</h3>
        <input v-model="newProduct.name" placeholder="Название" />
        <input v-model.number="newProduct.price" type="number" placeholder="Цена" />

        <!-- Выбор категории -->
        <select v-model="newProduct.categoryId">
          <option v-for="category in cartStore.categories" :key="category.id" :value="category.id">
            {{ category.name }}
          </option>
        </select>

        <button @click="addProduct">Сохранить</button>
        <button @click="showAddModal = false">Отмена</button>
      </div>
    </div>
  </transition>
  <transition name="modal">
    <!-- Модальное окно для редактирования товара -->
    <div v-if="showEditModal" class="modal">
      <div class="modal-content">
        <h3>✏️ Редактировать товар</h3>
        
        <input v-model="editProduct.name" placeholder="Название" />
        <input v-model.number="editProduct.price" type="number" placeholder="Цена" />

        <!-- Выбор категории -->
        <select v-model="editProduct.categoryId">
          <option v-for="category in categories" :key="category.id" :value="category.id">
            {{ category.name }}
          </option>
        </select>

        <button @click="updateProduct">💾 Сохранить</button>
        <button @click="showEditModal = false">❌ Отмена</button>
      </div>
    </div>
  </transition>
  <transition name="modal">
    <div v-if="showCartModal" class="modal">
      <div class="modal-content">
        <h3>Выберите количество</h3>
        <input type="number" v-model="selectedQuantity" min="1"/>
        <button @click="addToCart">Добавить</button>
        <button @click="showCartModal = false">Отменить</button>
      </div>
    </div>
  </transition>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import DataTable from '@/components/DataTableProductComponent.vue';
import api from '@/api.js';

import { useAuthStore } from '@/store/authStore';
import { useCartStore } from '@/store/cartStore';
const cartStore = useCartStore();
const authStore = useAuthStore();
const columns = ['name', 'price', 'categoryName', 'actions'];
const products = ref([]);
const categories = ref([]);
const searchQuery = ref('');
const minPrice = ref(null);
const maxPrice = ref(null);
const showAddModal = ref(false);
const showEditModal = ref(false);
const showCartModal = ref(false);
const editProduct = ref({}); // Выбранный товар
const selectedProduct = ref({});
const selectedQuantity = ref({});
const newProduct = ref({ name: '', price: 0 });
const isLoading = ref(true); // Флаг загрузки
const selectedCategory = ref(""); // Для хранения выбранной категории

const currentPage = ref(1);
const pageSize = ref(3);
const totalPages = ref(1);
const totalItems = ref(0);

const filteredProducts = ref({});

import { useToast } from 'vue-toastification';
const toast = useToast();

const prevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--;
    fetchProducts();
  }
};

const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
    fetchProducts();
  }
};

const applyFilter = async () => {
  currentPage.value = 1;
  fetchProducts();
};

const fetchProducts = async () => {
  isLoading.value = true;
  try {
    const response = await api.get('/products', {
      params: {
        page: currentPage.value,
        pageSize: pageSize.value,
        search: searchQuery.value,
        minPrice: minPrice.value,
        maxPrice: maxPrice.value,
        categoryId: selectedCategory.value || null,
      }
      
    });
    console.log({
        page: currentPage.value,
        pageSize: pageSize.value,
        search: searchQuery.value,
        minPrice: minPrice.value,
        maxPrice: maxPrice.value,
        categoryId: selectedCategory.value || null,
      });
      console.log(response.data.items);
    filteredProducts.value = response.data.items;
    totalPages.value = response.data.totalPages;
    totalItems.value = response.data.totalItems;
  } catch (error) {
    console.error('Ошибка загрузки товаров:', error);
  } finally {
    isLoading.value = false;
  }
};

// const fetchProducts = async () => {
//   isLoading.value = true;
//   try {
//     const response = await api.get(`/products?page=${currentPage.value}&pageSize=${pageSize.value}`);
//     products.value = response.data.items; // Список товаров
//     totalPages.value = response.data.totalPages; // Количество страниц
//     totalItems.value = response.data.totalItems;
//   } catch (error) {
//     console.error('Ошибка загрузки товаров:', error);
//   } finally {
//     isLoading.value = false;
//   }
// };

// const fetchProducts = async () => {
//   try {
//     const response = await api.get('/products'); // Запрос к API
//     products.value = response.data;
//   } catch (error) {
//     console.error('Ошибка загрузки товаров:', error);
//   } finally {
//     isLoading.value = false; // Закончили загрузку
//   }
// };

const addToCart = async () => {
  try {
    console.log(selectedProduct.value);

    cartStore.addToCart(selectedProduct.value, selectedQuantity.value)
    showCartModal.value = false;
    toast.info('Товар добавлен в корзину.');
  } catch (error) {
    toast.error("Ошибка добавления товара в корзину.");
    console.error('Ошибка обновления товара:', error);
  }
};



const fetchCategories = async () => {
  try {
    const response = await api.get('/categories');
    categories.value = response.data;
  } catch (error) {
    console.error('Ошибка загрузки категорий:', error);
  }
};

const addProduct = async () => {
  if (!newProduct.value.name || !newProduct.value.price || !newProduct.value.categoryId) {
    toast.warning("Заполните все поля!");
    return;
  }

  try {
    const response = await api.post('/products', newProduct.value);
    //products.value.push(response.data); // Добавляем товар в список
    applyFilter();
    showAddModal.value = false; // Закрываем модалку
    toast.success('Товар успешно добавлен!');
  } catch (error) {
    toast.error("Ошибка добавления товара.");
    console.error('Ошибка добавления товара:', error);
  }
};


// Открываем модалку с данными товара
const openEditModal = (product) => {
  editProduct.value = { ...product }; // Копируем объект, чтобы не менять сразу
  showEditModal.value = true;
};

// Открываем модалку с данными товара
const openCartModal = (product) => {
  selectedProduct.value = { ...product }; // Копируем объект, чтобы не менять сразу
  selectedQuantity.value = 1;
  showCartModal.value = true;
};

// Отправляем обновлённые данные
const updateProduct = async () => {
  try {
    await api.put(`/products/${editProduct.value.id}`, editProduct.value);
    showEditModal.value = false; // Закрываем модалку

    // 🔄 Обновляем список товаров
    const response = await api.get('/products');
    //products.value = response.data;
    applyFilter();
    toast.info('Информация обновлена.');
  } catch (error) {
    toast.error("Ошибка добавления товара.");
    console.error('Ошибка обновления товара:', error);
  }
};

const deleteProduct = async (id) => {
  if (!confirm('Вы уверены, что хотите удалить этот товар?')) return;

  try {
    await api.delete(`/products/${id}`);
    
    // 🔄 Обновляем список товаров
    const response = await api.get('/products');
    //products.value = response.data;
    currentPage.value = 1;
    fetchProducts();
    toast.info('Товар удалён.');
  } catch (error) {
    toast.error("Ошибка добавления товара.");
    console.error('Ошибка удаления товара:', error);
  }
};

// // 🔍 Реактивный поиск и фильтрация
// const filteredProducts = computed(() => {
//   return products.value.filter(product => {
//     const matchesSearch = searchQuery.value.trim() === '' || 
//   product.name.toLowerCase().includes(searchQuery.value.toLowerCase());
//     const matchesMinPrice = (minPrice.value || 0) <= product.price; // 👈 Если `null`, то 0
//     const matchesMaxPrice = (maxPrice.value || Infinity) >= product.price; // 👈 Если `null`, то бесконечность
//     const matchesCategory = selectedCategory.value === "" || 
//       product.categoryName === selectedCategory.value;
//     return matchesSearch && matchesMinPrice && matchesMaxPrice && matchesCategory;
//   });
// });

// Загружаем данные при монтировании компонента
onMounted(() => {
  fetchProducts();
  fetchCategories();

  cartStore.getCategories();
});
</script>

<style scoped>
.filters {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
}
.filters input {
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
}
button {
  margin: 10px 0;
  padding: 8px;
  cursor: pointer;
}
.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}
.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
}

.spinner {
  width: 50px;
  height: 50px;
  border: 5px solid #ddd;
  border-top-color: #007bff;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 20px auto;
}
@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
button {
  background-color: #007bff;
  color: white;
  padding: 8px 12px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: 0.2s;
}
button:hover {
  background-color: #0056b3;
}
button:active {
  transform: scale(0.98);
}
.modal-enter-active, .modal-leave-active {
  transition: opacity 0.3s ease;
}
.modal-enter-from, .modal-leave-to {
  opacity: 0;
}
</style>
