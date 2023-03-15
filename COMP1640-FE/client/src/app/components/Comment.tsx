import React, { useEffect, useState } from 'react'
import { addComment, loadComments } from '../../pages/comment/commentSlice';
import { ChatComment } from '../models/Comment';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useDispatch, useSelector } from 'react-redux';
import { makeStyles } from '@mui/styles';
import { RootState, useAppSelector } from '../store/configureStore';
import { Paper, Grid, Avatar, Box, Divider, Button, Checkbox, FormControlLabel, TextField, IconButton } from '@mui/material';
import moment from 'moment';
import { useTheme } from '@mui/styles';
import Loading from './Loading';
import { Send } from '@mui/icons-material';
interface CommentProps {
    ideaId: string;
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
const Comment: React.FC<CommentProps> = ({ ideaId }) => {
    const dispatch = useDispatch();
    const classes = useStyles();
    const { user } = useAppSelector(state => state.account);
    const [text, setText] = useState('');
    const [connection, setConnection] = useState<any>(null);
    const [isAnonymous, setIsAnonymous] = useState(false);
    const [isLoading, setIsLoading] = useState(true);
    const comments = useSelector((state: RootState) => state.comment.comments);
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

    const sendComment = (comment: ChatComment) => {
        try {
            connection.invoke('SendComment', comment)
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
    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const comment: ChatComment = {
            ideaId: ideaId,
            username: user?.name,
            content: text,
            isAnonymous: isAnonymous,
        };
        sendComment(comment);
        setText('');
        setIsAnonymous(false);
    };
    return (
        <>
            {isLoading ? <Loading /> : (
                <Box paddingBottom={4}>
                    <form className={classes.form} onSubmit={handleSubmit}>
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
                                                disabled={!text.trim()}
                                            >
                                                <Send />
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
                    </form>
                    {
                        comments.map((item: any) => (
                            <Paper
                                sx={{ backgroundColor: theme.palette.comment.main }}
                                style={{ padding: "1rem 1rem 0rem" }}
                            >
                                <Grid container wrap="nowrap" spacing={2}>
                                    <Grid item>
                                        <Avatar alt="Profile Image" src={item.isAnonymous === false ? item.avatar : ''} />
                                    </Grid>
                                    <Grid item xs zeroMinWidth>
                                        <Grid container wrap="nowrap" spacing={2}>
                                            <Grid item xs zeroMinWidth>
                                                <Box fontSize="1.1rem" component="h4" justifyContent="left">
                                                    {item.isAnonymous === false ? item.username : 'Anonymous'}
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
                </Box>
            )}
        </>
    )
}

export default Comment