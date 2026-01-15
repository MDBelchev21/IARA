import { apiClient } from '../config/api';
import type { Inspection, BaseFilter } from '../types';

export const inspectionService = {
  getAll: async (filters: BaseFilter<any>): Promise<Inspection[]> => {
    const response = await apiClient.post<Inspection[]>('/Inspection/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<Inspection> => {
    const response = await apiClient.get<Inspection>(`/Inspection/Get/${id}`);
    return response.data;
  },

  add: async (inspection: Inspection): Promise<number> => {
    const response = await apiClient.post<number>('/Inspection/Add', inspection);
    return response.data;
  },

  edit: async (inspection: Inspection): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/Inspection/Edit', inspection);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/Inspection/Delete/${id}`);
    return response.data;
  },

  complete: async (id: number): Promise<boolean> => {
    const response = await apiClient.post<boolean>(`/Inspection/CompleteInspection/${id}`);
    return response.data;
  },
};
