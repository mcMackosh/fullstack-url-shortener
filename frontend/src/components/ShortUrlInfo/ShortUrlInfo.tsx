import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { getUrlById, ShortUrlResponseDto } from '../../api/urlApi';

export const ShortUrlInfo: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [urlInfo, setUrlInfo] = useState<ShortUrlResponseDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id) return;

    setLoading(true);
    getUrlById(id)
      .then(setUrlInfo)
      .catch(() => setError('Failed to load information'))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div style={{ color: 'red' }}>{error}</div>;
  if (!urlInfo) return <div>Url is not found</div>;

  return (
    <div>
      <h2>Info about URL</h2>
      <p><strong>Original URL:</strong> {urlInfo.originalUrl}</p>
      <p><strong>Shorted URL:</strong> {urlInfo.shortUrl}</p>
      <p><strong>Craeted by:</strong> {urlInfo.createdBy}</p>
      <p><strong>Create date:</strong> {new Date(urlInfo.createdAt).toLocaleString()}</p>
      <p><strong>Clicks:</strong> {urlInfo.clicks}</p>
    </div>
  );
};
