import api from '@/api.js';



 const fetchCategories1 = async () => {
    try {
      const response = await api.get('/categories');
      return response.data;
    } catch (error) {
      console.error('Ошибка загрузки категорий:', error);
    }
  };

 const fetchCategories2 = async () => {
    try {
      const response = await api.get('/categories');
      return response.data;
    } catch (error) {
      console.error('Ошибка загрузки категорий:', error);
    }
  };

export const objectApiManager1 = { fetchCategories1 };
export const objectApiManager2 = { fetchCategories2 };


let Dima=(bur) =>  {
    console.log(bur);
}
export default Dima;