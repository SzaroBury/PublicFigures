import "./Header.css"
import { useAuth } from '../../utils/providers/AuthProvider/AuthProvider';
import { Link } from "react-router-dom";
import { useState, useEffect } from "react";

function Header() {
    const [theme, setTheme] = useState(localStorage.getItem('theme') || 'light');
    const { isLoggedIn, user, logout, openLoginPopup } = useAuth();


    useEffect(() => {
        document.documentElement.setAttribute('data-bs-theme', theme);
        localStorage.setItem('theme', theme);
    }, [theme]);

    const toggleTheme = () => {
        console.log("Header: toggleTheme()")
        setTheme(theme === 'light' ? 'dark' : 'light');
    };

    return (
        <div className="header">
            <div className="nav col-lg-auto me-lg-auto align-items-center">
                <Link to="/" className="nav-link mb-2 mb-lg-0 text-decoration-none">
                    <span className="fs-4">RespectCounter</span>
                </Link>
                <Link className="nav-link" to="/activities">Activities</Link>
                <Link className="nav-link" to="/persons">Persons</Link>
                <Link className="nav-link" to="/about">About</Link>
            </div>

            <div className="d-flex h-100 align-items-center">
                <button className="btn me-3" onClick={toggleTheme}>
                {
                    theme === 'light' ? (
                        <i className="bi bi-moon-stars-fill"></i> 
                        
                    ) : (
                        <i className="bi bi-brightness-high-fill"></i>
                    )
                }
                </button>
                {isLoggedIn ? (
                        <div className="d-flex h-100 align-items-center">
                            <span className='me-3'>{user.userName}</span>
                            <button className='btn btn-outline-primary' onClick={logout}>Log out</button>    
                        </div>        
                    ) : (
                        <div>
                            <button className='btn btn-outline-primary' onClick={openLoginPopup}>Log in</button>
                        </div>
                    )
                }
            </div>
            
        </div>
    );
};

export default Header;





