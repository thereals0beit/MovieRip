using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MovieRip
{
    public partial class addToPlaylist : Form
    {
        private Form1 parentForm = null;

        public addToPlaylist(Form1 p)
        {
            InitializeComponent();

            parentForm = p;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (streamResolver.IsBusy)
            {
                streamResolver.CancelAsync();
            }

            streamResolver.RunWorkerAsync(textBox1.Text);
        }

        private void streamResolver_DoWork(object sender, DoWorkEventArgs e)
        {
            textBox1.Text = "";
            button1.Enabled = false;
            statusStrip1.Items[0].Image = MovieRip.Properties.Resources.icon_wait;
            statusStrip1.Items[0].Text = "Checking to see if link is valid...";
            textBox1.Enabled = false;

            VideoProviders.BaseProvider provider = App.Core.getProvider((string)e.Argument);

            bool success = false;

            if (provider != null)
            {
                success = parentForm.playList.AddToPlaylist(parentForm.listBox2, (string)e.Argument);
            }
            else
            {
                success = parentForm.playList.PopulatePlaylist(parentForm.listBox2, (string)e.Argument);
            }

            if (success == false)
            {
                statusStrip1.Items[0].Image = MovieRip.Properties.Resources.red_light_large;
                statusStrip1.Items[0].Text = "Failed to add [" + (string)e.Argument + "]!!";
                textBox1.Enabled = true;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Dispose(true);

                //statusStrip1.Items[0].Image = MovieRip.Properties.Resources.green_light_large;
                //statusStrip1.Items[0].Text = "Added last entry successfully!";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            VideoProviders.BaseProvider provider = App.Core.getProvider(textBox1.Text);

            if (provider == null)
            {
                VideoProviders.Hosts.LinkCollector linkCollector = App.Core.getLinkCollector(textBox1.Text);

                if (linkCollector == null)
                {
                    button1.Text = "Add Movie";
                    button1.Enabled = false;
                    statusStrip1.Items[0].Image = MovieRip.Properties.Resources.yellow_light_large;
                    statusStrip1.Items[0].Text = "No Movie Host Found";
                }
                else
                {
                    statusStrip1.Items[0].Text = "Valid " + linkCollector.getName() + " link found!";
                    statusStrip1.Items[0].Image = linkCollector.getIcon();
                    button1.Enabled = true;
                    button1.Text = "Add all movies on host page";
                }
            }
            else
            {
                statusStrip1.Items[0].Text = "Valid " + provider.getName() + " link found!";
                statusStrip1.Items[0].Image = provider.getIcon();
                button1.Enabled = true;
                button1.Text = "Add Movie";
            }
        }
    }
}
