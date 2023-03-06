import { AsyncThunk, createAsyncThunk, createSlice, Slice } from '@reduxjs/toolkit';
import { Department } from '../../app/models/Department';
import agent from '../../app/api/agent';
import { toast } from 'react-toastify';

interface DepartmentState {
    departments: Department[];
    department: Department | null;
    loading: boolean;
    isFetchDepartmentId: boolean;
    error: string | null;
}

const initialState: DepartmentState = {
    departments: [],
    department: null,
    loading: false,
    isFetchDepartmentId: false,
    error: null,
};
export const getDepartments: AsyncThunk<Department[], void, {}> = createAsyncThunk(
    'departments/getDepartments',
    async () => {
        const response = await agent.Department.listDepartment();
        return response;
    }
);
export const addDepartment = createAsyncThunk('departments/addDepartment', async (values: any) => {
    const response = await agent.Department.createDepartment(values);
    return response;
});

export const updateDepartment = createAsyncThunk('departments/updateDepartment', async (values: any) => {
    const updatedDepartment: Department = {
        id: values.id,
        name: values.name,
    };
    const response = await agent.Department.updateDepartment(updatedDepartment, values.id);
    return response;
});

export const deleteDepartment = createAsyncThunk(
    'departments/deleteDepartment',
    async (id: string) => {
        try {
            await agent.Department.deleteDepartment(id);
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

export const departmentSlice: Slice<DepartmentState> = createSlice({
    name: 'departments',
    initialState,
    reducers: {
    },
    extraReducers: builder => {
        //set get all departments
        builder
            .addCase(getDepartments.pending, (state) => {
                state.loading = true;
            })
            .addCase(getDepartments.fulfilled, (state, action) => {
                state.departments = action.payload;
                state.loading = false;
            })
            .addCase(getDepartments.rejected, (state) => {
                state.loading = false;
            });

        //set add department
        builder.addCase(addDepartment.fulfilled, (state, action) => {
            state.departments.push(action.payload);
        });

        //set update department
        builder.addCase(updateDepartment.fulfilled, (state, action) => {
            const index = state.departments.findIndex(
                department => department.id === action.payload.id
            );
            if (index !== -1) {
                state.departments[index] = action.payload;
            }
        });

        //set delete department
        builder.addCase(deleteDepartment.fulfilled, (state, action) => {
            const index = state.departments.findIndex(
                department => department.id === action.payload.id
            );
            if (index !== -1) {
                state.departments.splice(index, 1);
            }
        });
    },
});

export default departmentSlice.reducer;