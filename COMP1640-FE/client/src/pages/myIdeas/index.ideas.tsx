import React, { useEffect, useState } from 'react';
import Grid from '@mui/material/Grid';
import { Box, Button, Typography, List, ListItemText, Divider, IconButton } from '@mui/material';
import HomePageItem from '../../app/components/HomePageItem';
import AppPagination from '../../app/components/AppPagination';
import CategoryButton from '../../app/components/CategoryButton';
import { useParams } from "react-router-dom";
import { useTheme } from '@emotion/react';
import { useNavigate } from 'react-router-dom';
import { Idea } from '../../app/models/Idea';
import { AddCircleOutline } from '@mui/icons-material';
import Filter from '../../app/components/filter/Filter';
import { useSelector } from 'react-redux';
import { RootState, useAppDispatch } from '../../app/store/configureStore';
import { getIdeas } from './ideasSlice';
import Loading from '../../app/components/Loading';
import BackButton from '../../app/components/BackButton';
import { get } from 'http';
import { getCategories } from '../category/categorySlice';
const viewOptions = [
  { label: "Most Viewed", value: "most_viewed" },
  { label: "Latest", value: "latest" },
  { label: "Oldest", value: "oldest" },
  { label: "Most Liked", value: "most_liked" },
  { label: "Most Disliked", value: "most_disliked" }
];

const ListIdeas = () => {
  const theme: any = useTheme();
  const navigate = useNavigate();
  const { name, id } = useParams();
  const [editMode, setEditMode] = useState(false);
  const [recordForEdit, setRecordForEdit] = useState<Idea | undefined>(undefined);
  const [selectedViewOption, setSelectedViewOption] = useState('most_viewed');
  const { ideas, loading } = useSelector((state: RootState) => state.idea);
  const { categories } = useSelector((state: RootState) => state.category);
  const [ideaData, setIdeaData] = useState(ideas)
  const [idea, setIdea] = useState([]);
  const [creatatble, setIsCreatable] = useState(true);

  const dispatch = useAppDispatch();
  let fetchMount = true;
  useEffect(() => {
    if (fetchMount) {
      dispatch(getIdeas(id));
      dispatch(getCategories());
    }
    return () => {
      fetchMount = false;
    };
  }, []);

  useEffect(() => {
    const today = new Date().getTime();
    const closureDate = new Date(ideas[0]?.topic.closureDate).getTime();
    setIsCreatable(today < closureDate)
  }, [ideas])

  useEffect(() => {
    switch (selectedViewOption) {
      case 'most_viewed':
        setIdeaData([...ideas].sort((a, b) => b.view - a.view));
        break;
      case 'latest':
        setIdeaData([...ideas].sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()));
        break;
      case 'oldest':
        setIdeaData([...ideas].sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()));
        break;
      case 'most_liked':
        setIdeaData([...ideas].sort((a, b) => b.upVote - a.upVote));
        break;
      case 'most_disliked':
        setIdeaData([...ideas].sort((a, b) => b.downVote - a.downVote));
        break;
      default:
        setIdeaData(ideas);
        break;
    }
  }, [selectedViewOption, ideas]);

  useEffect(() => {
    window.scrollTo(0, 0)
  }, [idea, ideas, ideaData])
  function cancelEdit() {
    if (recordForEdit) setRecordForEdit(undefined);
    setEditMode(false);
  }
  const handleViewOptionChange = (value: string) => {
    setSelectedViewOption(value);
  };
  return (
    <>
      {loading ? <Loading /> : (
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
          <BackButton />
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
                color: theme.palette.secondary.main,
                width: "100%",
              }}
            >
              {name}
            </Typography>
            <Box m="0.5rem 0rem" display="flex" alignItems="center">
              <Box
                component="img"
                alt="profile"
                src={ideas[0]?.topic.avatar}
                height="2.5rem"
                width="2.5rem"
                borderRadius="50%"
                sx={{ objectFit: "cover", mr: "1rem" }}
              />
              <Box>
                <Box component="h4" mb=".5rem">
                  Creator: {ideas[0]?.topic.username}
                </Box>
              </Box>
              <Box sx={{ ml: "auto" }}>
                <List sx={{ display: "flex", flexDirection: { xs: "row", sm: "column" } }}>
                  <ListItemText
                    primary={`Closure Date: ${new Date(`${ideas[0]?.topic.closureDate}`).toLocaleDateString('en-GB')}`}
                    secondary={`Final Closure Date: ${new Date(`${ideas[0]?.topic.finalClosureDate}`).toLocaleDateString('en-GB')}`}
                    primaryTypographyProps={{ variant: "body1", textAlign: { xs: "right", sm: "right" }, mb: { xs: 0, sm: 1 }, mr: { xs: 1, sm: 0 } }}
                    secondaryTypographyProps={{ variant: "body1", textAlign: { xs: "right", sm: "right" } }}
                  />
                </List>
              </Box>
            </Box>
            <Divider variant="fullWidth" />
          </Box>
          {(creatatble) ?
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <Box sx={{ mr: 2 }}>
                <Button
                  variant="contained"
                  size="medium"
                  color="success"
                  onClick={() => navigate(`/ideaform/${id}/slug`)}
                  startIcon={<AddCircleOutline />}
                >
                  Create a new Idea
                </Button>
              </Box>
              <Box sx={{ ml: 2 }}>
                <Filter options={viewOptions} selectedValue={selectedViewOption} onChange={handleViewOptionChange} />
              </Box>
            </Box> : (null)}
          <Box mt="5%" display="flex" alignContent="center" alignItems="center">
            <Grid container spacing={0.5} columns={{ xs: 4, sm: 8, md: 12 }}>
              {idea.map((item: any) => (
                <HomePageItem data={item} />
              )
              )}
            </Grid>
          </Box>
          <AppPagination
            setItem={setIdea} // Update this line
            data={ideaData} // Update this line
            size={6}
          />

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
                categories.map((item: any) => (
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

export default ListIdeas