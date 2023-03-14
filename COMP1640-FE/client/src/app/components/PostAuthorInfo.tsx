import { Grid } from '@mui/material';
import { Box } from '@mui/system';
import React from 'react';

interface PostAuthorInfoProps {
  data: any
  top: any
}

const PostAuthorInfo = ({ data, top }: PostAuthorInfoProps) => {

  return (
    <Grid container mt="1rem">
      <Grid item xs={2} md={(top) ? 1 : 2}>
        <Box
          component="img"
          alt="profile"
          src={data.user?.avatar}
          height="2.5rem"
          width="2.5rem"
          borderRadius="50%"
          sx={{ objectFit: "cover" }}
        />
      </Grid>
      <Grid pl="1rem" item xs={10} md={(top) ? 11 : 10}>
        <Box component="h4" m="0rem">By: {data.user?.userName}</Box>
        <Box component="small">{new Date(data.lastUpdate).toLocaleDateString('en-GB')}</Box>
      </Grid>
    </Grid>
  )

}

export default PostAuthorInfo