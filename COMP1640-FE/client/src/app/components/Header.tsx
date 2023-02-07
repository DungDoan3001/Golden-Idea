import { Typography, Box, useTheme } from "@mui/material";
import React from "react";
interface HeaderProps{
    title : any; 
    subtitle: any
}
const Header = ({ title, subtitle }:HeaderProps) => {
  const theme: any = useTheme();
  return (
    <Box>
      <Typography
        variant="h2"
        color={theme.palette.secondary[100]}
        fontWeight="bold"
        sx={{ mb: "5px" }}
      >
        {title}
      </Typography>
      <Typography variant="h5" color={theme.palette.secondary[300]}>
        {subtitle}
      </Typography>
    </Box>
  );
};

export default Header;