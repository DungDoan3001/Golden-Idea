export interface User {
    id: string;
    name: string;
    userName: string;
    email: string;
    password: string;
    address: string;
    phoneNumber: string,
    file: string;
    departmentId: string;
    role: string;
}
export interface UserLogin {
    name: string;
    emailaddress: string;
    Avatar: string;
    role: string[];
    token?: string;
}