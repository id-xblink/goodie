import api from '@/api.js';

const getFilteredUsers = async (params) => {
  try {
    const response = await api.get('/users/dto', params);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const getRoles = async () => {
  try {
    const response = await api.get('/roles');
    roles.value = response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const addUser = async (password, newUser) => {
  try {
    const response = await api.post(`/users?password=${encodeURIComponent(password)}`, newUser.value);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const updateUser = async (userId, editUser) => {
  try {
    const response = await api.put(`/users/${userId}`, editUser);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

const deleteUser = async (id) => {
  try {
    const response = await api.delete(`/users/${id}`);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

//registerView
const registerUser = async (password, newUser) => {
  try {
    const response = await api.post(`/users/registration?password=${encodeURIComponent(password)}`, newUser);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
};

//authStore
const loginUser = async (user) => {
  try {
    const response = await api.post('/auth/login', user);
    return response.data;
  } catch (error) {
    console.error('Ошибка: ', error);
  }
}