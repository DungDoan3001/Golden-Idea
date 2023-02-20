import React from 'react';
import Grid from '@mui/material/Grid';
import { Box, Typography, useTheme } from '@mui/material';
import CategoryButton from '../../app/components/CategoryButton';
import PostAuthorInfo from './PostAuthorInfo';

interface HomePageItemProps {
  profileImage: any;
  thumbnail: any;
  content: any;
}

const HomePageItem = ({ profileImage, thumbnail, content }: HomePageItemProps) => {
  const theme: any = useTheme();

  return (
    <Grid item xs={4}>
      <Box
        m="1rem"
        sx={{ backgroundColor: theme.palette.background.alt, p: "1.5rem", borderRadius: "0.55rem" }}
      >
        <Box
          component="img"
          alt="profile"
          src={thumbnail}
          height="100%"
          width="100%"
          sx={{ objectFit: "cover", borderRadius: "0.5rem" }}
        />
        <CategoryButton category="Anime" />
        <Typography
          variant="h3"
          color={theme.palette.secondary[100]}
          fontWeight="bold"
          sx={{ mb: "5px" }}
        >
          Title
        </Typography>
        <Typography
          variant="h5"
          color={theme.palette.secondary[300]}
        >
          {content.length <= 50 ? content : content.substring(0, 50) + "..."}
        </Typography>
        <PostAuthorInfo profileImage={profileImage} />
      </Box>
    </Grid>
  );
}

export default HomePageItem