import { apiClient } from '../config/api';
import type { RecreationalCatch, BaseFilter } from '../types';

export const recreationalCatchService = {
  getAll: async (filters: BaseFilter<any>): Promise<RecreationalCatch[]> => {
    const response = await apiClient.post<RecreationalCatch[]>('/RecreationalCatch/GetAll', filters);
    return response.data;
  },

  get: async (id: number): Promise<RecreationalCatch> => {
    const response = await apiClient.get<RecreationalCatch>(`/RecreationalCatch/Get/${id}`);
    return response.data;
  },

  add: async (data: RecreationalCatch): Promise<number> => {
    const response = await apiClient.post<number>('/RecreationalCatch/Add', data);
    return response.data;
  },

  update: async (data: RecreationalCatch): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/RecreationalCatch/Edit', data);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/RecreationalCatch/Delete/${id}`);
    return response.data;
  },
};
