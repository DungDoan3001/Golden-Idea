import './style.scss'
import {
    LockReset, Password
} from "@mui/icons-material";
import Image from '../../app/assets/GoldenIdea.svg'
import { LoadingButton } from '@mui/lab';
import { useEffect } from "react";
import { useForm, FormProvider, SubmitHandler } from "react-hook-form";
import { toast } from "react-toastify";
import { useParams } from "react-router-dom";
import { useStoreContext } from '../../app/context/ContextProvider';
import agent from '../../app/api/agent';
import FormInput from '../../app/components/FormInput';
import axios from 'axios';
import { router } from '../../app/routes/Routers';
import * as Yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';

const resetPasswordSchema = Yup.object().shape({
    password: Yup.string().required("Required Field!"),
    confirmPassword: Yup.string()
        .required("Required Field!")
        .oneOf([Yup.ref('password')], 'Not match!'),
});


export type ResetPasswordInput = Yup.InferType<typeof resetPasswordSchema>;
const ResetPass = () => {
    const store = useStoreContext();
    const { resetCode } = useParams();

    const methods = useForm<ResetPasswordInput>({
        resolver: yupResolver(resetPasswordSchema),
    });

    const {
        reset,
        handleSubmit,
        formState: { isSubmitSuccessful },
    } = methods;

    useEffect(() => {
        if (isSubmitSuccessful) {
            reset();
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [isSubmitSuccessful]);

    const resetPassword = async (data: ResetPasswordInput) => {
        if (!resetCode) {
            toast.error("Reset code is missing", { position: "top-right" });
            return;
        }
        console.log(data);
        try {
            store.setRequestLoading(true);
            const res = await fetch(`https://goldenidea.azurewebsites.net/api/authentication/change-password/${resetCode}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            const result = await res.json();
            if (result.succeeded === true) {
                store.setRequestLoading(false);
                toast.success('Change password success - Login with new password ', {
                    position: "top-right",
                });
                router.navigate('/');
            }
            else {
                toast.error(`${result.statusCode} - ${result.message}`, {
                    position: "top-right",
                });
                store.setRequestLoading(false);
            }
        } catch (error) {
            console.log(error);
            store.setRequestLoading(false);
        }
        // try {
        //     store.setRequestLoading(true);
        //     const response = await agent.Account.resetpassword(resetCode, data)
        //     store.setRequestLoading(false);
        //     if (response.data && response.data.message) {
        //         toast.success(response.data.message, {
        //             position: "top-right",
        //         });
        //     }
        //     toast.success('Change password success - Login with new password ', {
        //         position: "top-right",
        //     });
        //     router.navigate("/");
        // } catch (error: any) {
        //     if (axios.isAxiosError(error)) {
        //         const response = error.response;
        //         if (response && response.status === 400) {
        //             toast.error(response.data.message, { position: "top-right" });
        //         }
        //         else if (response && response.status === 404) {
        //             toast.error("There is a conflict while changing password", { position: "top-right" });
        //             return;
        //         }
        //         else {
        //             toast.error("Invalid Token. Make sure that you use a latest email link .", {
        //                 position: "top-right",
        //             });
        //         }
        //     } else {
        //         toast.error("Invalid Token. Make sure that you use a latest email link .", {
        //             position: "top-right",
        //         });
        //     }
        //     store.setRequestLoading(false);
        // }
    };

    const onSubmitHandler: SubmitHandler<ResetPasswordInput> = (values) => {
        if (resetCode) {
            resetPassword(values);
        } else {
            toast.error("Please provide the password reset code", {
                position: "top-right",
            });
        }
    };
    return (
        <div className="container">
            <div className="forms-container">
                <div className="signin-signup" style={{ marginTop: '6rem' }}>
                    <FormProvider {...methods}>
                        <form onSubmit={handleSubmit(onSubmitHandler)} className="sign-in-form" >
                            <h2 className="title">Change password</h2>
                            <div className="input-field">
                                <i><LockReset /></i>
                                <FormInput placeholder="New Password" name="password" type="password" />
                            </div>
                            <div className="input-field">
                                <i><Password /></i>
                                <FormInput
                                    placeholder="Confirm Password"
                                    name="confirmPassword"
                                    type="password"
                                />
                            </div>
                            <LoadingButton
                                loading={store.requestLoading}
                                type='submit'
                                sx={{
                                    width: '150px', backgroundColor: '#cca752',
                                    border: "none", outline: "none", height: '49px',
                                    cursor: "pointer", transition: '0.5s', borderRadius: '49px', color: '#fff', textTransform: 'uppercase', fontWeight: '600', margin: '10px 0',
                                    '&:hover': {
                                        backgroundColor: '#d6c191',
                                        boxShadow: 'none',
                                    },
                                }}
                            >
                                Reset Password
                            </LoadingButton>
                        </form>
                    </FormProvider>
                </div>
            </div>
            <div className="panels-container">
                <div className="panel left-panel">
                    <img src={Image} className="image" alt="" />
                </div>
            </div>
        </div>
    )
}

export default ResetPass