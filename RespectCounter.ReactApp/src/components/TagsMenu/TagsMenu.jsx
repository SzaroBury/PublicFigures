import { getAllTags, getFavouriteTags, getRecentlyBrowsedTags } from '../../services/tagService';
import './TagsMenu.css';
import { useState, useEffect } from 'react';

function TagsMenu({ countMode = 'count', tagsSelected, setTagsSelected, currentTags }) {

    const handleTagClick = (tag) => {
        if (tagsSelected.some(ts => ts.name === tag.name)) {
            setTagsSelected(tagsSelected.filter(ts => ts.name !== tag.name));
        } else {
            setTagsSelected([...tagsSelected, tag]);
        };
    };

    return (
        <div className="tags-menu">
            <section>
                <h5 className='m-3'>Tags</h5>
            </section>
            <CurrentTags currentTags={currentTags} />
            <RecentBrowsedTags tagsSelected={tagsSelected} />
            <FavouriteTags tagsSelected={tagsSelected} />
            <TrendingTags tagsSelected={tagsSelected} />
            <AllTags count={countMode} tagsSelected={tagsSelected} onTagClick={handleTagClick} />
        </div>
    );
}

function CurrentTags({ currentTags }) {
    return (
        currentTags && currentTags.length > 0 && (
            <section>
                <h6>Current tags:</h6>
                <ul>
                    {currentTags.map((tag) => (
                        <li key={"CurrentTag_" + tag}>{tag}</li>
                    ))}
                </ul>
                <hr />
            </section>
        )
    );
}

function TrendingTags() {
    // to do

    return null;
}

function RecentBrowsedTags() {
    const [tags, setTags] = useState([]);

    useEffect(() => {
        loadTags();
    }, []);

    const loadTags = () => {
        console.log("TagsMenu.RecentBrowsedTags: loadTags()");
        getRecentlyBrowsedTags()
            .then(response => {
                setTags(response.data);
            })
            .catch(error => {
                if (error.response) {
                    console.error(`HTTP error! Status: ${error.response.status}`);
                }
                else if (error.request) {
                    console.error("No response received: ", error.request);
                }
                else {
                    console.error("Error setting up the request: ", error.message);
                }
            }
            );
    }

    if (tags.length === 0) {
        return null;
    }

    return (
        <section>
            <h6>Recent browsed:</h6>
            <ul>
                {tags.map((tag) => (
                    <li key={"RecentTag_" + tag.id}>{tag.name} ({tag.count})</li>
                ))}
            </ul>
        </section>
    );
}

function FavouriteTags() {
    const [tags, setTags] = useState([]);

    useEffect(() => {
        loadTags();
    }, []);

    const loadTags = () => {
        console.log("TagsMenu.FavouriteTags: loadTags()");

        getFavouriteTags()
            .then(response => {
                setTags(response.data);
            })
            .catch(error => {
                if (error.response) {
                    console.error(`HTTP error! Status: ${error.response.status}`);
                }
                else if (error.request) {
                    console.error("No response received: ", error.request);
                }
                else {
                    console.error("Error setting up the request: ", error.message);
                }
            });
    }

    if (tags.length === 0) {
        return null;
    }

    return (
        <section>
            <h6>Favourite:</h6>
            <ul>
                {tags.map((tag) => (
                    <li key={"FavouriteTag_" + tag.name}>{tag.name} ({tag.count})</li>
                ))}
            </ul>
        </section>
    );
}

function AllTags({ count, tagsSelected, onTagClick }) {
    const [tags, setTags] = useState([]);

    useEffect(() => {
        loadTags();
    }, []);

    const loadTags = () => {
        console.log("TagsMenu.AllTags: loadTags()");
        getAllTags()
            .then(response => {
                setTags(response.data);
                console.log('tags', response.data);
            })
            .catch(error => {
                if (error.response) {
                    console.error(`HTTP error! Status: ${error.response.status}`);
                }
                else if (error.request) {
                    console.error("No response received: ", error.request);
                }
                else {
                    console.error("Error setting up the request: ", error.message);
                }
            });
    }

    return (
        <section>
            {tags.length === 0 ? (
                <ul className="placeholder-glow">
                    {Array.from({ length: 10 }, (_, index) => (
                        <li key={`placeholder_${index}`} className="placeholder">
                            Placeholder
                        </li>
                    ))}
                </ul>
            ) : (
                <ul>
                    {tags
                        .sort((a, b) => b.count - a.count)
                        .filter(tag => tag[count] > 0)
                        .map(tag => (
                            <li
                                key={`Tag_${tag.name}`}
                                className={`tag-item ${tagsSelected.some(ts => ts.name === tag.name) ? "selected-tag" : ""}`}
                                onClick={() => onTagClick(tag)}
                            >
                                {tag.name} ({tag[count]})
                            </li>
                        ))
                    }
                </ul>
            )}
        </section>
    );
}

export default TagsMenu;