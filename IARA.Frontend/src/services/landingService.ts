import { apiClient } from '../config/api';
import type { Landing, BaseFilter } from '../types';

export const landingService = {
  getAll: async (filters: BaseFilter<any>): Promise<Landing[]> => {
    const response = await apiClient.post<Landing[]>('/Landing/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<Landing> => {
    const response = await apiClient.get<Landing>(`/Landing/Get/${id}`);
    return response.data;
  },

  add: async (landing: Landing): Promise<number> => {
    const response = await apiClient.post<number>('/Landing/Add', landing);
    return response.data;
  },

  edit: async (landing: Landing): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/Landing/Edit', landing);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/Landing/Delete/${id}`);
    return response.data;
  },
};
