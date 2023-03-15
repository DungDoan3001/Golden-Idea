import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { store } from "../store/configureStore";
// import { PaginatedResponse } from "../models/pagination";
const sleep = () => new Promise(resolve => setTimeout(resolve, 8000));

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

//Login here (Bearer token)
axios.interceptors.request.use(config => {
    const token: any = store.getState().account.user?.token;
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

axios.interceptors.response.use(async response => {
    if (process.env.NODE_ENV === 'development') await sleep();
    return response;
}, (error: AxiosError) => {
    const { data, status }: any = error.response!;
    const navigate = useNavigate();
    switch (status) {
        case 400:
            if (data.errors) {
                const modelStateErrors: string[] = [];
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        modelStateErrors.push(data.errors[key])
                    }
                }
                throw modelStateErrors.flat();
            }
            toast.error(data.title);
            break;
        case 401:
            toast.error(data.title);
            break;
        case 403:
            toast.error('You are not allowed to do that!');
            break;
        case 500:
            navigate('/server-error', {
                state: { error: data }
            });
            break;
        default:
            break;
    }
    return Promise.reject(error.response);
})

const requests = {
    get: (url: string, params?: URLSearchParams) => axios.get(url, { params }).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody),
    postForm: (url: string, data: FormData) => axios.post(url, data, {
        headers: { 'Content-type': 'multipart/form-data' }
    }).then(responseBody),
    putForm: (url: string, data: FormData) => axios.put(url, data, {
        headers: { 'Content-type': 'multipart/form-data' }
    }).then(responseBody)
}
const createFormData = (item: any) => {
    let formData = new FormData();
    for (const key in item) {
        formData.append(key, item[key])
    }
    return formData;
}
const Download = {
    downloadDashboard: () => requests.get('ZipFiles/download-data-dashboard')
}
const Department = {
    listDepartment: () => requests.get('departments'),
    createDepartment: (values: any) => requests.post('departments', values),
    updateDepartment: (values: any, id: string) => requests.put(`departments/${id}`, values),
    deleteDepartment: (id: string) => requests.delete(`departments/${id}`)
}
const Category = {
    listCategory: () => requests.get('categories'),
    createCategory: (values: any) => requests.post('categories', values),
    updateCategory: (values: any, id: string) => requests.put(`categories/${id}`, values),
    deleteCategory: (id: string) => requests.delete(`categories/${id}`)
}

const Topic = {
    listTopics: () => requests.get('topics'),
    listUserTopics: (username: any) => requests.get(`topics/user/${username}`),
    createTopic: (values: any) => requests.post('topics', values),
    updateTopic: (values: any, id: string) => requests.put(`topics/${id}`, values),
    deleteTopic: (id: string) => requests.delete(`topics/${id}`),
    downloadZip: (id: string) => requests.get(`topics/${id}/download-zip`)
}
const Idea = {
    listIdeas: (id: any) => requests.get(`ideas/topic/${id}`),
    listDashboardIdeas: () => requests.get('ideas'),
    getIdeaDetail: (id: any) => requests.get(`ideas/id/${id}`),
    createCategory: (values: any) => requests.post('categories', values),
    updateCategory: (values: any, id: string) => requests.put(`categories/${id}`, values),
    deleteCategory: (id: string) => requests.delete(`categories/${id}`),
    getIdeaBySlug: (slug:any) =>requests.get(`ideas/slug/${slug}`),
}
const User = {
    listUsers: () => requests.get('User'),
    listRoles: () => requests.get('roles'),
    createUser: (user: any) => requests.postForm('authentication/register', createFormData(user)),
    updateUser: (user: any, id: any) => requests.putForm(`User/${id}`, createFormData(user)),
    deleteUser: (id: string) => requests.delete(`User/${id}`)
}
const Account = {
    login: (values: any) => requests.post('authentication/login', values),
    forgotpassword: (values: any) => requests.post('authentication/change-password', values),
    resetpassword: (resetCode: string, values: any) => requests.put(`authentication/change-password/${resetCode}`, values)
}
const Chart = {
    contributorChart: () => requests.get('charts/contributors-by-department'),
    breakdownChart: () => requests.get('charts/percentage-of-ideas-by-department'),
    exceptionChart: () => requests.get('charts/num-of-ideas-anonymous-by-department'),
    overallChart: () => requests.get('charts/get-staff-idea-comment-topic'),
    overviewChart: () => requests.get('charts/ideas-by-department'),
    dailyChart: () => requests.get('charts/daily-report-in-three-months'),
    commentChart: () => requests.get('charts/num-of-comment-by-department'),
}
const agent = {
    Department,
    Category,
    Topic,
    User,
    Account,
    Chart,
    Idea,
    Download,
}

export default agent;