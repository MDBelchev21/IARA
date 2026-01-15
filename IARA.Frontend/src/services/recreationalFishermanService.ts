import { apiClient } from '../config/api';
import type { RecreationalFisherman, BaseFilter } from '../types';

export const recreationalFishermanService = {
  getAll: async (filters: BaseFilter<any>): Promise<RecreationalFisherman[]> => {
    const response = await apiClient.post<RecreationalFisherman[]>('/RecreationalFisherman/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<RecreationalFisherman> => {
    const response = await apiClient.get<RecreationalFisherman>(`/RecreationalFisherman/Get/${id}`);
    return response.data;
  },

  add: async (fisherman: RecreationalFisherman): Promise<number> => {
    const response = await apiClient.post<number>('/RecreationalFisherman/Add', fisherman);
    return response.data;
  },

  edit: async (fisherman: RecreationalFisherman): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/RecreationalFisherman/Edit', fisherman);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/RecreationalFisherman/Delete/${id}`);
    return response.data;
  },
};
