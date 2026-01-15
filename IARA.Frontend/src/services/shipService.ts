import { apiClient } from '../config/api';
import type { Ship, BaseFilter } from '../types';

export const shipService = {
  getAll: async (filters: BaseFilter<any>): Promise<Ship[]> => {
    const response = await apiClient.post<Ship[]>('/Ship/GetAll', filters);
    return response.data;
  },

  get: async (id: number): Promise<Ship> => {
    const response = await apiClient.get<Ship>(`/Ship/Get/${id}`);
    return response.data;
  },

  add: async (ship: Ship): Promise<number> => {
    const response = await apiClient.post<number>('/Ship/Add', ship);
    return response.data;
  },

  edit: async (ship: Ship): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/Ship/Edit', ship);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/Ship/Delete/${id}`);
    return response.data;
  },
};
