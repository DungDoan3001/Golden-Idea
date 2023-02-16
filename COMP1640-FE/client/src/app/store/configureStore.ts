import { configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { globalSlice } from "../utils/AppSlice";
export const store = configureStore({
    reducer: {
        global: globalSlice.reducer,
        }
    })
    
    export type RootState = ReturnType<typeof store.getState>;
    export type AppDispatch = typeof store.dispatch;
    
    export const useAppDispatch = () => useDispatch<AppDispatch>();
    export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;