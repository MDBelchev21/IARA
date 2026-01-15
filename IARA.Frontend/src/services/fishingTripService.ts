import { apiClient } from '../config/api';
import type { FishingTrip, BaseFilter } from '../types';

export const fishingTripService = {
  getAll: async (filters: BaseFilter<any>): Promise<FishingTrip[]> => {
    const response = await apiClient.post<FishingTrip[]>('/FishingTrip/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<FishingTrip> => {
    const response = await apiClient.get<FishingTrip>(`/FishingTrip/${id}`);
    return response.data;
  },

  add: async (trip: FishingTrip): Promise<number> => {
    const response = await apiClient.post<number>('/FishingTrip', trip);
    return response.data;
  },

  edit: async (trip: FishingTrip): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/FishingTrip', trip);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/FishingTrip/${id}`);
    return response.data;
  },
};
