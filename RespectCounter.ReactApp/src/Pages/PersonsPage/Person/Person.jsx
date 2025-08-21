import "./Person.css";
import { useNavigate } from "react-router-dom";
import ReactionButtons from "../../../components/ReactionButtons/ReactionButtons";
import { useAuth } from "../../../utils/providers/AuthProvider/AuthProvider";
import { hidePerson, verifyPerson } from "../../../services/personService";
import { useEffect, useRef, useState } from "react";

function Person({ person, index, showTags = false, showDescription = false, showReactions = true, showReactionButtons = true, showActionButtons = false }) {
    const navigate = useNavigate();
    const avatarUrl = person.AvatarUrl ?? "default.jpg";
    const { user } = useAuth();
    const isAdmin = user?.claims[2]?.value === "Admin";
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
        <div className='person border'>
            <div className='person-header' onClick={() => navigate(`/person/${person.id}`)}>
                {index !== undefined &&
                    <>
                        <div className='person-index'>
                            {index + 1}.
                        </div>
                    </>
                }
                <div className='person-image-container'>
                    <img className='person-image' src={`/assets/images/persons/${avatarUrl}`} alt={person.fullName} />
                </div>
                <div className="person-info">
                    <span className='person-name'>
                        {person.status === "Verified" &&
                            <span className="bi bi-patch-check-fill me-1"></span>
                        }
                        {person.fullName}
                    </span>
                    <span className='person-respect'>{person.profession}</span>
                </div>
            </div>
            {showTags &&
                <div className='person-content'>
                    <p>{person.tags.map(tag => tag.name).join(", ")}</p>
                </div>
            }
            {showDescription &&
                <p>{person.description}</p>
            }
            <div className="person-actions">
                {showReactions &&
                    <div className="person-reaction-buttons me-2">
                        <ReactionButtons
                            respect={person.respect}
                            defaultReaction={person.currentUsersReaction}
                            targetType="person"
                            targetId={person.id}
                            showButtons={showReactionButtons}
                        />
                    </div>
                }
                {isAdmin && showActionButtons && (
                    <div className="dropdown" style={{ position: "relative", display: "inline-block" }} ref={menuRef}>
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
                                <button className="dropdown-item" onClick={hidePerson}><span className="bi bi-eye-slash-fill me-3"></span>Hide</button>
                                <button className="dropdown-item" onClick={verifyPerson}><span className="bi bi-patch-check-fill me-3"></span>Verify</button>
                            </div>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
}

export default Person;