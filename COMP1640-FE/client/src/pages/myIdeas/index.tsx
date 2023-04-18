import React, { useEffect, useState } from 'react';
import { Box, List, ListItemButton, ListItemIcon, ListItemText, Typography } from '@mui/material';
import QuestionAnswerIcon from '@mui/icons-material/QuestionAnswer';
import { useTheme } from '@emotion/react';
import { useSelector } from 'react-redux';
import { RootState, useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { getUserTopics } from '../topic/topicSlice';
import Loading from '../../app/components/Loading';
import { useNavigate } from 'react-router-dom';
import Filter from '../../app/components/filter/Filter';
import { Pagination } from '@mui/material';
const viewOptions = [
  { label: "Open", value: "open" },
  { label: "Closed", value: "close" },
];
const MyIdeas = () => {
  const theme: any = useTheme();
  const { user_topics, loading } = useSelector((state: RootState) => state.topic);
  const { user } = useAppSelector(state => state.account);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;
  const [selectedViewOption, setSelectedViewOption] = useState('open');
  const dispatch = useAppDispatch();
  let fetchMount = true;
  useEffect(() => {
    if (fetchMount) {
      if (user?.name) {
        dispatch(getUserTopics(user.name));
      }
    }
    return () => {
      fetchMount = false;
    };
  }, []);
  const navigate = useNavigate();
  const [topic, setTopic] = useState(user_topics);
  useEffect(() => {
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    let sortedTopics = [...user_topics];

    switch (selectedViewOption) {
      case 'open':
        sortedTopics.sort((a, b) => new Date(b.closureDate).getTime() - new Date(a.closureDate).getTime());
        break;
      case 'close':
        sortedTopics.sort((a, b) => new Date(a.closureDate).getTime() - new Date(b.closureDate).getTime());
        break;
      default:
        break;
    }
    setTopic(sortedTopics.slice(startIndex, endIndex));
  }, [currentPage, selectedViewOption, user_topics]);
  const handleViewOptionChange = (value: string) => {
    setSelectedViewOption(value);
  };
  return (
    <>
      {loading ? (
        <Loading />
      ) : user_topics && user_topics.length ? (
        <Box
          sx={{
            [theme.breakpoints.up('sm')]: {
              width: '95%',
              marginTop: 5,
              ml: '3%'
            },
            [theme.breakpoints.down('sm')]: {
              width: '21rem',
              marginTop: 5,
              ml: '2rem',
            },
          }}
        >
          <Box sx={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'flex-end', mt: { xs: "-2rem", md: "-1rem" } }}>
            <Filter options={viewOptions} selectedValue={selectedViewOption} onChange={handleViewOptionChange} />
          </Box>
          <List sx={{ pt: "0.5rem", mt: "0.5rem" }}>
            {topic.map((item: any) => (
              <ListItemButton sx={{
                p: { xs: 1, sm: 2 },
                flexDirection: { xs: "column", sm: "row" },
                bgcolor: theme.palette.content.layout,
                borderRadius: "10px",
                boxShadow: "0px 5px 10px rgba(0, 0, 0, 0.15)",
                mb: { xs: 2, sm: 3 },
                position: 'relative', // add position property to set position of tag
              }}
                onClick={() => navigate(`/myTopic/${item.id}/${item.name}`)}>
                {new Date() > new Date(item.closureDate) && ( // check if current date is after closure date
                  <Typography
                    sx={{
                      position: 'absolute', // position tag absolutely within ListItemButton
                      top: '0px',
                      right: '0px',
                      px: 1,
                      py: '2px',
                      fontSize: '0.75rem',
                      fontWeight: 'bold',
                      color: 'white',
                      bgcolor: 'secondary.main',
                      borderRadius: '0px 10px 0px 10px', // adjust border radius to match ListItemButton
                    }}
                  >
                    CLOSED
                  </Typography>
                )}
                <ListItemIcon sx={{ mr: { xs: 0, sm: 2 } }}>
                  <QuestionAnswerIcon color="secondary" sx={{ fontSize: { xs: "2rem", sm: "2.5rem" } }} />
                </ListItemIcon>
                <Box sx={{ flexGrow: 1, mb: { xs: 1, sm: 0 }, mr: { xs: 0, sm: 2 } }}>
                  <ListItemText
                    primary={item.name.toUpperCase()}
                    secondary={`${item.totalIdea} Idea Posts`}
                    primaryTypographyProps={{ variant: "h5", textAlign: { xs: "center", sm: "justify" }, fontWeight: "bold" }}
                    secondaryTypographyProps={{ variant: "body1", textAlign: { xs: "center", sm: "justify" } }}
                  />
                </Box>
                <Box sx={{ flexShrink: 0 }}>
                  <List sx={{ display: "flex", flexDirection: { xs: "row", sm: "column" }, alignItems: { xs: "center", sm: "flex-end" } }}>
                    <ListItemText
                      primary={`Closure Date: ${new Date(item.closureDate).toLocaleDateString('en-GB')}`}
                      secondary={`Final Closure Date: ${new Date(item.finalClosureDate).toLocaleDateString('en-GB')}`}
                      primaryTypographyProps={{ variant: "body1", textAlign: { xs: "center", sm: "right" }, mb: { xs: 0, sm: 1 }, mr: { xs: 1, sm: 0 } }}
                      secondaryTypographyProps={{ variant: "body1", textAlign: { xs: "center", sm: "right" } }}
                    />
                  </List>
                </Box>
              </ListItemButton>
            ))}
            <Box mt="3rem" display='flex' justifyContent='center' alignItems='center' sx={{ marginBottom: 3 }}>
              <Pagination
                count={Math.ceil(user_topics.length / itemsPerPage)}
                page={currentPage}
                size="large"
                color='secondary'
                onChange={(event, value) => setCurrentPage(value)}
                sx={{ mt: 2 }}
              />
            </Box>
          </List>
        </Box>) : <Typography variant="h2" align="left" fontWeight={'bold'} marginLeft={4.5} marginTop={3}>YOU HAVE NOT ADDED ANY IDEA!</Typography>}
    </>
  );
}

export default MyIdeas