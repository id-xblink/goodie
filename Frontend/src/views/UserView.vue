<template>
  <div>
    <h1>👥 Список пользователей</h1>
    <div class="filters">
      <input v-model="searchQuery" placeholder="🔍 Поиск по номеру..." />
      <!-- <input v-model.number="minPrice" type="number" placeholder="💰 Мин. цена" />
      <input v-model.number="maxPrice" type="number" placeholder="💰 Макс. цена" /> -->
      <label>Фильтр по ролям:</label>
      <select v-model="selectedRole">
        <option value="">Все</option>
        <option v-for="role in roles" :key="role" :value="role">{{ role.name }}</option> 
                                                                <!-- ?? ^^^ -->
      </select>
      <button @click="applyFilter" >Применить фильтры</button>
    </div>

    <button v-if="authStore.isAdmin" @click="showAddModal = true">
      ➕ Добавить пользователя
    </button>

    <div v-if="isLoading" class="spinner"></div>
    <DataTable v-else :columns="columns" :data="filteredUsers" :role="authStore.role" @edit="openEditModal" @delete="deleteUser"/>
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
        <h3>Добавить пользователя</h3>
        <input v-model="newUser.name" placeholder="Имя" />
        <input v-model="newUser.login" placeholder="Логин" />
        <input v-model="userPassword" placeholder="Пароль" />
        <!-- <input v-model.number="newUser.code" placeholder="Код" /> -->
        <input v-model="newUser.address" placeholder="Адрес" />
        <input v-model="newUser.discount" type="number" placeholder="Скидка"/>

        <!-- Выбор категории -->
        <select v-model="newUser.userRoleId">
          <option v-for="role in roles" :key="role.id" :value="role.id">
            {{ role.name }}
          </option>
        </select>

        <button @click="addUser">Сохранить</button>
        <button @click="showAddModal = false">Отмена</button>
      </div>
    </div>
  </transition>
  <transition name="modal">
  <!-- Модальное окно для редактирования товара -->
  <div v-if="showEditModal" class="modal">
    <div class="modal-content">
      <h3>✏️ Редактировать пользователя</h3>
      
      <input v-model="editUser.name" placeholder="Имя" />
      <input v-model="editUser.address" placeholder="Адрес" />
      <input v-model.number="editUser.discount" type="number" placeholder="Скидка" />

      <!-- Выбор категории -->
      <!-- <select v-model="editUser.customerId">
        <option value="">-</option>
        <option v-for="manager in managers" :key="manager.id" :value="manager.id">
          {{ manager.name + ' ' + manager.code }}
        </option>
      </select> -->

      <button @click="updateUser">💾 Сохранить</button>
      <button @click="showEditModal = false">❌ Отмена</button>
    </div>
  </div>
</transition>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import DataTable from '@/components/DataTableUserComponent.vue';
import api from '@/api.js';
import { useAuthStore } from '@/store/authStore';
const authStore = useAuthStore();
const columns = ['name', 'code', 'address', 'discount', 'userRole', 'actions'];
const users = ref([]);
const managers = ref([]);
const roles = ref([]);
const searchQuery = ref('');
const selectedRole = ref('');
const userPassword = ref('');
const showAddModal = ref(false);
const showEditModal = ref(false);
const editUser = ref({}); // Выбранный товар
const newUser = ref({});
const isLoading = ref(true); // Флаг загрузки

const filteredUsers = ref({});

const currentPage = ref(1);
const pageSize = ref(3);
const totalPages = ref(1);
const totalItems = ref(0);

const prevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--;
    fetchUsers();
  }
};

const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
    fetchUsers();
  }
};

const applyFilter = async () => {
  currentPage.value = 1;
  fetchUsers();
};

const fetchUsers = async () => {
  isLoading.value = true;
  try {
    // const response = await api.get('/users/dto'); // Запрос к API
    // users.value = response.data;
    const response = await api.get('/users/dto', {
      params: {
        page: currentPage.value,
        pageSize: pageSize.value,
        search: searchQuery.value,
        userRoleId: selectedRole.value.id || null,
      }
    });
    console.log(selectedRole.value.id);
    filteredUsers.value = response.data.users;
    totalPages.value = response.data.totalPages;
    totalItems.value = response.data.totalItems;
  } catch (error) {
    console.error('Ошибка загрузки пользователей:', error);
  } finally {
    isLoading.value = false; // Закончили загрузку
  }
};

const fetchRoles = async () => {
  try {
    const response = await api.get('/roles');
    roles.value = response.data;
  } catch (error) {
    console.error('Ошибка загрузки ролей:', error);
  }
};

// const fetchManagers = async () => {
//   try {
//     const response = await api.get('/users/managers');
//     managers.value = response.data;
//   } catch (error) {
//     console.error('Ошибка загрузки ролей:', error);
//   }
// };

const addUser = async () => {
  // console.log(newUser.value.name);
  // console.log(newUser.value.code);
  // console.log(newUser.value.roleId);
  // console.log(newUser.value.address);
  // console.log(newUser.value.discount);
  
  // console.log(!newUser.value.name);
  // console.log(!newUser.value.code);
  // console.log(!newUser.value.roleId);
  // console.log(!newUser.value.address);
  // console.log(!newUser.value.discount);
  if (!newUser.value.name || !userPassword.value || !newUser.value.login || !newUser.value.userRoleId  || !newUser.value.address || !Number.isFinite(newUser.value.discount)) {
    alert("Заполните все поля!");
    return;
  }
  alert("Всё хорошо!");

  try {
    console.log(userPassword.value);
    const response = await api.post(`/users?password=${encodeURIComponent(userPassword.value)}`, newUser.value);
    applyFilter();
    //users.value.push(response.data); // Добавляем товар в список
    showAddModal.value = false; // Закрываем модалку
  } catch (error) {
    console.error('Ошибка добавления товара:', error);
  }
};


// Открываем модалку с данными товара
const openEditModal = (user) => {
  editUser.value = { ...user }; // Копируем объект, чтобы не менять сразу
  showEditModal.value = true;
};

// Отправляем обновлённые данные
const updateUser = async () => {
  try {
    // if(editUser.value.customerId === '')
    //   editUser.value.customerId = null;
    const result = await api.put(`/users/${editUser.value.id}`, editUser.value);
    showEditModal.value = false; // Закрываем модалку
    console.log(result);

    // 🔄 Обновляем список товаров
    const response = await api.get('/users/dto');
    //users.value = response.data;
    applyFilter();
  } catch (error) {
    //console.log(error.response); // ← Посмотреть детали ошибки
    console.error('Ошибка обновления товара:', error);
  }
};

const deleteUser = async (id) => {
  if (!confirm('Вы уверены, что хотите удалить этого пользователя?')) return;

  try {
    await api.delete(`/users/${id}`);
    
    // 🔄 Обновляем список товаров
    const response = await api.get('/users/dto');
    users.value = response.data;
  } catch (error) {
    console.error('Ошибка удаления пользователя:', error);
  }
};

// 🔍 Реактивный поиск и фильтрация
// const filteredUsers = computed(() => {
//     const sortUsers = users.value.filter(user => {
//     const matchesSearch = searchQuery.value.trim() === '' || 
//   user.code.toLowerCase().includes(searchQuery.value.toLowerCase());
//     return matchesSearch;})

//     return selectedRole.value
//     ? sortUsers.filter(user => user.userRole === selectedRole.value.name)
//     : sortUsers;
// });

// Загружаем данные при монтировании компонента
onMounted(() => {
    fetchUsers();
    fetchRoles();
    //fetchManagers();
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