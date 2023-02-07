import { configureStore } from "@reduxjs/toolkit";
import { useDispatch } from "react-redux";
import { globalSlice } from "../utils/AppSlice";
export const store = configureStore({
    reducer: {
        global: globalSlice.reducer,
        }
    })
    
//export type AppDispatch = typeof store.dispatch;
export const useAppDispatch = () => useDispatch<any>();