import React, { useEffect, useState } from 'react'
import { addComment, loadComments } from '../../pages/comment/commentSlice';
import { ChatComment } from '../models/Comment';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useDispatch, useSelector } from 'react-redux';
import { makeStyles } from '@mui/styles';
import { RootState, useAppSelector } from '../store/configureStore';
import { Paper, Grid, Avatar, Box, Divider, Checkbox, TextField, IconButton, Typography, CircularProgress } from '@mui/material';
import moment from 'moment';
import { useTheme } from '@mui/styles';
import Loading from './Loading';
import { Send } from '@mui/icons-material';
import AppPagination from './AppPagination';
import { GenericHTMLFormElement } from 'axios';
import { toast } from 'react-toastify';
// import * as toxicity from "@tensorflow-models/toxicity";
// import * as tf from '@tensorflow/tfjs'
// import BadWords from 'bad-words';


interface CommentProps {
  ideaId: any;
  isComment: boolean;
}
const useStyles = makeStyles((theme: any) => ({
  form: {
    marginBottom: theme.spacing(2),
    paddingTop: 0,
    paddingLeft: 0,
    paddingRight: 0,
    width: '100%',
  },
  inputAdornment: {
    marginLeft: theme.spacing(1),
  },
}));
const Comment: React.FC<CommentProps> = ({ ideaId, isComment }) => {
  const dispatch = useDispatch();
  const classes = useStyles();
  const { user } = useAppSelector(state => state.account);
  const [text, setText] = useState('');
  const [connection, setConnection] = useState<any>(null);
  const [isAnonymous, setIsAnonymous] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const comments = useSelector((state: RootState) => state.comment.comments);
  const [comment, setComment] = useState([]);
  const [isSending, setIsSending] = useState(false);
  const [sendIcon, setSendIcon] = useState(<Send />);
  const theme: any = useTheme()
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
      setConnection(connection)
    });
    return () => {
      connection.stop().catch(error => console.log('Error while stopping connection: ' + error));
    };
  }, [dispatch, ideaId]);

  const PERSPECTIVE_API_URL = `https://commentanalyzer.googleapis.com/v1alpha1/comments:analyze?key=${process.env.REACT_APP_PERSPECTIVE_API_KEY}`;
  const validateComment = async (commentText: string): Promise<string | null> => {
    const response = await fetch(PERSPECTIVE_API_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        comment: { text: commentText },
        requestedAttributes: {
          TOXICITY: {},
        },
      }),
    });
    if (!response.ok) {
      // handle error
      console.error('Error:', response);
      // show toast message
      toast.error('Cannot check the content of your comment');
      return null;
    }
    const { attributeScores } = await response.json();

    if (attributeScores.TOXICITY.summaryScore.value >= 0.8) {
      return 'Your comment has been flagged as potentially toxic and goes against our community standards. Please revise your comment to ensure it aligns with our guidelines.';
    }
    return null;
  };
  const sendComment = async (comment: ChatComment) => {
    try {
      await connection.invoke('SendComment', comment)
    }
    catch (error) {
      console.log(error)
    }
  }
  const handleTextChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setText(event.target.value);
  };

  const handleAnonymousChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setIsAnonymous(event.target.checked);
  };
  const handleSubmit = async (event: React.FormEvent<GenericHTMLFormElement>) => {
    event.preventDefault();
    setText('Checking your content...');
    setIsSending(true);
    setSendIcon(<CircularProgress size={24} />);
    if (user && user.name) {
      const validationResult = await validateComment(text);
      if (validationResult !== null) {
        // Display error toast
        toast.error(validationResult);
        setText('');
        setIsSending(false);
        setSendIcon(<Send />);
      } else {
        const comment: ChatComment = {
          ideaId: ideaId,
          username: user.name,
          content: text,
          isAnonymous: isAnonymous,
        };
        sendComment(comment);
        setText('');
        setIsSending(false);
        setSendIcon(<Send />);
        setIsAnonymous(false);
      }
    }
  };
  return (
    <Box
      m="4rem"
      width="100%"
      sx={{
        [theme.breakpoints.up('sm')]: {
          width: '80%',
          m: "1rem 6rem",
        },
        [theme.breakpoints.down('sm')]: {
          width: '21rem',
          m: "1rem 2rem",
        },
      }}
    >
      <Typography
        m="1rem 0rem"
        variant="h3"
        color={theme.palette.content.main}
        fontWeight="semibold"
      >
        Comment
      </Typography>
      {isLoading ? <Loading /> : (
        <Box paddingBottom={4}>
          {isComment ? (<form className={classes.form} onSubmit={handleSubmit}>
            <Grid container spacing={2} alignItems="center" marginTop={3}>
              <Grid item xs={12} sm={12}>
                <TextField
                  id="comment-text"
                  label="Add a comment"
                  fullWidth
                  value={text}
                  onChange={handleTextChange}
                  InputProps={{
                    endAdornment: (
                      <IconButton
                        className={classes.inputAdornment}
                        type="submit"
                        disabled={!text.trim() || isSending}
                      >
                        {sendIcon}
                      </IconButton>
                    ),
                  }}
                />
              </Grid>
              <Grid item xs={12} sm={4} sx={{ marginTop: { sx: -20, sm: -1 } }}>
                <Checkbox
                  checked={isAnonymous}
                  onChange={handleAnonymousChange}
                  color="info"
                />
                Post as anonymous
              </Grid>
            </Grid>
          </form>) : (null)}
          {
            !comments.length ? (<Typography>No comment</Typography>) : (comment.map((item: any) => (
              <Paper
                sx={{ backgroundColor: theme.palette.comment.main }}
                style={{ padding: "1rem 1rem 0rem" }}
              >
                <Grid container wrap="nowrap" spacing={2}>
                  <Grid item>
                    <Avatar alt="Profile Image" src={item.isAnonymous ? '' : item.avatar} />
                  </Grid>
                  <Grid item xs zeroMinWidth>
                    <Grid container wrap="nowrap" spacing={2}>
                      <Grid item xs zeroMinWidth>
                        <Box fontSize="1.1rem" component="h4" justifyContent="left">
                          {item.isAnonymous ? 'Anonymous' : item.username}
                        </Box>
                      </Grid>
                      <Grid item xs zeroMinWidth>
                        <Box mt="0.25rem" fontSize="0.8rem" display="flex" justifyContent="right" alignItems="center" color="gray">
                          {moment.utc(item.createdDate).utcOffset(moment().utcOffset()).format("hh:mm A | DD/MM/YYYY")}
                        </Box>
                      </Grid>
                    </Grid>
                    {item.content}
                  </Grid>
                </Grid>
                <Divider variant="fullWidth" style={{ margin: "1rem 0rem" }} />
              </Paper>
            )))
          }
          <AppPagination
            setItem={(p: any) => setComment(p)}
            data={comments}
            size={5}
          />

        </Box>
      )}
    </Box>
  )
}

export default Comment