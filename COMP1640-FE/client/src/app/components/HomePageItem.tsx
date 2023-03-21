import React, { useEffect, useState } from 'react';
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
  const [content,setContent] = useState("");

  useEffect(() => {
    const regex = /(<([^>]+)>)/ig;
    const newString = data.content.replace(regex, '');
    setContent(newString);
  },[data])

  return (
    <Grid item xs={4} >
      <Box display="flex" alignContent="center" alignItems="center">
        <List>
          <Box display="flex" alignContent="center" alignItems="center">
            <ListItem
              sx={{
                display: "flex", alignContent: "center", alignItems: "center",
              }}>
              <Box
                sx={{
                  [theme.breakpoints.up('sm')]: {
                    height: "16vw",
                    width: "100%",
                  },
                  [theme.breakpoints.down('sm')]: {
                    height: "68vw",
                    width: "92vw",
                  },
                  backgroundColor: theme.palette.thumbnail.main, p: { xs: "3vw", sm: "0.75vw" }, borderRadius: "4rem 0rem"
                }}
              >
                <Link to={`/ideaDetail/${data.slug}`}>
                  <Box
                    component="img"
                    alt="thumbnail"
                    src={data.image}
                    sx={{
                      [theme.breakpoints.up('sm')]: {
                        height: "14.5vw",
                        width: "100%",
                      },
                      [theme.breakpoints.down('sm')]: {
                        height: "62vw",
                        width: "86vw",
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
          </Box>
          <ListItem>
            <Box>
              <CategoryButton search={false} category={data.category?.name} />
              <Link to={`/ideaDetail/${data.slug}`} style={{ textDecoration: 'none' }}>
                <Typography
                  mt="0.5rem"
                  variant="h3"
                  color={theme.palette.content.main}
                  fontWeight="bold"
                  textAlign="justify"
                  sx={{
                    '&:hover': { color: theme.palette.secondary.main },
                    overflow: "hidden",
                    textOverflow: "ellipsis",
                    display: "-webkit-box",
                    WebkitLineClamp: "2",
                    WebkitBoxOrient: "vertical",
                  }}
                >
                  {data.title}
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
                textAlign="justify"
                sx={{
                  overflow: "hidden",
                  textOverflow: "ellipsis",
                  display: "-webkit-box",
                  WebkitLineClamp: "4",
                  WebkitBoxOrient: "vertical",
                }}
              >
                {content}
              </Typography>
              <PostAuthorInfo
                avatar={data.user.avatar}
                userName={data.user.userName}
                lastUpdate={data.lastUpdate}
              />
            </Box>

          </ListItem>
        </List>
      </Box>
    </Grid >
  );
}

export default HomePageItem