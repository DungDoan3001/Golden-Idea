import React from "react"
import { Button, useTheme } from "@mui/material"
import { Box } from "@mui/system";

interface CategoryButtonProps {
  category: any;
}

const CategoryButton = ({ category }: CategoryButtonProps) => {
  const theme: any = useTheme();
  return (
    <Box mt="0.7rem">
      <Button
        sx={{
          backgroundColor: theme.palette.thumbnail.main,
          color: theme.palette.content.main,
          fontSize: "0.7rem",
          fontWeight: "bold",
          padding: "0.2rem 0rem",
          '&:hover': {
            backgroundColor: theme.palette.thumbnail.main,
            opacity: '0.7',
            cursor: 'pointer',
          }
        }}
      >
        {category}
      </Button >
    </Box>
  )
}

export default CategoryButton