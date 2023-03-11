import { LoadingButton } from '@mui/lab'
import { Box, Grid, Button, useTheme } from '@mui/material'
import { FieldValues, useForm } from 'react-hook-form'
import AppTextInput from '../../app/components/AppTextInput'
import { Category } from '../../app/models/Category'
import { useEffect } from 'react'
import { zodResolver } from '@hookform/resolvers/zod'
import { useAppDispatch } from '../../app/store/configureStore'
import { object, string } from "zod";
import './style.scss'
import { addCategory, getCategories, updateCategory } from './categorySlice'
import { toast } from 'react-toastify'

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
                const updatedCategory: Category = {
                    id: category.id,
                    name: data.name,
                };
                response = await dispatch(updateCategory(updatedCategory)).unwrap();
            } else {
                const newCategory: Category = {
                    name: data.name,
                };
                response = await dispatch(addCategory(newCategory)).unwrap();
            }
            toast.success('Successfully', {
                position: toast.POSITION.TOP_RIGHT,
            });
            cancelEdit();
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
                        <AppTextInput control={control} name='name' label='Category name' multiline={true} />
                    </Grid>
                </Grid>
                <Box display='flex' justifyContent='space-between' sx={{ ml: 10, mt: 5, mb: 1, [theme.breakpoints.up('sm')]: { ml: 25 } }}>
                    <Button onClick={cancelEdit} variant='contained' color='inherit' sx={{ marginRight: '0.5rem' }}>Cancel</Button>
                    <LoadingButton loading={isSubmitting} type='submit' variant='contained' color='success'>Submit</LoadingButton>
                </Box>
            </form>
        </Box>
    )
}

export default CategoryForm

