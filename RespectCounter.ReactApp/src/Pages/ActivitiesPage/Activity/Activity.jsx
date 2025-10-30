import "./Activity.css";
import { Link } from "react-router-dom";
import ReactionButtons from "../../../components/ReactionButtons/ReactionButtons";
import { useState, useRef, useEffect } from "react";
import { hideActivity, verifyActivity } from "../../../services/activityService";

function Activity({a: activity, showCommentsButton=true, showReactionButtons=true, showDescription=true}) {
    const avatarUrl = activity.AvatarUrl ?? "default.jpg";
    const isAdmin = true;//user?.claims[2]?.value === "Admin";
    const [showMenu, setShowMenu] = useState(false);
    const menuRef = useRef();

    useEffect(() => {
        function handleClickOutside(event) {
            if (menuRef.current && !menuRef.current.contains(event.target)) {
                setShowMenu(false);
            }
        }
        document.addEventListener("mousedown", handleClickOutside);
        return () => document.removeEventListener("mousedown", handleClickOutside);
    }, []);

    return (
        <div key={"activity_" + activity.id} className={'activity border ' + (activity.type === 0 ? "activity-act" : "activity-quote")}>
            <div className='activity-header'>
                <div className="d-flex">
                    <div className='activity-person'>
                        <img className='activity-person-image' src={`/assets/images/persons/${avatarUrl}`} alt={activity.author}/>
                    </div>
                    <div className="activity-person-info">
                        <span className="activity-person-name">{activity.personFullName}</span>
                        <span className='activity-person-respect'>{activity.personRespect} respect</span>
                    </div>
                </div>
                <div>
                    {activity.status === "Verified" &&
                        <span className="bi bi-patch-check-fill me-1"></span>
                    }
                    {activity.type === 0 ? (
                        <span className="bi bi-lightning" title="Action"/>
                    ) : (
                        <span className="bi bi-quote" title="Quote"/>
                    )}
                </div>
            </div>
            <div className='activity-content'>
                {activity.type === 0 ? (
                    <p ><strong>{activity.value}</strong></p>
                ) : (
                    <p className="text-center"><strong>"</strong><i>{activity.value}</i><strong>"</strong></p>
                )}
                { showDescription && <p> { activity.description } </p> }
                <span className='activity-source'>Source: {activity.source}</span>
            </div>
            
            <div className="activity-actions">
                {showCommentsButton && <CommentsButton act={activity} />}
                {showReactionButtons &&
                    <div className="activity-reaction-buttons">
                        <ReactionButtons 
                            respect={activity.respect} 
                            defaultReaction={activity.currentUsersReaction} 
                            targetType="activity" 
                            targetId={activity.id} />
                        {isAdmin && (
                            <div className="dropdown ms-2" style={{ position: "relative", display: "inline-block" }} ref={menuRef}>
                                <button
                                    className="btn btn-outline-primary bi bi-three-dots-vertical"
                                    title="More"
                                    onClick={() => setShowMenu((prev) => !prev)}
                                    type="button"
                                ></button>
                                {showMenu && (
                                    <div className="dropdown-menu show text-center" style={{
                                        display: "block",
                                        position: "absolute",
                                        right: 0,
                                        zIndex: 1000
                                    }}>
                                        <button className="dropdown-item" onClick={() => hideActivity()}><span className="bi bi-eye-slash-fill me-3"></span>Hide</button>
                                        <button className="dropdown-item" onClick={() => verifyActivity(activity.id)}><span className="bi bi-patch-check-fill me-3"></span>Verify</button>
                                    </div>
                                )}
                            </div>
                        )}
                    </div>
                }

            </div>
        </div>
    )    
}

function CommentsButton({act}) {
    return (
        <Link className="btn btn-outline-primary p-2" to={"/act/" + String(act.id)}>
            <span>
                {act.commentsCount} {act.commentsCount === 1 ? "Comment" : "Comments"}
            </span>
            <i className="bi bi-chat-left m-2"></i>
        </Link>
    );
};

export default Activity;