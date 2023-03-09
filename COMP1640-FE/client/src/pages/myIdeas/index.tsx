import React, { useState } from 'react';
import { Box, Divider, List, ListItemButton, ListItemIcon, ListItemText } from '@mui/material';
import QuestionAnswerIcon from '@mui/icons-material/QuestionAnswer';

import AppPagination from '../../app/components/AppPagination';

import { topicData } from "../../dataTest.js"

import { useTheme } from '@emotion/react';

const MyIdeas = () => {
  const theme: any = useTheme();

  const [topic, setTopic] = useState([]);

  return (
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
          </ListItemButton>
        ))}
        <AppPagination
          setItem={(p: any) => setTopic(p)}
          data={topicData}
          size={10}
        />
      </List>
    </Box>
  );
}

export default MyIdeas