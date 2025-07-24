import axios from 'axios';
import { API_URL } from './axiosInstance';



export interface LoginRequestDto {
  username: string;
  password: string;
}

export interface LoginResponseDto {
  token: string;
  username: string;
  role: string;
  id: string;
}

export interface RegisterUserDto {
  username: string;
  password: string;
}

export const login = async (credentials: LoginRequestDto): Promise<LoginResponseDto> => {
  const response = await axios.post<LoginResponseDto>(`${API_URL}/auth/login`, credentials);
  return response.data;
};

export const register = async (data: RegisterUserDto): Promise<void> => {
  await axios.post(`${API_URL}/auth/register`, data);
};
