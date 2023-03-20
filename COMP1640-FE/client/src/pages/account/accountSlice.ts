import { createAsyncThunk, createSlice, isAnyOf } from "@reduxjs/toolkit";
import { FieldValues } from "react-hook-form";
import { toast } from "react-toastify";
import agent from "../../app/api/agent";
import { UserLogin } from "../../app/models/User";
import { router } from "../../app/routes/Routers";


interface AccountState {
  user: UserLogin | null
}

const initialState: AccountState = {
  user: null
}
export const signInUser = createAsyncThunk<UserLogin, FieldValues>(
  'account/signInUser',
  async (data, thunkAPI) => {
    try {
      const userDto = await agent.Account.login(data);
      const { ...user } = userDto;
      sessionStorage.setItem('user', JSON.stringify(user));
      return user.token;
    } catch (error: any) {
      toast.error(`Make sure your username and password is correct`);
      return thunkAPI.rejectWithValue({ error: error.data })
    }
  }
)
export const fetchCurrentUser = createAsyncThunk<UserLogin>(
  'account/fetchCurrentUser',
  async (_, thunkAPI) => {
    thunkAPI.dispatch(setUser(JSON.parse(sessionStorage.getItem('user')!)))
    try {
      const userDto = sessionStorage.getItem('user');
      const user = JSON.parse(userDto!);
      sessionStorage.setItem('user', JSON.stringify(user));
      return user;
    } catch (error) {
      return thunkAPI.rejectWithValue(error);
    }
  },
  {
    condition: () => {
      if (!sessionStorage.getItem('user')) return false;
    }
  }
)
export const accountSlice = createSlice({
  name: 'account',
  initialState,
  reducers: {
    signOut: (state) => {
      state.user = null;
      sessionStorage.removeItem('user');
      router.navigate('/login');
    },
    setUser: (state, action) => {
      let tokenString = action.payload.toString();
      let claims = JSON.parse(
        atob(sessionStorage.getItem("user")!.split(".")[1])
      );
      let role = claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      state.user = {
        name: claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
        emailaddress: claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
        Avatar: claims.Avatar,
        role: typeof (role) === 'string' ? [role] : role,
        token: tokenString
      };
    }
  },
  extraReducers: (builder => {
    builder.addCase(fetchCurrentUser.rejected, (state) => {
      state.user = null;
      sessionStorage.removeItem('user');
      toast.error('Session expired - please login again');
      router.navigate('/login');
    })
    builder.addMatcher(isAnyOf(signInUser.fulfilled, fetchCurrentUser.fulfilled), (state, action) => {
      let tokenString = action.payload.toString();
      let claims = JSON.parse(
        atob(sessionStorage.getItem("user")!.split(".")[1])
      );
      let role = claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      state.user = {
        name: claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
        emailaddress: claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
        Avatar: claims.Avatar,
        role: typeof (role) === 'string' ? [role] : role,
        token: tokenString
      };
    });
    builder.addMatcher(isAnyOf(signInUser.rejected), (state, action) => {
      throw action.payload;
    })
  })
})

export const { signOut, setUser } = accountSlice.actions;
