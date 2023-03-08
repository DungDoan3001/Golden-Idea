import { FormControl, InputLabel, Select, MenuItem, FormHelperText } from "@mui/material";
import { useController, UseControllerProps } from "react-hook-form";
interface Select {
    id?: string;
    name: string;
}

interface Props extends UseControllerProps {
    label: string;
    items: Select[];
}

export default function AppSelect(props: Props) {
    const { fieldState, field } = useController({ ...props, defaultValue: '' })
    return (
        <FormControl fullWidth error={!!fieldState.error}>
            <InputLabel>{props.label}</InputLabel>
            <Select
                value={field.value}
                label={props.label}
                onChange={field.onChange}
            >
                {props.items.map((department) => (
                    <MenuItem value={department.id} key={department.id}>{department.name}</MenuItem>
                ))}
            </Select>
            <FormHelperText>{fieldState.error?.message}</FormHelperText>
        </FormControl>
    )
}