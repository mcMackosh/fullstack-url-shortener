import React, { useState } from 'react';
import { observer } from 'mobx-react-lite';
import { login } from '../../api/auth';
import { userStore } from '../../stores/UserStore';
import { useNavigate } from 'react-router-dom';
import { useError } from '../../context/ErrorContext';

export const LoginForm: React.FC = observer(() => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { showError } = useError();

  const handleLogin = async () => {
    try {
      const data = await login({ username, password });
      userStore.login({ username: data.username, role: data.role, id: data.id }, data.token);
      navigate('/urls');
    } catch (error: any) {
      const message = error?.response?.data?.message || 'Registration failed';
      showError(message);
      console.error(error);
    }
  };

  return (
    <div>
      <h2>Увійти</h2>
      <input
        type="text"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        placeholder="Username"
      />
      <input
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        placeholder="Password"
      />
      <button onClick={handleLogin}>Enter</button>
    </div>
  );
});
