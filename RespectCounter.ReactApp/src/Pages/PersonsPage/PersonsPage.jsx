import './PersonsPage.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../utils/providers/AuthProvider/AuthProvider';
import SortMenu from '../../components/SortMenu/SortMenu';
import TagsMenu from '../../components/TagsMenu/TagsMenu';
import Loading from '../../components/Loading/Loading';
import Person from './Person/Person';
import { getPersons } from '../../services/personService';

function PersonsPage({ props }) {
    const [loading, setLoading] = useState(true);
    const [title, setTitle] = useState('Persons - Most Respected');
    const [persons, setPersons] = useState([]);
    const [sortOption, setSortOption] = useState("MostRespected");
    const [scopeOption, setScopeOption] = useState(true);
    const [tagsSelected, setTagsSelected] = useState([]);
    const navigate = useNavigate();
    const { isLoggedIn, openLoginPopup } = useAuth();

    const handleSortChange = (sortOptionName, sortOptionDiplay) => {
        console.log("PersonsPage: handleSortChange(" + sortOptionName + ", " + sortOptionDiplay + ")")
        setLoading(true);
        setTitle("Persons - " + sortOptionDiplay);
        setSortOption(sortOptionName);
    };

    const handleScopeChange = () => {
        console.log("PersonsPage: handleScopeChange()");
        setLoading(true);
        const targetValue = !scopeOption;
        setScopeOption(targetValue);
    };

    const handleCreateButton = () => {
        console.log('PersonsPage: handleCreateButton()');
        if (isLoggedIn) {
            navigate('/person/create');
        } else {
            openLoginPopup();
        }
    }

    const loadPersons = (sortOption, onlyVerified) => {
        console.log('PersonsPage: loadPersons(sortOption: "' + sortOption + '", onlyVerified: "' + onlyVerified + '")')
        setPersons([]);

        const params = { tags: tagsSelected, order: sortOption, onlyVerified };

        getPersons(params)
            .then(response => {
                setPersons(response.data);
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
    }

    useEffect(() => {
        setLoading(true);
        loadPersons(sortOption, scopeOption);
    }, [sortOption, scopeOption]);

    return (
        <>
            <SortMenu page="Persons" onSortOptionChange={handleSortChange} onScopeChange={handleScopeChange} />
            <div></div>
            <div>
                <div className='text-center'>
                    <button className='btn btn-outline-primary' title="Propose a new person" onClick={handleCreateButton}>
                        <span className='me-2'>Propose a new person</span>
                        <i className="bi bi-person-plus-fill"></i>
                    </button>
                </div>
                <h2>{title}</h2>
                <Loading loading={loading} />
                {persons.map((per, index) =>
                    <Person key={"Person_" + per.id} person={per} index={index} showActionButtons="true" />
                )}
            </div>
            <div></div>
            <TagsMenu countMode="countPersons" tagsSelected={tagsSelected} setTagsSelected={setTagsSelected} />
        </>
    );
}

export default PersonsPage;

