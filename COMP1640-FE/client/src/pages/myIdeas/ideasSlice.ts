import { AsyncThunk, createAsyncThunk, createSlice, Slice } from '@reduxjs/toolkit';
import { Idea } from '../../app/models/Idea';
import agent from '../../app/api/agent';
import { toast } from 'react-toastify';

export interface GetMyIdeasParams {
  topicId: any;
  username: string;
}
interface IdeaState {
  ideas: Idea[];
  ideas_dashboard: Idea[];
  ideas_user: Idea[]
  idea: Idea | null;
  loading: boolean;
  error: string | null;
}

const initialState: IdeaState = {
  ideas: [],
  ideas_dashboard: [],
  ideas_user: [],
  idea: null,
  loading: false,
  error: null,
};
export const getIdeas: AsyncThunk<Idea[], any, {}> = createAsyncThunk(
  'ideas/getIdeas',
  async (topicId: any) => {
    const response = await agent.Idea.listIdeas(topicId);
    return response;
  }
);
export const getMyIdeas: AsyncThunk<Idea[], GetMyIdeasParams, {}> = createAsyncThunk(
  'ideas/getMyIdeas',
  async ({ topicId, username }) => {
    const response = await agent.Idea.listUserIdeas(topicId, username);
    console.log(response);
    return response;
  }
);
export const getDashboardIdeas: AsyncThunk<Idea[], void, {}> = createAsyncThunk(
  'ideas/getDashboardIdeas',
  async () => {
    const response = await agent.Idea.listDashboardIdeas();
    return response;
  }
);

export const getIdea: AsyncThunk<Idea, string, {}> = createAsyncThunk(
  'ideas/getIdea',
  async (id: string) => {
    const response = await agent.Idea.getIdeaDetail(id);
    return response;
  }
);

export const addIdea = createAsyncThunk('idea/addIdea', async (values: any) => {
  const response = await agent.Idea.createIdea(values);
  return response;
});

export const getIdeaBySlug: AsyncThunk<Idea, any, {}> = createAsyncThunk(
  'idea/getIdeaBySlug',
  async (slug: any) => {
    const response = await agent.Idea.getIdeaBySlug(slug);
    return response;
  }
);

export const ideaSlice: Slice<IdeaState> = createSlice({
  name: 'ideas',
  initialState,
  reducers: {},
  extraReducers: builder => {
    // set get all ideas by topic id
    builder
      .addCase(getIdeas.pending, (state) => {
        state.loading = true;
      })
      .addCase(getIdeas.fulfilled, (state, action) => {
        state.ideas = action.payload;
        state.loading = false;
      })
      .addCase(getIdeas.rejected, (state) => {
        state.loading = false;
      });
    // set get all ideas to dashboard
    builder
      .addCase(getDashboardIdeas.pending, (state) => {
        state.loading = true;
      })
      .addCase(getDashboardIdeas.fulfilled, (state, action) => {
        state.ideas_dashboard = action.payload;
        state.loading = false;
      })
      .addCase(getDashboardIdeas.rejected, (state) => {
        state.loading = false;
      });
    // set get idea by id
    builder
      .addCase(getIdea.pending, (state) => {
        state.loading = true;
        state.idea = null;
      })
      .addCase(getIdea.fulfilled, (state, action) => {
        state.idea = action.payload;
        state.loading = false;
      })
      .addCase(getIdea.rejected, (state) => {
        state.loading = false;
      });
    // set get ideas by user
    builder
      .addCase(getMyIdeas.pending, (state) => {
        state.loading = true;
      })
      .addCase(getMyIdeas.fulfilled, (state, action) => {
        state.ideas_user = action.payload;
        state.loading = false;
      })
      .addCase(getMyIdeas.rejected, (state) => {
        state.loading = false;
      });
    // set get idea by slug
    builder
      .addCase(getIdeaBySlug.pending, (state) => {
        state.loading = true;
        state.idea = null;
      })
      .addCase(getIdeaBySlug.fulfilled, (state, action) => {
        state.idea = action.payload;
        state.loading = false;
      })
      .addCase(getIdeaBySlug.rejected, (state) => {
        state.loading = false;
      });
    // set add idea
    builder.addCase(addIdea.fulfilled, (state, action) => {
      state.ideas.push(action.payload);
    });
  }
})
export default ideaSlice.reducer;
