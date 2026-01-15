import { apiClient } from '../config/api';
import type { Person, BaseFilter } from '../types';

export const inspectorService = {
  getAll: async (filters: BaseFilter<any>): Promise<Person[]> => {
    const response = await apiClient.post<Person[]>('/Inspector/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<Person> => {
    const response = await apiClient.get<Person>(`/Inspector/Get/${id}`);
    return response.data;
  },

  add: async (person: Person): Promise<number> => {
    const response = await apiClient.post<number>('/Inspector/Add', person);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/Inspector/Delete/${id}`);
    return response.data;
  },

  makeExisting: async (personId: number): Promise<number> => {
    const response = await apiClient.post<number>(`/Inspector/MakeExisting?personId=${personId}`);
    return response.data;
  },
};
