using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace CSC_Automator
{
    public class ConfigData
    {
        public bool TargetTypeEnabled;
        public string TargetTypeArg;
        public bool CustomCscEnabled;
        public string CustomCscArg;
        public bool IconEnabled;
        public string IconArg;
        public bool OptimizeEnabled;
        public string OptimizeArg;
        public bool AutoOpenEnabled;
        public string AutoOpenArg;
        public string Language;
        public string SavePath;

        public ConfigData()
        {
            TargetTypeEnabled = true;
            TargetTypeArg = "exe";
            CustomCscEnabled = false;
            CustomCscArg = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe";
            IconEnabled = false;
            IconArg = "";
            OptimizeEnabled = true;
            OptimizeArg = "/optimize";
            AutoOpenEnabled = true;
            AutoOpenArg = "Si";
            Language = "Español";
            SavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save");
        }
    }

    class Program
    {
        static ConfigData config = new ConfigData();
        static string configFilePath = "";
        static bool isEnglish = false;

        static void Main(string[] args)
        {
            string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save");
            config.SavePath = defaultPath;
            configFilePath = Path.Combine(config.SavePath, "options.config");
            
            if (!Directory.Exists(config.SavePath)) 
                Directory.CreateDirectory(config.SavePath);

            LoadConfig();
            isEnglish = (config.Language.ToLower() == "english");

            RunMainMenu();
        }

        static void RunMainMenu()
        {
            bool running = true;
            int menuIdx = 0;
            string[] itemsES = { "Compilar", "Configuración", "Salir" };
            string[] itemsEN = { "Compile", "Config", "Quit" };

            while (running)
            {
                string[] items = isEnglish ? itemsEN : itemsES;
                Console.Clear();
                DrawHeader("C# TO .EXE COMPILER - DiRoCo022");
                
                Console.WriteLine("");
                for (int i = 0; i < items.Length; i++)
                {
                    if (i == menuIdx) {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(string.Format(" [{0}] {1} ", i + 1, items[i]));
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(string.Format("\n {0}: {1}", isEnglish ? "Save Location" : "Ruta de guardado", config.SavePath));
                Console.ResetColor();

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow) menuIdx = (menuIdx == 0) ? items.Length - 1 : menuIdx - 1;
                else if (key.Key == ConsoleKey.DownArrow) menuIdx = (menuIdx == items.Length - 1) ? 0 : menuIdx + 1;
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (menuIdx == 0) RunCompile();
                    else if (menuIdx == 1) RunConfigMenu();
                    else if (menuIdx == 2) running = false;
                }
                else if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1) { RunCompile(); }
                else if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2) { RunConfigMenu(); }
                else if (key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.NumPad3) { running = false; }
            }
        }

        static void RunConfigMenu()
        {
            bool inConfig = true;
            int configIdx = 0;
            string[] menuES = { "Tipo de Destino", "Ruta CSC", "Icono", "Optimización", "Auto-Abrir", "Idioma", "Guardar y Volver" };
            string[] menuEN = { "Target Type", "CSC Path", "Icon", "Optimization", "Auto-Open", "Language", "Save and Back" };

            while (inConfig)
            {
                string[] currentMenu = isEnglish ? menuEN : menuES;
                Console.Clear();
                DrawHeader(isEnglish ? "OPTIONS" : "CONFIGURACIÓN");
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Format(" {0,-20} | {1,-10} | {2,-30}", isEnglish ? "Name" : "Nombre", isEnglish ? "Active?" : "¿Activo?", "Arguments"));
                Console.WriteLine(new string('-', 68));
                Console.ResetColor();

                for (int i = 0; i < currentMenu.Length; i++)
                {
                    if (i == configIdx) {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(string.Format(" {0,-20} | {1,-10} | {2,-30}", currentMenu[i], GetAppliesValue(i), GetArgsValue(i)));
                    Console.ResetColor();
                }

                Console.WriteLine(isEnglish ? "\n[Arrows: Move] [Enter: Edit] [Esc: Back]" : "\n[Flechas: Mover] [Enter: Editar] [Esc: Volver]");

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow) configIdx = (configIdx == 0) ? currentMenu.Length - 1 : configIdx - 1;
                else if (key.Key == ConsoleKey.DownArrow) configIdx = (configIdx == currentMenu.Length - 1) ? 0 : configIdx + 1;
                else if (key.Key == ConsoleKey.Enter) {
                    HandleConfigEdit(configIdx);
                    if (configIdx == 6) inConfig = false; 
                }
                else if (key.Key == ConsoleKey.Escape) inConfig = false;
            }
            SaveConfig();
        }

        static string GetAppliesValue(int index)
        {
            bool status = false;
            switch(index)
            {
                case 0: status = config.TargetTypeEnabled; break;
                case 1: status = config.CustomCscEnabled; break;
                case 2: status = config.IconEnabled; break;
                case 3: status = config.OptimizeEnabled; break;
                case 4: status = config.AutoOpenEnabled; break;
                default: return "---";
            }
            return string.Format("[{0}]", status ? "X" : " ");
        }

        static string GetArgsValue(int index)
        {
            switch(index)
            {
                case 0: return config.TargetTypeArg;
                case 1: return Truncate(config.CustomCscArg, 25);
                case 2: return string.IsNullOrEmpty(config.IconArg) ? (isEnglish ? "(Empty)" : "(Vacío)") : Truncate(config.IconArg, 25);
                case 3: return config.OptimizeArg;
                case 4: return config.AutoOpenArg;
                case 5: return config.Language;
                case 6: return Truncate(config.SavePath, 25);
                default: return "";
            }
        }

        static void HandleConfigEdit(int index)
        {
            switch (index)
            {
                case 0: 
                    config.TargetTypeEnabled = !config.TargetTypeEnabled; 
                    if(config.TargetTypeEnabled) config.TargetTypeArg = (config.TargetTypeArg == "exe") ? "winexe" : "exe";
                    break;
                case 1: 
                    config.CustomCscEnabled = !config.CustomCscEnabled; 
                    if(config.CustomCscEnabled) EditString(ref config.CustomCscArg, isEnglish ? "CSC Path:" : "Ruta de csc.exe:");
                    break;
                case 2: 
                    config.IconEnabled = !config.IconEnabled; 
                    if(config.IconEnabled) EditString(ref config.IconArg, isEnglish ? "Icon Path (.ico):" : "Ruta del icono (.ico):");
                    break;
                case 3: config.OptimizeEnabled = !config.OptimizeEnabled; break;
                case 4: config.AutoOpenEnabled = !config.AutoOpenEnabled; break;
                case 5: 
                    config.Language = (config.Language == "Español") ? "English" : "Español"; 
                    isEnglish = (config.Language == "English");
                    break;
                case 6: 
                    EditString(ref config.SavePath, isEnglish ? "New Save Path:" : "Nueva ruta de guardado:");
                    if (!Directory.Exists(config.SavePath)) Directory.CreateDirectory(config.SavePath);
                    configFilePath = Path.Combine(config.SavePath, "options.config");
                    SaveConfig(); 
                    break;
            }
        }

        static void EditString(ref string val, string prompt)
        {
            Console.Clear();
            Console.WriteLine(string.Format("\n {0}", prompt));
            Console.WriteLine(isEnglish ? " (Paste path or drag file)" : " (Pega la ruta o arrastra el archivo)");
            Console.Write(" -> ");
            string input = Console.ReadLine();
            if (input != null) input = input.Replace("\"", "").Trim();
            if (!string.IsNullOrEmpty(input)) val = input;
        }

        static void RunCompile()
        {
            bool redoCompile = true;
            while (redoCompile)
            {
                Console.Clear();
                DrawHeader(isEnglish ? "COMPILATION" : "COMPILACIÓN");
                
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(isEnglish ? " -> Drag .cs file here: " : " -> Arrastra el archivo .cs aquí: ");
                string input = Console.ReadLine();
                if (input != null) input = input.Replace("\"", "").Trim();

                if (string.IsNullOrEmpty(input) || !File.Exists(input)) { 
                    string errorMsg = isEnglish ? "File not found." : "El archivo no existe.";
                    int choice = ShowPostCompileOptions(null, errorMsg);
                    if (choice == 1) continue; // Compilar otro
                    else return;
                }

                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string output = Path.Combine(desktop, Path.GetFileNameWithoutExtension(input) + ".exe");

                if (File.Exists(output))
                {
                    try { using (FileStream fs = File.Open(output, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { fs.Close(); } }
                    catch (IOException) { 
                        string errorMsg = isEnglish ? "Output file is in use. Please close it." : "El archivo de salida está en uso. Por favor, ciérralo.";
                        int choice = ShowPostCompileOptions(null, errorMsg);
                        if (choice == 1) continue; else return;
                    }
                }

                List<string> argsList = new List<string>();
                argsList.Add(string.Format("/out:\"{0}\"", output));
                argsList.Add("/nologo");
                if (config.TargetTypeEnabled) argsList.Add(string.Format("/target:{0}", config.TargetTypeArg));
                if (config.OptimizeEnabled) argsList.Add(config.OptimizeArg);
                if (config.IconEnabled && !string.IsNullOrEmpty(config.IconArg)) 
                {
                    if (!File.Exists(config.IconArg)) PrintError(isEnglish ? "Warning: Icon not found." : "Advertencia: Icono no encontrado.");
                    else argsList.Add(string.Format("/win32icon:\"{0}\"", config.IconArg));
                }
                argsList.Add(string.Format("\"{0}\"", input));

                // Añadimos la referencia necesaria para BigInteger
                argsList.Add("/r:System.Numerics.dll");
                string finalArgs = string.Join(" ", argsList.ToArray());
                string cscPath = config.CustomCscEnabled ? config.CustomCscArg : "csc.exe";

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = cscPath;
                psi.Arguments = finalArgs;
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.CreateNoWindow = true;

                try
                {
                    using (Process p = Process.Start(psi))
                    {
                        string outputLog = p.StandardOutput.ReadToEnd();
                        string errorLog = p.StandardError.ReadToEnd();
                        p.WaitForExit();

                        if (p.ExitCode == 0)
                        {
                            if (config.AutoOpenEnabled) {
                                try { Process.Start("explorer.exe", string.Format("/select,\"{0}\"", output)); } catch {}
                            }
                            int choice = ShowPostCompileOptions(output, null);
                            if (choice == 1) continue; else return;
                        }
                        else
                        {
                            string log = outputLog + "\n" + errorLog;
                            int choice = ShowPostCompileOptions(null, log);
                            if (choice == 1) continue; else return;
                        }
                    }
                }
                catch (Exception ex) 
                { 
                    int choice = ShowPostCompileOptions(null, ex.Message);
                    if (choice == 1) continue; else return;
                }
            }
        }

        // Retorna: 0 para Salir al Menú, 1 para Re-compilar
        static int ShowPostCompileOptions(string createdExePath, string errorLog)
        {
            bool hasExe = !string.IsNullOrEmpty(createdExePath) && File.Exists(createdExePath);
            int localIdx = 0;
            
            while (true)
            {
                Console.Clear();
                DrawHeader(isEnglish ? "COMPILATION RESULT" : "RESULTADO DE COMPILACIÓN");

                if (hasExe)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(isEnglish ? " [STATUS] Success!" : " [ESTADO] ¡Ejecutado correctamente!");
                    Console.ResetColor();
                    Console.WriteLine(string.Format(" Path: {0}", createdExePath));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(isEnglish ? " [STATUS] Error / Compilation Failed" : " [ESTADO] ¡Error de compilación!");
                    Console.ResetColor();
                    if (!string.IsNullOrEmpty(errorLog))
                    {
                        Console.WriteLine("\n--- LOG ---");
                        Console.WriteLine(Truncate(errorLog, 500));
                        Console.WriteLine("-----------\n");
                    }
                }

                List<string> options = new List<string>();
                if (hasExe) options.Add(isEnglish ? "Run executable" : "Ejecutar ejecutable");
                if (hasExe) options.Add(isEnglish ? "Modify (Rename/Move)" : "Modificar (Renombrar/Mover)");
                options.Add(isEnglish ? "Compile another file" : "Compilar otro archivo");
                options.Add(isEnglish ? "Back to Main Menu" : "Volver al menú principal");

                Console.WriteLine(new string('-', 35));
                for (int i = 0; i < options.Count; i++)
                {
                    if (i == localIdx) {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(string.Format(" > {0} ", options[i]));
                    Console.ResetColor();
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow) localIdx = (localIdx == 0) ? options.Count - 1 : localIdx - 1;
                else if (key.Key == ConsoleKey.DownArrow) localIdx = (localIdx == options.Count - 1) ? 0 : localIdx + 1;
                else if (key.Key == ConsoleKey.Enter)
                {
                    string choice = options[localIdx];
                    if (choice.Contains("Run") || choice.Contains("Ejecutar"))
                    {
                        try { Process.Start(createdExePath); }
                        catch (Exception ex) { PrintError(ex.Message); Console.ReadKey(true); }
                    }
                    else if (choice.Contains("Modify") || choice.Contains("Modificar"))
                    {
                        ModifyExecutable(ref createdExePath);
                        hasExe = File.Exists(createdExePath);
                    }
                    else if (choice.Contains("another") || choice.Contains("otro"))
                    {
                        return 1; // Señal de re-compilar
                    }
                    else if (choice.Contains("Back") || choice.Contains("Volver"))
                    {
                        return 0; // Señal de volver al menú
                    }
                }
            }
        }

        static void ModifyExecutable(ref string exePath)
        {
            int modIdx = 0;
            string[] modItemsES = { "Renombrar", "Mover", "Cancelar" };
            string[] modItemsEN = { "Rename", "Move", "Cancel" };

            while (true)
            {
                string[] items = isEnglish ? modItemsEN : modItemsES;
                Console.Clear();
                DrawHeader(isEnglish ? "MODIFY EXECUTABLE" : "MODIFICAR EJECUTABLE");
                Console.WriteLine(string.Format(" Current: {0}\n", exePath));

                for (int i = 0; i < items.Length; i++)
                {
                    if (i == modIdx) {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(string.Format(" {0} ", items[i]));
                    Console.ResetColor();
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow) modIdx = (modIdx == 0) ? items.Length - 1 : modIdx - 1;
                else if (key.Key == ConsoleKey.DownArrow) modIdx = (modIdx == items.Length - 1) ? 0 : modIdx + 1;
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (modIdx == 0) // Rename
                    {
                        Console.Write(isEnglish ? "\n New name (without .exe): " : "\n Nuevo nombre (sin .exe): ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newName))
                        {
                            string dir = Path.GetDirectoryName(exePath);
                            string newPath = Path.Combine(dir, newName + ".exe");
                            try { File.Move(exePath, newPath); exePath = newPath; return; } catch (Exception ex) { PrintError(ex.Message); Console.ReadKey(true); }
                        }
                    }
                    else if (modIdx == 1) // Move
                    {
                        Console.Write(isEnglish ? "\n New folder path: " : "\n Nueva ruta de carpeta: ");
                        string newDir = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newDir))
                        {
                            newDir = newDir.Replace("\"", "").Trim();
                            if (Directory.Exists(newDir)) {
                                string name = Path.GetFileName(exePath);
                                string newPath = Path.Combine(newDir, name);
                                try { File.Move(exePath, newPath); exePath = newPath; return; } catch (Exception ex) { PrintError(ex.Message); Console.ReadKey(true); }
                            } else { PrintError(isEnglish ? "Directory not found." : "Directorio no encontrado."); Console.ReadKey(true); }
                        }
                    }
                    else return;
                }
                else if (key.Key == ConsoleKey.Escape) return;
            }
        }

        static void SaveConfig()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("TargetType.SeAplica = " + config.TargetTypeEnabled);
                sb.AppendLine("TargetType.Arguments = " + config.TargetTypeArg);
                sb.AppendLine("CscPath.SeAplica = " + config.CustomCscEnabled);
                sb.AppendLine("CscPath.Arguments = " + config.CustomCscArg);
                sb.AppendLine("Icon.SeAplica = " + config.IconEnabled);
                sb.AppendLine("Icon.Arguments = " + config.IconArg);
                sb.AppendLine("Optimize.SeAplica = " + config.OptimizeEnabled);
                sb.AppendLine("AutoOpen.SeAplica = " + config.AutoOpenEnabled);
                sb.AppendLine("Language = " + config.Language);
                sb.AppendLine("SavePath = " + config.SavePath);
                File.WriteAllText(configFilePath, sb.ToString());
            }
            catch { }
        }

        static void LoadConfig()
        {
            try
            {
                if (File.Exists(configFilePath))
                {
                    string[] lines = File.ReadAllLines(configFilePath);
                    foreach (string line in lines)
                    {
                        if (!line.Contains("=")) continue;
                        string[] parts = line.Split('=');
                        string key = parts[0].Trim();
                        string val = parts[1].Trim();
                        if (key == "TargetType.SeAplica") config.TargetTypeEnabled = bool.Parse(val);
                        else if (key == "TargetType.Arguments") config.TargetTypeArg = val;
                        else if (key == "CscPath.SeAplica") config.CustomCscEnabled = bool.Parse(val);
                        else if (key == "CscPath.Arguments") config.CustomCscArg = val;
                        else if (key == "Icon.SeAplica") config.IconEnabled = bool.Parse(val);
                        else if (key == "Icon.Arguments") config.IconArg = val;
                        else if (key == "Optimize.SeAplica") config.OptimizeEnabled = bool.Parse(val);
                        else if (key == "AutoOpen.SeAplica") config.AutoOpenEnabled = bool.Parse(val);
                        else if (key == "Language") config.Language = val;
                        else if (key == "SavePath") config.SavePath = val;
                    }
                }
            }
            catch { }
        }

        static void DrawHeader(string text)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔" + new string('═', 58) + "╗");
            int padding = (58 - text.Length) / 2;
            Console.WriteLine("║" + new string(' ', padding) + text + new string(' ', 58 - text.Length - padding) + "║");
            Console.WriteLine("╚" + new string('═', 58) + "╝");
            Console.ResetColor();
        }

        static void PrintError(string m) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(string.Format("\n[!] {0}", m)); Console.ResetColor(); }
        static string Truncate(string value, int max) {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Length <= max) return value;
            return value.Substring(0, max) + "...";
        }
    }
}