import React from 'react';
import { RegisterOptions, useFormContext } from 'react-hook-form';

type FormInputProps = {
    placeholder: string;
    name: string;
    type?: string;
};
type CustomRegisterOptions = RegisterOptions & {
    dateFormat?: string;
};

const FormInput: React.FC<FormInputProps> = ({
    placeholder,
    name,
    type,
}) => {
    const {
        register,
        formState: { errors },
    } = useFormContext();
    // Set the dateFormat to 'MMMM d, yyyy h:mm aa' when the type is 'date'
    const dateFormat = type === 'date' ? 'MMMM d, yyyy h:mm aa' : undefined;
    return (
        <div>
            <input
                type={type}
                placeholder={placeholder}
                style={{
                    marginTop: '14px',
                    background: 'none',
                    outline: 'none',
                    border: 'none',
                    lineHeight: 1,
                    fontWeight: 600,
                    fontSize: '1.1rem',
                    color: '#333',
                }}
                {...register(name, { dateFormat } as CustomRegisterOptions)}
            />
            {errors[name] && (
                <span style={{
                    marginLeft: '-3rem',
                    background: 'none',
                    outline: 'none',
                    border: 'none',
                    color: 'red',
                }}>
                    {errors[name]?.message as string}
                </span>
            )}
        </div>
    );
};

export default FormInput;