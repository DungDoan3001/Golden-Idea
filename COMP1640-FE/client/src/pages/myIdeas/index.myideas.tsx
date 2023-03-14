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
import { useSelector } from 'react-redux';
import { RootState, useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { getIdeas } from './ideasSlice';
import Loading from '../../app/components/Loading';
const viewOptions = [
    { label: "Most Viewed", value: "most_viewed" },
    { label: "Latest", value: "latest" },
    { label: "Oldest", value: "oldest" },
    { label: "Most Liked", value: "most_liked" },
    { label: "Most Disliked", value: "most_disliked" }
];

const ListMyIdeas = () => {
    const theme: any = useTheme();
    const { name, id } = useParams();
    const [editMode, setEditMode] = useState(false);
    const [recordForEdit, setRecordForEdit] = useState<Idea | undefined>(undefined);
    const [selectedViewOption, setSelectedViewOption] = useState('most_viewed');
    const { user } = useAppSelector(state => state.account);
    const { ideas, loading } = useSelector((state: RootState) => state.idea);
    const [idea, setIdea] = useState(ideas.filter((idea: any) => idea.user?.userName === user?.name)); // filter the ideas based on user's username
    const dispatch = useAppDispatch();
    let fetchMount = true;
    useEffect(() => {
        if (fetchMount) {
            dispatch(getIdeas(id));
        }
        return () => {
            fetchMount = false;
        };
    }, []);
    useEffect(() => {
        switch (selectedViewOption) {
            case 'most_viewed':
                setIdea([...idea].sort((a, b) => b.view - a.view));
                break;
            case 'latest':
                setIdea([...idea].sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()));
                break;
            case 'oldest':
                setIdea([...idea].sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()));
                break;
            case 'most_liked':
                setIdea([...idea].sort((a, b) => b.upVote - a.upVote));
                break;
            case 'most_disliked':
                setIdea([...idea].sort((a, b) => b.downVote - a.downVote));
                break;
            default:
                setIdea(ideas.filter((idea: any) => idea.user?.userName === user?.name));
                break;
        }
    }, [selectedViewOption, ideas]);
    function cancelEdit() {
        if (recordForEdit) setRecordForEdit(undefined);
        setEditMode(false);
    }
    const handleViewOptionChange = (value: string) => {
        setSelectedViewOption(value);
    };
    return (
        <>
            {loading ? <Loading /> : (<Box alignItems="center" justifyContent="center"
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
                <Box sx={{
                    position: 'center',
                    mb: "2rem",
                    mx: 'auto',
                }}>
                    <Typography
                        variant="h1"
                        color={theme.palette.content.main}
                        fontWeight="bold"
                        sx={{
                            textAlign: 'center',
                            textTransform: 'uppercase',
                            position: 'relative',
                            display: 'inline-block',
                            '&:after': {
                                content: '""',
                                position: 'absolute',
                                top: '-10px',
                                left: '-10px',
                                right: '-10px',
                                bottom: '-10px',
                                border: `4px solid ${theme.palette.secondary.main}`,
                                borderStyle: 'dashed',
                                borderRadius: '10px',
                            },
                        }}
                    >
                        {name}
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
                        data={ideas.filter((idea: any) => idea.user?.userName == user?.name)}
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
            )}
        </>
    );
}

export default ListMyIdeas