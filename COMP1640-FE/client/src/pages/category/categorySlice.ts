import { createAsyncThunk, createEntityAdapter, createSlice } from "@reduxjs/toolkit";
import { Category } from "../../app/models/Category";
interface CategoryState {
    category: Category | null;
    status: string;
}
const initialState: CategoryState = {
	category: null,
    status: 'idle'
}