import { Delete } from '@mui/icons-material';
import { IconButton } from '@mui/material';
import React, { useState } from 'react';

const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5 MB in bytes
const MAX_FILES = 5;

interface FileInputProps {
    onFileSelect: (files: FileList) => void;
}

const FileInput: React.FC<FileInputProps> = ({ onFileSelect }) => {
    const [selectedFiles, setSelectedFiles] = useState<File[]>([]);
    const [errorMessage, setErrorMessage] = useState<string>('');
    const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newFiles = event.target.files;
        if (!newFiles) {
            return;
        }
        const numFiles = newFiles.length;
        if (selectedFiles.length + numFiles > MAX_FILES) {
            setErrorMessage(`You can only upload up to ${MAX_FILES} files.`);
            return;
        }
        const oversizedFiles: string[] = [];
        const validFiles: File[] = [];
        for (let i = 0; i < numFiles; i++) {
            const file = newFiles.item(i);
            if (!file) {
                continue;
            }
            if (file.size > MAX_FILE_SIZE) {
                oversizedFiles.push(file.name);
            } else {
                validFiles.push(file);
            }
        }
        if (oversizedFiles.length > 0) {
            setErrorMessage(`The following files are too large: ${oversizedFiles.join(', ')}.`);
            return;
        }
        setErrorMessage('');
        setSelectedFiles([...selectedFiles, ...validFiles]);
        onFileSelect(newFiles);
    };
    const handleRemoveFile = (index: number) => {
        const newSelectedFiles = [...selectedFiles];
        newSelectedFiles.splice(index, 1);
        setSelectedFiles(newSelectedFiles);
    };
    return (
        <div>
            <div>
                <input type="file" accept=".doc,.docx,.pdf,.zip" multiple onChange={handleFileSelect} />
                {errorMessage && <div style={{ color: 'red' }}>{errorMessage}</div>}
            </div>
            {selectedFiles.length > 0 && (
                <div>
                    <h3>Selected Files:</h3>
                    <ul>
                        {selectedFiles.map((file, index) => (
                            <li key={file.name}>
                                {file.name} ({file.size / 1024} KB)
                                <IconButton aria-label="delete" size="large" color="error" onClick={() => handleRemoveFile(index)}>
                                    <Delete fontSize="inherit" />
                                </IconButton>
                            </li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default FileInput;