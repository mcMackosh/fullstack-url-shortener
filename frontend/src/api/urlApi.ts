import axiosInstance, { API_URL } from './axiosInstance';

export interface CreateUrlRequestDto {
  originalUrl: string;
}
export interface ShortUrlResponseDto {
  id: string;
  originalUrl: string;
  shortUrl: string;
  createdBy: string;
  createdAt: string;
  clicks: number;
}

export interface ShortUrlResponseDto {
  id: string;
  originalUrl: string;
  shortUrl: string;
  createdBy: string;
  createdAt: string;
  clicks: number;
}

export const fetchAllUrls = async (): Promise<ShortUrlResponseDto[]> => {
  const response = await axiosInstance.get(`${API_URL}/Url`);
  return response.data;
};

export const createUrl = async (data: CreateUrlRequestDto): Promise<ShortUrlResponseDto> => {
  const response = await axiosInstance.post(`${API_URL}/Url`, data);
  return response.data;
};

export const deleteUrl = async (id: string): Promise<void> => {
  await axiosInstance.delete(`${API_URL}/Url/${id}`);
};

export const getUrlById = async (id: string): Promise<ShortUrlResponseDto> => {
  const res = await axiosInstance.get(`${API_URL}/Url/${id}`);
  return res.data;
};
