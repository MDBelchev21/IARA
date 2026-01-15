import { apiClient } from '../config/api';
import type { RecreationalTicketType, BaseFilter } from '../types';

export const recreationalTicketTypeService = {
  getAll: async (filters: BaseFilter<any>): Promise<RecreationalTicketType[]> => {
    const response = await apiClient.post<RecreationalTicketType[]>('/RecreationalTicketType/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<RecreationalTicketType> => {
    const response = await apiClient.get<RecreationalTicketType>(`/RecreationalTicketType/Get/${id}`);
    return response.data;
  },

  add: async (data: RecreationalTicketType): Promise<number> => {
    const response = await apiClient.post<number>('/RecreationalTicketType/Add', data);
    return response.data;
  },

  update: async (data: RecreationalTicketType): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/RecreationalTicketType/Edit', data);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/RecreationalTicketType/Delete/${id}`);
    return response.data;
  },
};
