using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormMergePDF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void DoMerge()
        {
            List<string> ret = new List<string>();

            foreach (string item in listBox1.Items)
            {
                ret.Add(item);
            }

            string[] files = ret.ToArray();

            const string filename = "Merged_PDF.pdf";

            if (File.Exists(filename))
                File.Delete(filename);

            MergeMultiplePDFIntoSinglePDF(filename, files);
        }

        private void DoSplit(string[] files)
        {
            // Get a fresh copy of the sample PDF file
            //string filename = "Portable Document Format.pdf";
            //File.Copy(Path.Combine("../../../../PDFs/", filename),
            //  Path.Combine(Directory.GetCurrentDirectory(), filename), true);

            string downloadPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + @"Downloads\Splitted_PDF\";

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            foreach (string file in files)
            {
                // Open the file
                PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                string name = Path.GetFileNameWithoutExtension(file);

                for (int i = 0; i < inputDocument.PageCount; i++)
                {
                    // Create new document
                    PdfDocument outputDocument = new PdfDocument();
                    outputDocument.Version = inputDocument.Version;
                    outputDocument.Info.Title =
                      String.Format("Page {0} of {1}", i + 1, inputDocument.Info.Title);
                    outputDocument.Info.Creator = inputDocument.Info.Creator;

                    // Add the page and save it
                    outputDocument.AddPage(inputDocument.Pages[i]);

                    string outputPath = String.Format("{0} - Page {1}.pdf", name, i + 1);

                    outputDocument.Save(downloadPath + outputPath);
                }

                Process.Start(downloadPath);
            }
        }

        //Get Filename
        //private string GetFileName(string path)
        //{
        //    return Path.GetFileName(path);
        //}


        /// <summary>
        /// Drag/Drop files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            //Take Dropped files and store in Array
            string[] droppedFiles = (string[]) e.Data.GetData(DataFormats.FileDrop);

            //Loop through all Dropped files and Display them
            foreach (string file in droppedFiles)
            {
                string AllowedExtension = Path.GetExtension(file);
                
                if (AllowedExtension.ToUpper() == ".PDF")
                {
                    //string fileName = Path.GetFileName(file);
                    //MessageBox.Show("You dropped in [\"" + fileName + "\"]");
                    listBox1.Items.Add(file);
                }
                else
                {
                    MessageBox.Show("Allowed Extension is PDF Files only...");
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private static void MergeMultiplePDFIntoSinglePDF(string outputFilePath, string[] pdfFiles)
        {
            outputFilePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + @"Downloads\Merged_PDF.pdf";

            //Console.WriteLine("Merging started.....");
            PdfDocument outputPDFDocument = new PdfDocument();
            foreach (string pdfFile in pdfFiles)
            {
                if (pdfFile.Contains("Merged_PDF"))
                {
                    continue;
                }

                PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);

                outputPDFDocument.Version = inputPDFDocument.Version;
                foreach (PdfPage page in inputPDFDocument.Pages)
                {
                    outputPDFDocument.AddPage(page);
                }
            }
            if (outputPDFDocument.PageCount == 0) throw new Exception($"Page Count is {outputPDFDocument.PageCount}");

            outputPDFDocument.Save(outputFilePath);
            
            Process.Start(outputFilePath);
        }

        private void merge_btn_Click(object sender, EventArgs e)
        {
            while (listBox1.Items.Count > 0)
            {
                DoMerge();
                return;
            }

            MessageBox.Show("The Item List is empty...");
        }

        private void remove_btn_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index == -1)
            {
                MessageBox.Show("Please select the item you wish to remove...");
                return;
            }

            listBox1.Items.RemoveAt(index);
        }

        private void clear_btn_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {   
                    string[] arrAllFiles = dialog.FileNames;

                    foreach (var item in arrAllFiles)
                    {
                        if (Path.GetExtension(item).ToUpper() == ".PDF")
                        {
                            listBox1.Items.Add(item);
                        }
                        else
                        {
                            MessageBox.Show("Allowed Extension is a PDF Files only...");
                        }
                    }

                    //string file = dialog.FileName;
                    //string AllowedExtension = Path.GetExtension(file);

                    //if (AllowedExtension.ToUpper() == ".PDF")
                    //{
                    //    //string fileName = Path.GetFileName(file);
                    //    //MessageBox.Show("You dropped in [\"" + fileName + "\"]");
                    //    listBox1.Items.Add(file);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Allowed Extension is a PDF Files only...");
                    //}
                }
            }
        }

        private void label1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string[] arrAllFiles = dialog.FileNames;

                    foreach (var item in arrAllFiles)
                    {
                        if (Path.GetExtension(item).ToUpper() == ".PDF")
                        {
                            listBox1.Items.Add(item);
                        }
                        else
                        {
                            MessageBox.Show("Allowed Extension is a PDF Files only...");
                        }
                    }
                }
            }
        }

        private void label3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string[] arrAllFiles = dialog.FileNames;

                    foreach (var item in arrAllFiles)
                    {
                        if (Path.GetExtension(item).ToUpper() == ".PDF")
                        {
                            listBox1.Items.Add(item);
                        }
                        else
                        {
                            MessageBox.Show("Allowed Extension is a PDF Files only...");
                        }
                    }
                }
            }
        }

        private void split_btn_Click(object sender, EventArgs e)
        {
            while (listBox1.Items.Count > 0)
            {
                string[] files = listBox1.Items.Cast<string>().ToArray();

                DoSplit(files);
                return;
            }

            MessageBox.Show("The Item List is empty...");
        }

        //FileInfo f = new FileInfo(listBox1.SelectedItem.ToString());
        //var ss = f.OpenRead();

        //                if (ss.Length > 0)
        //                {
        //                    axAcroPDF1.src = f.FullName;
        //                }
}
}
