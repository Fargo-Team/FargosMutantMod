using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using static Fargowiltas.Common.Systems.Shaders.ShaderCompileManager;

namespace Fargowiltas.Common.Systems.Shaders
{
    public class ShaderCompileManager : ModSystem
    {
        /// <summary>
        /// Internal class containing information of files to compile
        /// </summary>
        internal class CompileFile
        {
            public string fxPath;
            public string compilePath;
            public bool filter;
            public bool startedCompiling;
            public bool finishedCompiling;
            public bool doneProcessing;

            public CompileFile(string fxPath, bool filter)
            {
                this.fxPath = fxPath;
                compilePath = Path.Combine(CompileArea, Path.GetFileName(fxPath));
                this.filter = filter;
                startedCompiling = false;
                finishedCompiling = false;
                doneProcessing = false;
            }
            public string ToFXC(bool compDir) => (compDir ? compilePath : fxPath).Replace(".fx", ".fxc");
        }

        public override void PostUpdateEverything()
        {
            base.PostUpdateEverything();
            foreach (var watcher in fileWatchers)
            {
                ProcessWatcher(watcher);
            }
        }

        /// <summary>
        /// Directory containing the executable compiler
        /// </summary>
        private static string CompileDirectory => Path.Combine(Main.SavePath, "FXC");

        /// <summary>
        /// Directory where files are stored for compilation
        /// </summary>
        private static string CompileArea => Path.Combine(Path.Combine($"{Path.Combine(Program.SavePathShared, "ModSources")}\\Fargowiltas".Replace("\\..\\tModLoader", string.Empty), "Assets\\AutoloadedEffects\\Compiler"));

        /// <summary>
        /// List of files marked for compilation
        /// </summary>
        private static List<CompileFile> MarkedForCompile = new List<CompileFile>();

        #region Watchers
        /// <summary>
        /// Reponsible for watching when files are changed
        /// </summary>
        private record Watcher(Mod Mod, string Directory, bool Filter, FileSystemWatcher FSW);
        private static List<Watcher> fileWatchers = new List<Watcher>();
        
        /// <summary>
        /// Loads file watchers for the given mod
        /// </summary>
        /// <param name="mod"></param>
        public static void LoadWatchers(Mod mod)
        {
            string modSourcesPath = $"{Path.Combine(Program.SavePathShared, "ModSources")}\\{mod.Name}".Replace("\\..\\tModLoader", string.Empty);
            if (!Directory.Exists(modSourcesPath))
                return;

            string shadersPath = $"{modSourcesPath}\\Assets\\AutoloadedEffects\\Shaders";
            if (Directory.Exists(shadersPath))
                CreateWatcher(mod, shadersPath, false);

            string filtersPath = $"{modSourcesPath}\\Assets\\AutoloadedEffects\\Filters";
            if (Directory.Exists(filtersPath))
                CreateWatcher(mod, filtersPath, false);
        }

        /// <summary>
        /// Creates a file watcher instance
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        public static void CreateWatcher(Mod mod, string dir, bool filter)
        {
            if (!Directory.Exists(dir))
                return;

            FileSystemWatcher newFSW = new(dir)
            {
                Filter = "*.fx",
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Attributes | NotifyFilters.LastAccess | NotifyFilters.FileName | NotifyFilters.Security | NotifyFilters.CreationTime
            };
            newFSW.Changed += WatcherChange;
            fileWatchers.Add(new(mod, dir, filter, newFSW));
        }

        /// <summary>
        /// Called by a file watcher whenever a file is changed. <para/>
        /// Responsible for marking the changed file for compilation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void WatcherChange(object sender, FileSystemEventArgs args)
        {
            if (Main.gameMenu || args.FullPath.Contains("\\Compiler") || MarkedForCompile.Any(f => f.fxPath == args.FullPath))
                return;
            MarkForCompilation(args.FullPath, args.FullPath.Contains("\\Filters"));
        }

        /// <summary>
        /// Processes files watched by the file watcher
        /// </summary>
        /// <param name="watcher"></param>
        private static void ProcessWatcher(Watcher watcher)
        {
            List<CompileFile> files = [.. MarkedForCompile];
            foreach (var file in files)
            {
                if (!file.fxPath.Contains(watcher.Directory))
                    continue;
                if (!file.startedCompiling)
                    Compile(file);
                else if (file.finishedCompiling && !file.doneProcessing)
                    FinishFile(file, watcher);

            }
        }
        #endregion

