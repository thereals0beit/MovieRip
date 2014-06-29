using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRip.App
{
    public class jwPlayer : AxShockwaveFlashObjects.AxShockwaveFlash
    {
        public void jw_init(string path, string id, string filename = null, string skinname = null, string imagename = null, string provider = null)
        {
            string flashVariablesString = "id=" + id;

            if (filename != null)
            {
                flashVariablesString += "&file=" + filename;
            }

            if (skinname != null)
            {
                flashVariablesString += "&skin=" + skinname;
            }

            if (imagename != null)
            {
                flashVariablesString += "&image=" + imagename;
            }

            flashVariablesString += "&autostart=false";

            if (provider != null)
            {
                flashVariablesString += "&provider=" + provider;
            }

            this.Hide();
            this.Rewind();
            this.Stop();

            this.FlashVars = flashVariablesString;
            this.Quality2 = "High";
            this.AllowFullScreen = "true";
            this.AllowScriptAccess = "always";

            this.Show();
            this.LoadMovie(0, path);
            this.Play();

            this.BGColor = "000000";
            this.BackgroundColor = 0;
        }

        private object parseResult(string result)
        {
            if (result == null) return null;

            System.Text.RegularExpressions.Match regexMatch = System.Text.RegularExpressions.Regex.Match(result, "<(.*)>(.+?)</(.*)>");

            if (regexMatch.Success)
            {
                switch (regexMatch.Groups[1].Value)
                {
                    case "number":
                        {
                            return Convert.ToSingle(regexMatch.Groups[2].Value);
                        }
                    case "string":
                        {
                            return regexMatch.Groups[2].Value;
                        }
                    case "array":
                        {
                            return null; // TODO
                        }
                }
            }
            else
            {
                regexMatch = System.Text.RegularExpressions.Regex.Match(result, "<(.+?)/>");

                if (regexMatch.Success)
                {
                    switch (regexMatch.Groups[0].Value)
                    {
                        case "true":
                        case "false":
                            {
                                return Convert.ToBoolean(regexMatch.Groups[0].Value);
                            }
                        case "null":
                            {
                                return null;
                            }
                    }
                }
            }

            return null;
        }

        private string convertObjectToString(object obj)
        {
            Type parameterType = obj.GetType();

            if (parameterType.IsEquivalentTo(typeof(string)))
            {
                return "<string>" + (string)obj + "</string>";
            }
            else if (
                parameterType.IsEquivalentTo(typeof(Int16)) ||
                parameterType.IsEquivalentTo(typeof(Int32)) ||
                parameterType.IsEquivalentTo(typeof(Int64)) ||
                parameterType.IsEquivalentTo(typeof(float)) ||
                parameterType.IsEquivalentTo(typeof(double)) ||
                parameterType.IsEquivalentTo(typeof(UInt16)) ||
                parameterType.IsEquivalentTo(typeof(UInt32)) ||
                parameterType.IsEquivalentTo(typeof(UInt64)))
            {
                return "<number>" + (float)obj + "</number>";
            }
            else if (parameterType.IsEquivalentTo(typeof(Boolean)))
            {
                return ((Boolean)obj) ? "<true/>" : "<false/>";
            }
            else if (parameterType.IsEquivalentTo(typeof(System.Collections.ArrayList)))
            {
                System.Collections.ArrayList arrayListCom = (System.Collections.ArrayList)obj;

                string returnValue = "<array>";

                for (int a = 0; a < arrayListCom.Count; a++)
                {
                    returnValue += "<property id=\"" + a + "\">" + convertObjectToString((object)arrayListCom[a]) + "</property>";
                }

                returnValue += "</array>";
            }

            return "<null/>";
        }

        private string executeFunctionCall(string functionName, params object[] parameters)
        {
            string returnValue = string.Empty;

            if (parameters.Length == 0)
            {
                try
                {
                    returnValue = this.CallFunction("<invoke name=\"" + functionName + "\" returntype=\"xml\"> </invoke>");
                }
                catch (Exception)
                {
                    returnValue = null;
                }
            }
            else
            {
                string callParameters = "<arguments>";

                for (int i = 0; i < parameters.Length; i++)
                {
                    callParameters += convertObjectToString(parameters[i]);
                }

                callParameters += "</arguments>";

                try
                {
                    returnValue = this.CallFunction("<invoke name=\"" + functionName + "\" returntype=\"xml\">" + callParameters + "</invoke>");
                }
                catch (Exception)
                {
                    returnValue = null;
                }
            }

            return returnValue;
        }

        // Event handlers

        public void jw_addEventListener(string eventType, string callback)
        {
            executeFunctionCall("jwAddEventListener", eventType, callback);
        }

        public void jw_removeEventListener(string eventType, string callback)
        {
            executeFunctionCall("jwRemoveEventListener", eventType, callback);
        }

        // Getters

        public float jw_getBufferComplete()
        {
            return (float)parseResult(executeFunctionCall("jwGetBuffer"));
        }

        public float jw_getDuration()
        {
            return (float)parseResult(executeFunctionCall("jwGetDuration"));
        }

        public bool jw_getFullscreen()
        {
            return (bool)parseResult(executeFunctionCall("jwGetFullscreen"));
        }

        public float jw_getHeight()
        {
            return (float)parseResult(executeFunctionCall("jwGetHeight"));
        }

        public bool jw_getMute()
        {
            return (bool)parseResult(executeFunctionCall("jwGetMute"));
        }

        public string jw_getPlaylist()
        {
            return executeFunctionCall("jwGetPlaylist"); // TODO; Fix the parseResult function to allow it to return arrays
        }

        public float jw_getPlaylistIndex()
        {
            return (float)parseResult(executeFunctionCall("jwGetPlaylistIndex"));
        }

        public float jw_getPosition()
        {
            return (float)parseResult(executeFunctionCall("jwGetPosition"));
        }

        public string jw_getState()
        {
            return (string)parseResult(executeFunctionCall("jwGetState"));
        }

        public float jw_getWidth()
        {
            return (float)parseResult(executeFunctionCall("jwGetWidth"));
        }

        public string jw_getVersion()
        {
            return (string)parseResult(executeFunctionCall("jwGetVersion"));
        }

        public float jw_getVolume()
        {
            return (float)parseResult(executeFunctionCall("jwGetVolume"));
        }

        // Player API Calls

        public void jw_play()
        {
            executeFunctionCall("jwPlay");
        }

        public void jw_pause()
        {
            executeFunctionCall("jwPause");
        }

        public void jw_stop()
        {
            executeFunctionCall("jwStop");
        }

        public void jw_seek(float seekPosition)
        {
            executeFunctionCall("jwSeek", seekPosition);
        }

        public void jw_load(string toLoad)
        {
            executeFunctionCall("jwLoad", toLoad);
        }

        public void jw_playListItem(float item)
        {
            executeFunctionCall("jwPlaylistItem", item);
        }

        public void jw_playListNext()
        {
            executeFunctionCall("jwPlaylistNext");
        }

        public void jw_playlistPrev()
        {
            executeFunctionCall("jwPlaylistPrev");
        }

        public void jw_setMute(object shouldMute = null)
        {
            if (shouldMute == null)
            {
                jw_setMute(!jw_getMute());
            }
            else
            {
                executeFunctionCall("jwSetMute", (bool)shouldMute);
            }
        }

        public void jw_setVolume(float volume)
        {
            executeFunctionCall("jwSetVolume", volume);
        }

        public void jw_setFullscreen(object shouldFullscreen = null)
        {
            if (shouldFullscreen == null)
            {
                jw_setFullscreen(!jw_getFullscreen());
            }
            else
            {
                executeFunctionCall("jwSetFullscreen", shouldFullscreen);
            }
        }

        // Player Controls APIs

        public void jw_showControlbar()
        {
            executeFunctionCall("jwControlbarShow");
        }

        public void jw_hideControlbar()
        {
            executeFunctionCall("jwControlbarHide");
        }

        public void jw_showDisplay()
        {
            executeFunctionCall("jwDisplayShow");
        }

        public void jw_hideDisplay()
        {
            executeFunctionCall("jwDisplayHide");
        }

        public void jw_hideDock()
        {
            executeFunctionCall("jwDockHide");
        }

        public void jw_dockSetButton(string name, string click, string sout, string over)
        {
            executeFunctionCall("jwDockSetButton", name, click, sout, over);
        }

        public void jw_showDock()
        {
            executeFunctionCall("jwDockShow");
        }

        // Extra API
    };
}
