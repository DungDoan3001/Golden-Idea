import React, { useEffect, useState } from 'react';
import Grid from '@mui/material/Grid';
import { Box, Button, Typography } from '@mui/material';
import HomePageItem from '../../app/components/HomePageItem';
import AppPagination from '../../app/components/AppPagination';
import CategoryButton from '../../app/components/CategoryButton';
import { useParams } from "react-router-dom";
import { postData } from "../../dataTest.js"
import { categoryData } from '../../dataTest.js';
import { useTheme } from '@emotion/react';
import { Idea } from '../../app/models/Idea';
import { AddCircleOutline } from '@mui/icons-material';
import Filter from '../../app/components/filter/Filter';
const viewOptions = [
    { label: "Most Viewed", value: "most_viewed" },
    { label: "Latest", value: "latest" },
    { label: "Oldest", value: "oldest" },
    { label: "Most Liked", value: "most_liked" },
    { label: "Most Disliked", value: "most_disliked" }
];

const ListIdeas = () => {
    const theme: any = useTheme();
    const { name } = useParams();
    const [idea, setIdea] = useState([]);
    const [editMode, setEditMode] = useState(false);
    const [recordForEdit, setRecordForEdit] = useState<Idea | undefined>(undefined);
    const [selectedViewOption, setSelectedViewOption] = useState('most_viewed');
    function cancelEdit() {
        if (recordForEdit) setRecordForEdit(undefined);
        setEditMode(false);
    }
    const handleViewOptionChange = (value: string) => {
        setSelectedViewOption(value);
    };
    return (
        <Box alignItems="center" justifyContent="center"
            width="100%"
            sx={{
                [theme.breakpoints.up('sm')]: {
                    width: '90%',
                    m: '3rem',
                },
                [theme.breakpoints.down('sm')]: {
                    width: '100%',
                    m: '3.5rem',
                },
            }}
        >
            <Box mb="2rem">
                <Typography
                    variant="h1"
                    color={theme.palette.content.main}
                    fontWeight="bold"
                    sx={{ mb: "5px", textAlign: 'center' }}
                >
                    {name?.toUpperCase()}
                </Typography>
            </Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Box sx={{ mr: 2 }}>
                    <Button
                        variant="contained"
                        size="medium"
                        color="success"
                        onClick={() => setEditMode(true)}
                        startIcon={<AddCircleOutline />}
                    >
                        Create a new Idea
                    </Button>
                </Box>
                <Box sx={{ ml: 2 }}>
                    <Filter options={viewOptions} selectedValue={selectedViewOption} onChange={handleViewOptionChange} />
                </Box>
            </Box>
            <Box mt="5%" alignItems="center" justifyContent="center">
                <Grid container spacing={2} columns={{ xs: 4, sm: 8, md: 12 }}>
                    {idea.map((item: any) => (
                        <HomePageItem data={item} />
                    )
                    )}
                </Grid>
                <AppPagination
                    setItem={(p: any) => setIdea(p)}
                    data={postData}
                    size={6}
                />
            </Box>
            <Box sx={{
                [theme.breakpoints.up('sm')]: {
                    p: '4rem',
                },
                [theme.breakpoints.down('sm')]: {
                    p: '1rem',
                    pb: '4rem',
                },
            }}
            >
                <Grid container spacing={0.5}>
                    {
                        categoryData.map((item: any) => (
                            <Grid item xs={6} sm={4} md={2.4}>
                                <CategoryButton search={true} category={item.name} />
                            </Grid>
                        ))
                    }
                </Grid>
            </Box>
        </Box >
    );
}

export default ListIdeas