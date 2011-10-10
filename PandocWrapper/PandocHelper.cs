using System;
using System.Diagnostics;
using System.Text;

namespace PandocWrapper
{
    public static class PandocHelper
    {
        private const string Url = @"C:\\Program Files (x86)\\Pandoc\\bin\\pandoc.exe";

        public static string Convert(string source)
        {
            string outputString = string.Empty;
            if (source != null)
            {
                const string processName = Url;
                var args = String.Format(@"-r html -t mediawiki");

                var psi = new ProcessStartInfo(processName, args)
                              {RedirectStandardOutput = true, RedirectStandardInput = true};

                var p = new Process {StartInfo = psi};
                psi.UseShellExecute = false;
                p.Start();

                var inputBuffer = Encoding.UTF8.GetBytes(source);
                p.StandardInput.BaseStream.Write(inputBuffer, 0, inputBuffer.Length);
                p.StandardInput.Close();

                using (var sr = new System.IO.StreamReader(p.StandardOutput.BaseStream))
                {

                    outputString = sr.ReadToEnd();
                }
            }

            return outputString;
        }

    }
}
