import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography } from '@mui/material';

export default function TermsAndConditionsDialog(props: { open: any; onClose: any; onAgree: any; }) {
  const { open, onClose, onAgree } = props;

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle variant="h2" sx={{ textAlign: "center", fontWeight: "bold" }}>
        Terms and Conditions
      </DialogTitle>
      <DialogContent>
        <Typography>
          Welcome to our website. If you continue to browse and use this
          website, you are agreeing to comply with and be bound by the
          following terms and conditions of use, which together with our
          privacy policy govern our relationship with you in relation to this
          website. If you disagree with any part of these terms and conditions,
          please do not use our website.
        </Typography>
        <Typography variant="subtitle1" sx={{ fontWeight: "bold" }}>
          1. Website Use
        </Typography>
        <Typography>
          The term "us" or "we" refers to the owner of the website whose
          registered office is located at VietNam. The term "you" refers to the
          user or viewer of our website.
        </Typography>
        <Typography>
          The use of this website is subject to the following terms of use:
        </Typography>
        <Typography variant="subtitle2" sx={{ fontWeight: "bold" }}>
          1.1 Content
        </Typography>
        <Typography>
          The content of the pages of this website is for your general
          information and use only. It is subject to change without notice.
        </Typography>
        <Typography variant="subtitle2" sx={{ fontWeight: "bold" }}>
          1.2 Cookies
        </Typography>
        <Typography>
          This website uses cookies to monitor browsing preferences. If you do
          allow cookies to be used, the following personal information may be
          stored by us for use by third parties: OpenAI, ALanAI.
        </Typography>
        <Typography variant="subtitle1" sx={{ fontWeight: "bold" }}>
          2. Disclaimers
        </Typography>
        <Typography>
          Neither we nor any third parties provide any warranty or guarantee as
          to the accuracy, timeliness, performance, completeness or suitability
          of the information and materials found or offered on this website for
          any particular purpose. You acknowledge that such information and
          materials may contain inaccuracies or errors and we expressly exclude
          liability for any such inaccuracies or errors to the fullest extent
          permitted by law.
        </Typography>
        <Typography variant="subtitle1" sx={{ fontWeight: "bold" }}>
          3. Limitations of Liability
        </Typography>
        <Typography>
          Your use of any information or materials on this website is entirely
          at your own risk, for which we shall not be liable. It shall be your
          own responsibility to ensure that any products, services or information
          available through this website meet your specific requirements.
        </Typography>
        <Typography variant="subtitle1" sx={{ fontWeight: "bold" }}>
          4. Intellectual Property
        </Typography>
        <Typography>
          This website contains material which is owned by or licensed to us.
          This material includes, but is not limited to, the design, layout,
          look, appearance and graphics. Reproduction is prohibited other than in
          accordance with the copyright notice, which forms part of these terms
          and conditions.
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} variant='text' color='error'>Cancel</Button>
        <Button onClick={onAgree} variant='text' color='info'>Agree</Button>
      </DialogActions>
    </Dialog>
  );
}