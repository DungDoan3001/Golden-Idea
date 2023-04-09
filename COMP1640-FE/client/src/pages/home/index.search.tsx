import React, { useEffect, useState } from 'react'
import { useSelector } from 'react-redux';
import { useParams } from 'react-router-dom'
import { RootState, useAppDispatch } from '../../app/store/configureStore';
import { getSearchIdeas } from '../myIdeas/ideasSlice';
import { Box, Grid, Typography, useTheme } from '@mui/material';
import AppPagination from '../../app/components/AppPagination';
import HomePageItem from '../../app/components/HomePageItem';
import Loading from '../../app/components/Loading';

const SearchPage = () => {
    const { filter } = useParams();
    const theme: any = useTheme();
    const { ideas_search, loading } = useSelector((state: RootState) => state.idea);
    const [idea, setIdea] = useState(ideas_search);
    const dispatch = useAppDispatch();
    let fetchMount = true;
    useEffect(() => {
        if (fetchMount) {
            dispatch(getSearchIdeas(filter));
        }
        return () => {
            fetchMount = false;
        };
    }, [filter]);
    console.log(ideas_search)
    return (
        <>
            {loading ? (
                <Loading />
            ) : ideas_search && ideas_search.length ? (
                <Box alignItems="center" justifyContent="center"
                    width="100%"
                    sx={{
                        [theme.breakpoints.up('sm')]: {
                            width: '90%',
                            m: '3rem',
                            pb: '1rem',
                        },
                        [theme.breakpoints.down('sm')]: {
                            width: '100%',
                            m: '3.5rem',
                        },
                    }}
                >
                    <Typography variant="h2" align="left" fontWeight={'bold'} marginLeft={4.5} marginTop={3}>
                        TOP RESULTS
                    </Typography>
                    <Box mt="5%" alignItems="center" justifyContent="center">
                        <Grid container spacing={2} columns={{ xs: 4, sm: 8, md: 12 }}>
                            {idea.map((item: any) => (
                                <HomePageItem data={item} />
                            )
                            )}
                        </Grid>
                        <AppPagination
                            setItem={(p: any) => setIdea(p)}
                            data={ideas_search}
                            size={6}
                        />
                    </Box>
                </Box >)
                : <Typography variant="h2" align="left" fontWeight={'bold'} marginLeft={4.5} marginTop={3}>
                    NO MATCHING RESULT!
                </Typography>}
        </>
    )
}

export default SearchPage