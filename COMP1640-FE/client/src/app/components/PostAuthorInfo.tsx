import { Grid } from '@mui/material';
import { Box } from '@mui/system';
import React from 'react';

interface PostAuthorInfoProps {
  post: any
  top: any
}

const PostAuthorInfo = ({ post,top }: PostAuthorInfoProps) => {

  return (
    <Grid container mt="1rem">
      <Grid item xs={2} md={(top)?1:2}>
        <Box
          component="img"
          alt="profile"
          src={post.userImg}
          height="2.5rem"
          width="2.5rem"
          borderRadius="50%"
          sx={{ objectFit: "cover" }}
        />
      </Grid>
      <Grid pl="1rem" item xs={10} md={(top)?11:10}>
        <Box component="h5" m="0rem">By: {post.userId}</Box>
        <Box component="small">{post.lastUpdate}</Box>
      </Grid>
    </Grid>
  )

}

export default PostAuthorInfo