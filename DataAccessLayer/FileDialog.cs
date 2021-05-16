using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;


namespace SWE2_TOURPLANNER.DataAccessLayer
{
    public class FileDialog
    {
        public FileDialog()
        {
        }

        public string SaveFileDialogFunc()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Save text Files",
                DefaultExt = "json",
                Filter = "All files (*.*)|*.*",
                FilterIndex = 1
            };
            saveFileDialog1.ShowDialog();

            return saveFileDialog1.FileName;
        }

        public string OpenFileDialogFunc()
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "Json files (*.json)|*.json",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != string.Empty)
            {
                return openFileDialog.FileName;
            }

            return "ERROR";
        }

        public string SavePdfDialogFunc()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Save text Files",
                DefaultExt = "pdf",
                Filter = "All files (*.*)|*.*",
                FilterIndex = 1
            };
            saveFileDialog1.ShowDialog();

            return saveFileDialog1.FileName;
        }
    }
}