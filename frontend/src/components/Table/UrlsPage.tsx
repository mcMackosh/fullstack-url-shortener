import React, { useEffect, useState } from 'react';

import { observer } from 'mobx-react-lite';
import { userStore } from '../../stores/UserStore';
import { fetchAllUrls, ShortUrlResponseDto } from '../../api/urlApi';
import { AddUrlForm } from './AddUrlForm';
import { UrlTable } from './UrlTable';
import { useError } from '../../context/ErrorContext';

export const UrlsPage: React.FC = observer(() => {
  const [urls, setUrls] = useState<ShortUrlResponseDto[]>([]);
  const { showError } = useError();

   const loadUrls = async () => {
    try {
      const data = await fetchAllUrls();
      setUrls(data);
    } catch (e: any) {
      showError(e.response?.data?.message || 'Error loading URLs');
    }
  };

  useEffect(() => {
    loadUrls();
  }, []);

  return (
    <div style={{ maxWidth: '800px', margin: '0 auto', paddingTop: '40px' }}>
      <h2>Short URLs</h2>
      {userStore.isAuth && <AddUrlForm onSuccess={loadUrls} />}
      <UrlTable urls={urls} onRefresh={loadUrls} />
    </div>
  );
});
