import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ChatComment } from "../../app/models/Comment";

interface CommentState {
    comments: ChatComment[];
}

const initialState: CommentState = {
    comments: [],
};
export const commentsSlice = createSlice({
    name: "comments",
    initialState,
    reducers: {
        addComment: (state, action: PayloadAction<ChatComment>) => {
            state.comments.push(action.payload);
        },
        setComments: (state, action: PayloadAction<ChatComment[]>) => {
            state.comments = action.payload;
        },
    },
});


export default commentsSlice.reducer;