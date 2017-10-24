using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DetectaCabeceras
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run()
        {
            string[] args = System.Environment.GetCommandLineArgs();

            // Si el directorio a revisar no es especificado el programa se cierra
            if (args.Length != 2)
            {
                // Muestra lo que se debe llamar, en este caso DetectaCabeceras.exe
                Console.WriteLine("Usar: DetectaCabecera.exe (directorio)");
                Console.ReadKey();
                return;
            }

            // Crea un nuevo sistema de seguimiento de cambios en el directorio.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = args[1];
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            
            // Solo analiza los archivos .txt
            watcher.Filter = "*.txt";

            // Agregamos los eventos de escucha al manejador
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Comenzamos a escuchar
            watcher.EnableRaisingEvents = true;

            // Espera que el usuario escriba q para dejar de analizar el directorio.
            Console.WriteLine("Presione \'q\' para salir de la aplicación.");
            while (Console.Read() != 'q') ;
        }

        // Definicion de los eventos
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            //Especificamente cuando hay un archivo que se crea, se modifica o se elimina.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Especificamente cuando un archivo es renombrado dentro del directorio.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }
    }
}
