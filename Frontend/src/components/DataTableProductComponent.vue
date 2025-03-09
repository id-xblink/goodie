<template>
  <table class="table">
    <thead>
      <tr>
        <th v-for="col in props.columns" :key="col">{{ col }}</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="row in props.data" :key="row.id">
        <!-- <td v-for="col in props.columns" :key="col">{{ row[col] }}</td> -->
        <td v-for="col in props.columns" :key="col">
          <template v-if="col === 'actions'">
            <button v-if="authStore.isAdmin" @click="$emit('edit', row)">✏️</button>
            <button v-if="authStore.isAdmin" @click="$emit('delete', row.id)">🗑️</button>
            <button v-if="!authStore.isAdmin" @click="$emit('addToCart', row)">🛒</button>
          </template>
          <template v-else>
            {{ row[col] }}
          </template>
        </td>
      </tr>
    </tbody>
  </table>
</template>

<script setup>
import { useAuthStore } from '@/store/authStore';
const authStore = useAuthStore();

const props = defineProps({
  columns: Array, // Заголовки столбцов
  data: Array,    // Данные для таблицы
  role: String
});

console.log(props);
</script>

<style scoped>
.table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}
.table th, .table td {
  border: 1px solid #ddd;
  padding: 10px;
  text-align: left;
}
.table th {
  background: #2c3e50;
  color: white;
}
</style>
