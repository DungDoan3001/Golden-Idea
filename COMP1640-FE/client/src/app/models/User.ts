export interface User {
    id: string;
    userName:string;
    password: string;
    email: string;
    address: string;
    avatarURL:string;
    deparmentID?: string;
    roles?: string[];
    token: string;
}