import { apiClient } from '../config/api';
import type { FishingPermit, BaseFilter } from '../types';

export const fishingPermitService = {
  getAll: async (filters: BaseFilter<any>): Promise<FishingPermit[]> => {
    const response = await apiClient.post<FishingPermit[]>('/FishingPermit/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<FishingPermit> => {
    const response = await apiClient.get<FishingPermit>(`/FishingPermit/Get/${id}`);
    return response.data;
  },

  add: async (permit: FishingPermit): Promise<number> => {
    const response = await apiClient.post<number>('/FishingPermit/Add', permit);
    return response.data;
  },

  edit: async (permit: FishingPermit): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/FishingPermit/Edit', permit);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/FishingPermit/Delete/${id}`);
    return response.data;
  },

  revoke: async (id: number): Promise<boolean> => {
    const response = await apiClient.post<boolean>(`/FishingPermit/RevokePermit/${id}`);
    return response.data;
  },

  isValid: async (id: number): Promise<boolean> => {
    const response = await apiClient.get<boolean>(`/FishingPermit/IsPermitValid/${id}`);
    return response.data;
  },
};
