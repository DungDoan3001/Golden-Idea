import React, { useEffect, useState } from 'react'
import { addComment, loadComments } from '../../pages/comment/commentSlice';
import { ChatComment } from '../models/Comment';
import { HttpTransportType, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useDispatch, useSelector } from 'react-redux';
import { RootState, store } from '../store/configureStore';
import { Typography, Paper, Grid, Avatar, Box, Divider } from '@mui/material';
import { useTheme } from '@emotion/react';
import AppPagination from './AppPagination';
import moment from 'moment';
interface CommentProps {
  ideaId: string;
}

const Comment: React.FC<CommentProps> = ({ ideaId }) => {
  const theme: any = useTheme();
  const dispatch = useDispatch();
  const [text, setText] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const comments = useSelector((state: RootState) => state.comment.comments);
  const [commentData, setcommentData] = useState(comments);

  useEffect(() => {
    var token = sessionStorage.getItem('user');
    const connection = new HubConnectionBuilder()
      .withUrl(`https://goldenidea.azurewebsites.net/chat?ideaId=${ideaId}`, {
        accessTokenFactory: () => `${token!}` // Return access token
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    connection.start().then(() => {
      setIsLoading(false);
      console.log("Connected")
      connection.on('ReceiveComment', (comment: ChatComment) => {
        dispatch(addComment(comment));
      });

      connection.on('LoadComments', (comments: any[]) => {
        dispatch(loadComments(comments));
      });

      connection.invoke('OnConnectedAsync');
    });

    return () => {
      connection.stop();
    };
  }, [dispatch, ideaId]);
  useEffect(() => {
    console.log(comments);
  },[])

  const handleTextChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setText(event.target.value);
  };
  return (
    <>
      <Typography
        m="1rem 0rem"
        variant="h3"
        color={theme.palette.content.main}
        fontWeight="semibold"
      >
        Comment
      </Typography>
      {
        commentData.map((item: any) => (
          <Paper
            sx={{ backgroundColor: theme.palette.comment.main }}
            style={{ padding: "1rem 1rem 0rem" }}
          >
            <Grid container wrap="nowrap" spacing={2}>
              <Grid item>
                <Avatar alt="Profile Image" src={item.avatar} />
              </Grid>
              <Grid item xs zeroMinWidth>
                <Grid container wrap="nowrap" spacing={2}>
                  <Grid item xs zeroMinWidth>
                    <Box fontSize="1.1rem" component="h4" justifyContent="left">
                      {item.username}
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
          setItem={(p: any) => setcommentData(p)}
          data={comments}
          size={5}
        />
      </Box>
    </>
  )
}

export default Comment