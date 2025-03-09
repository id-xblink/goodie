<template>
  <div>
    <h1>📜 Мои заказы</h1>

    <div class="filters">
      <label>Фильтр по статусу:</label>
      <select v-model="selectedStatus">
        <option value="">Все</option>
        <option v-for="status in statuses" :key="status" :value="status.id">{{ status.name }}</option>
      </select>
    </div>

    <table v-if="orders.length > 0">
      <thead>
        <tr>
          <th>№ заказа</th>
          <th>Дата</th>
          <th>Статус</th>
          <th>Доставка</th>
          <th>Действия</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="order in filteredOrders" :key="order.id">
          <td>{{ order.orderNumber }}</td>
          <td>{{ new Date(order.orderDate).toLocaleString() }}</td>
          <td>{{ order.status }}</td>
          <td>{{ order.shipmentDate ? new Date(order.shipmentDate).toLocaleDateString() : "—" }}</td>
          <td v-if="authStore.role === 'Manager' || authStore.role === 'Admin'">
            <!-- <button v-if="order.status === 'Новый'" @click="updateOrderStatus(order, getStatusId('Выполняется'))">📦 Начать выполнение</button> -->
            <button v-if="authStore.isAdmin && order.status === 'Новый'" @click="openDateModal(order)">📦 Начать выполнение</button>
            <button v-if="authStore.isAdmin && order.status === 'Выполняется' && new Date(order.shipmentDate).toLocaleDateString() <= new Date().toLocaleDateString()" @click="updateOrderStatus(order, getStatusId('Выполнен'))">✅ Завершить</button>
          </td>
          <td v-if="order.status === 'Новый'">
            <button @click="deleteOrder(order.id)">❌ Удалить</button>
          </td>
          <td>
            <button class="details-btn" @click="openOrderDetails(order)">🔍 Подробнее</button>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-else>📭 У вас пока нет заказов.</p>

    <div v-if="showModal" class="modal">
      <div class="modal-content">
        <button class="close-btn" @click="showModal = false">✖</button>
        <h3>🛍 Товары в заказе #{{ selectedOrder?.orderNumber }}</h3>
        <table>
          <thead>
            <tr>
              <th>Название</th>
              <th>Цена</th>
              <th>Кол-во</th>
              <th>Сумма</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in orderItems" :key="item.productId">
              <td>{{ item.productName }}</td>
              <td>{{ item.price }} ₽</td>
              <td>{{ item.quantity }}</td>
              <td>{{ item.price * item.quantity }} ₽</td>
            </tr>
          </tbody>
        </table>
        <p><strong>💰 Итог:</strong> {{ totalAmount }} ₽</p>
      </div>
    </div>

      <!-- Модальное окно выбора даты -->
      <div v-if="showDateModal" class="modal">
        <div class="modal-content">
          <button class="close-btn" @click="showDateModal = false">✖</button>
          <h3>📅 Выберите дату выполнения</h3>
          <input type="date" v-model="selectedDate" :min="minDate" :max="maxDate">
          <button @click="confirmStartOrder">✅ Подтвердить</button>
        </div>
      </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import api from '@/api.js';
import { useAuthStore } from '@/store/authStore';

const authStore = useAuthStore();
const orders = ref([]);
const selectedStatus = ref('');
const statuses = ref([]);

const showModal = ref(false);
const selectedOrder = ref(null);
const orderItems = ref([]);


const showDateModal = ref(false);
const selectedDate = ref('');

const openOrderDetails = async (order) => {
  selectedOrder.value = order;
  showModal.value = true;

  try {
    const response = await api.get(`/orders/${order.id}`);
    orderItems.value = response.data.items; // Получаем товары в заказе
  } catch (error) {
    console.error("Ошибка загрузки товаров заказа:", error);
  }
};

//Вычисление минимальной даты
const minDate = computed(() =>
   new Date().toISOString().split('T')[0]
);

//Вычисление максимальной даты
const maxDate = computed(() => {
    const date = new Date();
    date.setDate(date.getDate() + 14);
    return date.toISOString().split('T')[0]; // Преобразуем в формат "YYYY-MM-DD"
});
    //new Date().setDate(new + 14).toISOString().split('T')[0]

    
//);

// Вычисляем итоговую сумму заказа
const totalAmount = computed(() => 
  orderItems.value.reduce((sum, item) => sum + item.price * item.quantity, 0)
);

const openDateModal = async (order) => {
      selectedOrder.value = order;
      showDateModal.value = true;
      selectedDate.value = minDate.value;
};

const closeDateModal = async () => {
  selectedOrder.value = null;
    showDateModal.value = false;
    selectedDate.value = null;
};

const fetchStatuses = async () => {
  try {
    const response = await api.get('/orderstatus'); // 👈 Получаем статусы с backend
    statuses.value = response.data; // [{ id: "...", name: "Новый" }, { id: "...", name: "Выполняется" }, ...]
  } catch (error) {
    console.error("Ошибка загрузки статусов:", error);
  }
};

