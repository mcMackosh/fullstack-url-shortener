import React, { useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { userStore } from '../../stores/UserStore';
import axios from 'axios';
import { useError } from '../../context/ErrorContext';

export const AboutPage: React.FC = observer(() => {
  const [description, setDescription] = useState('');
  const [editDescription, setEditDescription] = useState('');
  const [isEditing, setIsEditing] = useState(false);
  
  const { showError } = useError();

  useEffect(() => {
    const fetchDescription = async () => {
      try {
        const response = await axios.get('https://localhost:7286/api/About');
        setDescription(response.data.description);
        setEditDescription(response.data.description);
      } catch (error: any) {
        showError(error.response?.data?.message || 'Failed to load description');
      }
    };

    fetchDescription();
  }, [showError]);

  const handleSave = async () => {
    try {
      await axios.put(
        'https://localhost:7286/api/About',
        { description: editDescription },
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        }
      );
      setDescription(editDescription);
      setIsEditing(false);
    } catch (error: any) {
      showError(error.response?.data?.message || 'Failed to update description');
    }
  };

  const isAdmin = userStore.isAuth && userStore.user?.role === 'ADMIN';

  return (
    <div className="container mx-auto mt-8 max-w-3xl">
      <h1 className="text-2xl font-bold mb-4">About URL Shortener</h1>

      {isEditing ? (
        <div>
          <textarea
            className="w-full h-40 border rounded p-2"
            value={editDescription}
            onChange={(e) => setEditDescription(e.target.value)}
          />
          <div className="mt-2 flex gap-2">
            <button
              className="bg-blue-500 text-white px-4 py-2 rounded"
              onClick={handleSave}
            >
              Save
            </button>
            <button
              className="bg-gray-400 text-white px-4 py-2 rounded"
              onClick={() => setIsEditing(false)}
            >
              Cancel
            </button>
          </div>
        </div>
      ) : (
        <>
          <p className="whitespace-pre-line">{description}</p>
          {isAdmin && (
            <button
              className="mt-4 bg-blue-500 text-white px-4 py-2 rounded"
              onClick={() => setIsEditing(true)}
            >
              Edit
            </button>
          )}
        </>
      )}
    </div>
  );
});
