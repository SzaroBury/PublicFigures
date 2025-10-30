import "./TagDropdown.css";
import { useCallback, useEffect, useState } from "react";
import { getSimpleTags } from "../../../../services/tagService";

function TagDropdown({ personTags, onTagsChange }) {
    const [inputValue, setInputValue] = useState("");
    const [showDropdown, setShowDropdown] = useState(false);
    const [allTags, setAllTags] = useState([]);
    const [selectedTags, setSelectedTags] = useState([]);
    const [displayedPersonTags, setDisplayedPersonTags] = useState([]);
    const [filteredTags, setFilteredTags] = useState([]);

    const loadTags = useCallback(() => {
        console.log('TagDropdown: loadTags()');
        setAllTags([]);

        getSimpleTags()
            .then(response => {
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
    }, []);

    const handleTagSelect = (tag) => {
        if (!selectedTags.some((t) => t.id === tag.id)) {
            setSelectedTags([...selectedTags, tag]);
        }
        if (displayedPersonTags.some(t => t.id === tag.id)) {
            setDisplayedPersonTags(displayedPersonTags.filter((t) => t.id !== tag.id));
        }
        setInputValue("");
        setShowDropdown(false);
    };

    const handleTagUnselected = (tag) => {
        const updatedTags = selectedTags.filter((t) => t.id !== tag.id);
        setSelectedTags(updatedTags);

        if (personTags.some((t) => t.id === tag.id) && !displayedPersonTags.some((t) => t.id === tag.id)) {
            setDisplayedPersonTags([...displayedPersonTags, tag]);
        }
    };

    const handlePersonTagSelected = (tag) => {
        const updatedPersonTags = displayedPersonTags.filter((t) => t.id !== tag.id);
        setDisplayedPersonTags(updatedPersonTags);
        setSelectedTags([...selectedTags, tag]);
    };

    const handleKeyDown = (event) => {
        console.log(`User hit some key`);
        // console.log(`User hit key: ${event.key}`);

        if(event.key === 'Enter')
        {
            console.log('User hit the Enter key.');
        }
    };

    useEffect(() => {
        const updatedTags = personTags.filter(t => !selectedTags.some(st => st.name === t.name));
        setDisplayedPersonTags(updatedTags);
    }, [personTags, selectedTags]);

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

    useEffect(() => {
        loadTags();
    }, [loadTags]);

    return (
        <div className="position-relative">
            <div className="form-control" onKeyDown={handleKeyDown}>
                {
                    selectedTags.map((tag) =>
                        <button key={'btn_tag_' + tag.name} className="btn btn-outline-primary me-1" onClick={() => handleTagUnselected(tag)}>{tag.name}</button>
                    )
                }
                {
                    displayedPersonTags.map((tag) =>
                        <button key={'btn_tag_' + tag.name} className="btn btn-outline-secondary me-1" onClick={() => handlePersonTagSelected(tag)}>{tag.name}?</button>
                    )
                }
                <input className="border-0 p-2"
                    name="tags"
                    value={inputValue}
                    onChange={(e) => setInputValue(e.target.value)}
                    onFocus={() => setShowDropdown(true)}
                    onBlur={() => setTimeout(() => setShowDropdown(false), 250)}
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
}

export default TagDropdown