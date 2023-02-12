export interface User {
    userName:string;
    password: string;
    email: string;
    address: string;
    avatarURL:string;
    deparmentID?: string;
    roles?: string[];
}