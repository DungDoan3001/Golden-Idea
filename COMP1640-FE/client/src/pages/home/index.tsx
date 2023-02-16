import { useState } from "react"
import ConfirmDialog from "../../app/components/ConfirmDialog"

const Home = () => {
  const [confirmDialog, setConfirmDialog] = useState({ isOpen: false, title: '', subTitle: '' })
  return (
    <div>index
      <button onClick={() => {
              setConfirmDialog({
              isOpen: true,
              title: 'Are you sure to delete this record?',
              subTitle: "You can't undo this operation",
              })
      }}>DEF</button>
      <button onClick={() => {
              setConfirmDialog({
                ...confirmDialog,
                isOpen: false
              })
      }}>ABC</button>
      <ConfirmDialog
                confirmDialog={confirmDialog}
                setConfirmDialog={setConfirmDialog}
      />
    </div>
  )
}

export default Home