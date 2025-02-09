import apiService from './apiService';
import { ENDPOINTS } from '../common/constants';

const ordersService = {
  search: (page: number, searchTerms: string) => {
    return apiService.get(ENDPOINTS.ORDERS_PATH + `?page=${page}&customer=${searchTerms}`);
  },
  mine: () => {
    return apiService.get(ENDPOINTS.ORDERS_PATH + 'mine');
  },
  create: () => {
    return apiService.post(ENDPOINTS.ORDERS_PATH);
  },
  details: (id: number) => {
    return apiService.get(ENDPOINTS.ORDERS_PATH + id);
  },
  cancel: (id: number) => {
    return apiService.put(ENDPOINTS.ORDERS_PATH + id + '/cancel');
  },
  complete: (id: number) => {
    return apiService.put(ENDPOINTS.ORDERS_PATH + id + '/complete');
  }
};

export default ordersService;