import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { LoginForm } from './components/Auth.tsx/LoginForm';
import { userStore } from './stores/UserStore';
import { UrlsPage } from './components/Table/UrlsPage';
import { ShortUrlInfo } from './components/ShortUrlInfo/ShortUrlInfo';
import { JSX } from 'react';
import Header from './components/Other/Header';
import { AboutPage } from './components/About/About';
import { RegisterForm } from './components/Auth.tsx/RegisterForm';

const RequireAuth: React.FC<{ children: JSX.Element }> = ({ children }) => {
  if (!userStore.isAuth) {
    return <Navigate to="/login" replace />;
  }
  return children;
};

function App() {
  return (
    <BrowserRouter>
    <Header />
      <Routes>
        <Route path="/login" element={<LoginForm />} />
        <Route path="/registration" element={<RegisterForm />} />
        <Route
          path="/main"
          element={
            <UrlsPage />
          }
        />

        <Route
          path="/short-url/:id"
          element={
            <RequireAuth>
              <ShortUrlInfo />
            </RequireAuth>
          }
        />
        <Route path="/about" element={<AboutPage />} />

        <Route path="*" element={<Navigate to="/main" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
