import { apiClient } from '../config/api';
import type { ShipOwner, BaseFilter } from '../types';

export const shipOwnerService = {
  getAll: async (filters: BaseFilter<any>): Promise<ShipOwner[]> => {
    const response = await apiClient.post<ShipOwner[]>('/ShipOwner/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<ShipOwner> => {
    const response = await apiClient.get<ShipOwner>(`/ShipOwner/Get/${id}`);
    return response.data;
  },

  add: async (owner: ShipOwner): Promise<number> => {
    const response = await apiClient.post<number>('/ShipOwner/Add', owner);
    return response.data;
  },

  edit: async (owner: ShipOwner): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/ShipOwner/Edit', owner);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/ShipOwner/Delete/${id}`);
    return response.data;
  },
};
