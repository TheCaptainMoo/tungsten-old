using System.Reflection;
using System.Runtime.Loader;
using Tungsten_Interpreter.Utilities.Parser;

namespace Tungsten_Interpreter.Utilities.ComponentController
{
    class AssemblyLoader : AssemblyLoadContext
    {
        public static void LoadAssemblies(string[] directory)
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            currentDirectory = Path.Combine(Path.Combine(currentDirectory, "Extensions"), Path.Combine(directory));

            try
            {
                string[] files = Directory.GetFiles(currentDirectory);

                foreach (string file in files)
                {
                    Assembly assembly = LoadPlugin(file);
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        Type iLexer = typeof(ILexer);
                        Type iNestedLexer = typeof(INestedLexer);

                        if (iLexer.IsAssignableFrom(type))
                        {
                            ILexer lexer = (ILexer)Activator.CreateInstance(type);
                            Program.methods.Add(lexer.Name, lexer);
                        }
                        else if (iNestedLexer.IsAssignableFrom(type))
                        {
                            INestedLexer lexer = (INestedLexer)Activator.CreateInstance(type);
                            Program.nestedMethods.Add(lexer.Name, lexer);
                        }
                    }
                }
            } 
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("The Interpreter cannot load directory at: {0} | Some Functionality May Be Missing", currentDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine("External files could not be loaded.");
            }
        }

        static Assembly LoadPlugin(string relativePath)
        {
            var pluginLocation = relativePath;
            var loadContext = new AssemblyLoader(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        private AssemblyDependencyResolver _resolver;
        public AssemblyLoader(string pluginPath)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
            return IntPtr.Zero;
        }

    }
}
