import api from "../utils/interceptors/refreshInterceptor";

export const getPersons = async ({search = '', tags = [], order = '', onlyVerified = false, page, pageSize } = {}) => {
    const params = {};
    if (search) params.search = params.search = search;
    if (tags && tags.length > 0) params.tags = tags.map(ts => ts.name).join(",");
    if (order) params.order = order;
    if (page) params.page = page;
    if (pageSize) params.pageSize = pageSize;
    const targetUrl = onlyVerified ? '' : '/all'

    return api.get(`/api/persons${targetUrl}`, { params });
};

export const getPerson = (personId) => {
    return api.get(`/api/person/${personId}`);
};

export const getPersonsNames = () => {
    return api.get(`/api/persons/names`);
};

export const postPerson = (person) => {
    return api.post('/api/person', person);
};

export const hidePerson = (personId) => {
    return api.post('/api/person/hide', { personId });
};

export const verifyPerson = (personId) => {
    return api.put(`/api/person/${personId}/verify`);
};