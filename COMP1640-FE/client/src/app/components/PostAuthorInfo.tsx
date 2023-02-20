import { Grid } from '@mui/material';
import { Box } from '@mui/system';
import React from 'react';

interface PostAuthorInfoProps {
  profileImage: any;
}

const PostAuthorInfo = ({ profileImage }: PostAuthorInfoProps) => {

  return (
    <Grid container ml="0rem">
      <Grid item xs={2} md={2}>
        <Box
          component="img"
          alt="profile"
          src={profileImage}
          height="2.5rem"
          width="2.5rem"
          borderRadius="50%"
          sx={{ objectFit: "cover" }}
        />
      </Grid>
      <Grid item xs={10} md={10}>
        <Box component="h5" m="0">By: Name</Box>
        <Box component="small">Datetime</Box>
      </Grid>
    </Grid>
  )

}

export default PostAuthorInfo