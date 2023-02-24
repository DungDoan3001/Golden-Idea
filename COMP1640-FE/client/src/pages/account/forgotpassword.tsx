import { object, string, TypeOf } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect } from "react";
import { useForm, FormProvider, SubmitHandler } from "react-hook-form";
import { LoadingButton } from '@mui/lab';
import { toast } from 'react-toastify';
import { useStoreContext } from "../../app/context/ContextProvider";
import agent from "../../app/api/agent";
import Image from '../../app/assets/GoldenIdea.svg'
import {
    ForwardToInbox
} from "@mui/icons-material";
import FormInput from "../../app/components/FormInput";

const forgotPasswordchema = object({
    email: string().min(1, "Email is required").email("Invalid email address"),
});

export type ForgotPasswordInput = TypeOf<typeof forgotPasswordchema>;

const ForgotPassword = () => {
    const store = useStoreContext();
    const methods = useForm<ForgotPasswordInput>({
        resolver: zodResolver(forgotPasswordchema),
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

    const forgotPassword = async (data: ForgotPasswordInput) => {
        try {
            store.setRequestLoading(true);
            //const response = await agent.Account.forgotpassword(data)
            console.log('ABC');
            store.setRequestLoading(false);
            // toast.success(response.data.message as string, {
            //     position: "top-right",
            // });
            toast.success('Send Email success', {
                position: "top-right",
            });
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
                type: "error"
            });
        }
    };

    const onSubmitHandler: SubmitHandler<ForgotPasswordInput> = (values) => {
        forgotPassword(values);
    };
    return (
        <div className="container">
            <div className="forms-container">
                <div className="signin-signup">
                    <FormProvider {...methods}>
                        <form
                            onSubmit={handleSubmit(onSubmitHandler)}
                            className="sign-in-form"
                        >
                            <h2 className="title">Change password</h2>
                            <div className="input-field">
                                <i>< ForwardToInbox /></i>
                                <FormInput
                                    placeholder="Email Address"
                                    name="email"
                                    type="email"
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
                                Send Reset Code
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
    );
};

export default ForgotPassword;