        #region Compile Process
        /// <summary>
        /// Tells the watcher to compile the file at the given path
        /// </summary>
        public static void MarkForCompilation(string fxPath, bool filter)
        {
            if (MarkedForCompile.Any(f => f.fxPath == fxPath))
                return;
            CompileFile cFile = new(fxPath, filter);
            MarkedForCompile.Add(cFile);
        }

        /// <summary>
        /// (Re)compiles the given file
        /// </summary>
        private static void Compile(CompileFile file)
        {
            Fargowiltas.Instance.Logger.Info($"Recompiling shader file '{Path.GetFileName(file.fxPath)}'");
            file.startedCompiling = true;
            SendToCompileDirectory(file);
            string fxPath = file.fxPath;
            string compilerPath = file.compilePath;
            if (Path.GetExtension(fxPath) != ".fx")
                return;

            string outputPath = compilerPath.Replace(".fx", ".fxc");
            string compileCommand = $"/T fx_2_0 \"{compilerPath}\" /Fo \"{outputPath}\"";

            Process compiler = new()
            {
                StartInfo = new($"{CompileDirectory}\\fxc.exe")
                {
                    WorkingDirectory = CompileDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Arguments = compileCommand
                }
            };
            compiler.Start();
            if (!compiler.WaitForExit(5000))
            {
                // your taking too long
                Main.NewText("Compile: Timeout", Color.Red);
                compiler.Kill();
                return;
            }

            // Report errors
            string stderr = compiler.StandardError.ReadToEnd();
            if (stderr?.Length > 0)
            {
                string[] errs = stderr.Split('\n');
                foreach (string err in errs)
                {
                    Main.NewText(stderr, Color.Red);
                }
            }
            compiler.Kill();
            file.finishedCompiling = true;
        }

        /// <summary>
        /// Sends the file to the compile directory for compilation
        /// </summary>
        /// <param name="file"></param>
        private static void SendToCompileDirectory(CompileFile file)
        {
            try
            {
                File.Delete(file.compilePath);
            }
            catch { }
            try
            {
                File.WriteAllText(file.compilePath, File.ReadAllText(file.fxPath));
            }
            catch { }
        }

        /// <summary>
        /// Completes the file change process after compilation
        /// </summary>
        /// <param name="file"></param>
        /// <param name="watcher"></param>
        private static void FinishFile(CompileFile file, Watcher watcher)
        {
            Watcher fileWatcher = fileWatchers.Find(f => f.Directory == Path.GetDirectoryName(file.fxPath));
            if (fileWatcher == null)
                return;

            // Check if the file actually compiled
            if (!File.Exists(file.ToFXC(true)))
            {
                Main.NewText($"Failed to compile file '{Path.GetFileName(file.fxPath)}': compiled file not found");
                File.Delete(file.compilePath);
                MarkedForCompile.Remove(file);
                return;
            }

            // Delete old file and copy from compiler
            File.Delete(file.ToFXC(false));
            try
            {
                File.Copy(file.ToFXC(true), file.ToFXC(false));
            }
            catch { }
            finally
            {
                // delete all compiler files
                File.Delete(file.ToFXC(true));
                File.Delete(file.compilePath);
            }
            file.doneProcessing = true;
            string modName = fileWatcher.Mod.Name;

            Main.QueueMainThreadAction(() =>
            {
                string id = $"{modName}:{Path.GetFileNameWithoutExtension(file.fxPath)}";
                FileStream shaderFileData = new FileStream(file.ToFXC(false), FileMode.Open);
                MemoryStream shaderData = new();
                shaderFileData.CopyTo(shaderData);

                Ref<Effect> newEffect = new(new(Main.instance.GraphicsDevice, shaderData.ToArray()));

                if (file.filter)
                {
                    // Update filter (if exists)
                    if (ShaderSystem.Filters.TryGetValue(id, out var filter))
                    {
                        filter.Effect = newEffect;
                        filter.Parameters.Clear();
                    }
                    else
                    {
                        ShaderSystem.SetFilter(id, new(newEffect));
                    }
                }
                else
                {
                    // Update shader (if exists)
                    Dictionary<string, object> paras = [];
                    if (ShaderSystem.Shaders.TryGetValue(id, out var shader))
                        paras = shader.Parameters;

                    ShaderSystem.SetShader(id, new(newEffect));

                    // Transfer parameters to the new shader
                    FargoShader s = ShaderSystem.Shaders[id];
                    foreach (var kv in paras)
                        s.TrySetParameter(kv.Key, kv.Value);
                }

                Main.NewText($"Shader file '{Path.GetFileName(file.fxPath)}' successfully recompiled.");
                MarkedForCompile.Remove(file);
            });
        }
        #endregion
    }
}
