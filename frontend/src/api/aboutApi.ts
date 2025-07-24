import axiosInstance from './axiosInstance';

const API_URL = '/About';

export const updateAbout = async (content: string): Promise<void> => {
  await axiosInstance.put(
    API_URL, { content: content}
  );
};