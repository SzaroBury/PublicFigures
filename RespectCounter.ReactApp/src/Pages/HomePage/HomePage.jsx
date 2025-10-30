import "./HomePage.css";
import { useState, useEffect } from 'react';
import { getPersons } from "../../services/personService";
import { getActivities } from "../../services/activityService";
import { Link } from 'react-router-dom';
import Loading from '../../components/Loading/Loading';
import Person from '../PersonsPage/Person/Person';
import Activity from "../ActivitiesPage/Activity/Activity";

function HomePage() {
    const displayName = HomePage.name;
    const [loadingPersons, setLoadingPersons] = useState(true);
    const [persons, setPersons] = useState([]);
    const [loadingActivities, setLoadingActivities] = useState(true);
    const [activities, setActivities] = useState([]);

    const loadPersons = () => {
        console.log('HomePage: loadPersons()');
        setPersons([]);
        setLoadingPersons(true);
        
        const params = { order: "MostRespected", pageSize: 5 };
        getPersons(params)
            .then(response => {
                setPersons(response.data.items);
                setLoadingPersons(false);
            })
            .catch(error => {
                setLoadingPersons(false);

                if (error.response) {
                    console.error(`HTTP error! Status: ${error.response.status}`);
                } else if (error.request) {
                    console.error("No response received: ", error.request);
                } else {
                    console.error("Error setting up the request: ", error.message);
                }
            });
    }

    const loadActivities = () => {
        console.log('HomePage: loadActivities()');
        setActivities([]);
        setLoadingActivities(true);

        const params = { order: 'Trending', onlyVerified: true, pageSize: 3 };
        getActivities(params)
            .then(response => {
                setActivities(response.data);
                setLoadingActivities(false);
            })
            .catch(error => {
                setLoadingActivities(false);

                if (error.response) {
                    console.error(`HTTP error! Status: ${error.response.status}`);
                } else if (error.request) {
                    console.error("No response received: ", error.request);
                } else {
                    console.error("Error setting up the request: ", error.message);
                }
            });
    };

    useEffect(() => {
        loadPersons();
        loadActivities()
    }, []);

    return (
        <>
            <div className='home-page'>
                <h1 className="text-center">Welcome To Respect Counter</h1>
                <p className="text-center">See what it is <Link to="/about">about</Link>.</p>

                <div className="home-sections">
                    <div className="home-section">
                        <h2 className="text-center">Trending Activities</h2>
                        <Loading loading={loadingActivities} />
                        {activities.map((act, index) => 
                            <Activity 
                                key={"Act" + act.id} 
                                a={act} 
                                showCommentsButton={false}
                                showReactionButtons={false}
                                showDescription={false}/>
                        )}
                        <div className="text-center">
                            <Link to="/activities">See more</Link>
                        </div>
                    </div>
                    <div className="home-section">
                        <h2 className="text-center">Most respected persons:</h2>
                        <Loading loading={loadingPersons} />
                        {persons.map((per, index) =>
                            <Person key={"Person_" + per.id} person={per} index={index} showReactionButtons={false} />
                        )}
                        <div className="text-center">
                            <Link to="/persons">See more</Link>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default HomePage;