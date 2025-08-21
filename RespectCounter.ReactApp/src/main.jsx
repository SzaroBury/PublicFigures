import './index.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { NotificationProvider } from './utils/providers/NotificationProvider/NotificationProvider';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
      <NotificationProvider>
        <App />
      </NotificationProvider>
  </React.StrictMode>
);

reportWebVitals();