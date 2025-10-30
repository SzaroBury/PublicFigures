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
    const [currentPageNumber, setCurrentPageNumber] = useState(1);
    const [pageInfo, setPageInfo] = useState({totalItems: 0, pageNumber: 1, pageSize: 10, totalPages: 1})
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

    const loadPersons = (sortOption, onlyVerified, tags, page) => {
        console.log('PersonsPage: loadPersons(sortOption: "' + sortOption + '", onlyVerified: "' + onlyVerified + '")')
        setPersons([]);

        const params = { 
            tags: tags, 
            order: sortOption, 
            onlyVerified, 
            page: page, 
            pageSize: 7
        };

        getPersons(params)
            .then(response => {
                setPersons(response.data.items);
                setPageInfo({
                    totalItems: response.data.totalItems,
                    pageNumber: response.data.pageNumber,
                    pageSize: response.data.pageSize,
                    totalPages: response.data.totalPages
                });
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
        loadPersons(sortOption, scopeOption, tagsSelected, currentPageNumber);
    }, [sortOption, scopeOption, tagsSelected, currentPageNumber]);

    useEffect(() => {
        setCurrentPageNumber(1);
    }, [sortOption, scopeOption, tagsSelected]);

    return (
        <>
            <SortMenu page="Persons" onSortOptionChange={handleSortChange} onScopeChange={handleScopeChange} />
            <div></div>
            <div>
                <div className='d-flex justify-content-between align-items-center'>
                    <div>
                        <h2>{title}</h2>
                        <span>
                            Showing {persons.length > 0 ? pageInfo.pageSize * (pageInfo.pageNumber - 1) + 1 : 0} - {pageInfo.pageSize * (pageInfo.pageNumber - 1) + persons.length} out of {pageInfo.totalItems} persons.
                        </span>
                    </div>
                    <button className='btn btn-outline-primary' title="Propose a new person" onClick={handleCreateButton}>
                        <span className='me-2'>Propose a new person</span>
                        <i className="bi bi-person-plus-fill"></i>
                    </button>
                </div>
                <Loading loading={loading} />
                {persons.map((per, index) =>
                    <Person key={"Person_" + per.id} person={per} index={(pageInfo.pageSize*(pageInfo.pageNumber - 1)) + index} showTags='true' showActionButtons="true" />
                )}
                <div>
                    <div className='d-flex justify-content-center m-3'>
                        <button 
                            className='btn btn-outline-primary ms-3'
                            onClick={() => setCurrentPageNumber(prev => {return prev - 1})}
                            disabled={currentPageNumber === 1} 
                        >
                            Previous
                        </button>
                        {Array.from(Array(pageInfo.totalPages).keys()).map((page) =>
                            <button 
                                className={`btn btn-outline-primary ms-3 ${page + 1 === currentPageNumber ? 'active' : ''}`}
                                onClick={() => setCurrentPageNumber(page + 1)}
                            >
                                {page + 1}
                            </button>
                        )}
                        <button 
                            className='btn btn-outline-primary ms-3'
                            onClick={() => setCurrentPageNumber(prev => {return prev + 1})}
                            disabled={currentPageNumber === pageInfo.totalPages} 
                        >
                            Next
                        </button>
                    </div>
                </div>
            </div>
            <div></div>
            <TagsMenu countMode="countPersons" tagsSelected={tagsSelected} setTagsSelected={setTagsSelected} />
        </>
    );
}

export default PersonsPage;

