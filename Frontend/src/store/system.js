import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '@/api'

export const useSystemStore = defineStore('system', () => {
  const apiStatus = ref('Проверка подключения...')

  async function checkApiStatus() {
    //console.log('check');
    try {
      await api.get('/health') // Заменить на актуальный URL проверки API
      apiStatus.value = 'Система: Онлайн ✅'
    } catch (error) {
      apiStatus.value = 'Система: Офлайн ❌'
    }
  }

  
  setInterval(checkApiStatus, 1000) // Проверяем каждые 1 секунд
  checkApiStatus() // Проверяем сразу при загрузке

  return { apiStatus }
})
