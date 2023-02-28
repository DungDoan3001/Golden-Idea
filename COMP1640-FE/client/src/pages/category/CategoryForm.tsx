import { LoadingButton } from '@mui/lab'
import { Box, Grid, Button, useTheme } from '@mui/material'
import { FieldValues, useForm } from 'react-hook-form'
import AppTextInput from '../../app/components/AppTextInput'
import { Category } from '../../app/models/Category'
import { useEffect } from 'react'
import { zodResolver } from '@hookform/resolvers/zod'
import { useAppDispatch } from '../../app/store/configureStore'
import { object, string } from "zod";

interface Props {
    category?: Category;
    cancelEdit: () => void;
}
const validationSchema = object({
    name: string().min(1, "Required Field!"),
})
const CategoryForm = ({ category, cancelEdit }: Props) => {
    const { control, reset, handleSubmit, formState: { isDirty, isSubmitting } } = useForm({
        resolver: zodResolver(validationSchema)
    });
    const theme = useTheme();
    const dispatch = useAppDispatch();
    useEffect(() => {
        if (category && !isDirty) reset(category);
    }, [category, reset, isDirty]);
    async function handleSubmitData(data: FieldValues) {
        try {
            let response: Category;
            if (category) {
                //response = await agent.Admin.updateCategory(data);
                console.log(data);
                console.log(category);
            } else {
                //response = await agent.Admin.createCategory(data);
                console.log(data);
                console.log(category);
            }
            //dispatch(setCategory(response));
            cancelEdit();
        } catch (error) {
            console.log(error);
        }
    }
    return (
        <Box sx={{ p: 4 }}>
            <form onSubmit={handleSubmit(handleSubmitData)}>
                <Grid container sx={{ [theme.breakpoints.up('md')]: { width: '140%' } }}>
                    <Grid item xs={12} sm={8}>
                        <AppTextInput control={control} name='name' label='Category name' />
                    </Grid>
                </Grid>
                <Box display='flex' justifyContent='space-between' sx={{ ml: 10, mt: 5, mb: 1, [theme.breakpoints.up('md')]: { ml: 25 } }}>
                    <Button onClick={cancelEdit} variant='contained' color='inherit' sx={{ marginRight: '0.5rem' }}>Cancel</Button>
                    <LoadingButton loading={isSubmitting} type='submit' variant='contained' color='success'>Submit</LoadingButton>
                </Box>
            </form>
        </Box>
    )
}

export default CategoryForm

