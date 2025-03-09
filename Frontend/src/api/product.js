import api from '@/api.js';

const getFilteredProducts = async (params) => {
  try {
    const response = await api.get('/products', params);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const getCategories = async () => {
  try {
    const response = await api.get('/categories');
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const addProduct = async (newProduct) => {
  try {
    const response = await api.post('/products', newProduct.value);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const updateProduct = async (productId, editProduct) => {
  try {
    const response = await api.put(`/products/${productId}`, editProduct);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const deleteProduct = async (productId) => {
  try {
    const response = await api.delete(`/products/${id}`);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};