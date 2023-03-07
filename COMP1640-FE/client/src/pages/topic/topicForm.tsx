import { LoadingButton } from '@mui/lab';
import { Box, Grid, Button, useTheme } from '@mui/material';
import { FieldValues, useForm } from 'react-hook-form';
import AppTextInput from '../../app/components/AppTextInput';
import { Topic } from '../../app/models/Topic';
import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import * as z from 'zod';
import './style.scss';
import { addTopic, getTopics, updateTopic } from './topicSlice';
import { toast } from 'react-toastify';
import moment from 'moment';

interface Props {
    topic?: Topic;
    cancelEdit: () => void;
}
const validationSchema = z.object({
    closureDate: z.string().min(1, 'Closure date is required').regex(/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2})$/),
    finalClosureDate: z.string().min(1, 'Final closure date is required').regex(/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2})$/),
}).refine((data) => {
    const closureDateObj = new Date(data.closureDate);
    const finalClosureDateObj = new Date(data.finalClosureDate);
    return finalClosureDateObj > closureDateObj;
}, {
    message: 'Final closure date must be after closure date',
    path: ['finalClosureDate'],
});
const TopicForm = ({ topic, cancelEdit }: Props) => {
    const { control, reset, handleSubmit, formState: { isDirty, isSubmitting } } = useForm({
        resolver: zodResolver(validationSchema),
    });
    const theme = useTheme();
    const dispatch = useAppDispatch();
    const { user } = useAppSelector(state => state.account);
    useEffect(() => {
        if (topic && !isDirty) reset(topic);
    }, [topic, reset, isDirty]);
    async function handleSubmitData(data: FieldValues) {
        try {
            let response: Topic;
            if (topic) {
                const updatedTopic: Topic = {
                    id: topic.id,
                    name: data.name,
                    username: topic.username,
                    closureDate: new Date(data.closureDate), // convert string to date object
                    finalClosureDate: new Date(data.finalClosureDate), // convert string to date object
                };
                response = await dispatch(updateTopic(updatedTopic)).unwrap();
            } else {
                const newTopic: Topic = {
                    name: data.name,
                    username: user?.name,
                    closureDate: new Date(data.closureDate), // convert string to date object
                    finalClosureDate: new Date(data.finalClosureDate), // convert string to date object
                };
                response = await dispatch(addTopic(newTopic)).unwrap();
            }
            cancelEdit();
            await dispatch(getTopics()).unwrap();
        } catch (error: any) {
            toast.error('Failed to load resource: the server responded with a status of 409 (Conflict)', {
                position: toast.POSITION.TOP_RIGHT,
            });
        }
    }
    return (
        <Box>
            <form onSubmit={handleSubmit(handleSubmitData)}>
                <Grid container sx={{ [theme.breakpoints.up('sm')]: { marginLeft: '17rem', marginTop: '12px', width: '150%' }, [theme.breakpoints.down('sm')]: { mt: 3 } }}>
                    <Grid item xs={12} sm={8}>
                        <AppTextInput control={control} name='name' label='Topic name' rows={2} multiline={true} />
                        <AppTextInput control={control} name='closureDate' label='Closure date' type='datetime-local' defaultValue={(new Date('2023-03-05T06:29:17.879')).toLocaleString()} />
                        <AppTextInput control={control} name='finalClosureDate' label='Final closure date' type='datetime-local' defaultValue={(new Date('2023-03-05T06:29:17.879')).toLocaleString()} />
                    </Grid>
                </Grid>
                <Box display='flex' justifyContent='space-between' sx={{ ml: 10, mt: 5, mb: 1, [theme.breakpoints.up('sm')]: { ml: 25 } }}>
                    <Button onClick={cancelEdit} variant='contained' color='inherit' sx={{ marginRight: '0.5rem' }}>Cancel</Button>
                    <LoadingButton loading={isSubmitting} type='submit' variant='contained' color='success'>Submit</LoadingButton>
                </Box>
            </form>
        </Box>
    );
};

export default TopicForm;