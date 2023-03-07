import { TextField, InputLabel, InputAdornment, IconButton } from "@mui/material";
import { useController, UseControllerProps } from "react-hook-form";
import Lock from '@mui/icons-material/Lock';
import React from "react";
import { generatePassword } from "../utils/PasswordGenerating";
interface Props extends UseControllerProps {
    label: string;
    multiline?: boolean;
    rows?: number;
    type?: string;
}

export default function AppTextInput(props: Props) {
    const { fieldState, field } = useController({ ...props, defaultValue: '' })
    const [password, setPassword] = React.useState('');

    const handleGeneratePassword = () => {
        setPassword(generatePassword(12));
    };

    return (
        <div className="app-text-input">
            <TextField
                {...props}
                {...field}
                sx={{ width: '100%' }}
                multiline={props.multiline}
                rows={props.rows}
                type={props.type}
                fullWidth
                variant="outlined"
                error={!!fieldState.error}
                helperText={fieldState.error?.message}
                InputLabelProps={{
                    shrink: true,
                }}
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
            />
        </div>
    )
}