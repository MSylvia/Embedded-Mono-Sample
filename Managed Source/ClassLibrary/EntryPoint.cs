﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.IO;
using System.CodeDom.Compiler;

namespace ClassLibraryNamespace
{
    public class ClassLibraryManager
    {
        public ClassLibraryManager()
        {
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionOccurred;

			// Uncomment to test bug 6035 related to a child domain's AssemblyResolve event.
			//Xamarin6035_AssemblyResolve_Crash();

			// Uncomment to test bug 5938 related to a CSharpCodeProvider freeze
            // This does not work as of Mono 3.0.2, CodeDomProvider.GetProvider("C#") throws an exception.
			//TestJIT();
		}

		private static void UnhandledExceptionOccurred(object sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine(e.ExceptionObject.ToString());
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

            foreach (var script in Directory.GetFiles(scriptsDirectory, "*.cs", SearchOption.AllDirectories))
            {
				Console.WriteLine("Compiling script {0}", script);

				using (var provider = CodeDomProvider.CreateProvider("C#"))
				{
					Action action = () => provider.CompileAssemblyFromFile(compilerParameters, script);
					try
					{
						TryDo(action, 5000);
					}
					catch (TimeoutException ex)
					{
						Console.WriteLine("Failed to compile script {0}, timeout was triggered", script);
					}
				}
            }
        }

		static void TryDo(Action action, int timeout)
		{
			var waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

			{
				AsyncCallback callback = ar => waitHandle.Set();
				action.BeginInvoke(callback, null);

				if (!waitHandle.WaitOne(timeout))
				{
					waitHandle.Reset();
					throw new TimeoutException("Failed to complete in the timeout specified.");
				}
			}
		}

		public void Xamarin6035_AssemblyResolve_Crash()
		{
			var appDomain = AppDomain.CreateDomain("Xamarin6035_AppDomain");
			appDomain.AssemblyResolve += (sender, args) =>
			{
				Console.WriteLine("resolving {0} requested by {1}", args.Name, args.RequestingAssembly.FullName);

				return null;
			};
		}
    }
}
