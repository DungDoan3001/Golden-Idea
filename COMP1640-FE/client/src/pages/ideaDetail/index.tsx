import React, { useEffect, useState } from 'react';
import parse from 'html-react-parser';
import { Avatar, Box, Divider, Grid, IconButton, Paper, Typography, ListItemText, List } from '@mui/material';
import { useTheme } from '@emotion/react';

import ChatBubbleTwoToneIcon from '@mui/icons-material/ChatBubbleTwoTone';
import ThumbUpTwoToneIcon from '@mui/icons-material/ThumbUpTwoTone';
import ThumbDownTwoToneIcon from '@mui/icons-material/ThumbDownTwoTone';
import ThumbUpIcon from '@mui/icons-material/ThumbUp';
import ThumbDownIcon from '@mui/icons-material/ThumbDown';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import DownloadIcon from '@mui/icons-material/Download';

import PostAuthorInfo from '../../app/components/PostAuthorInfo';

import { useNavigate, useParams } from 'react-router-dom';
import { RootState, useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { deleteIdea, getIdeaBySlug } from '../myIdeas/ideasSlice';
import { useSelector } from 'react-redux';
import Loading from '../../app/components/Loading';
import Comment from '../../app/components/Comment';
import BackButton from '../../app/components/BackButton';
import { toast } from 'react-toastify';
import ConfirmDialog from '../../app/components/ConfirmDialog';
import agent from '../../app/api/agent';

const IdeaDetail = () => {
  const theme: any = useTheme();
  const navigate = useNavigate();
  const { slug } = useParams();
  const [isLike, setIslike] = useState(false);
  const [isDislike, setIsDislike] = useState(false);
  const [loadReaction, setLoadReaction] = useState(false);
  const [confirmDialog, setConfirmDialog] = useState({ isOpen: false, title: '', subTitle: '', onConfirm: () => { } })
  const { idea, loading } = useSelector((state: RootState) => state.idea);
  const { user } = useAppSelector(state => state.account);
  const [isCommentAvailable, setIsCommentAvailable] = useState(true);
  const [isEditable, setIsEditable] = useState(true);
  const [fileIcon, setFileIcon] = useState<any>();
  const [content, setContent] = useState("");

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
    const fetchData = async () => {
      if (idea != null && user && user.name) {
        setLoadReaction(true);
        const data = {
          username: user?.name
        }
        const response = await agent.Idea.postView(idea?.id, data)
        const res = await agent.Idea.getReaction(idea?.id, user?.name);
        if (res.react === 1) setIslike(true)
        else if (res.react === -1) setIsDislike(true)
        setLoadReaction(false);
      }
    }
    fetchData();
  }, [idea, user]);

  useEffect(() => {
    if (idea != null && user && user.name) {
      const finalClosureDate = idea?.topic.finalClosureDate;
      if (finalClosureDate) {
        const today = new Date().getTime();
        const CommentDate = new Date(finalClosureDate).getTime();
        setIsCommentAvailable(today < CommentDate);
      }

      const today = new Date().getTime();
      const closureDate = new Date(idea?.topic.closureDate).getTime();
      setIsEditable(today < closureDate && user?.name === idea?.user.userName);

      switch (idea?.files[0].fileExtention) {
        case "pdf":
          setFileIcon("https://cdn.discordapp.com/attachments/1074670576809033798/1087750328423829575/pdf.png");
          break;
        case "doc":
          setFileIcon("https://media.discordapp.net/attachments/1074670576809033798/1087750328709038080/word.png");
          break;
        case "docx":
          setFileIcon("https://media.discordapp.net/attachments/1074670576809033798/1087750328709038080/word.png");
          break;
        case "zip":
          setFileIcon("https://cdn.discordapp.com/attachments/1074670576809033798/1087750329292050462/zip.png");
          break;
      }
      const regex = /(<([^>]+)>)/ig;
      const newString = idea.content.replace(regex, '');
      setContent(newString);
    }
  }, [idea, user]);
  const ClickLike = async () => {
    if (idea != null && user && user.name) {
      try {
        (isLike ? setIslike(false) : setIslike(true));
        setIsDislike(false);
        const data = { ideaId: idea?.id, username: user?.name };
        const response = await agent.Idea.postReaction("upvote", data);
      } catch (error) {
        console.log(error);
      }
    }
  }
  const ClickDislike = async () => {
    if (idea != null && user && user.name) {
      try {
        (isDislike ? setIsDislike(false) : setIsDislike(true));
        setIslike(false);
        const data = { ideaId: idea?.id, username: user?.name };
        const response = await agent.Idea.postReaction("downvote", data);
      } catch (error) {
        console.log(error);
      }
    }
  }

  function delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
  const handleDelete = (id: any) => {
    dispatch(deleteIdea(id))
      .catch((error: any) => {
        // handle error
        toast.error(error.toString(), {
          style: { marginTop: '50px' },
          position: toast.POSITION.TOP_RIGHT
        });
      });
    setConfirmDialog({
      ...confirmDialog,
      isOpen: false
    });
    delay(1000);
    navigate(-1);
  }

  return (
    <>
      {(loading || loadReaction) && user && user.name ? <Loading /> :
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
              {idea?.topic.name}
            </Typography>
            <Box m="0.5rem 0rem" display="flex" alignItems="center">
              <Box
                component="img"
                alt="profile"
                src={idea?.topic.avatar}
                height="2.5rem"
                width="2.5rem"
                borderRadius="50%"
                sx={{ objectFit: "cover", mr: "1rem" }}
              />
              <Box>
                <Box component="h4" mb=".5rem">
                  Creator: {idea?.topic.username}
                </Box>
              </Box>
              <Box sx={{ ml: "auto" }}>
                <List sx={{ display: "flex", flexDirection: { xs: "row", sm: "column" } }}>
                  <ListItemText
                    primary={`Closure Date: ${new Date(`${idea?.topic.closureDate}`).toLocaleDateString('en-GB')}`}
                    secondary={`Final Closure Date: ${new Date(`${idea?.topic.finalClosureDate}`).toLocaleDateString('en-GB')}`}
                    primaryTypographyProps={{ variant: "body1", textAlign: { xs: "right", sm: "right" }, mb: { xs: 0, sm: 1 }, mr: { xs: 1, sm: 0 } }}
                    secondaryTypographyProps={{ variant: "body1", textAlign: { xs: "right", sm: "right" } }}
                  />
                </List>
              </Box>
            </Box>
            <Box sx={{ backgroundColor: theme.palette.comment.main, borderRadius: "0.5rem" }}>
              <Box p="1rem 5%">
                <Typography
                  textAlign="justify"
                  mb="1rem"
                  variant="h2"
                  color={theme.palette.content.main}
                  fontWeight="bold"
                >
                  {parse(`${idea?.title}`)}
                </Typography>
                <Grid
                  display="bottom" alignItems="center" justifyContent="bottom"
                  container spacing={0.5} columns={{ xs: 4, sm: 8, md: 12 }}
                >
                  <Grid item xs={3} sm={4} md={6}>
                    <PostAuthorInfo
                      avatar={idea?.user.avatar}
                      userName={idea?.user.userName}
                      lastUpdate={idea?.lastUpdate}
                    />
                  </Grid>
                  <Grid item xs={9} sm={8} md={6}>
                    {(isEditable) ? (<Box display="flex" justifyContent="right" alignItems="right">
                      <IconButton
                        color="info"
                        style={{ marginRight: "1rem" }}
                        onClick={() => navigate(`/ideaform/${idea?.topic.id}/${slug}`)}
                      >
                        <EditIcon style={{ fontSize: "1.25rem" }} />
                      </IconButton>
                      <IconButton
                        color="error"
                        style={{ marginRight: "1rem" }}
                        onClick={() => setConfirmDialog({
                          isOpen: true,
                          title: 'Are you sure to delete this record?',
                          subTitle: "You can't undo this operation",
                          onConfirm: () => { handleDelete(idea?.id) }
                        })}
                      >
                        <DeleteIcon style={{ fontSize: "1.25rem" }} />
                      </IconButton>
                    </Box>) : (null)}
                  </Grid>
                </Grid>
                <Box m="2rem" display="flex" alignItems="center" justifyContent="center">
                  <Box
                    component="img"
                    alt="thumbnail"
                    src={idea?.image}
                    sx={{
                      width: { xs: "85vw", sm: "50vw" },
                      height: { xs: "65vw", sm: "35vw" },
                      objectFit: "cover",
                    }}
                  >
                  </Box>
                </Box>
                <Box>
                  <Typography
                    textAlign="justify"
                    variant="h5"
                    color={theme.palette.content.main}
                    fontSize="1rem"
                  >
                    {content}
                  </Typography>
                </Box>
                {(idea?.files) ?
                  <Box mt="2rem" display="flex" alignItems="center" justifyContent="left">
                    <Box
                      component="img"
                      alt="fileIcon"
                      src={fileIcon}
                      // src="https://cdn.discordapp.com/attachments/1074670576809033798/1087750328423829575/pdf.png"
                      height="2.5rem"
                      width="2.5rem"
                      sx={{
                        objectFit: "cover", mr: "1rem"
                      }} />
                      <Typography width="15rem" noWrap>
                    {`${idea.files[0].fileName}.${idea.files[0].fileExtention}`}
                    </Typography>
                    <IconButton sx={{ ml: "1rem" }}>
                      <DownloadIcon />
                    </IconButton>
                  </Box> : (null)}
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
                      Like
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
                      Dislike
                    </Box>
                  </IconButton>
                </Box>
              </Box>
            </Box>
          </Box>
          <Divider variant="fullWidth" />
          <Comment ideaId={idea?.id} isComment={isCommentAvailable} />
        </>
      }
      <ConfirmDialog
        confirmDialog={confirmDialog}
        setConfirmDialog={setConfirmDialog}
      />
    </>
  );
}
export default IdeaDetail