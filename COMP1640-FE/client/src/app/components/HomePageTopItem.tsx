import React from 'react';
import { useTheme } from '@emotion/react';
import { Grid, Typography } from '@mui/material';
import { Box } from '@mui/system';
import CategoryButton from './CategoryButton';
import PostAuthorInfo from './PostAuthorInfo';

interface HomePageTopItemProps {
  profileImage: any;
  thumbnail: any;
  content: any;
}

const HomePageTopItem = ({ profileImage, thumbnail, content }: HomePageTopItemProps) => {
  const theme: any = useTheme();
  return (
    <Box
      m="2rem"
      sx={{ backgroundColor: theme.palette.background.alt, p: "1.5rem", borderRadius: "0.55rem" }}
    >
      <Grid container columns={{ xs: 4, sm: 8, md: 12 }}>
        <Grid
          item xs={4.5}
          height="16rem"
          width="20rem"
        >
          <Box
            component="img"
            alt="profile"
            src={thumbnail}
            height="100%"
            width="100%"
            sx={{ objectFit: "cover", borderRadius: "0.5rem" }}
          />
        </Grid>
        <Grid item xs={7.5}>
          <Box m="0.5rem">
            <CategoryButton category="Anime" />
            <Box m="1rem">
              <Typography
                variant="h2"
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
                {content.length <= 200 ? content : content.substring(0, 200) + "..."}
              </Typography>
            </Box>
            <PostAuthorInfo profileImage={profileImage} />
          </Box>
        </Grid>
      </Grid>
    </Box>
  )
}
export default HomePageTopItem;