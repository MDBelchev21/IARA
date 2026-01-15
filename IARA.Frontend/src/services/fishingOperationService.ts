import { apiClient } from '../config/api';
import type { FishingOperation, BaseFilter } from '../types';

export const fishingOperationService = {
  async getAll(filter: BaseFilter<any>): Promise<FishingOperation[]> {
    const response = await apiClient.post('/FishingOperation/GetAll', filter);
    return response.data;
  },

  async get(id: number): Promise<FishingOperation> {
    const response = await apiClient.get(`/FishingOperation/${id}`);
    return response.data;
  },

  async getByTrip(tripId: number): Promise<FishingOperation[]> {
    const response = await apiClient.get(`/FishingOperation/ByTrip/${tripId}`);
    return response.data;
  },

  async add(operation: FishingOperation): Promise<FishingOperation> {
    const response = await apiClient.post('/FishingOperation/Add', operation);
    return response.data;
  },

  async edit(operation: FishingOperation): Promise<void> {
    await apiClient.put(`/FishingOperation/Edit`, operation);
  },

  async delete(id: number): Promise<void> {
    await apiClient.delete(`/FishingOperation/Delete/${id}`);
  },
};
