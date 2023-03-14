import { LoadingButton } from '@mui/lab'
import { Box, Button, Checkbox, FormControl, FormControlLabel, FormLabel, Grid, Radio, RadioGroup, Typography, useTheme } from '@mui/material'
import { watchFile } from 'fs'
import React, { useEffect, useState } from 'react'
import { Controller, FieldValues, useForm } from 'react-hook-form'
import AppImageInput from '../../app/components/AppImageInput'
import AppSelect from '../../app/components/AppSelect'
import AppTextInput from '../../app/components/AppTextInput'
import { toast } from 'react-toastify'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';
import { Category } from '../../app/models/Category'
import './style.scss'
import axios from 'axios'
import { Configuration, CreateImageRequestSizeEnum, OpenAIApi } from 'openai'
import FileInput from '../../app/components/FileInput'
import TermsAndConditionsDialog from '../../app/components/TermsAndConditionsDialog'

//Configuration of OpenAIApi
const configuration = new Configuration({
    apiKey: process.env.REACT_APP_OPEN_API_KEY
});
const openAI = new OpenAIApi(configuration);

interface FileObject extends File {
    size: number;
}
interface Props {
    cancelEdit: () => void;
    categories: Category[];
}
export const validationSchema = yup.object({
    Title: yup.string().min(2),
    Content: yup.string().min(2),
    CategoryId: yup.mixed().required(),
    file: yup.mixed()
        .test('fileSize', 'File size too large', function (value) {
            if (!value) return true;
            const file = value as FileObject;
            return file.size <= 2 * 1024 * 1024;
        }).notRequired(),
    UploadFiles: yup.mixed()
})
const IdeaForm = ({ cancelEdit, categories }: Props) => {
    const theme: any = useTheme();
    const [uploadOption, setUploadOption] = useState('upload');
    const [image, setImage] = useState<string | undefined>();
    const [dialogOpen, setDialogOpen] = useState(false);
    const [termsAgreed, setTermsAgreed] = useState(false);

    const handleCheckboxClick = () => {
        setDialogOpen(true);
    }

    const handleDialogClose = () => {
        setDialogOpen(false);
    }

    const handleAgree = () => {
        setDialogOpen(false);
        setTermsAgreed(true);
    }
    const handleFileSelect = (files: FileList) => {
        // Handle the selected files here
        console.log('Selected Files:', files);
    };
    const modules = {
        toolbar: [
            [{ 'header': '1' }, { 'header': '2' }, { 'font': [] }],
            [{ size: [] }],
            ['bold', 'italic', 'underline', 'strike', 'blockquote'],
            [{ 'list': 'ordered' }, { 'list': 'bullet' },
            [{ 'script': 'super' }, { 'script': 'sub' }],
            { 'indent': '-1' }, { 'indent': '+1' }],
            ['direction', { 'align': [] }],
            ['link', 'image', 'video', 'formula'],
            [{ 'color': [] }, { 'background': [] }],
            ['clean']
        ],
    }
    const styles = {
        height: '200px',
        marginTop: '-20px',
    }

    const { control, reset, handleSubmit, formState: { isDirty, isSubmitting }, watch, setValue, register } = useForm({
        resolver: yupResolver<any>(validationSchema),
    });
    const watchFile = watch('file', null);
    useEffect(() => {
        if (!watchFile && !isDirty)
            return () => {
                if (watchFile) URL.revokeObjectURL(watchFile.preview);
            }
    }, [reset, watchFile, isDirty]);
    async function handleSubmitData(data: FieldValues) {
        try {
            console.log(data);
            //   let response: User;
            //   if (user) {
            //     response = await dispatch(updateUser(data)).unwrap();
            //   } else {
            //     response = await dispatch(addUser(data)).unwrap();
            //   }
            toast.success('Successfully', {
                position: toast.POSITION.TOP_RIGHT,
            });
            cancelEdit();
        } catch (error: any) {
            toast.error('Failed to load resource: the server responded with a status of 409 (Conflict)', {
                position: toast.POSITION.TOP_RIGHT,
            });
        }
    }
    const handleGenerateImage = async () => {
        const prompt = watch('Title');
        if (!prompt) return;

        const imageParameters = {
            model: 'image-alpha-001',
            prompt: prompt,
            num_images: 1,
            size: '512x512' as CreateImageRequestSizeEnum,
        }
        const response = await openAI.createImage(imageParameters);
        const imageUrl = response.data.data[0].url;
        // create a new file object from the generated image URL
        if (imageUrl) {
            const xhr = new XMLHttpRequest();
            xhr.open('GET', imageUrl, true);
            xhr.responseType = 'blob';
            xhr.onload = () => {
                if (xhr.status === 200) {
                    const generatedFile = new File([xhr.response], 'generated-image.jpg', { type: 'image/jpeg' });
                    // set the generated file in the form
                    setValue('file', generatedFile);
                }
            };
            xhr.send();
        }
        setImage(imageUrl);
    }
    return (
        <Box>
            <form onSubmit={handleSubmit(handleSubmitData)}>
                <Grid container spacing={0} sx={{
                    [theme.breakpoints.up('sm')]: { mt: 5, mb: 2, width: '100%', },
                    [theme.breakpoints.down('sm')]: { mt: 3, height: '50vh', width: '130%', ml: 5 }
                }}>
                    <Grid item xs={12} sm={6} px={2}>
                        <AppTextInput control={control} name='Title' label='Title' multiline={true} />
                    </Grid>
                    <Grid item xs={12} md={6} px={2}>
                        <FormControl>
                            <AppSelect control={control} name='CategoryId' label='Categories' items={categories} />
                        </FormControl>
                    </Grid>
                    <Grid item xs={12} md={12} px={2} pb={5}>
                        <ReactQuill
                            value={watch('Content')}
                            onChange={data => setValue('Content', data)}
                            theme='snow'
                            modules={modules}
                            placeholder='Enter content here'
                            style={styles}
                        />
                    </Grid>
                    <Typography variant='h6' marginLeft={2} marginTop={2}>Thumbnail Image:</Typography>
                    <RadioGroup row aria-label="image upload option" name="imageUploadOption" sx={{ [theme.breakpoints.up('sm')]: { mt: 1, ml: 5 } }}
                        value={uploadOption} onChange={(e) => setUploadOption(e.target.value)}>
                        <FormControlLabel value="upload" control={<Radio />} label="Upload Image" />
                        <FormControlLabel value="generate" control={<Radio />} label="Generate Image by title using OpenAI's DALL-E API" />
                    </RadioGroup>
                    {uploadOption === "upload" ? (
                        <Grid container item xs={12} sm={12} marginTop={3} display='flex' justifyContent='space-around' alignItems='center'>
                            <Grid item>
                                <AppImageInput control={control} name='file' />
                            </Grid>
                            <Grid item>
                                {watchFile ? (
                                    <img src={watchFile.preview} alt="preview" style={{ height: '100px', width: 150, marginTop: -15 }} />
                                ) : (
                                    <img src={''} alt={''} style={{ height: '100px', width: 150, marginTop: -15 }} />
                                )}
                            </Grid>
                        </Grid>
                    ) : (
                        <Grid container item xs={12} sm={12} marginTop={3} display='flex' justifyContent='space-around' alignItems='center'>
                            <Grid item>
                                <Button variant="contained" color="primary" type="button" onClick={handleGenerateImage}>Generate Image</Button>
                            </Grid>
                            <Grid item>
                                {image ? (
                                    <img src={image} alt="generated" style={{ height: '100px', width: 150, marginTop: -15 }} />
                                ) : (
                                    <img src={''} alt={''} style={{ height: '100px', width: 150, marginTop: -15 }} />
                                )}
                            </Grid>
                        </Grid>
                    )}
                    <Grid item xs={12} md={12} px={2} pb={4}>
                        <h3>Upload Files</h3>
                        <FileInput onFileSelect={handleFileSelect} name='FileList' />
                    </Grid>
                    <Grid item xs={12} md={12} px={2}>
                        <FormControlLabel
                            control={
                                <Checkbox
                                    {...register("IsAnonymous")}
                                    defaultChecked={false}
                                    color="info"
                                />
                            }
                            label="Post Idea as anonymous"
                        />
                    </Grid>
                    <Grid item xs={12} md={12} px={2}>
                        <FormControlLabel
                            control={
                                <Checkbox
                                    checked={termsAgreed}
                                    onChange={handleCheckboxClick}
                                    color="info"
                                />
                            }
                            label="I agree to the Terms and Conditions and Privacy Policy"
                        />
                        <TermsAndConditionsDialog
                            open={dialogOpen}
                            onClose={handleDialogClose}
                            onAgree={handleAgree}
                        />
                    </Grid>
                </Grid>
                <Box display='flex' justifyContent='space-between' sx={{ ml: 10, mt: 5, mb: 1, [theme.breakpoints.up('sm')]: { ml: 25 }, [theme.breakpoints.down('sm')]: { mt: 35 } }}>
                    <Button onClick={cancelEdit} variant='contained' color='inherit' sx={{ marginRight: '0.5rem' }}>Cancel</Button>
                    <LoadingButton loading={isSubmitting} type='submit' variant='contained' color='success' disabled={!termsAgreed}
                    >Submit</LoadingButton>
                </Box>
            </form>
        </Box>
    )
}

export default IdeaForm