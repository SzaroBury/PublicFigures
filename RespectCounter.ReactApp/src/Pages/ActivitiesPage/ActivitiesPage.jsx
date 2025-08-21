import './ActivitiesPage.css';
import { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { getActivities } from '../../services/activityService';
import SortMenu from '../../components/SortMenu/SortMenu';
import TagsMenu from '../../components/TagsMenu/TagsMenu';
import Activity from './Activity/Activity';
import Loading from '../../components/Loading/Loading';
import { useAuth } from '../../utils/providers/AuthProvider/AuthProvider';

function ActivitiesPage(props) {
    const [loading, setLoading] = useState(true);
    const [title, setTitle] = useState("Activities - Latest added");
    const [sortOption, setSortOption] = useState("LatestAdded");
    const [onlyVerifiedOption, setOnlyVerifiedOption] = useState(true);
    const [tagsSelected, setTagsSelected] = useState([]);
    const [activities, setActivities] = useState([]);
    const navigate = useNavigate();
    const { isLoggedIn, openLoginPopup } = useAuth();

    const handleSortChange = (sortOptionName, sortOptionDiplay) => {
        console.log("ActivitiesPage: handleSortChange(" + sortOptionName + ", " + sortOptionDiplay + ")")
        setTitle("Activities - " + sortOptionDiplay);
        setSortOption(sortOptionName);
    };
    
    const handleScopeChange = () => {
        console.log("ActivitiesPage: handleScopeChange()");
        setOnlyVerifiedOption(!onlyVerifiedOption);
    };

    const handleCreateButton = () => {
        if(isLoggedIn) {
            navigate("/activity/create");
        } else {
            openLoginPopup()
        };
    };

    const loadActivities = useCallback(() => {
        console.log("ActivitiesPage: loadActivities()");
        setActivities([]);
    
        console.log('onlyVerifiedOption', onlyVerifiedOption);
        const params = { tags: tagsSelected, order: sortOption, onlyVerified: onlyVerifiedOption }
    
        getActivities(params)
            .then(response => {
                setActivities(response.data);
                console.log("Loaded activities: ", response);
                setLoading(false);
            })
            .catch(error => {
                setLoading(false);

                if (error.response) {
                    console.error(`HTTP error! Status: ${error.response.status}`);
                } else if (error.request) {
                    console.error("No response received: ", error.request);
                } else {
                    console.error("Error setting up the request: ", error.message);
                }
            });
    }, [onlyVerifiedOption, sortOption, tagsSelected]);

    useEffect(() => {
        setLoading(true);
        loadActivities();
    }, [sortOption, onlyVerifiedOption, tagsSelected, loadActivities]);

    return (
        <>
            <SortMenu page="Activities" onSortOptionChange={handleSortChange} onScopeChange={handleScopeChange}/>
            <div className='act-list'>
                <div className='text-center'>
                    <button className='btn btn-outline-primary' title="Post a new activity" onClick={handleCreateButton}>
                        <span className='me-2'>Post a new activity</span>
                        <i className="bi bi-file-earmark-plus-fill"></i>
                    </button>
                </div>
                <h2>{title}</h2>
                <Loading loading={loading}/>
                {activities.map((act) => 
                    <Activity key={"Act_" + act.id} a={act}/>
                )}
            </div>
            <TagsMenu countMode="countActivities" tagsSelected={tagsSelected} setTagsSelected={setTagsSelected}/>
        </>
    );
}

export default ActivitiesPage;