import './App.css'
import { BrowserRouter as Router} from 'react-router-dom';
import Header from './components/Header/Header';
import PageRenderer from './utils/pageRenderer/PageRenderer';
import Footer from './components/Footer/Footer';
import { useNotification } from './utils/providers/NotificationProvider/NotificationProvider';
import { setupRefreshInterceptor } from "./utils/interceptors/refreshInterceptor";
import { setupErrorInterceptor } from "./utils/interceptors/errorInterceptor";
import { callHandleLogout } from "./utils/providers/AuthProvider/AuthProvider";
import { AuthProvider } from './utils/providers/AuthProvider/AuthProvider';
import { useEffect } from 'react';

let interceptorsSetupFlag = false;

function App() {
    const { notify } = useNotification();
    
    useEffect(() => {
        if(!interceptorsSetupFlag) {
            setupRefreshInterceptor(notify, callHandleLogout);
            setupErrorInterceptor(notify);
            interceptorsSetupFlag = true;
        }
    }, []);

    return ( 
        <Router>
            <AuthProvider>
                <Header/>
                <PageRenderer/>
                <Footer/>
            </AuthProvider>
        </Router>
    );
};

export default App;