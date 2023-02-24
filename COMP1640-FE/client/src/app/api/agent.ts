import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { store } from "../store/configureStore";
// import { PaginatedResponse } from "../models/pagination";
const sleep = () => new Promise(resolve => setTimeout(resolve, 500));

axios.defaults.baseURL = process.env.REACT_APP_API_URL;
axios.defaults.withCredentials = true;

const responseBody = (response: AxiosResponse) => response.data;

//Login here (Bearer token)
axios.interceptors.request.use(config => {
    const token: any = store.getState().account.user?.token;
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

axios.interceptors.response.use(async response => {
    if (process.env.NODE_ENV === 'development') await sleep();
    const pagination = response.headers['pagination'];
    if (pagination) {
        //Pagination here!!!
        //response.data = new PaginatedResponse(response.data, JSON.parse(pagination));
        return response;
    }
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
            navigate('/server-error',{
                state: {error: data}
            });
            break;
        default:
            break;
    }
    return Promise.reject(error.response);
})

const requests = {
    get: (url: string, params?: URLSearchParams) => axios.get(url, {params}).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody),
    postForm: (url: string, data: FormData) => axios.post(url, data, {
        headers: {'Content-type': 'multipart/form-data'}
    }).then(responseBody),
    putForm: (url: string, data: FormData) => axios.put(url, data, {
        headers: {'Content-type': 'multipart/form-data'}
    }).then(responseBody)
}
const createFormData = (item: any) =>{
    let formData = new FormData();
    for (const key in item) {
        formData.append(key, item[key])
    }
    return formData;
}
const User = {
    listUser: () => requests.get('users'),
    createUser: (user: any) => requests.postForm('users', createFormData(user)),
    updateUser: (user: any) => requests.putForm('users', createFormData(user)),
    deleteUser: (id: number) => requests.delete(`users/${id}`)
}
const Account = {
    login: (values: any) => requests.post('authentication/login', values),
    currentUser: () => requests.get('account/currentUser'),
    forgotpassword: (input: any) => requests.postForm('account/forgotpassword',createFormData(input)),
    resetpassword: (input: any, resetCode: any) => requests.putForm(`authentication/change-password/${resetCode}`,createFormData(input))
}
const agent = {
    User,
    Account
}

export default agent;