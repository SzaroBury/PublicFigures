import './CreatePersonPage.css';
import { useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { postPerson } from '../../../services/personService';
import TagDropdown from './TagDropdown/TagDropdown';
import { useNotification } from '../../../utils/providers/NotificationProvider/NotificationProvider';

function CreatePersonPage() {
    const [formData, setFormData] = useState({ firstName: '', lastName: '', nickName: '', description: '', nationality: '', birthday: '', deathDate: '', tags: '' });
    const navigate = useNavigate();
    const { notify } = useNotification();

    const handleDataChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleTagsChange = useCallback((selectedTags) => {
        // console.log(`CreatePersonPage.handleTagsChange(selectedTags[${selectedTags.length}])`);
        setFormData(prev => ({
            ...prev,
            tags: selectedTags.length > 0
                ? selectedTags.map(t => t.name.toString()).join(',')
                : ''
        }));
    }, []);

    const handleCreateButton = async () => {
        console.log('CreatePersonPage: handleCreateButton()', formData);
        postPerson(formData)
            .then((response) => {
                navigate(`/person/${response.data.id}`);
                notify({type: 'success', message: 'Person was succesfully added.'})
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
    };

    const handleCancelButton = async () => {
        navigate(-1);
    }

    return (
        <>
            <div></div>
            <div>
                <h2 className='mt-5'>Create a new person</h2>
                <div className="input-group w-75 mb-3">
                    <span className='input-group-text'><span className='text-danger'>* </span> Names:</span>
                    <input className='form-control' name='firstName' placeholder='First name...' onChange={handleDataChange} />
                    <input className='form-control' name='lastName' placeholder='Last name...' onChange={handleDataChange} />
                </div>
                <div className="input-group w-75 mb-3">
                    <span className='input-group-text'>Nickname:</span>
                    <input className='form-control' name='nickName' placeholder='Nick name...' onChange={handleDataChange} />
                </div>
                <div className="input-group w-75 mb-3">
                    <span className='input-group-text'><span className='text-danger'>*</span>Profession:</span>
                    <input className='form-control' name='profession' placeholder='Nick name...' onChange={handleDataChange} />
                </div>
                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">
                        Description:
                    </span>
                    <textarea className="form-control" name="description" placeholder="" onChange={handleDataChange} />
                </div>
                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">Birth date:</span>
                    <input className="form-control" name="birthday" type="date" onChange={handleDataChange}/>
                    <span className="input-group-text">Death date:</span>
                    <input className="form-control" name="deathDate" type="date" onChange={handleDataChange}/>
                </div>
                <div className="input-group w-75 mb-3">
                    <span className='input-group-text'>Nationality:</span>
                    <input className="form-control" name="nationality" onChange={handleDataChange}/>
                </div>
                <span>Add some tags:</span>
                <div className="w-75 mb-3">
                    <TagDropdown onTagsChange={handleTagsChange} />
                </div>

                <div className="mt-3 w-75 text-end">
                    <span>
                        Fields marked with <span className="text-danger">*</span> are required.
                    </span>
                </div>
                <div className="mt-3 w-75 text-end">
                    <button className="btn btn-outline-primary m-2" onClick={handleCancelButton}>
                        <h6 className='m-2'>Back</h6>
                    </button>
                    <button className="btn btn-outline-primary" onClick={handleCreateButton}>
                        <h4 className='m-3'>Create</h4>
                    </button>
                </div>
            </div>
            <div></div>
        </>
    );
}

export default CreatePersonPage;