import "./PersonDetailsPage.css";
import { useNavigate, useParams } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import { useAuth } from "../../../utils/providers/AuthProvider/AuthProvider";
import Person from "../Person/Person";
import TagsMenu from "../../../components/TagsMenu/TagsMenu";
import SortMenu from "../../../components/SortMenu/SortMenu";
import Comment from "../../../components/Comment/Comment";
import CommentForm from "../../../components/CommentForm/CommentForm";
import Loading from "../../../components/Loading/Loading";
import Activity from "../../ActivitiesPage/Activity/Activity";
import { getPerson } from "../../../services/personService";
import { getActivitiesByPersonId } from "../../../services/activityService";
import { getCommentsByPersonId } from "../../../services/commentService";

function PersonDetailsPage() {
    const { id } = useParams();
    const [person, setPerson] = useState(null);
    const [detailsMode, setDetailsMode] = useState(0);
    const [loadingActivities, setLoadingActivities] = useState(false);
    const [activitiesFilterMode, setActivitiesFilterMode] = useState("");
    const [activitiesSortOption, setActivitiesSortOption] = useState("LatestAdded");
    const [activitiesScopeOption, setActivitiesScopeOption] = useState(true);
    const [loadingComments, setLoadingComments] = useState(false);
    const [commentsSortOption, setCommentsSortOption] = useState("MostRespected");
    const [comments, setComments] = useState([]);
    const [activities, setActivities] = useState([]);
    const { isLoggedIn } = useAuth();
    const navigate = useNavigate();

    const loadPerson = useCallback(() => {
        console.log("PersonDetailsPage: loadPerson(id: '" + id + "')");
        getPerson(id)
            .then(response => setPerson(response.data))
            .catch(error => {
                if (error.response) {
                    console.error(`HTTP error! Status: ${error.response.status}`);
                } else if (error.request) {
                    console.error("No response received: ", error.request);
                } else {
                    console.error("Error setting up the request: ", error.message);
                }
            });
    }, [id]);

    const loadComments = useCallback(() => {
        console.log("PersonDetailsPage: loadComments()");

        setComments([]);
        setLoadingComments(true);

        getCommentsByPersonId(id, { commentsSortOption })
            .then(response => {
                setLoadingComments(false);
                setComments(response);
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
    }, [id]);

    const loadActivities = useCallback(() => {
        console.log("PersonDetailsPage: loadActivities()");

        setActivities([]);
        setLoadingActivities(false);

        const params = {
            type: activitiesFilterMode,
            sort: activitiesSortOption,
            isVerified: activitiesScopeOption
        };
        getActivitiesByPersonId(id, params)
            .then(response => {
                setLoadingActivities(false);
                setActivities(response.data)
            }).catch(error => {
                if (error.response) {
                    console.error(`HTTP error! Status: ${error.response.status}`);
                } else if (error.request) {
                    console.error("No response received: ", error.request);
                } else {
                    console.error("Error setting up the request: ", error.message);
                }
            });
    }, [id, activitiesFilterMode, activitiesSortOption, activitiesScopeOption]);

    const handleSortChange = (sortOptionName, sortOptionDisplayName) => {
        if (detailsMode === 0) {
            setCommentsSortOption(sortOptionName);
        } else {
            setActivitiesSortOption(sortOptionName);
        }
    };

    const handleScopeChange = () => {
        setActivitiesScopeOption(!activitiesScopeOption);
    };

    useEffect(() => {
        loadPerson(id, setPerson);
        if (detailsMode === 0) {
            loadComments();
        } else {
            loadActivities();
        }
    }, [id, detailsMode, loadPerson, loadComments, loadActivities]);

    return (
        <>
            <SortMenu page={detailsMode === 0 ? "Comments" : "Activities"} onSortOptionChange={handleSortChange} onScopeChange={handleScopeChange} />
            <div></div>
            <div className="p-3">
                <div>
                    <button className="btn btn-outline-primary" onClick={() => navigate(-1)}><i className="bi bi-arrow-left"></i> Back</button>
                </div>
                <Loading loading={person === null} />
                {person &&
                    <>
                        <Person person={person} showTags='true' showDescription='true' showActionButtons={true} />
                        <div className="input-group d-flex">
                            <button
                                className={`btn btn-outline-primary flex-fill ${detailsMode === 0 ? "active" : ""}`}
                                onClick={() => setDetailsMode(0)}>Comments</button>
                            <button
                                className={`btn btn-outline-primary flex-fill${detailsMode === 1 ? " active" : ""}`}
                                onClick={() => setDetailsMode(1)}>Activities</button>
                        </div>
                        {detailsMode === 0 ? (
                            <div className="ps-3 pe-3 mt-2">
                                {isLoggedIn && <CommentForm targetPersonId={id} onCommentAdded={loadComments} />}
                                <div className="comment-container ms-5 me-5">
                                    <Loading loading={loadingComments} />
                                    {comments.map(com =>
                                        <Comment key={"Comment_" + com.id} comment={com} onReplyAdded={loadComments} />
                                    )}
                                </div>
                            </div>
                        ) : (
                            <div className="ps-3 pe-3 w-100">
                                <div className="input-group d-flex m-2 w-75 mx-auto">
                                    <button
                                        className={`btn btn-outline-primary flex-fill${activitiesFilterMode === "" ? " active" : ""}`}
                                        onClick={() => setActivitiesFilterMode("")}>
                                        All
                                    </button>
                                    <button
                                        className={`btn btn-outline-primary flex-fill${activitiesFilterMode === "Act" ? " active" : ""}`}
                                        onClick={() => setActivitiesFilterMode("Act")}>
                                        Actions
                                    </button>
                                    <button
                                        className={`btn btn-outline-primary flex-fill${activitiesFilterMode === "Quote" ? " active" : ""}`}
                                        onClick={() => setActivitiesFilterMode("Quote")}>
                                        Quotes
                                    </button>
                                </div>
                                <Loading loading={loadingActivities} />
                                {activities.map(act =>
                                    <Activity key={"Activity" + act.id} a={act} />
                                )}
                            </div>
                        )}
                    </>}
            </div>
            <div></div>
            <TagsMenu currentTags={person ? person.tags.map(t => t.name) : null} countMode="countPersons" tagsSelected={[]} setTagsSelected={() => { }} />
        </>
    );
}

export default PersonDetailsPage;