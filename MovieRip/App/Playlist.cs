using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.App
{
    public class PlaylistItem
    {
        public string title = string.Empty;
        public string rawLink = string.Empty;
        public System.Drawing.Image hostIcon = null;
    };

    public class Playlist
    {
        public List<PlaylistItem> playlistItems = new List<PlaylistItem>();

        public Boolean PopulatePlaylist(System.Windows.Forms.ListBox listBoxPopulate, string url)
        {
            VideoProviders.Hosts.LinkCollector linkCollector = App.Core.getLinkCollector(url);

            if (linkCollector != null)
            {
                List<VideoProviders.BaseProvider> baseList = linkCollector.getEmbeddedList();

                if (baseList.Count == 0)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < baseList.Count; i++)
                    {
                        this.AddToPlaylist(listBoxPopulate, baseList[i].getBaseUrl());
                    }

                    return true;
                }
            }

            return false;
        }

        public Boolean AddToPlaylist(System.Windows.Forms.ListBox listBoxPopulate, string singleUrl)
        {
            VideoProviders.BaseProvider provider = App.Core.getProvider(singleUrl);

            if (provider == null) return false;

            string title = provider.getVideoTitle();

            if (title == null) return false;

            PlaylistItem newItem = new PlaylistItem();

            newItem.hostIcon = provider.getIcon(); // This will eventually replace the host name in a more advanced box
            newItem.rawLink = singleUrl;
            newItem.title = title;

            playlistItems.Add(newItem);

            listBoxPopulate.Items.Add("[" + provider.getName() + "] " + title);

            return true;
        }

        public void RemoveFromPlaylist(System.Windows.Forms.ListBox listBoxPopulate, int idx)
        {
            listBoxPopulate.Items.RemoveAt(idx);
            playlistItems.RemoveAt(idx);
        }

        public void ClearPlaylist(System.Windows.Forms.ListBox listBoxPopulate)
        {
            listBoxPopulate.Items.Clear();
            playlistItems.Clear();
        }
    }
}

/*
                VideoProviders.Hosts.LinkCollector linkCollector = App.Core.getLinkCollector((string)e.Argument);

                if (linkCollector != null)
                {
                    List<VideoProviders.BaseProvider> baseList = linkCollector.getEmbeddedList();

                    if (baseList.Count == 0)
                    {
                        pictureBox1.Image = MovieRip.Properties.Resources.red_light_large;

                        didFailLastRequest = true;
                    }
                    else
                    {
                        // this.listBox2.Items.Clear();

                        for (int i = 0; i < baseList.Count; i++)
                        {
                            this.listBox2.actualData.Add(baseList[i].getBaseUrl());
                            this.listBox2.Items.Add("[" + baseList[i].getName() + "] " + baseList[i].getVideoTitle());
                        }

                        pictureBox1.Image = MovieRip.Properties.Resources.green_light_large;

                        success = true;
                    }
                }*/