import { apiClient } from '../config/api';
import type { RecreationalTicket, BaseFilter } from '../types';

export const recreationalTicketService = {
  getAll: async (filters: BaseFilter<any>): Promise<RecreationalTicket[]> => {
    const response = await apiClient.post<RecreationalTicket[]>('/RecreationalTicket/GetAll', filters);
    return response.data;
  },

  get: async (id: number): Promise<RecreationalTicket> => {
    const response = await apiClient.get<RecreationalTicket>(`/RecreationalTicket/Get/${id}`);
    return response.data;
  },

  add: async (ticket: RecreationalTicket): Promise<number> => {
    const response = await apiClient.post<number>('/RecreationalTicket/Add', ticket);
    return response.data;
  },

  edit: async (ticket: RecreationalTicket): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/RecreationalTicket/Edit', ticket);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/RecreationalTicket/Delete/${id}`);
    return response.data;
  },

  deactivate: async (id: number): Promise<boolean> => {
    const response = await apiClient.post<boolean>(`/RecreationalTicket/DeactivateTicket/${id}`);
    return response.data;
  },
};
