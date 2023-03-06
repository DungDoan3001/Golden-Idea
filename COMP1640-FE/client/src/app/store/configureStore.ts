import { configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { globalSlice } from "../utils/AppSlice";
import { accountSlice } from "../../pages/account/accountSlice";
import { departmentSlice } from "../../pages/department/departmentSlice";
import { categorySlice } from "../../pages/category/categorySlice";

export const store = configureStore({
    reducer: {
        global: globalSlice.reducer,
        account: accountSlice.reducer,
        department: departmentSlice.reducer,
        category: categorySlice.reducer,
    }
})

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;