import '../Comment.css';
import { useState } from "react";
import { useAuth } from '../../../utils/providers/AuthProvider/AuthProvider';
import { postComment } from '../../../services/commentService';

function ReplyForm({commentId, onReplyAdded, onCancel}) {
    const [ inputValue, setInputValue ] = useState('');
    const { isLoggedIn, openLoginPopup, logout } = useAuth();

    const handleInputValueChange = (e) => {
        setInputValue(e.target.value);
    };

    const handleAddCommentClick = async () => {
        if(!isLoggedIn) {
            openLoginPopup();
            return;
        }

        postComment(`/api/comment/${commentId}`, inputValue)
            .then((response) => {
                console.log('ReplyForm: success ', response.data);
                if(onReplyAdded) onReplyAdded();
                setInputValue('');
            })
            .catch((error) => {
                if(error.status === 401) {
                    logout();
                    onCancel();
                } else {
                    console.error('Error:', error);
                };
            });
    };

    return(
        <div className="comment-reply-form input-group">
            <input className="form-control" value={inputValue} onChange={handleInputValueChange} placeholder='Reply...'/>
            <button className="btn btn-outline-primary" onClick={onCancel}><i className="bi bi-x-square-fill"></i></button>
            <button className="btn btn-outline-primary" onClick={handleAddCommentClick}><i className="bi bi-caret-right-fill"></i></button>
        </div>
    );
}

export default ReplyForm;