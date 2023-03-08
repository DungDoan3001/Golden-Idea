import { TextField, InputLabel, InputAdornment, IconButton } from "@mui/material";
import { useController, UseControllerProps } from "react-hook-form";
import Lock from '@mui/icons-material/Lock';
import React, { useState } from "react";
import { generatePassword } from "../utils/PasswordGenerating";

interface Props extends UseControllerProps {
    label: string;
    name: string;
    multiline?: boolean;
    rows?: number;
    type?: string;
    defaultValue?: string;
}

export default function AppTextInput(props: Props) {
    const { field } = useController({ ...props, defaultValue: '' });
    const [password, setPassword] = useState(props.defaultValue || '');

    const handleGeneratePassword = () => {
        const newPassword = generatePassword(12);
        setPassword(newPassword);
        field.onChange(newPassword); // Manually set the generated password in the form field
    };

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setPassword(event.target.value);
        field.onChange(event.target.value); // Manually update the form field value
    };

    return (
        <div className="app-text-input">
            <TextField
                {...field}
                name={props.name}
                label={props.label}
                multiline={props.multiline}
                rows={props.rows}
                type={props.type}
                fullWidth
                variant="outlined"
                InputProps={props.type === "password" ? {
                    endAdornment: (
                        <InputAdornment position="end">
                            <IconButton
                                onClick={handleGeneratePassword}
                                edge="end"
                            >
                                <Lock />
                            </IconButton>
                        </InputAdornment>
                    ), value: password,
                } : {}}
                onChange={handleChange} // Add onChange handler to update the local state and form field value
            />
        </div>
    )
}