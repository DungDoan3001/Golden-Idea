import React from 'react';
import Grid from '@mui/material/Grid';
import { Box, Typography, useTheme } from '@mui/material';
import CategoryButton from '../../app/components/CategoryButton';
import PostAuthorInfo from './PostAuthorInfo';

interface HomePageItemProps {
  post:any
}

const HomePageItem = ({ post }: HomePageItemProps) => {
  const theme: any = useTheme();

  return (
    <Grid item xs={4}>
      <Box>
        <Box
          alignItems="center"
          sx={{
            backgroundColor: theme.palette.thumbnail.main, p: "1rem", borderRadius: "2rem 0rem"
          }}
        >
          <Box
            component="img"
            alt="thumbnail"
            src={post.img}
            height="100%"
            width="100%"
            sx={{
              objectFit: "cover",
              borderRadius: "2rem 0rem",
              '&:hover': {
                backgroundColor: theme.palette.thumbnail.main,
                opacity:'0.7',
                cursor: 'pointer',
              }
            }}
          />
        </Box>
        <CategoryButton search={false} category={post.catalog} />
        <Typography
          mt="0.5rem"
          variant="h3"
          color={theme.palette.content.main}
          fontWeight="bold"
        >
          {post.title.length <= 50 ? post.title : post.title.substring(0, 50) + "..."}
        </Typography>
        <Typography
          variant="h5"
          color={theme.palette.content.main}
          fontSize="1rem"
        >
          {post.content.length <= 100 ? post.content : post.content.substring(0, 100) + "..."}
        </Typography>
        <PostAuthorInfo top={false} post={post} />
      </Box>
    </Grid>
  );
}

export default HomePageItem