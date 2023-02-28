import React from 'react';
import { useTheme } from '@emotion/react';
import { Grid, Typography } from '@mui/material';
import { Box } from '@mui/system';
import CategoryButton from './CategoryButton';
import PostAuthorInfo from './PostAuthorInfo';

interface HomePageTopItemProps {
  post: any
}

const HomePageTopItem = ({ post }: HomePageTopItemProps) => {
  const theme: any = useTheme();

  return (
    <Box>
      <Grid container columns={{ xs: 4, sm: 8, md: 12 }}>
        <Grid
          item xs={5}
          height="16rem"
          width="20rem"
          mb="2rem"
        >
          <Box
            sx={{ backgroundColor: theme.palette.thumbnail.main, p: "1rem", borderRadius: "4rem 0rem" }}
          >
            <Box
              component="img"
              alt="profile"
              src={post.img}
              height="100%"
              width="100%"
              sx={{
                objectFit: "cover",
                borderRadius: "4rem 0rem",
                '&:hover': {
                  backgroundColor: theme.palette.thumbnail.main,
                  opacity: '0.7',
                  cursor: 'pointer',
                }
              }}
            />
          </Box>
        </Grid>
        <Grid item xs={7}>
          <Box m="0.7rem">
            <CategoryButton search={false} category={post.catalog} />
            <Box>
              <Typography
                variant="h2"
                color={theme.palette.content.main}
                fontWeight="bold"
                sx={{ mb: "5px" }}
              >
                {post.title.length <= 50 ? post.title : post.title.substring(0, 50) + "..."}
              </Typography>
              <Typography
                variant="h5"
                color={theme.palette.content.main}
              >
                {post.content.length <= 200 ? post.content : post.content.substring(0, 200) + "..."}
              </Typography>
            </Box>
            <PostAuthorInfo top={true} post={post} />
          </Box>
        </Grid>
      </Grid>
    </Box>
  )
}
export default HomePageTopItem;