using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.App
{
    class Forms
    {
        public enum eIconType
        {
            eWarning,
            eExclaimation,
            eError
        };

        public static System.Windows.Forms.Form parentForm = null;

        public static void MessageBox(string message, string title, eIconType iconType)
        {
            System.Windows.Forms.Form form1 = new System.Windows.Forms.Form();
            System.Windows.Forms.Button button1 = new System.Windows.Forms.Button();
            System.Windows.Forms.Button button2 = new System.Windows.Forms.Button();
            System.Windows.Forms.PictureBox imageStyle = new System.Windows.Forms.PictureBox();
            System.Windows.Forms.TextBox content = new System.Windows.Forms.TextBox();

            if (iconType == eIconType.eError)
            {
                imageStyle.Image = MovieRip.Properties.Resources.error;
                imageStyle.Size = MovieRip.Properties.Resources.error.Size;

                content.Text = "PROGRAM ERROR!\r\n\r\n************** Exception Text **************\r\n";
            }
            else if (iconType == eIconType.eWarning)
            {
                imageStyle.Image = MovieRip.Properties.Resources.warning;
                imageStyle.Size = MovieRip.Properties.Resources.warning.Size;

                content.Text = "PROGRAM WARNING!\r\n\r\n************** Exception Text **************\r\n";
            }
            else if (iconType == eIconType.eExclaimation)
            {
                imageStyle.Image = MovieRip.Properties.Resources.exclaimation;
                imageStyle.Size = MovieRip.Properties.Resources.exclaimation.Size;

                content.Text = "IMPORTANT NOTICE!\r\n\r\n************** Exception Text **************\r\n";
            }

            form1.Width = 500;

            imageStyle.Location = new System.Drawing.Point(10, 20);

            content.Multiline = true;
            content.ReadOnly = true;
            content.Text += message;
            content.Location = new System.Drawing.Point(imageStyle.Location.X + imageStyle.Size.Width + 10, imageStyle.Location.Y);
            content.Size = new System.Drawing.Size(form1.Size.Width - imageStyle.Size.Width - 40, form1.Height - 100);
            content.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            button1.Text = "OK";
            button1.Location = new System.Drawing.Point(content.Location.X, content.Location.Y + content.Size.Height + 10);
            button1.DialogResult = System.Windows.Forms.DialogResult.OK;

            button2.Text = "Cancel";
            button2.Location = new System.Drawing.Point(button1.Location.X + button1.Size.Width + 8, button1.Location.Y);
            button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            form1.MaximizeBox = false;
            form1.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            form1.Icon = System.Drawing.SystemIcons.Exclamation;
            form1.ShowIcon = false;
            form1.ShowInTaskbar = false;
            form1.Text = title;
            form1.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            form1.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            form1.AcceptButton = button1;
            form1.CancelButton = button2;
            form1.Location = new System.Drawing.Point(
                (parentForm.Location.X + parentForm.Size.Width / 2) - form1.Size.Width / 2,
                (parentForm.Location.Y + parentForm.Size.Height / 2) - form1.Size.Height / 2);

            form1.Controls.Add(button1);
            form1.Controls.Add(button2);
            form1.Controls.Add(imageStyle);
            form1.Controls.Add(content);

            form1.ShowDialog();
            form1.Dispose(); //OK and CANCEL don't really do much...
        }


        public static string qualitySelector(List<string> options)
        {
            System.Windows.Forms.Form qform = new System.Windows.Forms.Form();
            System.Windows.Forms.Button selectButton = new System.Windows.Forms.Button();
            System.Windows.Forms.ListBox optionsList = new System.Windows.Forms.ListBox();

            for (int i = 0; i < options.Count; i++)
            {
                optionsList.Items.Add(options[i]);
            }

            qform.MaximizeBox = false;
            qform.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            qform.Icon = System.Drawing.SystemIcons.Exclamation;
            qform.ShowIcon = false;
            qform.ShowInTaskbar = false;
            qform.Text = "Select Video Quality";
            qform.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            qform.AcceptButton = selectButton;
            qform.CancelButton = null;
            qform.Size = new System.Drawing.Size(200, 230);
            qform.Location = new System.Drawing.Point(
                (parentForm.Location.X + parentForm.Size.Width / 2) - qform.Size.Width / 2,
                (parentForm.Location.Y + parentForm.Size.Height / 2) - qform.Size.Height / 2);

            optionsList.Location = new System.Drawing.Point(10, 10);
            optionsList.Size = new System.Drawing.Size(qform.Size.Width - 25, qform.Size.Height - 70);

            selectButton.Text = "Use this quality";
            selectButton.Location = new System.Drawing.Point(optionsList.Location.X, optionsList.Location.Y + optionsList.Size.Height + 4);
            selectButton.Size = new System.Drawing.Size(optionsList.Size.Width, 26);
            selectButton.DialogResult = System.Windows.Forms.DialogResult.OK;

            optionsList.SelectedIndex = 0;

            qform.Controls.Add(selectButton);
            qform.Controls.Add(optionsList);

            qform.ShowDialog();

            int selectedItem = optionsList.SelectedIndex;

            qform.Dispose();

            return options[selectedItem];
        }

        public static void addToPlaylistDialog()
        {
            addToPlaylist pl = new addToPlaylist((Form1)parentForm);

            pl.Location = new System.Drawing.Point(
                (parentForm.Location.X + parentForm.Size.Width / 2) - pl.Size.Width / 2,
                (parentForm.Location.Y + parentForm.Size.Height / 2) - pl.Size.Height / 2);

            pl.ShowDialog();
            pl.Dispose();
        }
    }
}
