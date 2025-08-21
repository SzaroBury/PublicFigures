import "./ActivityDetailsPage.css";
import { useNavigate, useParams } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import { useAuth } from "../../../utils/providers/AuthProvider/AuthProvider";
import { getActivityById } from "../../../services/activityService";
import { getCommentsByActivityId } from "../../../services/commentService";
import TagsMenu from "../../../components/TagsMenu/TagsMenu";
import Activity from "../Activity/Activity";
import Comment from "../../../components/Comment/Comment";
import CommentForm from "../../../components/CommentForm/CommentForm";
import Loading from "../../../components/Loading/Loading";

function ActivityDetailsPage(params) {
    const { id } = useParams();
    const [ act, setAct ] = useState(null);
    const [ comments, setComments ] = useState([]);
    const { isLoggedIn } = useAuth();
    const navigate = useNavigate();
    const handleGoBack = (nav) => { nav(-1); }

    const loadActivity = useCallback(() => {
        console.log("ActivityDetailsPage: loadActivity(id: '" + id + "')");

        getActivityById(id)
            .then(response => {
                setAct(response.data);
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

    const loadComments = useCallback(() => {
        console.log("ActivityDetailsPage: loadComments()");

        getCommentsByActivityId(id)
            .then(response => {
                setComments(response.data);
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

    useEffect(() => {
        loadActivity();
        loadComments();
      }, [id, loadActivity, loadComments]);

    return (
        <>
            <div></div>
            <div className="p-3">
                <div>
                    <button className="btn btn-outline-primary" onClick={() => handleGoBack(navigate)}><i className="bi bi-arrow-left"></i> Back</button>
                </div>
                <Loading loading={act === null} />
                {act && 
                <>
                    <Activity a={act} showCommentsButton={false}/>
                    <div className="ps-3 pe-3">
                        {isLoggedIn && <CommentForm targetActivityId={id} onCommentAdded={loadComments}/>}
                        <div className="comment-container ms-5 me-5">
                            <Loading loading={act.commentsCount > 1 && comments.length === 0} />
                            {comments.map(com =>
                                <Comment key={"Comment_" + com.id} comment={com} onReplyAdded={loadComments}/>
                            )}
                        </div>
                    </div>
                </>}
            </div>
            <TagsMenu currentTags={act ? act.tags.split(",") : null} countMode="countActivities" tagsSelected={[]} setTagsSelected={() => {}}/>
        </>
    );
}

export default ActivityDetailsPage;