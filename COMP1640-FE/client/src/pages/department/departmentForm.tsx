import { LoadingButton } from '@mui/lab'
import { Box, Paper, Typography, Grid, Button, useTheme } from '@mui/material'
import { FieldValues, useForm } from 'react-hook-form'
import AppTextInput from '../../app/components/AppTextInput'
import { Department } from '../../app/models/Department'
import { useEffect } from 'react'
import { zodResolver } from '@hookform/resolvers/zod'
import { useAppDispatch } from '../../app/store/configureStore'
import { object, string } from "zod";

interface Props {
    department?: Department;
    cancelEdit: () => void;
}
const validationSchema = object({
    name: string().min(1, "Required Field!"),
})
const DepartmentForm = ({ department, cancelEdit }: Props) => {
    const { control, reset, handleSubmit, formState: { isDirty, isSubmitting } } = useForm({
        resolver: zodResolver(validationSchema)
    });
    const dispatch = useAppDispatch();
    const theme = useTheme()
    useEffect(() => {
        if (department && !isDirty) reset(department);
    }, [department, reset, isDirty]);
    async function handleSubmitData(data: FieldValues) {
        try {
            let response: Department;
            if (department) {
                //response = await agent.Admin.updateDepartment(data);
                console.log(data);
                console.log(department);
            } else {
                //response = await agent.Admin.createDepartment(data);
                console.log(data);
                console.log(department);
            }
            //dispatch(setDepartment(response));
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
                        <AppTextInput control={control} name='name' label='Department name' />
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

export default DepartmentForm

