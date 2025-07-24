import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import { ErrorProvider } from './context/ErrorContext';
import { ErrorSidebar } from './components/Other/ErrorSidebar';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
     <ErrorProvider>
      <ErrorSidebar />
        <App />
      </ErrorProvider>
  </React.StrictMode>
);
