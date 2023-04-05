import { useEffect, useState } from 'react';
import { Box, Divider, Grid, IconButton, Typography, ListItemText, List } from '@mui/material';
import { useTheme } from '@emotion/react';

import ThumbUpTwoToneIcon from '@mui/icons-material/ThumbUpTwoTone';
import ThumbDownTwoToneIcon from '@mui/icons-material/ThumbDownTwoTone';
import ThumbUpIcon from '@mui/icons-material/ThumbUp';
import ThumbDownIcon from '@mui/icons-material/ThumbDown';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import parse from 'html-react-parser';
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
import IdeaFormEdit from './ideaFormEdit';
import NoImage from '../../app/assets/null_img.jpg';
import pdf from '../../app/assets/pdf.png';
import word from '../../app/assets/word.png';
import image from '../../app/assets/image.png';
import text from '../../app/assets/text.png';
import excel from '../../app/assets/excel.png';
import AnonymousImage from '../../app/assets/anonymous.png';
import FileRender from '../../app/components/FileRender';
import { Visibility, Download, VisibilityOff } from '@mui/icons-material';

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
  const [editMode, setEditMode] = useState(false);
  const [selectedFileUrl, setSelectedFileUrl] = useState<string | null>(null);

  const dispatch = useAppDispatch();
  useEffect(() => {
    dispatch(getIdeaBySlug(slug));
    return () => {
      // cleanup function
    };
  }, [dispatch, slug]);

  useEffect(() => {
    const fetchData = async () => {
      if (idea && user && user.name) {
        setLoadReaction(true);
        const data = {
          username: user.name
        }
        await agent.Idea.postView(idea.id, data)
        const res = await agent.Idea.getReaction(idea.id, user.name);
        if (res.react === 1) setIslike(true)
        else if (res.react === -1) setIsDislike(true)
        setLoadReaction(false);
      }
    }
    fetchData();
  }, [idea, user]);

  useEffect(() => {
    if (idea && user && user.name) {
      const finalClosureDate = idea.topic.finalClosureDate;
      if (finalClosureDate) {
        const today = new Date().getTime();
        const CommentDate = new Date(finalClosureDate).getTime();
        setIsCommentAvailable(today < CommentDate);
      }

      const today = new Date().getTime();
      const closureDate = new Date(idea?.topic.closureDate).getTime();
      setIsEditable(today < closureDate && user.name === idea.user.userName);
    }
  }, [idea, user]);

  function cancelEdit() {
    setEditMode(false);
  }

  const ClickLike = async () => {
    if (idea && user && user.name) {
      try {
        (isLike ? setIslike(false) : setIslike(true));
        setIsDislike(false);
        const data = { ideaId: idea.id, username: user.name };
        await agent.Idea.postReaction("upvote", data);
      } catch (error) {
        console.log(error);
      }
    }
  }
  const ClickDislike = async () => {
    if (idea && user && user.name) {
      try {
        (isDislike ? setIsDislike(false) : setIsDislike(true));
        setIslike(false);
        const data = { ideaId: idea.id, username: user.name };
        await agent.Idea.postReaction("downvote", data);
      } catch (error) {
        console.log(error);
      }
    }
  }

  function delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  const handleEdit = (idea: any) => {

    setEditMode(true);
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

  const handleDownload = (i: any) => {
    if (idea && idea.files) {
      // using Java Script method to get PDF file
      fetch(idea.files[i].filePath).then(response => {
        response.blob().then(blob => {
          // Creating new object of PDF file
          const fileURL = window.URL.createObjectURL(blob);
          // Setting various property values
          let alink = document.createElement('a');
          alink.href = fileURL;
          // alink.download = idea.files[i].path;
          alink.download = idea.files[i].fileName.split('/').pop() + '.' + idea.files[i].fileExtension;
          alink.click();
        })
      })
    }
  }
  const getFileIcon = (fileExtension: any) => {
    switch (fileExtension) {
      case 'pdf':
        return pdf;
      case 'doc':
      case 'docx':
        return word;
      case 'xlsx':
      case 'csv':
        return excel;
      case 'txt':
        return text;
      case 'png':
      case 'jpg':
        return image;
      default:
        return NoImage;
    }
  }
  const handleFileClick = (fileUrl: string) => {
    if (selectedFileUrl === fileUrl) {
      // If the same file is clicked again, close the document by setting the selected file URL to null
      setSelectedFileUrl(null);
    } else {
      setSelectedFileUrl(fileUrl);
    }
  };
  return (
    <>
      {(loading || loadReaction) || !user || !user.name ? <Loading /> :
        (editMode) ? <IdeaFormEdit cancelEdit={cancelEdit} id={idea?.topic.id} idea={idea} /> :
          idea ? (<>
            <Box alignItems="center" justifyContent="center"
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
              <Box
                m="0.5rem 0rem"
                display="flex"
                alignItems="center"
                flexDirection={{ xs: "column", sm: "row" }}
                textAlign={{ xs: "center", sm: "left" }}
              >
                <Grid container>
                  <Grid item xs={12} sm={6}>
                    <Box pt="5%" display={{ xs: "block", sm: "flex" }} justifyContent={{ xs: "center", sm: "left" }} textAlign={{ xs: "center", sm: "left" }} alignItems="center">
                      <Box
                        component="img"
                        alt="profile"
                        src={idea?.topic.avatar}
                        height="2.5rem"
                        width="2.5rem"
                        borderRadius="50%"
                        sx={{ objectFit: "cover", mr: { xs: 0, sm: "1rem" }, mb: { xs: "1rem", sm: 0 } }}
                      />
                      <Box component="h4" mb=".5rem">
                        Creator: {idea?.topic.username}
                      </Box>
                    </Box>
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <Box display="flex" justifyContent={{ xs: "center", sm: "right" }} textAlign={{ xs: "center", sm: "right" }} alignItems="center">
                      <List>
                        <ListItemText
                          primary={`Closure Date: ${new Date(`${idea?.topic.closureDate}`).toLocaleDateString('en-GB')}`}
                          primaryTypographyProps={{
                            variant: "body1",
                            mb: { xs: "0.5rem", sm: 0 },
                          }}
                        />
                        <ListItemText
                          secondary={`Final Closure Date: ${new Date(`${idea?.topic.finalClosureDate}`).toLocaleDateString('en-GB')}`}
                          primaryTypographyProps={{
                            variant: "body1",
                            mb: { xs: "0.5rem", sm: 0 },
                          }}
                        />
                      </List>
                    </Box>
                  </Grid>
                </Grid>
              </Box>
              <Box sx={{ backgroundColor: theme.palette.comment.main, borderRadius: "0.5rem" }}>
                <Box p="1rem 5%">
                  <Typography
                    textAlign="left"
                    mb="1rem"
                    variant="h2"
                    color={theme.palette.content.main}
                    fontWeight="bold"
                  >
                    {idea?.title}
                  </Typography>
                  <Grid
                    display="bottom" alignItems="center" justifyContent="bottom"
                    container spacing={0.5} columns={{ xs: 4, sm: 8, md: 12 }}
                  >
                    <Grid item xs={3} sm={4} md={6}>
                      <PostAuthorInfo
                        avatar={idea?.isAnonymous ? AnonymousImage : idea?.user.avatar}
                        userName={idea?.isAnonymous ? 'Anonymous' : idea?.user.userName}
                        lastUpdate={idea?.lastUpdate}
                      />
                    </Grid>
                    <Grid item xs={9} sm={8} md={6}>
                      {(isEditable) ? (<Box display="flex" justifyContent="right" alignItems="right">
                        <IconButton
                          color="info"
                          style={{ marginRight: "1rem" }}
                          onClick={() => handleEdit(idea)}
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
                      src={(idea?.image) ? idea?.image : NoImage}
                      sx={{
                        width: { xs: "85vw", sm: "50vw" },
                        height: { xs: "65vw", sm: "35vw" },
                        objectFit: "contain",
                      }}
                    >
                    </Box>
                  </Box>
                  <Box
                    sx={{
                      maxWidth: { xs: "85vw", sm: "60vw" },
                      overflowX: "auto",
                      width: "100%",
                      height: { xs: "auto", sm: "auto" },
                      objectFit: "contain",
                      margin: { xs: "2rem 0", sm: "4rem 0" },
                    }}
                  >
                    <Typography
                      variant="body1"
                      textAlign="justify"
                      color={theme.palette.content.main}
                      fontSize={{ xs: theme.typography.pxToRem(14), sm: theme.typography.pxToRem(16) }}
                      lineHeight={{ xs: 1.5, sm: 1.8 }}
                    >
                      {parse(idea?.content || "")}
                    </Typography>
                  </Box>
                  {(idea && idea.files) ?
                    idea.files.map((item: any, index: any) => (
                      <>
                        <Box mt="2rem" display="flex" alignItems="center" justifyContent="left">
                          <Box
                            component="img"
                            alt="fileIcon"
                            src={getFileIcon(item.fileExtension)}
                            height="2.5rem"
                            width="2.5rem"
                            sx={{
                              objectFit: "cover", mr: "1rem"
                            }} />
                          <Typography
                            width="15rem"
                            noWrap
                          >
                            {`${item.fileName}.${item.fileExtension}`}
                          </Typography>
                          <IconButton onClick={() => handleFileClick(item.filePath)} sx={{ ml: "1rem" }}>
                            {selectedFileUrl === item.filePath ? <VisibilityOff color="error" /> : <Visibility />}
                          </IconButton>
                          <IconButton onClick={() => handleDownload((index))} sx={{ ml: "1rem" }}>
                            <Download />
                          </IconButton>
                        </Box>
                      </>
                    )
                    ) : (null)}
                  {selectedFileUrl !== null ? (
                    <FileRender fileUrl={selectedFileUrl} />
                  ) : null}
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
          ) : <Typography variant="h2" align="left" fontWeight={'bold'} marginLeft={4.5} marginTop={3}>SORRY! NO DATA WITH THIS SLUG</Typography>
      }
      <ConfirmDialog
        confirmDialog={confirmDialog}
        setConfirmDialog={setConfirmDialog}
      />
    </>
  );
}
export default IdeaDetail
