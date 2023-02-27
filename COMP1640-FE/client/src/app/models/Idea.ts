export interface Idea {
    id: string;
    title:string;
    content: string;
    image: any;
    lastUpdate: Date;
    userID?:string;
    categoryID?: string;
    topicID?: string;
}