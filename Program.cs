namespace Fudge_Payload_Extrator
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await WriteCharacters("x0.txt", Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(GetBetween(File.ReadAllText(args[0]), "atob('", "'))"))));
                await WriteCharacters("x1.txt", Encoding.ASCII.GetString(Convert.FromBase64String(File.ReadAllText("x0.txt"))));
                await WriteCharacters("x1.txt", Encoding.ASCII.GetString(Convert.FromBase64String(File.ReadAllText("x0.txt"))));
                await WriteBytes("Payload.exe", Convert.FromBase64String(GetBetween(File.ReadAllText("x1.txt"), "saveData('", "',")));
                for (var i = 0; i < 2; i++)
                {
                    Del($"x{i}.txt");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        static void Del(string path)
        {
            File.Delete(path);
        }
        static async Task WriteBytes(string path, byte[] content)
        {
            using (FileStream fsStream = new FileStream(path, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                writer.Write(content);
            }
        }
        static async Task WriteCharacters(string path, string content)
        {
            using (StreamWriter writer = File.CreateText(path))
            {
                await writer.WriteAsync(content);
            }
        }
        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (!strSource.Contains(strStart) || !strSource.Contains(strEnd)) return "";
            var start = strSource.IndexOf(strStart, 0, StringComparison.Ordinal) + strStart.Length;
            var end = strSource.IndexOf(strEnd, start, StringComparison.Ordinal);
            return strSource.Substring(start, end - start);
        }
    }
}