import { apiClient } from '../config/api';
import type { LoginRequest, RegisterRequest, LoginResponse, RefreshTokenRequest, ValidateTokenResponse } from '../types';

export const authService = {
  login: async (data: LoginRequest): Promise<LoginResponse> => {
    const response = await apiClient.post<LoginResponse>('/Auth/login', data);
    return response.data;
  },

  register: async (data: RegisterRequest): Promise<LoginResponse> => {
    const response = await apiClient.post<LoginResponse>('/Auth/register', data);
    return response.data;
  },

  refresh: async (data: RefreshTokenRequest): Promise<LoginResponse> => {
    const response = await apiClient.post<LoginResponse>('/Auth/refresh', data);
    return response.data;
  },

  logout: async (): Promise<void> => {
    await apiClient.post('/Auth/logout');
  },

  validate: async (): Promise<ValidateTokenResponse> => {
    const response = await apiClient.get<ValidateTokenResponse>('/Auth/validate');
    return response.data;
  },
};
