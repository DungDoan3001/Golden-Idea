import React, { useEffect, useState } from 'react';
import { Avatar, Box, Divider, Grid, IconButton, Paper, Typography } from '@mui/material';
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
  const [comments, setComments] = useState([]);
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
              '&:after': {
                content: '""',
                position: 'absolute',
                top: '-10px',
                left: '-10px',
                right: '-10px',
                bottom: '-10px',
                border: `2px solid ${theme.palette.secondary.main}`,
                borderStyle: 'dashed',
                borderRadius: '10px',
              },
            }}
          >
            {idea?.topic.name}
          </Typography>
          <Box sx={{ backgroundColor: theme.palette.comment.main, borderRadius: "0.5rem" }}>
            <Box p="1rem 5%">
              <Grid
                display="bottom" alignItems="center" justifyContent="bottom"
                container spacing={1} columns={{ xs: 4, sm: 8, md: 12 }}
              >
                <Grid item xs={3} sm={4} md={6}>
                  {/* <PostAuthorInfo top={false} data={idea} /> */}
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
          <Typography
            m="1rem 0rem"
            variant="h3"
            color={theme.palette.content.main}
            fontWeight="semibold"
          >
            Comment
          </Typography>
          <Divider variant="fullWidth" />
          {
            comments.map((item: any) => (
              <Paper
                sx={{ backgroundColor: theme.palette.comment.main }}
                style={{ padding: "1rem 1rem 0rem" }}
              >
                <Grid container wrap="nowrap" spacing={2}>
                  <Grid item>
                    <Avatar alt="Profile Image" src={item.userImg} />
                  </Grid>
                  <Grid item xs zeroMinWidth>
                    <Grid container wrap="nowrap" spacing={2}>
                      <Grid item xs zeroMinWidth>
                        <Box fontSize="1.1rem" component="h4" justifyContent="left">
                          {item.userName}
                        </Box>
                      </Grid>
                      <Grid item xs zeroMinWidth>
                        <Box mt="0.25rem" fontSize="0.8rem" display="flex" justifyContent="right" alignItems="center" color="gray">
                          {moment(item.createdDate).format("hh:mm | DD/MM/YYYY")}
                        </Box>
                      </Grid>
                    </Grid>
                    {item.content}
                  </Grid>
                </Grid>
                <Divider variant="fullWidth" style={{ margin: "1rem 0rem" }} />
              </Paper>
            ))
          }
          <Box pb="2rem">
            <AppPagination
              setItem={(p: any) => setComments(p)}
              data={commentData}
              size={5}
            />
          </Box>
          <Comment ideaId='8bc9ff66-f901-44da-b9e3-294e0de0ac15' />
    </Box >
      }
    </>
  );

}

export default IdeaDetail