import React, { useEffect, useState } from 'react';
import Grid from '@mui/material/Grid';
import { Box } from '@mui/material';
import HomePageItem from '../../app/components/HomePageItem';
import HomePageTopItem from '../../app/components/HomePageTopItem';
import AppPagination from '../../app/components/AppPagination';
import Service from '../../app/utils/Service';

const Home = () => {
  const [post, setPost] = useState([]);
  const [topPost, setTopPost] = useState([]);

  useEffect(() => {
    Service.getData(0, 1).then((response: any) => {
      setTopPost(response.data)

      console.log(response);
    })
  }, []);

  return (
    <Box m="4rem" alignItems="center" justifyContent="center">
      {topPost.map((topPost: any) => (
        <HomePageTopItem post={topPost} />
      )
      )}
      <Box mt="10%" alignItems="center" justifyContent="center">
        <Grid container spacing={2} columns={{ xs: 4, sm: 8, md: 12 }}>
          {post.map((post: any) => (
            <HomePageItem post={post} />
          )
          )}
        </Grid>
        <AppPagination
          setPost={(p: any) => setPost(p)}
        />
      </Box>
    </Box>
  );
}

export default Home