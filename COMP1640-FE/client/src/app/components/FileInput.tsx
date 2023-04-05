import { Delete, AddBox, Description, Image, InsertDriveFile, PictureAsPdf } from '@mui/icons-material';
import { Avatar, IconButton } from '@mui/material';
import { styled } from '@mui/material/styles';
import React, { useState } from 'react';
import { toast } from 'react-toastify';

const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5 MB in bytes

const FileUploaderContainer = styled('div')({
    display: 'flex',
    flexDirection: 'column',
    gap: '8px',
    width: '100%',
});

const FileInputWrapper = styled('div')({
    display: 'flex',
    alignItems: 'center',
    gap: '8px',
});

const UploadedFilesList = styled('div')({
    display: 'flex',
    flexDirection: 'column',
    gap: '8px',
});

const UploadedFileItem = styled('div')({
    display: 'flex',
    alignItems: 'center',
    gap: '8px',
    backgroundColor: '#F5F5F5',
    borderRadius: '4px',
    padding: '8px',
});

interface FileInputProps {
    onChange: (files: any) => void;
    name: string;
    maxFiles: number;
}
export const getFileIcon = (fileName: string) => {
    const ext = fileName.split('.').pop()?.toLowerCase();
    switch (ext) {
        case 'png':
        case 'jpg':
            return <Image />;
        case 'pdf':
            return <PictureAsPdf />;
        case 'doc':
        case 'docx':
            return <Description />;
        case 'xlsx':
        case 'txt':
        case 'csv':
            return <InsertDriveFile />;
        default:
            return <InsertDriveFile />;
    }
};
const FileInput: React.FC<FileInputProps> = ({ onChange, maxFiles }) => {
    const [uploadedFiles, setUploadedFiles] = useState<File[]>([]);
    const [fileLimit, setFileLimit] = useState<boolean>(false);

    const handleUploadFiles = (files: File[]) => {
        const uploaded = [...uploadedFiles];
        let limitExceeded = false;
        files.some((file) => {
            if (uploaded.findIndex((f) => f.name === file.name) === -1) {
                if (file.size > MAX_FILE_SIZE) {
                    toast.error(`${file.name} exceeds the maximum file size of 5MB.`);
                    return true;
                }
                uploaded.push(file);
                if (uploaded.length === maxFiles) setFileLimit(true);
                if (uploaded.length > maxFiles) {
                    toast.error(`You can only add a maximum of ${maxFiles} files.`);
                    setFileLimit(false);
                    limitExceeded = true;
                    return true;
                }
            }
        });
        if (!limitExceeded) {
            setUploadedFiles(uploaded);
            onChange(uploaded);
        }
    };

    const handleFileEvent = (e: React.ChangeEvent<HTMLInputElement>) => {
        const chosenFiles = Array.prototype.slice.call(e.target.files);
        handleUploadFiles(chosenFiles);
    };

    const handleRemoveFile = (index: number) => {
        const uploaded = [...uploadedFiles];
        uploaded.splice(index, 1);
        setUploadedFiles(uploaded);
        if (uploaded.length < maxFiles) {
            setFileLimit(false);
        }
    };
    return (
        <FileUploaderContainer>
            <UploadedFilesList>
                {uploadedFiles.map((file, index) => (
                    <UploadedFileItem key={file.name}>
                        <Avatar>
                            {getFileIcon(file.name)}
                        </Avatar>
                        <div className="file-name">
                            {file.name}
                        </div>
                        <IconButton color="error" onClick={() => handleRemoveFile(index)}>
                            <Delete />
                        </IconButton>
                    </UploadedFileItem>
                ))}
            </UploadedFilesList>
            <FileInputWrapper>
                <label htmlFor="fileUpload">
                    <IconButton color="info" size='large' component="span">
                        <AddBox />
                    </IconButton>
                </label>
                <input
                    id="fileUpload"
                    type="file"
                    multiple
                    accept=".pdf,.png,.doc,.docx,.txt,.xlsx,.jpg,.csv"
                    onChange={handleFileEvent}
                    style={{ display: 'none' }}
                />
                <span>{uploadedFiles.length} files uploaded</span>
            </FileInputWrapper>
        </FileUploaderContainer>
    );
};

export default FileInput;