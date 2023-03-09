import React from 'react';
import Grid from '@mui/material/Grid';
import { Box, Typography, useTheme } from '@mui/material';
import CategoryButton from '../../app/components/CategoryButton';
import PostAuthorInfo from './PostAuthorInfo';

import RemoveRedEyeTwoToneIcon from '@mui/icons-material/RemoveRedEyeTwoTone';
import ThumbsUpDownTwoToneIcon from '@mui/icons-material/ThumbsUpDownTwoTone';

interface HomePageItemProps {
  data: any
}

const HomePageItem = ({ data }: HomePageItemProps) => {
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
            src={data.img}
            height="100%"
            width="100%"
            sx={{
              objectFit: "cover",
              borderRadius: "2rem 0rem",
              '&:hover': {
                backgroundColor: theme.palette.thumbnail.main,
                opacity: '0.7',
                cursor: 'pointer',
              }
            }}
          />
        </Box>
        <CategoryButton search={false} category={data.catalog} />
        <Typography
          mt="0.5rem"
          variant="h3"
          color={theme.palette.content.main}
          fontWeight="bold"
        >
          {data.title.length <= 50 ? data.title : data.title.substring(0, 50) + "..."}
        </Typography>
        <Box component="line" display="flex" alignItems="center" justifyContent="left">
          <Box component="line" display="flex" alignItems="center" justifyContent="left">
            <RemoveRedEyeTwoToneIcon style={{ fontSize: "1.1rem", color: theme.palette.content.icon }} />
            <Box pl="0.5rem">
              20
            </Box>
          </Box>
          <Box ml="2rem" component="line" display="flex" alignItems="center" justifyContent="left">
            <ThumbsUpDownTwoToneIcon style={{ fontSize: "1.1rem", color: theme.palette.content.icon }} />
            <Box pl="0.5rem">
              20 | 10
            </Box>
          </Box>
        </Box>
        <Typography
          mt="0.5rem"
          variant="h5"
          color={theme.palette.content.main}
          fontSize="1rem"
        >
          {data.content.length <= 100 ? data.content : data.content.substring(0, 100) + "..."}
        </Typography>
        <PostAuthorInfo top={false} data={data} />
      </Box>
    </Grid>
  );
}

export default HomePageItem