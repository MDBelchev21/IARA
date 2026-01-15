import { apiClient } from '../config/api';
import type { Catch, BaseFilter } from '../types';

export const catchService = {
  async getAll(filter: BaseFilter<any>): Promise<Catch[]> {
    const response = await apiClient.post('/Catch/GetAll', filter);
    return response.data;
  },

  async get(id: number): Promise<Catch> {
    const response = await apiClient.get(`/Catch/${id}`);
    return response.data;
  },

  async getByOperation(operationId: number): Promise<Catch[]> {
    const response = await apiClient.get(`/Catch/ByOperation/${operationId}`);
    return response.data;
  },

  async add(catchData: Catch): Promise<Catch> {
    const response = await apiClient.post('/Catch/Add', catchData);
    return response.data;
  },

  async edit(catchData: Catch): Promise<void> {
    await apiClient.put(`/Catch/Edit`, catchData);
  },

  async delete(id: number): Promise<void> {
    await apiClient.delete(`/Catch/Delete/${id}`);
  },
};
