import React from "react"
import { Button, useTheme } from "@mui/material"

interface CategoryButtonProps {
  category: any;
}

const CategoryButton = ({ category }: CategoryButtonProps) => {
  const theme: any = useTheme();
  return (
    <Button
      sx={{
        backgroundColor: theme.palette.secondary.light,
        color: theme.palette.background.alt,
        fontSize: "0.75rem",
        fontWeight: "bold",
        padding: "0.35rem",
      }
      }
    >
      {category}
    </Button >
  )
}

export default CategoryButton