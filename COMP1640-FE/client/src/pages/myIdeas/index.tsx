import React, { useEffect, useState } from 'react';
import { Box, List, ListItemButton, ListItemIcon, ListItemText, Typography } from '@mui/material';
import QuestionAnswerIcon from '@mui/icons-material/QuestionAnswer';
import AppPagination from '../../app/components/AppPagination';
import { useTheme } from '@emotion/react';
import { useSelector } from 'react-redux';
import { RootState, useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { getUserTopics } from '../topic/topicSlice';
import Loading from '../../app/components/Loading';
import { useNavigate } from 'react-router-dom';

const MyIdeas = () => {
  const theme: any = useTheme();
  const { user_topics, loading } = useSelector((state: RootState) => state.topic);
  const { user } = useAppSelector(state => state.account);
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

  return (
    <>
      {loading ? (
        <Loading />
      ) : user_topics && user_topics.length ? (
        <Box
          sx={{
            [theme.breakpoints.up('sm')]: {
              width: '95%',
              marginTop: 10,
              ml: '3%'
            },
            [theme.breakpoints.down('sm')]: {
              width: '465px',
              marginTop: 5,
              ml: '25px',
              mr: '15px'
            },
          }}
        >
          <List sx={{
            paddingTop: "0.1rem",
            marginTop: {
              xs: "-5px",
              md: "-30px"
            },
          }}>
            {topic.map((item: any) => (
              <ListItemButton sx={{
                p: { xs: 1, sm: 2 }, flexDirection: { xs: "column", sm: "row" },
                bgcolor: theme.palette.content.layout,
                borderRadius: "10px",
                boxShadow: "0px 5px 10px rgba(0, 0, 0, 0.15)",
                mb: { xs: 2, sm: 3 },
              }}
                onClick={() => navigate(`/myTopic/${item.id}/${item.name}`)}>
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
            <AppPagination
              setItem={(p: any) => setTopic(p)}
              data={user_topics}
              size={5}
            />
          </List>
        </Box>) : <Typography variant="h2" align="left" fontWeight={'bold'} marginLeft={4.5} marginTop={3}>YOU HAVE NOT ADDED ANY IDEA!</Typography>}
    </>
  );
}

export default MyIdeas