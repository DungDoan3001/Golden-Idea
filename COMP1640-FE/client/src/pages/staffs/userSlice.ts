import { AsyncThunk, createAsyncThunk, createSlice, Slice } from '@reduxjs/toolkit';
import { User } from '../../app/models/User';
import agent from '../../app/api/agent';
import { toast } from 'react-toastify';

interface UserState {
    users: User[];
    user: User | null;
    loading: boolean;
    isFetchUserId: boolean;
    error: string | null;
}

const initialState: UserState = {
    users: [],
    user: null,
    loading: false,
    isFetchUserId: false,
    error: null,
};

export const getUsers: AsyncThunk<User[], void, {}> = createAsyncThunk(
    'users/getUsers',
    async () => {
        const response = await agent.User.listUsers();
        return response;
    }
);

export const addUser = createAsyncThunk('users/addUser', async (values: any) => {
    const response = await agent.User.createUser(values);
    return response;
});

export const updateUser = createAsyncThunk('users/updateUser', async (values: any) => {
    const updatedUser: User = {
        id: values.id,
        email: values.email,
        name: values.name,
        username: values.username,
        password: values.password,
        address: values.address,
        phoneNumber: values.phoneNumber,
        departmentId: values.departmentId,
        file: values.file,
        role: values.role
    };
    const response = await agent.User.updateUser(updatedUser, values.id);
    return response;
});

export const deleteUser = createAsyncThunk(
    'users/deleteUser',
    async (id: string) => {
        try {
            await agent.User.deleteUser(id);
            toast.success('Delete Record Successfully!', {
                style: { marginTop: '50px' },
                position: toast.POSITION.TOP_RIGHT
            });
            return { id };
        } catch (error: any) {
            // handle error
            toast.error(' Delete on table Users violates foreign key constraint on table Posts', {
                style: { marginTop: '50px' },
                position: toast.POSITION.TOP_RIGHT
            });
            throw error;
        }
    }
);

export const userSlice: Slice<UserState> = createSlice({
    name: 'users',
    initialState,
    reducers: {
    },
    extraReducers: builder => {
        //set get all users
        builder
            .addCase(getUsers.pending, (state) => {
                state.loading = true;
            })
            .addCase(getUsers.fulfilled, (state, action) => {
                state.users = action.payload;
                state.loading = false;
            })
            .addCase(getUsers.rejected, (state) => {
                state.loading = false;
            });

        //set add user
        builder.addCase(addUser.fulfilled, (state, action) => {
            state.users.push(action.payload);
        });

        //set update user
        builder.addCase(updateUser.fulfilled, (state, action) => {
            const index = state.users.findIndex(
                user => user.id === action.payload.id
            );
            if (index !== -1) {
                state.users[index] = action.payload;
            }
        });

        //set delete user
        builder.addCase(deleteUser.fulfilled, (state, action) => {
            const index = state.users.findIndex(
                user => user.id === action.payload.id
            );
            if (index !== -1) {
                state.users.splice(index, 1);
            }
        });
    },
});

export default userSlice.reducer;