import './Comment.css';
import { useCallback, useEffect, useState } from "react";
import { useAuth } from '../../utils/providers/AuthProvider/AuthProvider';
import ReactionButtons from '../ReactionButtons/ReactionButtons';
import ReplyForm from './ReplyForm/ReplyForm';
import Replies from './Replies/Replies';

function Comment({comment, onReplyAdded}) {
    const imagePath = comment.AvatarUrl ?? "default.png";
    const [showReplyForm, setShowReplyForm] = useState(false);
    const [timestamp, setTimestamp] = useState('');
    const {isLoggedIn, openLoginPopup} = useAuth();

    const handleReplyAdded = () => {
        setShowReplyForm(false);
        onReplyAdded();
    };

    const handleReplyButton = () => {
        if(isLoggedIn) {
            setShowReplyForm((prev) => !prev)
        } else {
            openLoginPopup();
        };
    }

    const calculateTimestamp = useCallback(() => {
        const currentTime = new Date();
        const commentCreated = Date.parse(comment.created);
        const diffTime = Math.abs(currentTime - commentCreated);
        
        const diffMinutes = Math.floor(diffTime / (1000 * 60)); 
        const diffHours = Math.floor(diffMinutes / 60); 
        const diffDays = Math.floor(diffHours / 24);
        
        if (diffDays > 0) {
            setTimestamp(`${diffDays} day${diffDays > 1 ? 's' : ''} ago`);
        } else if (diffHours > 0) {
            setTimestamp(`${diffHours} hour${diffHours > 1 ? 's' : ''} ago`);
        } else if (diffMinutes > 0) {
            setTimestamp(`${diffMinutes} minute${diffMinutes > 1 ? 's' : ''} ago`);
        } else {
            setTimestamp('Just now');
        }
    }, [comment.created]);

    useEffect(() => {
        calculateTimestamp(); 
    }, [calculateTimestamp]);

    return(
        <>
            <div className={"comment border" + (showReplyForm ? ' comment-opened-reply-form' : '')}>
                <div className="comment-header">
                    <img className='comment-user-image' src={`/assets/images/users/${imagePath}`} alt={comment.createdBy}/>
                    <div className='comment-info'>
                        <span className='comment-username'>{comment.createdBy} </span>
                        <span className="comment-timestamp">{timestamp}</span>
                    </div>
                </div>
                <p className="comment-content">{comment.content}</p>
                <div className='comment-actions'>
                    <button className='comment-reply-btn btn btn-outline-primary' onClick={handleReplyButton}>
                        <i className="bi bi-reply-fill me-2"></i>Reply
                    </button>
                    <div className="reaction-buttons-container">
                        <ReactionButtons 
                            respect={comment.respect} 
                            defaultReaction={comment.currentUsersReaction}
                            targetType="comment"
                            targetId={comment.id}
                        />
                    </div>
                </div>
            </div>
            { 
                showReplyForm && 
                <ReplyForm 
                    commentId={comment.id} 
                    onCancel={() => setShowReplyForm(false)} 
                    onReplyAdded={handleReplyAdded}
                /> 
            }
            <Replies 
                replies={comment.children} 
                count={comment.childrenCount} 
            />
        </>
    );
}

export default Comment;