import { configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { globalSlice } from "../utils/AppSlice";
import { accountSlice } from "../../pages/account/accountSlice";
import { departmentSlice } from "../../pages/department/departmentSlice";
import { categorySlice } from "../../pages/category/categorySlice";
import { topicSlice } from "../../pages/topic/topicSlice";
import { userSlice } from "../../pages/staffs/userSlice";
import { ideaSlice } from "../../pages/myIdeas/ideasSlice";
import { commentsSlice } from "../../pages/comment/commentSlice";

export const store = configureStore({
    reducer: {
        global: globalSlice.reducer,
        account: accountSlice.reducer,
        user: userSlice.reducer,
        department: departmentSlice.reducer,
        category: categorySlice.reducer,
        topic: topicSlice.reducer,
        idea: ideaSlice.reducer,
        comment: commentsSlice.reducer,
    }
})

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;