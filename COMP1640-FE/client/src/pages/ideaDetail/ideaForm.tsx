import { LoadingButton } from '@mui/lab'
import { Box, Button, Checkbox, FormControl, FormControlLabel, FormLabel, Grid, Radio, RadioGroup, Typography, useTheme } from '@mui/material'
import { useEffect, useState } from 'react'
import { FieldValues, useForm } from 'react-hook-form'
import AppImageInput from '../../app/components/AppImageInput'
import AppSelect from '../../app/components/AppSelect'
import AppTextInput from '../../app/components/AppTextInput'
import { toast } from 'react-toastify'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';
import './style.scss'
import axios from 'axios'
import { Configuration, CreateImageRequestResponseFormatEnum, CreateImageRequestSizeEnum, OpenAIApi } from 'openai'
import FileInput from '../../app/components/FileInput'
import TermsAndConditionsDialog from '../../app/components/TermsAndConditionsDialog'
import { RootState, useAppDispatch, useAppSelector } from '../../app/store/configureStore'
import { useNavigate, useParams } from 'react-router-dom';
import { useSelector } from 'react-redux'
import { getCategories } from '../category/categorySlice'

//Configuration of OpenAIApi
const configuration = new Configuration({
  apiKey: process.env.REACT_APP_OPEN_API_KEY
});
const openAI = new OpenAIApi(configuration);

interface FileObject extends File {
  size: number;
}
// interface Props {
//   idea: any;
//   cancelEdit: () => void;
//   id: any;
// }
export const validationSchema = yup.object({
  Title: yup.string().min(2),
  Content: yup.string().min(2),
  CategoryId: yup.mixed().required(),
  File: yup.mixed()
    .test('fileSize', 'File size too large', function (value) {
      if (!value) return true;
      const file = value as FileObject;
      return file.size <= 2 * 1024 * 1024;
    }).notRequired(),
  ListFile: yup.mixed()
})
const IdeaForm = () => {
  const theme: any = useTheme();
  const navigate = useNavigate();
  const { topicId } = useParams();
  const { user } = useAppSelector(state => state.account);
  const { categories } = useSelector((state: RootState) => state.category);

  const dispatch = useAppDispatch();
  const [uploadOption, setUploadOption] = useState('upload');
  const [image, setImage] = useState<string | undefined>();
  const [dialogOpen, setDialogOpen] = useState(false);
  const [termsAgreed, setTermsAgreed] = useState(false);

  let fetchMount = true;
  useEffect(() => {
    if (fetchMount) {
      dispatch(getCategories());
    }
    return () => {
      fetchMount = false;
    };
  }, []);

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
  const handleFileUpload = (files: File[]) => {
    setValue('ListFile', files);
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
  const watchFile = watch('File', null);
  useEffect(() => {
    if (!watchFile && !isDirty)
      return () => {
        if (watchFile) URL.revokeObjectURL(watchFile.preview);
      }
  }, [reset, watchFile, isDirty]);

  function delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  async function handleSubmitData(data: FieldValues) {
    try {
      setValue('Username', user?.name);
      setValue('TopicId', topicId);
      await axios({
        method: "post",
        url: `https://goldenidea.azurewebsites.net/api/ideas`,
        data: data,
        headers: { "Content-Type": "multipart/form-data" },
      });
      toast.success('Successfully', {
        position: toast.POSITION.TOP_RIGHT,
      });
    } catch (error: any) {
      console.log(error);
      toast.error('Failed to load resource: the server responded with a status of 409 (Conflict)', {
        position: toast.POSITION.TOP_RIGHT,
      });
    }
    delay(1000);
    navigate(-1)
  }

  const handleGenerateImage = async () => {
    const prompt = watch('Title');
    if (!prompt) return;

    const imageParameters = {
      model: 'image-alpha-001',
      prompt: prompt,
      num_images: 1,
      size: '512x512' as CreateImageRequestSizeEnum,
      response_format: 'b64_json' as CreateImageRequestResponseFormatEnum,
    };

    try {
      const response = await openAI.createImage(imageParameters);
      const imageBase64 = response.data.data[0].b64_json;
      let imageUrl;
      if (imageBase64) {
        const byteCharacters = atob(imageBase64);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const timestamp = new Date().getTime();
        const imageBlob = new Blob([byteArray], { type: 'image/png' });
        const imageName = `${timestamp}-generated-image.png`;; // replace with desired file name
        const imageFile = new File([imageBlob], imageName, { type: 'image/png' }); // replace with desired file type
        setValue("File", imageFile);
        imageUrl = URL.createObjectURL(imageBlob);
      }
      setImage(imageUrl);
      console.log(imageUrl);
    } catch (error) {
      console.error(error);
    }
  };
  return (
    <Box sx={{
      [theme.breakpoints.up('sm')]: {
        width: '100%',
      },
      [theme.breakpoints.down('sm')]: {
        width: '20rem',
        m: "1rem 2rem",
      },
    }}>
      <form onSubmit={handleSubmit(handleSubmitData)}>
        <Grid container spacing={0} sx={{
          [theme.breakpoints.up('sm')]: { mt: 5, mb: 2, width: '100%', },
          [theme.breakpoints.down('sm')]: { mt: 3, width: '130%', ml: 5 }
        }}>
          <Grid item xs={11} sm={6} px={2}>
            <AppTextInput control={control} name='Title' label='Title' multiline={true} />
          </Grid>
          <Grid item xs={11} md={6} px={2}>
            <FormControl>
              <AppSelect control={control} name='CategoryId' label='Categories' items={categories} />
            </FormControl>
          </Grid>
          <Grid item xs={11} md={12} px={2} pb={5}>
            <ReactQuill
              value={watch('Content')}
              onChange={data => setValue('Content', data)}
              theme='snow'
              modules={modules}
              placeholder='Enter content here'
              style={styles}
            />
          </Grid>
          <Box pt="3rem">
            <Typography variant='h6' marginLeft={2} marginTop={2}>Thumbnail Image:</Typography>
            <RadioGroup row aria-label="image upload option" name="imageUploadOption" sx={{ [theme.breakpoints.up('sm')]: { mt: 1, ml: 5 } }}
              value={uploadOption} onChange={(e) => setUploadOption(e.target.value)}>
              <FormControlLabel value="upload" control={<Radio />} label="Upload Image" />
              <FormControlLabel value="generate" control={<Radio />} label="Generate Image by title using OpenAI's DALL-E API" />
            </RadioGroup>
          </Box>
          {uploadOption === "upload" ? (
            <Grid container item xs={12} sm={12} marginTop={3} display='flex' justifyContent='space-around' alignItems='center'>
              <Grid item>
                <AppImageInput control={control} name='File' />
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
                <Button variant="contained" color="primary" onClick={handleGenerateImage}>Generate Image</Button>
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
            <FileInput onChange={handleFileUpload} name='ListFile' maxFiles={5} />
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
        <Box p="2rem 0rem" display='flex' justifyContent='space-between' sx={{ ml: 10, mt: 5, mb: 1, [theme.breakpoints.up('sm')]: { ml: 25 }, [theme.breakpoints.down('sm')]: { mt: 35 } }}>
          <Button onClick={() => navigate(-1)} variant='contained' color='inherit' sx={{ marginRight: '0.5rem' }}>Cancel</Button>
          <LoadingButton loading={isSubmitting} type='submit' variant='contained' color='success' disabled={!termsAgreed}
          >Submit</LoadingButton>
        </Box>
      </form>
    </Box>
  )
}

export default IdeaForm