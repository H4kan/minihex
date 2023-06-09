namespace minihex.engine.test.Helpers
{
    public static class WriterHelper
    {
        public static void SaveContentToFile(List<string> content, string fileName)
        {
            var fullPath = Directory.GetCurrentDirectory();
            var projectPath = fullPath.Remove(fullPath.IndexOf("minihex"));
            var directoryFilePath = $"{projectPath}/minihex/analysis/testresults";

            if (!Directory.Exists(directoryFilePath))
            {
                Directory.CreateDirectory(directoryFilePath);
            }

            string filePath = Path.Combine(directoryFilePath, fileName);
            File.WriteAllLines(filePath, content);
        }
    }
}
