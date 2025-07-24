import React, { createContext, useContext, useState, ReactNode } from 'react';

interface ErrorContextType {
  message: string | null;
  showError: (msg: string) => void;
  hideError: () => void;
}

const ErrorContext = createContext<ErrorContextType | undefined>(undefined);

export const ErrorProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [message, setMessage] = useState<string | null>(null);

  const showError = (msg: string) => setMessage(msg);
  const hideError = () => setMessage(null);

  return (
    <ErrorContext.Provider value={{ message, showError, hideError }}>
      {children}
    </ErrorContext.Provider>
  );
};

export const useError = (): ErrorContextType => {
  const context = useContext(ErrorContext);
  if (!context) {
    throw new Error('some error');
  }
  return context;
};
