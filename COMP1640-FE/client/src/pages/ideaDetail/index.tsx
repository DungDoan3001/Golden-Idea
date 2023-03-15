import React, { useEffect, useState } from 'react';
import { Avatar, Box, Divider, Grid, IconButton, Paper, Typography, ListItemText, List } from '@mui/material';
import Service from '../../app/utils/Service';
import { useTheme } from '@emotion/react';
import moment from 'moment';

import ChatBubbleTwoToneIcon from '@mui/icons-material/ChatBubbleTwoTone';
import ThumbUpTwoToneIcon from '@mui/icons-material/ThumbUpTwoTone';
import ThumbDownTwoToneIcon from '@mui/icons-material/ThumbDownTwoTone';
import ThumbUpIcon from '@mui/icons-material/ThumbUp';
import ThumbDownIcon from '@mui/icons-material/ThumbDown';


import { postData } from "../../dataTest.js"
import { commentData } from '../../dataTest.js';

import PostAuthorInfo from '../../app/components/PostAuthorInfo';
import AppPagination from '../../app/components/AppPagination';

import { useParams } from 'react-router-dom';
import { RootState, useAppDispatch } from '../../app/store/configureStore';
import { getIdeaBySlug } from '../myIdeas/ideasSlice';
import { useSelector } from 'react-redux';
import Loading from '../../app/components/Loading';
import Comment from '../../app/components/Comment';

const IdeaDetail = () => {
  const theme: any = useTheme();
  const { slug } = useParams();
  const [isLike, setIslike] = useState(false);
  const [isDislike, setisDislike] = useState(false);
  const { idea, loading } = useSelector((state: RootState) => state.idea);
  const dispatch = useAppDispatch();
  let fetchMount = true;
  useEffect(() => {
    if (fetchMount) {
      dispatch(getIdeaBySlug(slug));
    }
    return () => {
      fetchMount = false;
    };
  }, []);

  useEffect(() => {
    if (idea != null) {
    }
  }, []);

  const ClickLike = () => {
    (isLike ? setIslike(false) : setIslike(true));
    setisDislike(false);
  }
  const ClickDislike = () => {
    (isDislike ? setisDislike(false) : setisDislike(true));
    setIslike(false);
  }

  return (
    <>
      {loading ? <Loading /> :
        <>
          <Box alignItems="center" justifyContent="center"
            width="100%"
            sx={{
              [theme.breakpoints.up('sm')]: {
                width: '80%',
                m: "1rem 6rem",
              },
              [theme.breakpoints.down('sm')]: {
                width: '110%',
                m: "1rem 2rem",
              },
            }}
          >
            <Typography
              m="2rem 0rem"
              variant="h1"
              color={theme.palette.content.main}
              fontWeight="bold"
              sx={{
                textAlign: 'center',
                textTransform: 'uppercase',
                position: 'relative',
                display: 'inline-block',
                color: theme.palette.secondary.main,
              }}
            >
              {idea?.topic.name}
            </Typography>
            <Divider variant="fullWidth" />
            <Box m="1rem 0rem" display="flex" alignItems="center" justifyContent="right">
              <Box
                component="img"
                alt="profile"
                src={idea?.topic.avatar}
                height="2.5rem"
                width="2.5rem"
                borderRadius="50%"
                sx={{ objectFit: "cover" }}
              />
              <Box component="h4" m=".5rem 1rem">
                Topic By: {idea?.topic.username}
              </Box>
              <Box>
                <List sx={{ display: "flex", flexDirection: { xs: "row", sm: "column" }, alignItems: { xs: "center", sm: "flex-end" } }}>
                  <ListItemText
                    primary={`Closure Date: ${new Date(idea?.topic.closureDate).toLocaleDateString('en-GB')}`}
                    secondary={`Final Closure Date: ${new Date(idea?.topic.finalClosureDate).toLocaleDateString('en-GB')}`}
                    primaryTypographyProps={{ variant: "body1", textAlign: { xs: "right", sm: "right" }, mb: { xs: 0, sm: 1 }, mr: { xs: 1, sm: 0 } }}
                    secondaryTypographyProps={{ variant: "body1", textAlign: { xs: "right", sm: "right" } }}
                  />
                </List>
              </Box>
            </Box>
            <Divider variant="fullWidth" />
            <Box sx={{ backgroundColor: theme.palette.comment.main, borderRadius: "0.5rem" }}>
              <Box p="1rem 5%">
                <Grid
                  display="bottom" alignItems="center" justifyContent="bottom"
                  container spacing={1} columns={{ xs: 4, sm: 8, md: 12 }}
                >
                  <Grid item xs={3} sm={4} md={6}>
                    <PostAuthorInfo
                      avatar={idea?.user.avatar}
                      userName={idea?.user.userName}
                      lastUpdate={idea?.lastUpdate}
                    />
                  </Grid>
                </Grid>
                <Typography
                  textAlign="left"
                  mb="1rem"
                  variant="h2"
                  color={theme.palette.content.main}
                  fontWeight="bold"
                >
                  {idea?.title}
                </Typography>
                <Box m="2rem">
                  <Box
                    component="img"
                    alt="thumbnail"
                    src={idea?.image}
                    height="100%"
                    width="100%"
                  >
                  </Box>
                </Box>
                <Typography
                  textAlign="justify"
                  variant="h5"
                  color={theme.palette.content.main}
                  fontSize="1rem"
                >
                  {idea?.content}
                </Typography>
                <Box m="1rem 0rem">
                  <IconButton onClick={ClickLike}>
                    {(isLike) ?
                      <ThumbUpIcon
                        style={{
                          fontSize: "1.5rem",
                          color: theme.palette.content.main,
                          paddingRight: "0.5rem"
                        }}
                      /> :
                      <ThumbUpTwoToneIcon
                        style={{
                          fontSize: "1.5rem",
                          color: theme.palette.content.icon,
                          paddingRight: "0.5rem"
                        }}
                      />
                    }
                    <Box fontSize="1rem">
                      {idea?.upVote}
                    </Box>
                  </IconButton>
                  <IconButton onClick={ClickDislike}>
                    {(isDislike) ?
                      <ThumbDownIcon
                        style={{
                          fontSize: "1.5rem",
                          color: theme.palette.content.main,
                          paddingRight: "0.5rem"
                        }}
                      /> :
                      <ThumbDownTwoToneIcon
                        style={{
                          fontSize: "1.5rem",
                          color: theme.palette.content.icon,
                          paddingRight: "0.5rem"
                        }}
                      />
                    }

                    <Box fontSize="1rem">
                      {idea?.downVote}
                    </Box>
                  </IconButton>
                  <IconButton disabled>
                    <ChatBubbleTwoToneIcon
                      style={{
                        fontSize: "1.5rem",
                        color: theme.palette.content.icon,
                        paddingRight: "0.5rem"
                      }}
                    />
                    <Box fontSize="1rem">
                      12
                    </Box>
                  </IconButton>
                </Box>
              </Box>
            </Box>
          </Box>
          <Divider variant="fullWidth" />
          <Comment ideaId={idea?.id} />
        </>
      }
    </>
  );
}

export default IdeaDetail