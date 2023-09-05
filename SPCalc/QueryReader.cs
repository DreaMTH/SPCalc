#define db

using System.Text;

namespace SPCalc
{
    internal class QueryReader : IDisposable
    {
        private bool disposed = false;
        protected string Path { get; set; }
        public QueryReader()
        {
            Path = string.Empty;
        }
        public QueryReader(string Path)
        {
            this.Path = Path;
        }
        public string Open()
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<string> strings = new List<string>();

            try
            {
                strings = File.ReadAllLines(this.Path).ToList();
            }
            catch (System.IO.DirectoryNotFoundException ex) { Console.WriteLine($"Wrong directory!\n\t{ex.Message}"); }
            catch (System.IO.IOException ex) { Console.WriteLine($"Wrong file!\n\t{ex.Message}"); }

#if db
            for (int i = 0; i < strings.Count; ++i)
            {
                Console.WriteLine(strings[i]);
            }
#endif
        
            
            return string.Join(Environment.NewLine, strings);
        }
        public string Open(string Path)
        {
            this.Path = Path;
            return Open();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                this.Path = string.Empty;

            }
            disposed = true;
        }
        ~QueryReader() { Dispose(false); }
    }
}
