import "./CreateActivityPage.css";
import { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { useNotification } from "../../../utils/providers/NotificationProvider/NotificationProvider";
import { useAuth } from "../../../utils/providers/AuthProvider/AuthProvider";
import { getPersonsNames } from "../../../services/personService";
import { getPersonTags } from "../../../services/tagService";
import { postActivity } from "../../../services/activityService";
import PersonDropdown from "./PersonDropdown/PersonDropdown";
import TagDropdown from "./TagDropdown/TagDropdown";

function CreateActivityPage() {
    const [formData, setFormData] = useState({ type: 1, personId: '', title: '', description: '', happend: '', location: '', source: '', tags: '' });
    const [persons, setPersons] = useState([]);
    const [personTags, setPersonTags] = useState([]);
    const { logout, openLoginPopup } = useAuth();
    const navigate = useNavigate();
    const { notify } = useNotification();

    const loadPersons = async () => {
        console.log("CreateActivityPage: loadPersons()");
        setPersons([]);

        getPersonsNames()
            .then(response => {
                setPersons(response.data);
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

    const loadPersonTags = useCallback(async (id) => {
        console.log('CreateActivityPage.loadPersonTags(id: \'' + id + '\')');
        setPersonTags([]);

        getPersonTags(id)
            .then(response => {
                setPersonTags(response.data);
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
    }, []);

    const handleDataChange = (e) => {
        const { name, value } = e.target;
        if (name === "type") {
            setFormData({ ...formData, [name]: Number(value) })
        } else if (name === "happend") {
            setFormData({ ...formData, [name]: new Date(value).toISOString() })
        } else {
            setFormData({ ...formData, [name]: value });
        };
    };

    const handlePersonChange = useCallback((id) => {
        console.log(`CreateActivityPage.handlePersonChange(id: '${id}')`);
        if (id) {
            setFormData({ ...formData, personId: id });
            loadPersonTags(id);
        } else {
            setFormData({ ...formData, personId: '' });
            setPersonTags([]);
        }
    }, [loadPersonTags]);

    const handleTagsChange = useCallback((selectedTags) => {
        console.log(`CreateActivityPage.handleTagsChange(selectedTags[${selectedTags.length}])`);
        if (selectedTags.length > 0) {
            setFormData({ ...formData, tags: selectedTags.map(t => t.name.toString()).join(',') });
        } else {
            setFormData({ ...formData, tags: '' });
        }
    }, []);

    const handleCreateButton = async () => {
        console.log('CreateActivityPage: handleCreateButton()', formData);
        postActivity(formData)
            .then((response) => {
                console.log('CreateActivityPage: success ', response.data);
                notify({type: 'success', message: 'Activity was successfully added.'});
                navigate(`/act/${response.data.id}`);
            })
            .catch(error => {
                if (error.response && error.response.status === 401) {
                    console.warn('Unauthorized! Redirecting to login...');
                    logout();
                    openLoginPopup();
                } else {
                    console.error('An error occurred:', error);
                }
            });
    }

    useEffect(() => {
        loadPersons();
    }, []);

    return (
        <>
            <div></div>
            <div>
                <h2 className="mt-5">Create a new activity</h2>
                <span>Type<span className="text-danger">*</span>: </span>
                <div className="input-group w-75 mb-3">
                    <div className="mt-1">
                        <div className="form-check form-check-inline">
                            <input
                                id="inlineRadioCitation"
                                className="form-check-input"
                                type="radio"
                                name="type"
                                value={Number(1)}
                                onChange={handleDataChange}
                                checked={formData.type === 1}
                            />
                            <label className="form-check-label" htmlFor="inlineRadioCitation">Quotation</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input
                                id="inlineRadioAct"
                                className="form-check-input"
                                type="radio"
                                name="type"
                                value={Number(2)}
                                onChange={handleDataChange}
                                checked={formData.type === 2}
                            />
                            <label className="form-check-label" htmlFor="inlineRadioAct">Act</label>
                        </div>
                    </div>
                </div>
                <div className="w-75 mb-3">
                    <PersonDropdown title={"Person"} options={persons} onPersonChange={handlePersonChange} />
                </div>
                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">
                        {formData.type === 1 ? "Exact quotation" : "Title"}
                        <span className="text-danger">*</span>
                    </span>
                    <input className="form-control" name="title" onChange={handleDataChange} required />
                </div>
                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">
                        Description
                    </span>
                    <textarea className="form-control" name="description" placeholder="" onChange={handleDataChange} />
                </div>
                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">Happend<span className="text-danger">*</span>:</span>
                    <input className="form-control" name="happend" type="date" onChange={handleDataChange} />
                </div>
                {formData.type === 2 &&
                    <div className="input-group w-75 mb-3">
                        <span className="input-group-text">
                            Location
                        </span>
                        <input className="form-control" name="location" placeholder="" onChange={handleDataChange} />
                    </div>
                }

                <div className="input-group w-75 mb-3">
                    <span className="input-group-text">
                        Source
                        <span className="text-danger">*</span>
                    </span>
                    <input className="form-control" name="source" placeholder="" onChange={handleDataChange} required />
                </div>
                <span>Add some tags:</span>
                <div className="w-75 mb-3">
                    <TagDropdown personTags={personTags} onTagsChange={handleTagsChange} />
                </div>

                <div className="mt-3 w-75 text-end">
                    <span>
                        Fields marked with <span className="text-danger">*</span> are required.
                    </span>
                </div>
                <div className="mt-3 w-75 text-end">
                    <button className="btn btn-outline-primary" onClick={handleCreateButton}>Create</button>
                </div>


            </div>
            <div></div>
        </>
    );
}

export default CreateActivityPage;