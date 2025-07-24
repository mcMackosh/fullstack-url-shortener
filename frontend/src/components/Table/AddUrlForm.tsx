import React, { useState } from 'react';
import { createUrl } from '../../api/urlApi';
import { useError } from '../../context/ErrorContext';

interface Props {
  onSuccess: () => void;
}

export const AddUrlForm: React.FC<Props> = ({ onSuccess }) => {
  const [url, setUrl] = useState('');
  const { showError } = useError();

  const handleAdd = async () => {
    try {
      await createUrl({ originalUrl: url });
      setUrl('');
      onSuccess();
    } catch (e: any) {
      const message = e.response?.data?.message || 'Error while creating URL';
      showError(message);
    }
  };

  return (
    <div>
      <input
        type="text"
        value={url}
        onChange={(e) => setUrl(e.target.value)}
        placeholder="Enter URL"
      />
      <button onClick={handleAdd}>Short</button>
    </div>
  );
};
