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
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.DefaultExt = "json";
            saveFileDialog1.Filter = "All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.ShowDialog();

            return saveFileDialog1.FileName;
        }

        public string OpenFileDialogFunc()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Json files (*.json)|*.json";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != string.Empty)
            {
                return openFileDialog.FileName;
            }

            return "ERROR";
        }
    }
}