import "./PersonDropdown.css";
import { useState, useEffect } from "react";

function PersonDropdown({title, options, onPersonChange}) {
    const [inputValue, setInputValue] = useState("");
    const [showDropdown, setShowDropdown] = useState(false);
    const [selectedOption, setSelectedOption] = useState({id: '', fullName: ''});

    const filteredOptions = options.filter((option) =>
        option.fullName.toLowerCase().includes(inputValue.toLowerCase())
    );

    const handleSelect = (option) => {
        setSelectedOption(option);
        setShowDropdown(false);
    };

    const handleClear = () => {
        setInputValue('');
        onPersonChange('');
    }

    useEffect(() => {
        console.log('selectedOption', selectedOption);
        setInputValue(selectedOption.fullName);
        onPersonChange(selectedOption.id);
    }, [selectedOption.fullName, selectedOption.id, onPersonChange])

    return(
    <div className="position-relative">
        <div className="input-group" style={{ display: 'flex', alignItems: 'center' }}>
            {title && 
                <span className="input-group-text"  style={{ whiteSpace: 'nowrap' }}>
                    {title}
                    <span className="text-danger">*</span>
                </span>
            }
            <div className="position-relative" style={{ flexGrow: 1 }}>
                <input className="dropdown-input form-control" 
                    value={inputValue}
                    onChange={(e) => setInputValue(e.target.value) }
                    onFocus={() => setShowDropdown(true)}
                    onBlur={() => setTimeout(() => setShowDropdown(false), 350)}
                    type='text' 
                    placeholder="Type to search..." 
                    required
                />
                {inputValue && (
                    <button className="dropdown-clear" onClick={() => handleClear}>
                        <i className="bi bi-x-lg"></i>
                    </button>
                )}
                {showDropdown && (
                    <ul className="dropdown-list">
                    {
                        filteredOptions.map((option) => (
                            <li 
                                className="dropdown-option"
                                key={option.id} 
                                onClick={() => handleSelect(option)} 
                            >
                                {option.fullName}
                            </li>
                        ))
                    }
                    </ul>
                )}
            </div>
        </div>
        
        
    </div>
    );
}

export default PersonDropdown