import React from 'react';
import { useTheme } from '@emotion/react';
import { Grid, Typography } from '@mui/material';
import { Box } from '@mui/system';
import CategoryButton from './CategoryButton';
import PostAuthorInfo from './PostAuthorInfo';

import RemoveRedEyeTwoToneIcon from '@mui/icons-material/RemoveRedEyeTwoTone';
import ThumbsUpDownTwoToneIcon from '@mui/icons-material/ThumbsUpDownTwoTone';
interface HomePageTopItemProps {
  data: any
}

const HomePageTopItem = ({ data }: HomePageTopItemProps) => {
  const theme: any = useTheme();

  return (
    <Box>
      <Box mb="2rem">
        <Typography
          variant="h1"
          color={theme.palette.content.main}
          fontWeight="bold"
          sx={{ mb: "5px" }}
        >
          sample-topic-1
        </Typography>
      </Box>
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
              src={data.img}
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
            <CategoryButton search={false} category={data.catalog} />
            <Box>
              <Typography
                m="0.5rem 0rem"
                variant="h2"
                color={theme.palette.content.main}
                fontWeight="bold"
                sx={{ mb: "5px" }}
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
                m="0.5rem 0rem"
                variant="h5"
                color={theme.palette.content.main}
              >
                {data.content.length <= 200 ? data.content : data.content.substring(0, 200) + "..."}
              </Typography>
            </Box>
            <Box m="0.5rem 0rem">
              <PostAuthorInfo top={true} data={data} />
            </Box>
          </Box>
        </Grid>
      </Grid>
    </Box>
  )
}
export default HomePageTopItem;