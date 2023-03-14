import React, { useEffect, useState } from 'react'
import { addComment, loadComments } from '../../pages/comment/commentSlice';
import { ChatComment } from '../models/Comment';
import { HttpTransportType, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useDispatch, useSelector } from 'react-redux';
import { RootState, store } from '../store/configureStore';
interface CommentProps {
    ideaId: string;
}

const Comment: React.FC<CommentProps> = ({ ideaId }) => {
    const dispatch = useDispatch();
    const [text, setText] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const comments = useSelector((state: RootState) => state.comment.comments);

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

    const handleTextChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setText(event.target.value);
    };
    return (
        <div>Comment</div>
    )
}

export default Comment