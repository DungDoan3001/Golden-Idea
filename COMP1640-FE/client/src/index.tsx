import React from 'react';
import ReactDOM from 'react-dom/';
import './index.css';
import { Provider } from 'react-redux';
import { store } from './app/store/configureStore';
import { RouterProvider } from 'react-router-dom';
import { ContextProvider } from './app/context/ContextProvider';
import { createBrowserHistory } from 'history';
import { router } from './app/routes/Routers';
export const history = createBrowserHistory();
ReactDOM.render(
  <React.StrictMode>
    <Provider store={store}>
      <ContextProvider>
        <RouterProvider router={router} />
      </ContextProvider>
    </Provider>
  </React.StrictMode>,
  document.getElementById('root')
);
