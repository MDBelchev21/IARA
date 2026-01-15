import { apiClient } from '../config/api';
import type { Violation, BaseFilter } from '../types';

export const violationService = {
  getAll: async (filters: BaseFilter<any>): Promise<Violation[]> => {
    const response = await apiClient.post<Violation[]>('/Violation/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<Violation> => {
    const response = await apiClient.get<Violation>(`/Violation/Get/${id}`);
    return response.data;
  },

  add: async (violation: Violation): Promise<number> => {
    const response = await apiClient.post<number>('/Violation/Add', violation);
    return response.data;
  },

  edit: async (violation: Violation): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/Violation/Edit', violation);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/Violation/Delete/${id}`);
    return response.data;
  },

  issueFine: async (id: number, amount: number): Promise<boolean> => {
    const response = await apiClient.post<boolean>(`/Violation/IssueFine/${id}?amount=${amount}`);
    return response.data;
  },
};
