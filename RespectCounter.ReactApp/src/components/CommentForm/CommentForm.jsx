import "./CommentForm.css";
import { useState } from "react";
import { useAuth } from "../../utils/providers/AuthProvider/AuthProvider";
import { postComment } from "../../services/commentService";

function CommentForm({targetActivityId = "", targetPersonId = "", onCommentAdded}) {
    const [ content, setContent ] = useState("");
    const { isLoggedIn, openLoginPopup, logout } = useAuth();
    let targetUrl = "";

    if (targetActivityId !== "") 
    {
        targetUrl = `/api/activity/${targetActivityId}/comment`;
    } 
    else if (targetPersonId !== "") 
    {
        targetUrl = `/api/person/${targetPersonId}/comment`;
    } 

    const handleChange = (e) => {
        setContent(e.target.value);
    };

    const handleAddCommentClick = async () => {
        if(!isLoggedIn) 
        {
            openLoginPopup();
            return;
        }
        
        try {
            console.log('CommentForm: sending a comment request.');
            const response = await postComment(targetUrl, content);
            console.log('CommentForm: success ', response.data);
            if(onCommentAdded) onCommentAdded();
            setContent("");
        } catch (error) {
            if (error.response && error.response.status === 401) {
                console.warn('Unauthorized! Redirecting to login...');
                logout();
                openLoginPopup();
            } else {
                console.error('An error occurred:', error);
            }
        }
    };
    
    return (
        <div className="comment-form input-group">
            <textarea className="comment-form-input form-control" value={content} onChange={handleChange} placeholder="Share your thoughts..."></textarea>
            <button className="btn btn-outline-primary" onClick={handleAddCommentClick}><i className="bi bi-send-fill"></i></button>
        </div>
    );
}

export default CommentForm;