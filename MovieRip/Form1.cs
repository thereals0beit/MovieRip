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
    public partial class Form1 : Form
    {
        private bool didFailLastRequest = false;
        public App.Playlist playList = new App.Playlist();

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            VideoProviders.BaseProvider provider = App.Core.getProvider(textBox1.Text);

            if (provider == null)
            {
                VideoProviders.Hosts.LinkCollector linkCollector = App.Core.getLinkCollector(textBox1.Text);

                if (linkCollector == null)
                {
                    label3.Text = "Please input a video link above!";
                    pictureBox2.Image = MovieRip.Properties.Resources.yellow_light_large;
                    button2.Enabled = false;
                    button2.Text = "Play Now";
                }
                else
                {
                    label3.Text = "Valid " + linkCollector.getName() + " link found!";
                    pictureBox2.Image = linkCollector.getIcon();
                    button2.Enabled = true;
                    button2.Text = "Add host videos to playlist";
                }
            }
            else
            {
                label3.Text = "Valid " + provider.getName() + " link found!";
                pictureBox2.Image = provider.getIcon();
                button2.Enabled = true;
                button2.Text = "Play Now";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            App.Forms.parentForm = this;

            App.Config.Manager.programStart(Application.StartupPath); // Initialize the Configuration Files

            string[] availableSkins = System.IO.Directory.GetFiles(Application.StartupPath + "\\jwplayer\\skins\\", "*.zip", System.IO.SearchOption.TopDirectoryOnly);

            for (int i = 0; i < availableSkins.Length; i++)
            {
                string finalName = availableSkins[i].Substring(availableSkins[i].LastIndexOf('\\') + 1);

                listBox1.Items.Add(finalName);
            }

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.Items[i].ToString() == App.Config.Manager.jwplayer.skin)
                {
                    listBox1.SelectedIndex = i;
                }
            }

            this.textBox2.Text = App.Config.Manager.jwplayer.startMovie;

            this.axShockwaveFlash1.jw_init(Application.StartupPath + "\\jwplayer\\player.swf",
                "movieRip",
                App.Config.Manager.jwplayer.startMovie,
                "file:///" + Application.StartupPath + "/jwplayer/skins/" + App.Config.Manager.jwplayer.skin,
                "file:///" + Application.StartupPath + "/jwplayer/preview.jpg");
        }

        private void updateVideoInformation_Tick(object sender, EventArgs e)
        {
            button7.Enabled = (listBox2.Items.Count > 0);

            if (axShockwaveFlash1 == null) return;
            if (axShockwaveFlash1.IsHandleCreated == false) return;

            try
            {
                if (didFailLastRequest)
                {
                    this.label5.Text = "Video Status: ERROR";
                    this.label6.Visible = false;
                }
                else
                {
                    this.label5.Text = "Video Status: " + this.axShockwaveFlash1.jw_getState();
                    this.label6.Text = "Downloaded: " + Math.Floor(this.axShockwaveFlash1.jw_getBufferComplete()).ToString() + "%";
                    this.label6.Visible = true;
                }
            }
            catch (Exception) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            didFailLastRequest = false;

            if (streamResolver.IsBusy)
            {
                streamResolver.CancelAsync();
            }

            streamResolver.RunWorkerAsync(textBox1.Text);

            textBox1.Text = "";
        }

        private void streamResolver_DoWork(object sender, DoWorkEventArgs e)
        {
            button2.Enabled = false;
            pictureBox1.Image = MovieRip.Properties.Resources.icon_wait;
            pictureBox2.Image = MovieRip.Properties.Resources.yellow_light_large;
            listBox2.Enabled = false;

            VideoProviders.BaseProvider provider = App.Core.getProvider((string)e.Argument);

            bool success = false;

            if (provider != null)
            {
                string useQuality = null;

                List<string> qualityOptions = provider.getQualityOptions();

                if (qualityOptions != null)
                {
                    useQuality = App.Forms.qualitySelector(qualityOptions);
                }

                movieTitleAA.Text = provider.getVideoTitle();

                string videoUrl = provider.getVideoUrl(useQuality);

                if (videoUrl != null)
                {
                    this.axShockwaveFlash1.jw_load(videoUrl);
                    this.axShockwaveFlash1.jw_play();

                    pictureBox1.Image = MovieRip.Properties.Resources.green_light_large;

                    success = true;
                }
            }
            else
            {
                if (playList.PopulatePlaylist(listBox2, (string)e.Argument))
                {
                    pictureBox1.Image = MovieRip.Properties.Resources.green_light_large;

                    success = true;
                }
            }

            if (success == false)
            {
                pictureBox1.Image = MovieRip.Properties.Resources.red_light_large;

                didFailLastRequest = true;
            }

            listBox2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1) return;

            didFailLastRequest = false;

            if (streamResolver.IsBusy)
            {
                streamResolver.CancelAsync();
            }

            streamResolver.RunWorkerAsync(playList.playlistItems[listBox2.SelectedIndex].rawLink);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            playList.ClearPlaylist(listBox2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1) return;

            playList.RemoveFromPlaylist(listBox2, listBox2.SelectedIndex);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // App.Forms.MessageBox("Sorry, this feature is not yet available...", "Error: TODO", App.Forms.eIconType.eError);

            App.Forms.addToPlaylistDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Save Playlist
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Load Playlist
        }
    }
}