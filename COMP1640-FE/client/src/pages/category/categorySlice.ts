import { AsyncThunk, createAsyncThunk, createSlice, Slice } from '@reduxjs/toolkit';
import { Category } from '../../app/models/Category';
import agent from '../../app/api/agent';
import { toast } from 'react-toastify';

interface CategoryState {
    categories: Category[];
    category: Category | null;
    loading: boolean;
    isFetchCategoryId: boolean;
    error: string | null;
}

const initialState: CategoryState = {
    categories: [],
    category: null,
    loading: false,
    isFetchCategoryId: false,
    error: null,
};
export const getCategories: AsyncThunk<Category[], void, {}> = createAsyncThunk(
    'categories/getCategories',
    async () => {
        const response = await agent.Category.listCategory();
        return response;
    }
);
export const addCategory = createAsyncThunk('categories/addCategory', async (values: any) => {
    const response = await agent.Category.createCategory(values);
    return response;
});

export const updateCategory = createAsyncThunk('categories/updateCategory', async (values: any) => {
    const updatedCategory: Category = {
        id: values.id,
        name: values.name,
    };
    const response = await agent.Category.updateCategory(updatedCategory, values.id);
    return response;
});

export const deleteCategory = createAsyncThunk(
    'categories/deleteCategory',
    async (id: string) => {
        try {
            await agent.Category.deleteCategory(id);
            toast.success('Delete Record Successfully!', {
                style: { marginTop: '50px' },
                position: toast.POSITION.TOP_RIGHT
            });
            return { id };
        } catch (error: any) {
            // handle error
            toast.error(' Delete on table Departments violates foreign key constraint on table Users', {
                style: { marginTop: '50px' },
                position: toast.POSITION.TOP_RIGHT
            });
            throw error;
        }
    }
);

export const categorySlice: Slice<CategoryState> = createSlice({
    name: 'categories',
    initialState,
    reducers: {
    },
    extraReducers: builder => {
        //set get all categories
        builder
            .addCase(getCategories.pending, (state) => {
                state.loading = true;
            })
            .addCase(getCategories.fulfilled, (state, action) => {
                state.categories = action.payload;
                state.loading = false;
            })
            .addCase(getCategories.rejected, (state) => {
                state.loading = false;
            });

        //set add category
        builder.addCase(addCategory.fulfilled, (state, action) => {
            state.categories.push(action.payload);
        });

        //set update category
        builder.addCase(updateCategory.fulfilled, (state, action) => {
            const index = state.categories.findIndex(
                category => category.id === action.payload.id
            );
            if (index !== -1) {
                state.categories[index] = action.payload;
            }
        });

        //set delete category
        builder.addCase(deleteCategory.fulfilled, (state, action) => {
            const index = state.categories.findIndex(
                category => category.id === action.payload.id
            );
            if (index !== -1) {
                state.categories.splice(index, 1);
            }
        });
    },
});

export default categorySlice.reducer;