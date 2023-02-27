import * as React from 'react';
import './style.scss'
import {
    LockReset, Margin, Password
} from "@mui/icons-material";
import Image from '../../app/assets/GoldenIdea.svg'
import { LoadingButton } from '@mui/lab';
import { object, string, TypeOf } from "zod";
import { useEffect } from "react";
import { useForm, FormProvider, SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "react-toastify";
import { useNavigate, useParams } from "react-router-dom";
import { useStoreContext } from '../../app/context/ContextProvider';
import agent from '../../app/api/agent';
import FormInput from '../../app/components/FormInput';

const resetPasswordSchema = object({
    password: string().min(1, "Required Field!"),
    confirm: string().min(1, "Required Field!"),
}).refine((data) => data.password === data.confirm, {
    message: "Not match!",
    path: ["confirm"],
});

export type ResetPasswordInput = TypeOf<typeof resetPasswordSchema>;
const ResetPass = () => {
    const store = useStoreContext();
    const navigate = useNavigate();
    const { resetCode } = useParams();
    console.log(resetCode);

    const methods = useForm<ResetPasswordInput>({
        resolver: zodResolver(resetPasswordSchema),
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
        try {
            store.setRequestLoading(true);
            const response = await agent.Account.resetpassword(resetCode, data)
            store.setRequestLoading(false);
            toast.success(response.data.message as string, {
                position: "top-right",
            });
            navigate("/login");
        } catch (error: any) {
            store.setRequestLoading(false);
            const resMessage =
                (error.response &&
                    error.response.data &&
                    error.response.data.message) ||
                error.message ||
                error.toString();
            toast.error(resMessage, {
                position: "top-right",
            });
        }
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
                                    name="confirm"
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