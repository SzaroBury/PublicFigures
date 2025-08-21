import "./ReactionButtons.css";
import { useAuth } from "../../utils/providers/AuthProvider/AuthProvider";
import { useEffect, useState } from "react";
import { postReaction } from "../../services/reactionService";

function ReactionButtons({respect, defaultReaction, targetType = "", targetId = "", showButtons = true}) {
    const {isLoggedIn, openLoginPopup} = useAuth();
    const [displayedRespect, setDisplayedRespect] = useState(respect);
    const [clickedReaction, setClickedReaction] = useState(defaultReaction);

    const handleClick = async (reaction) => {
        if(!isLoggedIn) {
            openLoginPopup();
            return;
        }
        
        if (targetType === "") {
            throw Error("");
        }
        
        const prevClickedReaction = clickedReaction;
        const prevDisplayedRespect = displayedRespect;
        setDisplayedRespect(displayedRespect + reaction);
        setClickedReaction(reaction);
        
        try {
            // to do: if reaction === clickedReaction -> remove reaction
            const response = await postReaction({ targetType, targetId, reaction });
            setDisplayedRespect(response);
        } catch (error) {
            setDisplayedRespect(prevDisplayedRespect);
            setClickedReaction(prevClickedReaction);
            console.error('An error occurred:', error);
        }
    };

    useEffect(() => {
        if(!isLoggedIn) {
            setClickedReaction(-1);
        };
    }, [isLoggedIn]);

    useEffect(() => {
        setClickedReaction(defaultReaction) 
    }, [defaultReaction]);

    return (
        <div className="input-group">
            {showButtons && 
                <>
                    <button className={'btn btn-outline-primary ' + (clickedReaction === -2 ? 'active': '')} onClick={() => handleClick(-2)}>
                        <i className="bi bi-hand-thumbs-down-fill"></i>
                        <i className="bi bi-hand-thumbs-down-fill"></i>
                    </button>
                    <button className={'btn btn-outline-primary ' + (clickedReaction === -1 ? 'active': '')} onClick={() => handleClick(-1)}>
                        <i className="bi bi-hand-thumbs-down-fill"></i>
                    </button>
                </>
            }
            <span className='form-control text-center reaction-buttons-rating'>
                {displayedRespect}
            </span>
            {showButtons && 
                <>
                    <button className={'btn btn-outline-primary ' + (clickedReaction === 1 ? 'active': '')} onClick={() => handleClick(1)}>
                        <i className="bi bi-hand-thumbs-up-fill"></i>
                    </button>
                    <button className={'btn btn-outline-primary ' + (clickedReaction === 2 ? 'active': '')} onClick={() => handleClick(2)}>
                        <i className="bi bi-hand-thumbs-up-fill"></i>
                        <i className="bi bi-hand-thumbs-up-fill"></i>
                    </button>
                </>
            }
        </div>
    );
}

export default ReactionButtons;