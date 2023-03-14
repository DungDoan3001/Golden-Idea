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
        loadComments: (state, action: PayloadAction<any[]>) => {
            state.comments = action.payload;
        },
    },
});

export const { addComment, loadComments } = commentsSlice.actions
export default commentsSlice.reducer;