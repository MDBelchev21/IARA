import { apiClient } from '../config/api';
import type { ShipCrew, BaseFilter } from '../types';

export const shipCrewService = {
  getAll: async (filters: BaseFilter<any>): Promise<ShipCrew[]> => {
    const response = await apiClient.post<ShipCrew[]>('/ShipCrew/GetAll', filters);
    return response.data;
  },

  get: async (id: number): Promise<ShipCrew> => {
    const response = await apiClient.get<ShipCrew>(`/ShipCrew/Get/${id}`);
    return response.data;
  },

  add: async (crew: ShipCrew): Promise<number> => {
    const response = await apiClient.post<number>('/ShipCrew/Add', crew);
    return response.data;
  },

  edit: async (crew: ShipCrew): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/ShipCrew/Edit', crew);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/ShipCrew/Delete/${id}`);
    return response.data;
  },
};
