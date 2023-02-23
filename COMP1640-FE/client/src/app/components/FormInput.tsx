import React from 'react';
import { useFormContext } from 'react-hook-form';

type FormInputProps = {
    placeholder: string;
    name: string;
    type?: string;
};

const FormInput: React.FC<FormInputProps> = ({
    placeholder,
    name,
    type = 'text',
}) => {
    const {
        register,
        formState: { errors },
    } = useFormContext();
    return (
        <div>
            <input
                type={type}
                placeholder={placeholder}
                style={{
                    marginTop: '11px',
                    background: 'none',
                    outline: 'none',
                    border: 'none',
                    lineHeight: 1,
                    fontWeight: 600,
                    fontSize: '1.1rem',
                    color: '#333',
                }}
                {...register(name)}
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