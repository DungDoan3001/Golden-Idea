import { LoadingButton } from '@mui/lab';
import { Box, Grid, Button, useTheme } from '@mui/material';
import { FieldValues, useForm } from 'react-hook-form';
import AppTextInput from '../../app/components/AppTextInput';
import { Topic } from '../../app/models/Topic';
import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { object, string, date } from 'zod';

interface Props {
    topic?: Topic;
    cancelEdit: () => void;
}

const validationSchema = object({
    name: string().min(1, 'Required Field!'),
    closureDate: date(),
    finalClosureDate: date(),
});

const TopicForm = ({ topic, cancelEdit }: Props) => {
    const { control, reset, handleSubmit, formState: { isDirty, isSubmitting } } = useForm({
        resolver: zodResolver(validationSchema),
    });
    const theme = useTheme();
    const dispatch = useAppDispatch();
    //   const userName = useAppSelector(state => state.auth.user?.name);
    useEffect(() => {
        if (topic && !isDirty) reset(topic);
    }, [topic, reset, isDirty]);
    async function handleSubmitData(data: FieldValues) {
        try {
            //const topicData = { ...data, userName };
            const topicData = { ...data };
            let response: Topic;
            if (topic) {
                // response = await agent.Admin.updateTopic(topicData);
                console.log(topicData);
                console.log(topic);
            } else {
                // response = await agent.Admin.createTopic(topicData);
                console.log(topicData);
            }
            // dispatch(setTopic(response));
            cancelEdit();
        } catch (error) {
            console.log(error);
        }
    }
    return (
        <Box sx={{ p: 4 }}>
            <form onSubmit={handleSubmit(handleSubmitData)}>
                <Grid container sx={{ [theme.breakpoints.up('md')]: { width: '120%' } }}>
                    <Grid item xs={12} sm={8}>
                        <AppTextInput control={control} name='name' label='Topic name' />
                        <AppTextInput control={control} name='closureDate' label='Closure date' type='date' />
                        <AppTextInput control={control} name='finalClosureDate' label='Final closure date' type='date' />
                    </Grid>
                </Grid>
                <Box display='flex' justifyContent='space-between' sx={{ ml: 10, mt: 5, mb: 1, [theme.breakpoints.up('md')]: { ml: 25 } }}>
                    <Button onClick={cancelEdit} variant='contained' color='inherit' sx={{ marginRight: '0.5rem' }}>Cancel</Button>
                    <LoadingButton loading={isSubmitting} type='submit' variant='contained' color='success'>Submit</LoadingButton>
                </Box>
            </form>
        </Box>
    );
};

export default TopicForm;