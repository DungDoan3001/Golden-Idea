import React from 'react';
import Grid from '@mui/material/Grid';
import { Box } from '@mui/material';
import HomePageItem from '../../app/components/HomePageItem';
import HomePageTopItem from '../../app/components/HomePageTopItem';

const Home = () => {
  const profileImage = require("../../app/assets/profile.jpeg");
  const thumbnail = require("../../app/assets/101.jpg");
  const content = "asdaskhxkcjb kdcjqbkbkqjka slnjoasduh voudhodcld ncuadohcaj scoasjcboasjbc oajcbadjcnkscjk du sczxcascasca aszxcasc asdazxcaqf wascaw ascafqw asawd awsdasqw ascawqwd awdwdw asdaskhxkcjb kdcjqbkbkqjka slnjoasduh voudhodcld ncuadohcaj scoasjcboasjbc oajcbadjcnkscjk du sczxcascasca aszxcasc asdazxcaqf wascaw ascafqw asawd awsdasqw ascawqwd awd asdaskhxkcjb kdcjqbkbkqjka slnjoasduh voudhodcld ncuadohcaj scoasjcboasjbc oajcbadjcnkscjk du sczxcascasca aszxcasc asdazxcaqf wascaw ascafqw asawd awsdasqw ascawqwd awdwdwwdw asdaskhxkcjb kdcjqbkbkqjka slnjoasduh voudhodcld ncuadohcaj scoasjcboasjbc oajcbadjcnkscjk du sczxcascasca aszxcasc asdazxcaqf wascaw ascafqw asawd awsdasqw ascawqwd awdwdw";
  return (
    <Box>
      <HomePageTopItem profileImage={profileImage} thumbnail={thumbnail} content={content} />
      <Grid container spacing={2} columns={{ xs: 4, sm: 8, md: 12 }}>
        {[1, 2, 3, 4, 5, 6, 7, 8, 9].map(() => (
          <HomePageItem profileImage={profileImage} thumbnail={thumbnail} content={content} />
        ))}
      </Grid>
    </Box>
  );
}

export default Home