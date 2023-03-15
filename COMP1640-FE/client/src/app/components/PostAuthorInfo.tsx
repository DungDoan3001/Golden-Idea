import { Grid } from '@mui/material';
import { Box } from '@mui/system';
import React from 'react';

interface PostAuthorInfoProps {
  avatar: any
  userName: any
  lastUpdate: any
}

const PostAuthorInfo = ({ avatar, userName, lastUpdate }: PostAuthorInfoProps) => {

  return (
    <Grid container mt="1rem">
      <Grid item xs={2} md={2}>
        <Box
          component="img"
          alt="profile"
          src={avatar}
          height="2.5rem"
          width="2.5rem"
          borderRadius="50%"
          sx={{ objectFit: "cover" }}
        />
      </Grid>
      <Grid pl="1rem" item xs={10} md={10}>
        <Box component="h4" m="0rem">By: {userName}</Box>
        <Box component="small">{new Date(lastUpdate).toLocaleDateString('en-GB')}</Box>
      </Grid>
    </Grid>
  )

}

export default PostAuthorInfo