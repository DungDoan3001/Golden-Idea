import { useEffect, useState } from 'react';
import Grid from '@mui/material/Grid';
import { Box, Button, Divider, List, ListItemText, Pagination, Typography } from '@mui/material';
import HomePageItem from '../../app/components/HomePageItem';
import CategoryButton from '../../app/components/CategoryButton';
import { useNavigate, useParams } from "react-router-dom";
import { categoryData } from '../../dataTest.js';
import { useTheme } from '@emotion/react';
import { Idea } from '../../app/models/Idea';
import { AddCircleOutline } from '@mui/icons-material';
import Filter from '../../app/components/filter/Filter';
import { useSelector } from 'react-redux';
import { RootState, useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { getMyIdeas } from './ideasSlice';
import Loading from '../../app/components/Loading';
import BackButton from '../../app/components/BackButton';
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
  const navigate = useNavigate();
  const [selectedViewOption, setSelectedViewOption] = useState('latest');
  const { user } = useAppSelector(state => state.account);
  const { ideas_user, loading } = useSelector((state: RootState) => state.idea);
  const [idea, setIdea] = useState(ideas_user); // filter the ideas based on user's username
  const [creatatble, setIsCreatable] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 6;

  const dispatch = useAppDispatch();
  let fetchMount = true;
  useEffect(() => {
    if (fetchMount && user) {
      dispatch(getMyIdeas({ topicId: id, username: user?.name }));
    }
    return () => {
      fetchMount = false;
    };
  }, [name, id]);

  useEffect(() => {
    const today = new Date().getTime();
    const closureDate = new Date(ideas_user[0]?.topic.closureDate).getTime();
    setIsCreatable(today < closureDate)
  }, [ideas_user])

  useEffect(() => {
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    let sortedIdeas = [...ideas_user];
    switch (selectedViewOption) {
      case 'most_viewed':
        sortedIdeas.sort((a, b) => b.view - a.view);
        break;
      case 'latest':
        sortedIdeas.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        break;
      case 'oldest':
        sortedIdeas.sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime());
        break;
      case 'most_liked':
        sortedIdeas.sort((a, b) => b.upVote - a.upVote);
        break;
      case 'most_disliked':
        sortedIdeas.sort((a, b) => b.downVote - a.downVote);
        break;
      default:
        break;
    }
    setIdea(sortedIdeas.slice(startIndex, endIndex));
  }, [selectedViewOption, ideas_user, currentPage]);

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
            width: '19rem',
            m: '2rem',
          },
        }}
      >
        <Box sx={{
          position: 'center',
          mb: "2rem",
          mx: 'auto',
        }}>
          <BackButton />
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
          <Box
            m="0.5rem 0rem"
            display="flex"
            alignItems="center"
            flexDirection={{ xs: "column", sm: "row" }}
            textAlign={{ xs: "center", sm: "left" }}
          >
            {(ideas_user[0]) ? <Grid container>
              <Grid item xs={12} sm={6}>
                <Box pt="5%" display={{ xs: "block", sm: "flex" }} justifyContent={{ xs: "center", sm: "left" }} textAlign={{ xs: "center", sm: "left" }} alignItems="center">
                  <Box
                    component="img"
                    alt="profile"
                    src={ideas_user[0].topic.avatar}
                    height="2.5rem"
                    width="2.5rem"
                    borderRadius="50%"
                    sx={{ objectFit: "cover", mr: { xs: 0, sm: "1rem" }, mb: { xs: "1rem", sm: 0 } }}
                  />
                  <Box component="h4" mb=".5rem">
                    Creator: {ideas_user[0].topic.username}
                  </Box>
                </Box>
              </Grid>
              <Grid item xs={12} sm={6}>
                <Box display="flex" justifyContent={{ xs: "center", sm: "right" }} textAlign={{ xs: "center", sm: "right" }} alignItems="center">
                  <List>
                    <ListItemText
                      primary={`Closure Date: ${new Date(`${ideas_user[0].topic.closureDate}`).toLocaleDateString('en-GB')}`}
                      primaryTypographyProps={{
                        variant: "body1",
                        mb: { xs: "0.5rem", sm: 0 },
                      }}
                    />
                    <ListItemText
                      secondary={`Final Closure Date: ${new Date(`${ideas_user[0].topic.finalClosureDate}`).toLocaleDateString('en-GB')}`}
                      primaryTypographyProps={{
                        variant: "body1",
                        mb: { xs: "0.5rem", sm: 0 },
                      }}
                    />
                  </List>
                </Box>
              </Grid>
            </Grid> : (null)}
          </Box>
          <Divider variant="fullWidth" />
        </Box>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Box sx={{ mr: 2 }}>
            {(creatatble || !ideas_user[0]) ?
              <Button
                variant="contained"
                size="medium"
                color="success"
                onClick={() => navigate(`/ideaform/${id}`)}
                startIcon={<AddCircleOutline />}
              >
                Create a new Idea
              </Button> : (null)}
          </Box>
          <Box sx={{ ml: 2 }}>
            <Filter options={viewOptions} selectedValue={selectedViewOption} onChange={handleViewOptionChange} />
          </Box>
        </Box>
        <Box mt="5%" alignItems="center" justifyContent="center">
          <Grid container spacing={2} columns={{ xs: 4, sm: 8, md: 12 }} sx={{
            [theme.breakpoints.down('sm')]: {
              mr: '2rem',
              width: '80%',
            },
          }}>
            {idea.map((item: any) => (
              <HomePageItem data={item} />
            )
            )}
          </Grid>
          <Box mt="3rem" display='flex' justifyContent='center' alignItems='center' sx={{ marginBottom: 3 }}>
            <Pagination
              count={Math.ceil(ideas_user.length / itemsPerPage)}
              page={currentPage}
              size="large"
              color='secondary'
              onChange={(event, value) => setCurrentPage(value)}
              sx={{ mt: 2 }}
            />
          </Box>
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
                  <CategoryButton search={true} category={item} />
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
