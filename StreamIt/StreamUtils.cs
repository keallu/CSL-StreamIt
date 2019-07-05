using ColossalFramework;
using ColossalFramework.Plugins;
using ICities;
using System;
using System.IO;
using UnityEngine;

namespace StreamIt
{
    public static class StreamUtils
    {
        public static void ExportToFile(string fileName, bool includeTimeStampInFileName, bool includeDisabledMods, bool includeBuiltinMods, bool includeLocaleMods)
        {
            try
            {
                if (includeTimeStampInFileName)
                {
                    fileName = fileName + string.Format("-{0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now);
                }

                using (StreamWriter sw = new StreamWriter(fileName + ".html"))
                {
                    sw.WriteLine(@"<!DOCTYPE html>");
                    sw.WriteLine(@"<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">");
                    sw.WriteLine(@"<head>");
                    sw.WriteLine(@"<meta charset=""utf-8"" />");
                    sw.WriteLine(@"<title>Stream It! - Cities: Skylines - List of Mods</title>");
                    sw.WriteLine(@"<style>");
                    sw.WriteLine(@"table {border: 3px solid #000000;text-align: left;border-collapse: collapse;}");
                    sw.WriteLine(@"table td, table th {border: 1px solid #000000;padding: 5px 4px;}");
                    sw.WriteLine(@"table tbody td {font-size: 13px;}");
                    sw.WriteLine(@"table thead {background: #CFCFCF;border-bottom: 3px solid #000000;}");
                    sw.WriteLine(@"table thead th {font-size: 15px;font-weight: bold;color: #000000;text-align: left;}");
                    sw.WriteLine(@"</style>");
                    sw.WriteLine(@"</head>");
                    sw.WriteLine(@"<body>");
                    sw.WriteLine(@"<h1>Cities: Skylines - List of Mods</h1>");
                    sw.WriteLine(@"<table>");
                    sw.WriteLine(@"<thead>");
                    sw.WriteLine(@"<tr>");
                    sw.WriteLine(@"<th>Name</th>");
                    sw.WriteLine(@"<th>Description</th>");
                    sw.WriteLine(@"<th>Steam Workshop ID</th>");
                    sw.WriteLine(@"<th>Link to Steam Workshop</th>");
                    sw.WriteLine(@"</tr>");
                    sw.WriteLine(@"<thead>");
                    sw.WriteLine(@"<tbody>");

                    foreach (PluginManager.PluginInfo pluginInfo in Singleton<PluginManager>.instance.GetPluginsInfo())
                    {
                        ulong id = 0;

                        if (!pluginInfo.isEnabled && !includeDisabledMods)
                        {
                            continue;
                        }

                        if (pluginInfo.isBuiltin && !includeBuiltinMods)
                        {
                            continue;
                        }

                        if (!pluginInfo.isBuiltin && !IsWorkshopMod(pluginInfo.name, out id) && !includeLocaleMods)
                        {
                            continue;
                        }

                        IUserMod[] instances = pluginInfo.GetInstances<IUserMod>();

                        if (instances.Length == 1)
                        {
                            sw.WriteLine(@"<tr>");
                            sw.WriteLine(@"<td>" + instances[0].Name + @"</td>");
                            sw.WriteLine(@"<td>" + instances[0].Description + @"</td>");
                            sw.WriteLine(@"<td>" + id + @"</td>");

                            if (id > 0)
                                sw.WriteLine(@"<td><a target=""_blank"" href=""https://steamcommunity.com/sharedfiles/filedetails/?id=" + id.ToString() + @""">Link</a><br /><a href=""steam://url/CommunityFilePage/" + id.ToString() + @""">Open in Steam</a></td>");
                            else
                                sw.WriteLine(@"<td>&nbsp;</td>");

                            sw.WriteLine(@"</tr>");
                        }
                    }

                    sw.WriteLine(@"</tbody>");
                    sw.WriteLine(@"</table>");
                    sw.WriteLine(@"<p>This list was exported from Cities: Skylines with <a target=""_blank"" href=""https://steamcommunity.com/sharedfiles/filedetails/?id=1597285962""> Stream It!</a><p/>");
                    sw.WriteLine(@"</body>");
                    sw.WriteLine(@"</html>");
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamUtils:ExportToFile -> Exception: " + e.Message);
            }
        }

        private static bool IsWorkshopMod(string name, out ulong id)
        {
            return ulong.TryParse(name, out id);
        }
    }
}
