import React, { useEffect, useState } from 'react';
import { Box, List, ListItemButton, ListItemIcon, ListItemText, Typography } from '@mui/material';
import AppPagination from '../../app/components/AppPagination';
import { useTheme } from '@emotion/react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import Loading from '../../app/components/Loading';
import { RootState, useAppSelector, useAppDispatch } from '../../app/store/configureStore';
import { getTopics } from '../topic/topicSlice';
import { QuestionAnswer } from '@mui/icons-material';

const Home = () => {
  const theme: any = useTheme();
  const { topics, loading } = useSelector((state: RootState) => state.topic);
  const dispatch = useAppDispatch();
  let fetchMount = true;
  useEffect(() => {
    if (fetchMount) {
      dispatch(getTopics());
    }
    return () => {
      fetchMount = false;
    };
  }, []);

  const navigate = useNavigate();
  const [topic, setTopic] = useState(topics);

  return (
    <>
      {loading ? (
        <Loading />
      ) : (
        <Box
          sx={{
            [theme.breakpoints.up('sm')]: {
              width: '95%',
              marginTop: 10,
              ml: '3%'
            },
            [theme.breakpoints.down('sm')]: {
              width: '21rem',
              marginTop: 5,
              ml: '2rem',
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
                onClick={() => navigate(`/topic/${item.id}/${item.name}`)}>
                <ListItemIcon sx={{ mr: { xs: 0, sm: 2 } }}>
                  <QuestionAnswer color="secondary" sx={{ fontSize: { xs: "2rem", sm: "2.5rem" } }} />
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
              data={topics}
              size={5}
            />
          </List>
        </Box>
      )}
    </>
  );
}

export default Home