import React, { useEffect, useState } from 'react';
import { Box, Divider, List, ListItemButton, ListItemIcon, ListItemText } from '@mui/material';
import QuestionAnswerIcon from '@mui/icons-material/QuestionAnswer';
import AppPagination from '../../app/components/AppPagination';
import { topicData } from "../../dataTest.js"
import { useTheme } from '@emotion/react';
import { useSelector } from 'react-redux';
import { RootState, useAppDispatch } from '../../app/store/configureStore';
import { getTopics } from '../topic/topicSlice';
import Loading from '../../app/components/Loading';

const MyIdeas = () => {
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


  const [topic, setTopic] = useState(topics);

  return (
    <>
      {loading ? (<Loading />) : (
        <Box
          p="3rem"
          sx={{
            [theme.breakpoints.up('sm')]: {
              width: '100%',
            },
            [theme.breakpoints.down('sm')]: {
              width: '130%',
            },
          }}
        >
          <List style={{ paddingTop: "2rem" }}>
            {topic.map((item: any) => (
              <ListItemButton style={{ padding: "1.25rem" }}>
                <Divider absolute={true} />
                <ListItemIcon>
                  <QuestionAnswerIcon color="secondary" style={{ fontSize: "2.5rem" }} />
                </ListItemIcon>
                <ListItemText>
                  <span style={{
                    margin: "0rem 1rem",
                    fontSize: "1.2rem",
                  }}>
                    {item.name}
                  </span>
                  <Box m="0rem 1rem">
                    100k Posts
                  </Box>
                </ListItemText>
                <ListItemText style={{ textAlign: "right" }}>
                  Closure Date: {item.closureDate}
                </ListItemText>
                <ListItemText style={{ textAlign: "right" }}>
                  Final Closure Date: {item.finalClosureDate}
                </ListItemText>
              </ListItemButton>
            ))}
            <AppPagination
              setItem={(p: any) => setTopic(p)}
              data={topics}
              size={5}
            />
          </List>
        </Box>)}
    </>
  );
}

export default MyIdeas