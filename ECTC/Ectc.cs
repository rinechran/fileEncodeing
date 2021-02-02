using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ECTC
{

    public class Options
    {
        [Option(Required = true)]
        public String From { get; set; }

        [Option(Required = true)]
        public String To { get; set; }

        [Option(Required = true)]
        public String Path { get; set; }

        [Option("filename", Separator = ':')]
        public IEnumerable<String> FilenameExtensions { get; set; }

        [Option(Default = (bool)true)]
        public bool? Recursive { get; set; }
    }

    public class FileEncoder
    {
        static FileEncoder()
        {
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        static public Encoding GetEncodingInfo(string path)
        {
            Encoding currentEncoding;
            using (StreamReader sr = new StreamReader(path, true))
            {
                currentEncoding = sr.CurrentEncoding;
                sr.Close();
            }

            return currentEncoding;
        }
        static public void Convert(string path,string toEncoding)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                using (StreamWriter sw = new StreamWriter(path + "temp", false, Encoding.GetEncoding(toEncoding)))
                {
                    sw.WriteLine(sr.ReadToEnd());
                    sw.Close();
                    sw.Dispose();
                }
                sr.Close();
            }

            File.Delete(path);
            File.Move(path + "temp", path);

        }

    }
    public class Ectc
    {

        public Ectc(Options oth)
        {
            option = oth;
        }

        public bool IsPattern(string filename)
        {

            return true;
        }


        public void DirSerach(string path)
        {
            foreach (string file in Directory.GetFiles(path))
            {
                if (IsPattern(file) == true)
                {
                    Encoding currentEncoding = FileEncoder.GetEncodingInfo(file);
                    FileEncoder.Convert(file, option.To);
                }
            }

            if (option.Recursive == true)
            {
                foreach (string directory in Directory.GetDirectories(path))
                {
                    DirSerach(directory);
                }
            }
        }

        public void Run()
        {
            DirSerach(option.Path);
        }

        private readonly Options option;

    }
}
