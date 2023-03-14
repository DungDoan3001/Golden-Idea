import React, { useState } from 'react';
import { makeStyles } from '@mui/styles';
import {
    Button,
    IconButton,
    Menu,
    MenuItem,
    Tooltip,
} from '@mui/material';
import { FilterList } from '@mui/icons-material';
import { useStoreContext } from '../../context/ContextProvider';

interface FilterProps {
    options: { label: string; value: string }[];
    onChange: (value: string) => void;
    selectedValue: string;
}

const useStyles = makeStyles((theme: any) => ({
    root: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        [theme.breakpoints.down('sm')]: {
            flexDirection: 'column',
        },
    },
}));

const Filter: React.FC<FilterProps> = ({
    options,
    onChange,
    selectedValue,
}) => {
    const classes = useStyles();
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const { screenSize } = useStoreContext();

    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleOptionSelect = (value: string) => {
        onChange(value);
        handleClose();
    };

    const renderMenuItems = () =>
        options.map((option) => (
            <MenuItem
                key={option.value}
                onClick={() => handleOptionSelect(option.value)}
                selected={option.value === selectedValue}
            >
                {option.label}
            </MenuItem>
        ));

    const renderFilterButton = () => (
        <Tooltip title="Filter">
            <IconButton color="inherit" onClick={handleClick}>
                <FilterList />
            </IconButton>
        </Tooltip>
    );

    const renderSelectedOption = () => (
        <Button
            color="inherit"
            variant="outlined"
            onClick={handleClick}
        >
            {options.find(option => option.value === selectedValue)?.label || 'Filter'}
        </Button>
    );

    return (
        <div className={classes.root}>
            <Menu
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={handleClose}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
                transformOrigin={{ vertical: 'top', horizontal: 'center' }}
            >
                {renderMenuItems()}
            </Menu>
            {renderFilterButton()}
            {screenSize >= 900 ? renderSelectedOption() : null}
        </div>
    );
};

export default Filter;
