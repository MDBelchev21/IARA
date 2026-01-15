import { apiClient } from '../config/api';
import type { LegalEntity, BaseFilter } from '../types';

export const legalEntityService = {
  getAll: async (filters: BaseFilter<any>): Promise<LegalEntity[]> => {
    const response = await apiClient.post<LegalEntity[]>('/LegalEntity/getall', filters);
    return response.data;
  },

  get: async (id: number): Promise<LegalEntity> => {
    const response = await apiClient.get<LegalEntity>(`/LegalEntity/Get/${id}`);
    return response.data;
  },

  add: async (entity: LegalEntity): Promise<number> => {
    const response = await apiClient.post<number>('/LegalEntity/Add', entity);
    return response.data;
  },

  edit: async (entity: LegalEntity): Promise<boolean> => {
    const response = await apiClient.put<boolean>('/LegalEntity/Edit', entity);
    return response.data;
  },

  delete: async (id: number): Promise<boolean> => {
    const response = await apiClient.delete<boolean>(`/LegalEntity/Delete/${id}`);
    return response.data;
  },
};
