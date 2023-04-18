import { AsyncThunk, createAsyncThunk, createSlice, Slice } from '@reduxjs/toolkit';
import { Topic } from '../../app/models/Topic';
import agent from '../../app/api/agent';
import { toast } from 'react-toastify';

interface TopicState {
    topics: Topic[];
    user_topics: Topic[];
    topic: Topic | null;
    loading: boolean;
    isFetchTopicId: boolean;
    error: string | null;
}

const initialState: TopicState = {
    topics: [],
    user_topics: [],
    topic: null,
    loading: false,
    isFetchTopicId: false,
    error: null,
};

export const getTopics: AsyncThunk<Topic[], void, {}> = createAsyncThunk(
    'topics/getTopics',
    async () => {
        const response = await agent.Topic.listTopics();
        return response;
    }
);
export const getUserTopics: AsyncThunk<Topic[], string, {}> = createAsyncThunk(
    'topics/getUserTopics',
    async (id: any) => {
        const response = await agent.Topic.listUserTopics(id);
        return response;
    }
);

export const addTopic = createAsyncThunk('topics/addTopic', async (values: any) => {
    const response = await agent.Topic.createTopic(values);
    return response;
});

export const updateTopic = createAsyncThunk('topics/updateTopic', async (values: any) => {
    const updatedTopic: Topic = {
        id: values.id,
        name: values.name,
        username: values.username,
        closureDate: values.closureDate,
        finalClosureDate: values.finalClosureDate,
    };
    const response = await agent.Topic.updateTopic(updatedTopic, values.id);
    return response;
});

export const deleteTopic = createAsyncThunk(
    'topics/deleteTopic',
    async (id: string) => {
        try {
            await agent.Topic.deleteTopic(id);
            toast.success('Delete Record Successfully!', {
                style: { marginTop: '50px' },
                position: toast.POSITION.TOP_RIGHT
            });
            return { id };
        } catch (error: any) {
            // handle error
            toast.error(' Delete on table Topics violates foreign key constraint on table Posts', {
                style: { marginTop: '50px' },
                position: toast.POSITION.TOP_RIGHT
            });
            throw error;
        }
    }
);

export const topicSlice: Slice<TopicState> = createSlice({
    name: 'topics',
    initialState,
    reducers: {
    },
    extraReducers: builder => {
        //set get all topics
        builder
            .addCase(getTopics.pending, (state) => {
                state.loading = true;
            })
            .addCase(getTopics.fulfilled, (state, action) => {
                state.topics = action.payload;
                state.loading = false;
            })
            .addCase(getTopics.rejected, (state) => {
                state.loading = false;
            });
        //set get user topics
        builder
            .addCase(getUserTopics.pending, (state) => {
                state.loading = true;
            })
            .addCase(getUserTopics.fulfilled, (state, action) => {
                state.user_topics = action.payload;
                state.loading = false;
            })
            .addCase(getUserTopics.rejected, (state) => {
                state.loading = false;
            });
        //set add topic
        builder.addCase(addTopic.fulfilled, (state, action) => {
            state.topics.push(action.payload);
        });

        //set update topic
        builder.addCase(updateTopic.fulfilled, (state, action) => {
            const index = state.topics.findIndex(
                topic => topic.id === action.payload.id
            );
            if (index !== -1) {
                state.topics[index] = action.payload;
            }
        });

        //set delete topic
        builder.addCase(deleteTopic.fulfilled, (state, action) => {
            const index = state.topics.findIndex(
                topic => topic.id === action.payload.id
            );
            if (index !== -1) {
                state.topics.splice(index, 1);
            }
        });
    },
});

export default topicSlice.reducer;