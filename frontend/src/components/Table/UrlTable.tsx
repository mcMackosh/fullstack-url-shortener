import React from 'react';
import { useNavigate } from 'react-router-dom';
import { deleteUrl, ShortUrlResponseDto } from '../../api/urlApi';
import { userStore } from '../../stores/UserStore';
import { useError } from '../../context/ErrorContext';

interface Props {
  urls: ShortUrlResponseDto[];
  onRefresh: () => void;
}

export const UrlTable: React.FC<Props> = ({ urls, onRefresh }) => {
  const navigate = useNavigate();
  const { showError } = useError();


  const handleDelete = async (id: string, event: React.MouseEvent) => {
    event.stopPropagation();
    try {
      await deleteUrl(id);
      onRefresh();
    } catch (e: any) {
      const message = e.response?.data?.message || 'Error while deleting URL';
      showError(message);
    }
  };

  const canDelete = (createdBy: string) => {
    if (!userStore.user) return false;
    return userStore.user.role === 'ADMIN' || userStore.user.id === createdBy;
  };

  const handleRowClick = (id: string) => {
    navigate(`/short-url/${id}`);
  };

  return (
    <table style={{ width: '100%', marginTop: '20px', borderCollapse: 'collapse' }}>
      <thead>
        <tr>
          <th>Original</th>
          <th>Short</th>
          {userStore.isAuth && <th>Actions</th>}
        </tr>
      </thead>
      <tbody>
        {urls.map((url) => (
          <tr
            key={url.id}
            onClick={() => handleRowClick(url.id)}
            style={{ cursor: 'pointer' }}
          >
            <td>{url.originalUrl}</td>
            <td>{url.shortUrl}</td>
            {userStore.isAuth && (
              <td>
                {canDelete(url.createdBy) && (
                  <button onClick={(e) => handleDelete(url.id, e)}>Видалити</button>
                )}
              </td>
            )}
          </tr>
        ))}
      </tbody>
    </table>
  );
};
