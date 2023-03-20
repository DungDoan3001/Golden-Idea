import React from "react"
import { Box, IconButton } from '@mui/material';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import { useNavigate } from 'react-router-dom';
import { useTheme } from '@emotion/react';


const BackButton = () => {
  const theme: any = useTheme();
  const navigate = useNavigate();

  return (
    <Box p="1rem 0rem" fontSize="1rem" color={theme.palette.content.main}>
      <IconButton onClick={() => navigate(-1)}>
        <ArrowBackIosIcon
          style={{
            fontSize: "1.5rem",
            color: theme.palette.content.main,
            paddingRight: "0.5rem"
          }} />
        Back
      </IconButton>
    </Box>
  )
}

export default BackButton;