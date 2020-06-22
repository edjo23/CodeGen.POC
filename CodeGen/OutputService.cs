using System.IO;

namespace CodeGen
{
    public interface IOutputService
    {
        int UpdatedCount { get; }
        void Write(string content, string path);
    }

    public class OutputService : IOutputService
    {
        public OutputService()
        {
        }

        public int CreatedCount { get; set; }
        public int UpdatedCount { get; set; }

        public void Write(string path, string content)
        {
            var fileInfo = new FileInfo(path);
            var exists = fileInfo.Exists;

            Directory.CreateDirectory(fileInfo.DirectoryName);

            // TODO Check file contents and only output if different.
            File.WriteAllText(path, content);

            if (exists)
                UpdatedCount += 1;
            else
                CreatedCount += 1;
        }
    }
}
