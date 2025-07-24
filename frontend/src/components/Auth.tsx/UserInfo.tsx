import React from 'react';
import { observer } from 'mobx-react-lite';
import { userStore } from '../../stores/UserStore';

export const UserInfo: React.FC = observer(() => {
  return (
    <div>
      {userStore.isAuth ? (
        <>
          <p>Hello, {userStore.user?.username}!</p>
          <p>Role: {userStore.user?.role}</p>
          <button onClick={() => userStore.logout()}>Enter</button>
        </>
      ) : (
        <p>Not auth</p>
      )}
    </div>
  );
});
