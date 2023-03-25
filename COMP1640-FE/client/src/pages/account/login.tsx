import { AccountCircle, LockOutlined, Password } from '@mui/icons-material';
import { LoadingButton } from '@mui/lab';
import { Grid, InputAdornment, Paper, TextField, Typography } from '@mui/material';
import { FieldValues, useForm } from 'react-hook-form';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { signInUser } from './accountSlice';
import Image from '../../app/assets/test.svg'

export default function Login() {
    const navigate = useNavigate();
    const location = useLocation();
    const dispatch = useAppDispatch();
    const { user } = useAppSelector(state => state.account);
    const { register, handleSubmit, formState: { isSubmitting, errors, isValid } } = useForm({
        mode: 'onTouched'
    });

    async function submitForm(data: FieldValues) {
        try {
            await dispatch(signInUser(data));
            navigate(location.state?.from || '/home');
        } catch (error) {
            console.log(error);
        }
    }
    if (user) {
        navigate(location.state?.from || '/home');
        return null; // Return null to prevent rendering the rest of the component
    }
    return (
        <div className="container sign-up-mode">
            <div className="forms-container">
                <div className="signin-signup">
                    <form
                        onSubmit={handleSubmit(submitForm)}
                        className="sign-up-form"
                    >
                        <h2 className="title">Sign In</h2>
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            placeholder="Email"
                            {...register('email', { required: 'Email is required' })}
                            error={!!errors.email}
                            helperText={errors?.email?.message as string}
                            sx={{
                                "& fieldset": { border: 'none' },
                            }}
                            InputProps={{
                                startAdornment: (
                                    <InputAdornment position="start">
                                        <AccountCircle />
                                    </InputAdornment>
                                ),
                            }}
                            className='input-field'
                        />
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            placeholder="Password"
                            type="password"
                            {...register('password', { required: 'Password is required' })}
                            error={!!errors.password}
                            helperText={errors?.password?.message as string}
                            sx={{
                                "& fieldset": { border: 'none' },
                            }}
                            InputProps={{
                                startAdornment: (
                                    <InputAdornment position="start">
                                        <Password />
                                    </InputAdornment>
                                ),
                            }}
                            className='input-field'
                        />
                        <LoadingButton
                            disabled={!isValid}
                            loading={isSubmitting}
                            type="submit"
                            sx={{
                                width: '150px', backgroundColor: '#cca752',
                                border: "none", outline: "none", height: '49px',
                                cursor: "pointer", transition: '0.5s', borderRadius: '49px', color: '#fff', textTransform: 'uppercase', fontWeight: '600', margin: '15px 0',
                                '&:hover': {
                                    backgroundColor: '#d6c191',
                                    boxShadow: 'none',
                                },
                            }}
                        >
                            Sign In
                        </LoadingButton>
                        <Grid container marginLeft={{ sm: 30, md: 30 }}>
                            <Grid item>
                                <Link to='/forgot' style={{ textDecoration: 'none' }}>
                                    {"Forgot password?"}
                                </Link>
                            </Grid>
                        </Grid>
                    </form>
                </div>
            </div>
            <div className="panels-container">
                <div className="panel left-panel">
                    <img src={Image} className="image" alt="" />
                </div>
            </div>
        </div>


    );
}