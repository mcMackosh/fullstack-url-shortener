import React from 'react';
import { useNavigate } from 'react-router-dom';
import { userStore } from '../../stores/UserStore';

const Header: React.FC = () => {
    const navigate = useNavigate();

    const logout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('username');
        userStore.logout();
        navigate('/login');
        
    };

    const goTo = (path: string) => navigate(path);

    return (
        <div style={{
            display: 'flex',
            alignItems: 'center',
            padding: '10px',
            background: '#f0f0f0',
            borderBottom: '1px solid #ccc'
        }}>
            <button onClick={() => goTo('/')}>main</button>

            <div style={{ flexGrow: 1 }}></div>

            {userStore.isAuth && <span>{userStore.user?.username}:{userStore.user?.role}</span>}
            <div></div>
            {!userStore.isAuth && (
                <>
                    <button onClick={() => goTo('/login')}>log in</button>
                    <button onClick={() => goTo('/registration')}>check in</button>
                </>
            )}

            {userStore.isAuth && <button onClick={logout}>log out</button>}
            <button onClick={() => goTo('/about')}>about</button>
        </div>
    );
};

export default Header;
