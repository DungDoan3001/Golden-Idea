import React from 'react';
import ReactDOM from 'react-dom/';
import './index.css';
import App from './App';
import { Provider } from 'react-redux';
import { store } from './app/store/configureStore';
import { BrowserRouter } from 'react-router-dom';
import { ContextProvider } from './app/context/ContextProvider';
import { createBrowserHistory } from 'history';
export const history = createBrowserHistory();
ReactDOM.render(
  <React.StrictMode>
    <BrowserRouter>
      <Provider store={store}>
        <ContextProvider>
          <App />
        </ContextProvider>
      </Provider>
    </BrowserRouter>
  </React.StrictMode>,
  document.getElementById('root')
);
