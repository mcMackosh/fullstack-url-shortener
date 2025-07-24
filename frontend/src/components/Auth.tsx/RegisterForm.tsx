import React, { useState } from 'react';
import { observer } from 'mobx-react-lite';
import { register } from '../../api/auth';
import { useNavigate } from 'react-router-dom';
import { useError } from '../../context/ErrorContext';

export const RegisterForm: React.FC = observer(() => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { showError } = useError();

  const handleRegister = async () => {
    try {
      await register({ username, password });
      alert('Registration successful! Please log in.');
      navigate('/login');
    } catch (error: any) {
      const message = error?.response?.data?.message || 'Registration failed';
        showError(message);
    }
  };

  return (
    <div>
      <h2>Registration</h2>
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
      <button onClick={handleRegister}>Register</button>
    </div>
  );
});
