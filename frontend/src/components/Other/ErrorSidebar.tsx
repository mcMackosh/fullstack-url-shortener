import React, { useEffect } from 'react';
import { useError } from '../../context/ErrorContext';

export const ErrorSidebar: React.FC = () => {
  const { message, hideError } = useError();

  useEffect(() => {
    if (!message) return;

    const timer = setTimeout(() => {
      hideError();
    }, 4000);

    return () => clearTimeout(timer);
  }, [message, hideError]);

  if (!message) return null;

  return (
    <div
      style={{
        position: 'fixed',
        bottom: 20,
        right: 20,
        maxWidth: 320,
        backgroundColor: '#ffe6e6',
        border: '1px solid #f5c2c2',
        borderRadius: 8,
        padding: '12px 16px',
        boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
        color: '#a00',
        fontWeight: '600',
        display: 'flex',
        alignItems: 'center',
        gap: 12,
        zIndex: 10000,
        cursor: 'default',
        userSelect: 'none',
        animation: 'fadeInUp 0.3s ease forwards',
      }}
    >
      <div style={{ flex: 1, fontSize: 14, lineHeight: '1.3' }}>{message}</div>
      <button
        onClick={hideError}
        aria-label="Close error message"
        style={{
          background: 'transparent',
          border: 'none',
          fontSize: 18,
          lineHeight: 1,
          cursor: 'pointer',
          color: '#a00',
          fontWeight: 'bold',
          padding: 0,
          margin: 0,
          userSelect: 'none',
        }}
      >
        ✖
      </button>

      <style>{`
        @keyframes fadeInUp {
          from {
            opacity: 0;
            transform: translateY(10px);
          }
          to {
            opacity: 1;
            transform: translateY(0);
          }
        }
      `}</style>
    </div>
  );
};
