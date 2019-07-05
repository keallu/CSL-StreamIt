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
                    sw.WriteLine(@":root { --divider: rgba(0, 0, 0, 0.12); }");
                    sw.WriteLine(@"body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif; margin: 0;}");
                    sw.WriteLine(@"h1 { font-weight: 300; letter-spacing: -1.5px; display: block; box-sizing: border-box; max-height: 64px; max-width: 100%; background-color: #3700b3; color: #fff; margin: 0; padding: 16px; }");
                    sw.WriteLine(@"table { border: 0; border-spacing: 0; width: 100%; }");
                    sw.WriteLine(@"thead { background-color: #6200ee; color: #fff; margin: 0; }");
                    sw.WriteLine(@"th { font-weight: 600; height: 48px; border-block-end: 2px solid var(--divider);  word-wrap: break-word; }");
                    sw.WriteLine(@"td { font-weight: 350; height: 47px; padding-top:6px; padding-bottom: 6px; border-block-end: 1px solid var(--divider); }");
                    sw.WriteLine(@"td > a { font-weight: 600; font-size: 14px; text-transform: uppercase; text-decoration: none; }");
                    sw.WriteLine(@"td > a:hover { text-decoration: underline; }");
                    sw.WriteLine(@"th, td { text-align: left; vertical-align: middle; }");
                    sw.WriteLine(@"th:first-child, td:first-child, p { font-weight: 600; padding-left: 16px; padding-right:12px; }");
                    sw.WriteLine(@"td:nth-child(2n) { max-width: 50%; padding-right: 12px;}");
                    sw.WriteLine(@"th:nth-child(3n) { min-width: 100px; padding-right: 12px;}");
                    sw.WriteLine(@"th:last-child, td:last-child { min-width: 100px; padding-right: 16px; text-align: right; }");
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
                            sw.WriteLine(@"<td><a target=""_blank"" href=""https://steamcommunity.com/sharedfiles/filedetails/?id=" + id.ToString() + @""">" + id + @"</a></td>");

                            if (id > 0)
                                sw.WriteLine(@"<td><a href=""steam://url/CommunityFilePage/" + id.ToString() + @""">Open in Steam</a></td>");
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