// const fetchOrders = async () => {
//   try {
//     const response = await api.get(`/orders?customerId=${authStore.userId}`);
//     orders.value = response.data;
//   } catch (error) {
//     console.error("Ошибка загрузки заказов:", error);
//   }
// };

const fetchOrders = async () => {
  try {
    // let response;
    // if (authStore.isAdmin) {
    //   // Получить всё
    //   response = await api.get(`/orders?customerId=`).data;
    // } else {
    //   // Получить только своё
    //   response = await api.get(`/orders?customerId=${authStore.userId}`).data;
    // }
    const response = await api.get(`/orders?customerId=${authStore.isAdmin ? '' : authStore.userId}`);
    orders.value = response.data;
  } catch (error) {
    console.error("Ошибка загрузки заказов:", error);
  }
};

const getStatusId = (statusName) => {
  return statuses.value.find(s => s.name === statusName)?.id || null;
};

const getStatusName = (statusId) => {
  //console.log(statusId);
  return statuses.value.find(s => s.id === statusId)?.name || null;
};

const confirmStartOrder = async () => {
  //console.log(selectedDate.value);
  //console.log("Выбранная дата:", selectedDate.value instanceof Date ? selectedDate.value.toISOString().split("T")[0] : selectedDate.value);

  //return;
  try {
    if (!selectedDate.value) {
      alert("Введите дату!");
      return;
    }
    const payload = { 
      statusId: getStatusId("Выполняется"),
      shipmentDate: new Date(selectedDate.value).toISOString()
    };
    

    //payload.shipmentDate = new Date().toISOString();
    //order.shipmentDate = payload.shipmentDate; // ✅ Обновляем в локальном списке
    // console.log(`/orders/${selectedOrder.value.id}/status`);
    // console.log(payload);
    // console.log(new Date().toISOString())
    // return;
    await api.put(`/orders/${selectedOrder.value.id}/status`, payload);
    closeDateModal();
    fetchOrders();
  }
  catch (error) {
    console.error("Ошибка обновления заказа:", error);
    alert("Не удалось изменить статус заказа.");
  }
};

const updateOrderStatus = async (order, newStatusId) => {
  if (!newStatusId) {
    alert("Ошибка: статус не найден!");
    return;
  }

  try {
    const payload = { statusId: newStatusId };

    // Если устанавливаем "Выполняется" → передаём ShipmentDate
    if (newStatusId === getStatusId("Выполняется")) {
      payload.shipmentDate = new Date().toISOString();
      order.shipmentDate = payload.shipmentDate; // ✅ Обновляем в локальном списке
    }

    await api.put(`/orders/${order.id}/status`, payload);
    //order.status = newStatus; // Обновляем локально
    order.status = statuses.value.find(s => s.id === newStatusId)?.name || "Неизвестно";
    alert(`Статус заказа #${order.orderNumber} изменён на "${order.status}"!`);
  } catch (error) {
    console.error("Ошибка обновления заказа:", error);
    alert("Не удалось изменить статус заказа.");
  }
};

const deleteOrder = async (orderId) => {
  if (!confirm("Вы уверены, что хотите удалить этот заказ?")) return;

  try {
    await api.delete(`/orders/${orderId}`);
    orders.value = orders.value.filter(order => order.id !== orderId); // Удаляем заказ из списка
    alert("✅ Заказ успешно удалён!");
  } catch (error) {
    console.error("Ошибка удаления заказа:", error);
    alert("❌ Не удалось удалить заказ.");
  }
};

// Фильтрация заказов по статусу
const filteredOrders = computed(() => {
  return selectedStatus.value
    ? orders.value.filter(order => order.status === getStatusName(selectedStatus.value))
    : orders.value;
});

onMounted(fetchOrders);
onMounted(fetchStatuses);
</script>

<style scoped>
.filters {
  margin-bottom: 10px;
}
</style>

<style scoped>
/* Затемнение фона */
.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

/* Контейнер окна */
.modal-content {
  background: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  width: 500px;
  max-width: 90%;
  text-align: center;
  position: relative;
  animation: fadeIn 0.3s ease-in-out;
}

/* Кнопка закрытия */
.close-btn {
  position: absolute;
  top: 10px;
  right: 15px;
  background: transparent;
  border: none;
  font-size: 20px;
  cursor: pointer;
  color: #666;
}

.close-btn:hover {
  color: black;
}

/* Анимация появления */
@keyframes fadeIn {
  from { opacity: 0; transform: scale(0.9); }
  to { opacity: 1; transform: scale(1); }
}

/* Кнопка "Подробнее" в виде иконки */
.details-btn {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 18px;
}
.details-btn:hover {
  color: #007bff;
}
</style>
