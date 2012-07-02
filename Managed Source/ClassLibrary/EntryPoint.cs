using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.CodeDom.Compiler;

namespace ClassLibraryNamespace
{
    public class ClassLibraryManager
    {
        public ClassLibraryManager()
        {
            System.Console.WriteLine("sup.");

            TestJIT();
        }

        public void TestJIT()
        {
            var compilerParameters = new CompilerParameters();

            compilerParameters.GenerateExecutable = false;

            // Necessary for stack trace line numbers etc
            compilerParameters.IncludeDebugInformation = true;
            compilerParameters.GenerateInMemory = true;

            var scriptsDirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Scripts");

            Console.WriteLine(scriptsDirectory);

            var scripts = new List<string>();
            foreach (var script in Directory.GetFiles(scriptsDirectory, "*.cs", SearchOption.AllDirectories))
            {
                Console.WriteLine(script);
                scripts.Add(script);
            }

            Console.WriteLine("Compiling");

            CompilerResults results;
            using (var provider = CodeDomProvider.CreateProvider("csharp"))
                results = provider.CompileAssemblyFromFile(compilerParameters, scripts.ToArray());

            Console.WriteLine("done compiling, assembly is {0}", results.CompiledAssembly != null ? "OK" : "invalid!");
        }
    }
}
