import { zodResolver } from '@hookform/resolvers/zod';
import { LoadingButton } from '@mui/lab';
import { Grid, Box, Button, FormControl, FormLabel, RadioGroup, FormControlLabel, Radio, useTheme } from '@mui/material';
import React, { useEffect } from 'react'
import { useForm, FieldValues } from 'react-hook-form';
import { object, string, date } from 'zod';
import AppTextInput from '../../app/components/AppTextInput';
import { RootState, useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { User } from '../../app/models/User';
import { generatePassword } from '../../app/utils/PasswordGenerating';
import './style.scss'
import AppSelect from '../../app/components/AppSelect';
import { useSelector } from 'react-redux';
import { getDepartments } from '../department/departmentSlice';
import AppImageInput from '../../app/components/AppImageInput';

interface Props {
  user?: User;
  cancelEdit: () => void;
}

const validationSchema = object({
  name: string().min(1, 'Required Field!'),
  closureDate: date(),
  finalClosureDate: date(),
});

const StaffForm = ({ user, cancelEdit }: Props) => {
  const { control, reset, handleSubmit, formState: { isDirty, isSubmitting }, setValue } = useForm({
    resolver: zodResolver(validationSchema),
  });
  const theme: any = useTheme();
  const dispatch = useAppDispatch();
  const userName = useAppSelector(state => state.account.user?.name);
  const { departments } = useSelector((state: RootState) => state.department);
  let fetchMount = true;
  useEffect(() => {
    if (fetchMount) {
      dispatch(getDepartments());
    }
    return () => {
      fetchMount = false;
    };
  }, []);
  useEffect(() => {
    if (user && !isDirty) reset(user);
  }, [user, reset, isDirty]);
  const departmentNames: string[] = departments.map(department => department.name);

  async function handleSubmitData(data: FieldValues) {
    try {
      //const userData = { ...data, userName };
      const userData = { ...data };
      let response: User;
      if (user) {
        // response = await agent.Admin.updateuser(userData);
        console.log(userData);
        console.log(user);
      } else {
        // response = await agent.Admin.createuser(userData);
        console.log(userData);
      }
      // dispatch(setuser(response));
      cancelEdit();
    } catch (error) {
      console.log(error);
    }
  }
  return (
    <Box>
      <form onSubmit={handleSubmit(handleSubmitData)}>
        <Grid container spacing={0} sx={{ maxWidth: '800px', maxHeight: '420px', [theme.breakpoints.up('sm')]: { mt: 0.5, mb: 2 }, [theme.breakpoints.down('sm')]: { mt: 3, maxHeight: '90vh' } }}>
          <Grid item xs={12} sm={6} px={2}>
            <AppTextInput control={control} name='name' label='Name' multiline={true} />
          </Grid>
          <Grid item xs={12} md={6} px={2}>
            <AppTextInput control={control} name='userName' label='User name' multiline={true} />
          </Grid>
          <Grid item xs={12} md={6} px={2}>
            <AppTextInput control={control} name='email' label='Email' multiline={true} />
          </Grid>
          <Grid item xs={12} md={6} px={2}>
            <AppTextInput control={control} name='password' label='Password' type='password' multiline={true} />
          </Grid>
          <Grid item xs={12} md={6} px={2}>
            <AppTextInput control={control} name='address' label='Address' multiline={true} />
          </Grid>
          <Grid item xs={12} md={6} px={2}>
            <AppTextInput control={control} name='phoneNumber' label='Phone Number' multiline={true} />
          </Grid>
          <Grid item xs={12} md={6} px={2}>
            <FormControl component='fieldset'>
              <FormLabel component='legend' sx={{ fontSize: '0.8rem', fontWeight: 'bold' }}>Role</FormLabel>
              <RadioGroup row aria-label='role' name='role'>
                <FormControlLabel value='admin' control={<Radio />} label='Admin' />
                <FormControlLabel value='staff' control={<Radio />} label='Staff' />
                <FormControlLabel value='qa coordinator' control={<Radio />} label='QA Coordinator' />
                <FormControlLabel value='qa manager' control={<Radio />} label='QA Manager' />
              </RadioGroup>
            </FormControl>
          </Grid>
          <Grid item xs={12} md={6} px={2}>
            <FormControl>
              <AppSelect control={control} name='departmentId' label='Department Name' items={departmentNames} />
            </FormControl>
          </Grid>
          <Grid item xs={12} sm={12} px={2}>
            <AppImageInput control={control} name='file' />
          </Grid>
        </Grid>
        <Box display='flex' justifyContent='space-between' >
          <Button onClick={cancelEdit} variant='contained' color='inherit'>
            Cancel
          </Button>
          <LoadingButton loading={isSubmitting} type='submit' variant='contained' color='success'>
            Submit
          </LoadingButton>
        </Box>
      </form>
    </Box>
  );
};

export default StaffForm