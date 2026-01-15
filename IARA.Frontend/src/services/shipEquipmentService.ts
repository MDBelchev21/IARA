import { apiClient } from '../config/api';
import type { ShipEquipment, BaseFilter } from '../types';

export const shipEquipmentService = {
  async getAll(filter: BaseFilter<any>): Promise<ShipEquipment[]> {
    const response = await apiClient.post('/ShipEquipment/GetAll', filter);
    return response.data;
  },

  async get(id: number): Promise<ShipEquipment> {
    const response = await apiClient.get(`/ShipEquipment/Get/${id}`);
    return response.data;
  },

  async add(equipment: ShipEquipment): Promise<number> {
    const response = await apiClient.post('/ShipEquipment/Add', equipment);
    return response.data;
  },

  async edit(equipment: ShipEquipment): Promise<boolean> {
    const response = await apiClient.put('/ShipEquipment/Edit', equipment);
    return response.data;
  },

  async delete(id: number): Promise<boolean> {
    const response = await apiClient.delete(`/ShipEquipment/Delete/${id}`);
    return response.data;
  },
};
