import { apiClient } from '../config/api';
import type { Person, BaseFilter } from '../types';

export const personService = {
  getAll: async (filters: BaseFilter<any>): Promise<Person[]> => {
    const response = await apiClient.post<Person[]>('/Person/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<Person> => {
    const response = await apiClient.get<Person>(`/Person/Get/${id}`);
    return response.data;
  },

  add: async (person: Person): Promise<number> => {
    const response = await apiClient.post<number>('/Person/Add', person);
    return response.data;
  },

  edit: async (person: Person): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/Person/Edit', person);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/Person/Delete/${id}`);
    return response.data;
  },
};
