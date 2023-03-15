import React from 'react';
import Grid from '@mui/material/Grid';
import { Box, Typography, useTheme } from '@mui/material';
import CategoryButton from '../../app/components/CategoryButton';
import PostAuthorInfo from './PostAuthorInfo';

import RemoveRedEyeTwoToneIcon from '@mui/icons-material/RemoveRedEyeTwoTone';
import ThumbsUpDownTwoToneIcon from '@mui/icons-material/ThumbsUpDownTwoTone';
import { Link } from 'react-router-dom';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';

interface HomePageItemProps {
  data: any
}

const HomePageItem = ({ data }: HomePageItemProps) => {
  const theme: any = useTheme();
  return (
    <Grid item xs={4} >
      <Box display="flex" alignContent="center" alignItems="center">
        <List>
          <ListItem>
            <Box
              sx={{
                [theme.breakpoints.up('sm')]: {
                  height: "13rem",
                  width: "17rem",
                },
                [theme.breakpoints.down('sm')]: {
                  height: "16rem",
                  width: "23rem",
                },
                backgroundColor: theme.palette.thumbnail.main, p: "0.75rem", borderRadius: "4rem 0rem"
              }}
            >
              <Link to={`/ideaDetail/${data.slug}`}>
                <Box
                  component="img"
                  alt="thumbnail"
                  src={data.image}
                  sx={{
                    [theme.breakpoints.up('sm')]: {
                      height: "11.5rem",
                      width: "15.5rem",
                    },
                    [theme.breakpoints.down('sm')]: {
                      height: "14.5rem",
                      width: "21.5rem",
                    },
                    objectFit: "cover",
                    borderRadius: "4rem 0rem",
                    '&:hover': {
                      backgroundColor: theme.palette.thumbnail.main,
                      opacity: '0.7',
                      cursor: 'pointer',
                    }
                  }}
                />
              </Link>
            </Box>
          </ListItem>
          <ListItem>
            <Box>
              <CategoryButton search={false} category={data.category?.name} />
              <Link to={`/ideaDetail/${data.slug}`} style={{ textDecoration: 'none' }}>
                <Typography
                  mt="0.5rem"
                  variant="h3"
                  color={theme.palette.content.main}
                  fontWeight="bold"
                  sx={{ '&:hover': { color: theme.palette.secondary.main } }}
                >
                  {data.title.length <= 50 ? data.title : data.title.substring(0, 50) + "..."}
                </Typography>
              </Link>
              <Box component="line" display="flex" alignItems="center" justifyContent="left">
                <Box component="line" display="flex" alignItems="center" justifyContent="left">
                  <RemoveRedEyeTwoToneIcon style={{ fontSize: "1.1rem", color: theme.palette.content.icon }} />
                  <Box pl="0.5rem">
                    {data.view}
                  </Box>
                </Box>
                <Box ml="2rem" component="line" display="flex" alignItems="center" justifyContent="left">
                  <ThumbsUpDownTwoToneIcon style={{ fontSize: "1.1rem", color: theme.palette.content.icon }} />
                  <Box pl="0.5rem">
                    {data.upVote} | {data.downVote}
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

          </ListItem>
        </List>
      </Box>
    </Grid >
  );
}

export default HomePageItem