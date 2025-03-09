import api from '@/api.js';

const getDetailedOrder = async (orderId) => {
  try {
    const response = await api.get(`/orders/${orderId}`);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const getStatuses = async () => {
  try {
    const response = await api.get('/orderstatus');
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const getUserOrders = async (userId, isAdmin) => {
  try {
    const response = await api.get(`/orders?customerId=${isAdmin ? '' : userId}`);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const updateOrderToStart = async (orderId, orderData) => {
  try {
    const response = await api.put(`/orders/${orderId}/status`, orderData);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const updateOrderToFinish = async (orderId, orderData) => {
  try {
    const response = await api.put(`/orders/${orderId}/status`, orderData);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const deleteOrder = async (orderId) => {
  try {
    const response = await api.delete(`/orders/${orderId}`);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};
//cartview
const createOrder = async (orderData) => {
  try {
    const response = await api.post('/orders', orderData);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};