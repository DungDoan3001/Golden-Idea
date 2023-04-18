import { Box } from "@mui/material";
import DocViewer, { DocViewerRenderers } from "@cyntler/react-doc-viewer";
const FileRender = ({ fileUrl }: { fileUrl: string }) => {
    const docs = [
        { uri: fileUrl }, // Remote file
    ];
    return (
        <Box mt={5} sx={{ width: "100%", height: { xs: "500px", sm: "60rem" } }}>
            <DocViewer
                documents={docs}
                pluginRenderers={DocViewerRenderers}
                style={{ width: "100%", height: "100%" }}
            />
        </Box>
    );
};

export default FileRender;