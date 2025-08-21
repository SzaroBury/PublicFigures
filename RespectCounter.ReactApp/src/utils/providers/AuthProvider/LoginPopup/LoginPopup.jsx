import "./LoginPopup.css";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../AuthProvider";

function LoginPopup() {
    const [formData, setFormData] = useState({username: '', password: ''});
    const [showPassword, setShowPassword] = useState(false);
    const [validationMessage, setValidationMessage] = useState("");
    const {login, closeLoginPopup} = useAuth();
    const navigate = useNavigate();

    const handleDataChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleLoginClick = async (e) => {
        e.preventDefault();
        setValidationMessage("");
        try {
            await login(formData);
            closeLoginPopup();
            navigate("/");
        } catch (error) {
            if (error?.response?.status === 401) {
                setValidationMessage(`Failed to login. ${error.response.data.message} Please try again.`);
            } else {
                setValidationMessage("An error occurred. Please try again.");
            }
        }        
    };

    const handleSignUpClick = () => {
        navigate("/signup");
        closeLoginPopup();
    }

    return (
        <div className="popup-backdrop">
            <div className="popup-container border">
                <h2>Log In</h2>
                <div className="popup-cancel">
                    <button type="button" className="btn btn-outline-primary" onClick={closeLoginPopup}><i className="bi bi-x-lg"></i></button>
                </div>
                <form onSubmit={handleLoginClick}>
                    <div className="input-group mb-3">
                        <input type="text" className="form-control" placeholder="Username" name="username" required
                            onChange={handleDataChange}/>                        
                    </div>
                    <div className="input-group mb-3">
                        <input 
                            type={showPassword ? "text" : "password"} 
                            className="form-control" 
                            placeholder="Password" 
                            name="password" 
                            required 
                            onChange={handleDataChange} 
                        />
                        <button 
                            className="btn btn-outline-primary" 
                            type="button" 
                            id="button-addon2" 
                            onClick={() => setShowPassword((prev) => !prev)}
                        >
                            <i className={showPassword ? "bi bi-eye-slash" : "bi bi-eye"}></i>
                        </button>
                    </div>
                    <div className="popup-actions">
                        <button type="submit" className="btn btn-outline-primary">Log In</button>
                    </div>
                    <div className="text-center">
                        <span className="text-danger">{validationMessage}</span>
                    </div>
                </form>
                <div className="popup-signup">
                    or
                    <button className="btn btn-outline-primary mx-2" onClick={handleSignUpClick}>
                        Sign up
                    </button>
                    <label className="popup-signup-label">if you don't have an account yet.</label>
                </div>
                
            </div>
        </div>
    );
}

export default LoginPopup;
