export interface Idea {
  id?: string;
  title: string;
  slug: string;
  content: string;
  image: string;
  isAnonymous: boolean;
  lastUpdate: any;
  createdAt: any;
  upVote: number;
  downVote: number;
  view: number;
  user: {
    id: string;
    userName: string;
    email: string;
    avatar: string;
  };
  topic: {
    id: string;
    name: string;
    username: string;
    avatar: string;
    closureDate: any;
    finalClosureDate: any;
  };
  category: {
    id: string;
    name: string;
  };
  files: any;
}