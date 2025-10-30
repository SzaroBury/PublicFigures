import { useState, useEffect } from 'react';
import { getSimpleTags } from '../../../../services/tagService';

function TagDropdown({onTagsChange}) {
    const [inputValue, setInputValue] = useState("");
    const [showDropdown, setShowDropdown] = useState(false);
    const [allTags, setAllTags] = useState([]);
    const [selectedTags, setSelectedTags] = useState([]);
    const [filteredTags, setFilteredTags] = useState([]);

    useEffect(() => {
        loadTags();
    }, []);

    useEffect(() => {
        setFilteredTags(
            allTags.filter((tag) =>
                tag.name.toLowerCase().includes(inputValue.toLowerCase())
            )
        );
    }, [inputValue, allTags]);

    useEffect(() => {
        if (filteredTags.length === 0)
            setShowDropdown(false);
    }, [filteredTags]);

    useEffect(() => {
        onTagsChange(selectedTags);
    }, [onTagsChange, selectedTags]);

    const loadTags = () => {
        console.log('TagDropdown.loadTags()');
        setAllTags([]);

        getSimpleTags()
            .then((response) => {
                console.log('response.data', response.data);
                setAllTags(response.data);
            })
            .catch(error => {
                if (error.response) {
                    console.error(`HTTP error! Status: ${error.response.status}`);
                } else if (error.request) {
                    console.error("No response received: ", error.request);
                } else {
                    console.error("Error setting up the request: ", error.message);
                }
            });
    };

    const handleKeyDown = (event) => {
        if(event.key === 'Enter') {
            const tagName = event.target.value;
            if (!selectedTags.some((t) => t.name === tagName)) {
                console.log('tagName', tagName);
                setSelectedTags([...selectedTags, {id: tagName, name: tagName}]);
            }
            setInputValue('');
        }
    };

    const handleTagSelect = (tag) => {
        if (!selectedTags.some((t) => t.id === tag.id)) {
            setSelectedTags([...selectedTags, tag]);
        }
        setInputValue("");
        setShowDropdown(false);
    };

    const handleTagUnselected = (tag) => {
        const updatedTags = selectedTags.filter((t) => t.id !== tag.id);
        setSelectedTags(updatedTags);
    };

    return (
        <div className="position-relative">
            <div className="form-control">
                {
                    selectedTags.map((tag) =>
                        <button key={'btn_tag_' + tag.name} className="btn btn-outline-primary me-1" onClick={() => handleTagUnselected(tag)}>{tag.name}</button>
                    )
                }
                <input className="border-0 p-2"
                    value={inputValue}
                    onChange={(e) => setInputValue(e.target.value)}
                    onFocus={() => setShowDropdown(true)}
                    onBlur={() => setTimeout(() => setShowDropdown(false), 250)}
                    onKeyDown={handleKeyDown}
                    type='text'
                    placeholder="Type to search..."
                />
                {showDropdown && (
                    <ul className="dropdown-list">
                        {
                            filteredTags.map((tag) => (
                                <li
                                    className="tag-dropdown-option"
                                    key={tag.id}
                                    onClick={() => handleTagSelect(tag)}
                                >
                                    {tag.name}
                                </li>
                            ))
                        }
                    </ul>
                )}
            </div>
        </div>
    );
};

export default TagDropdown;