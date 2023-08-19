using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json.Nodes;
using Environmate.Apps;
using Microsoft.PowerShell.Commands;

namespace Environmate.Utils
{
    public static class EnvironmentUtils
    {
        // Fields
        // N/A

        // Properties
        // N/A

        // Methods 
        public static void ConfigureWindowsDeveloperEnvironment(ComputerSystem computerSystem)
        {
            // Configure Windows Features
            // Enable developer mode
            // Go through windows settings and dial this in
            // Set taskbar settings
            // Install Fonts
            ConfigureFileExplorer(computerSystem);
            ConfigureCommandPrompt(computerSystem);
            ConfigureCPlusPlus(computerSystem);
            ConfigureDotNetFramework(computerSystem);
            ConfigureWindowsPowerShell(computerSystem);
            ConfigureDotNet(computerSystem);
            ConfigurePowerShell(computerSystem);
            ConfigureWindowsTools(computerSystem);
            ConfigureWSL(computerSystem);
            ConfigureGit(computerSystem);
            ConfigurePodman(computerSystem);
            ConfigureWindowsTerminal(computerSystem);
            ConfigurePython(computerSystem);
            ConfigureNodeJs(computerSystem);
            ConfigureVisualStudio(computerSystem);
            ConfigureVisualStudioCode(computerSystem);
            ConfigureVim(computerSystem);
            // Optional

            ConfigureGolang(computerSystem);
            ConfigureJava(computerSystem);
            ConfigureSublime(computerSystem);
            ConfigureTypora(computerSystem);
        }

        public static void ConfigureWindowsGamingEnvironment(ComputerSystem computerSystem)
        {

        }

        public static void ConfigureWindowsProductivityEnvironment(ComputerSystem computerSystem)
        {

        }

        public static void ConfigureWindowsCreativeEnvironment(ComputerSystem computerSystem)
        {

        }

        public static void ConfigureWindowsPrivacyEnvironment(ComputerSystem computerSystem)
        {

        }

        public static void ConfigureCommandPrompt(ComputerSystem computerSystem)
        {
            string cmdProfile = @"C:\Users\codyl\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\profile.bat";
            File.Create(cmdProfile).Dispose();
            string contents = "@echo off\r\n\r\nREM Aliases\r\n\r\n";
            File.WriteAllText(cmdProfile, contents);
            List<string> profileContents = File.ReadAllLines(cmdProfile).ToList();
            int aliasesLineIndex = profileContents.IndexOf("REM Aliases");
            profileContents.Insert(aliasesLineIndex, @$"doskey ls=dir $*");
            File.WriteAllLines(cmdProfile, profileContents);
        }

        public static void ConfigureFileExplorer(ComputerSystem computerSystem)
        {

        }

        public static void ConfigureCPlusPlus(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Microsoft.VCRedist.2015+.x64");
            psHost.Run("winget install --id Microsoft.VCRedist.2015+.x86");
            // Install CMake
            // Install Clang compiler
            // Install GCC compiler
        }

        public static void ConfigureDotNetFramework(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Microsoft.DotNet.Framework.DeveloperPack_4");
            // add msbuild to the path if needed
        }

        public static void ConfigureWindowsPowerShell(ComputerSystem computerSystem)
        {
            string psProfile = @$"C:\Users\{Environment.UserName}\Documents\WindowsPowerShell\profile.ps1";
            File.Create(psProfile).Dispose();
            string contents = "# Modules\r\n\r\n\r\n# Aliases\r\n\r\n";
            File.WriteAllText(psProfile, contents);
            List<string> profileContents = File.ReadAllLines(psProfile).ToList();
            int modulesLineIndex = profileContents.IndexOf("# Modules");
            int aliasesLineIndex = profileContents.IndexOf("# Aliases");
            profileContents.Insert(modulesLineIndex, @$"oh-my-posh init pwsh --config ""$env:POSH_THEMES_PATH\powerline.omp.json"" | Invoke-Expression");
            profileContents.Insert(modulesLineIndex, @$"Import-Module -Name Terminal-Icons");
            profileContents.Insert(modulesLineIndex, @$"Import-Module -Name posh-git");
            File.WriteAllLines(psProfile, profileContents);
            // Change the font
            // Change the execution policy
        }

        public static void ConfigureDotNet(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Microsoft.DotNet.SDK.7");
            // add dotnet.exe to the path if needed
        }

        public static void ConfigurePowerShell(ComputerSystem computerSystem)
        {
            string username = Environment.UserName;
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Microsoft.PowerShell");
            //psHost.Run("winget install --id SAPIEN.PowerShellStudio");?
            psHost.Run("winget install --id JanDeDobbeleer.OhMyPosh");
            psHost.Run("Install-Module -Name TerminalIcons -Scope CurrentUser");
            psHost.Run("Install-Module -Name posh-git -Scope CurrentUser");
            string psProfile = @$"C:\Users\{username}\Documents\PowerShell\profile.ps1";
            File.Create(psProfile).Dispose();
            string contents = "# Modules\r\n\r\n\r\n# Aliases\r\n\r\n";
            File.WriteAllText(psProfile, contents);
            List<string> profileContents = File.ReadAllLines(psProfile).ToList();
            int modulesLineIndex = profileContents.IndexOf("# Modules");
            int aliasesLineIndex = profileContents.IndexOf("# Aliases");
            profileContents.Insert(modulesLineIndex, @$"oh-my-posh init pwsh --config ""$env:POSH_THEMES_PATH\powerline.omp.json"" | Invoke-Expression");
            profileContents.Insert(modulesLineIndex, @$"Import-Module -Name Terminal-Icons");
            profileContents.Insert(modulesLineIndex, @$"Import-Module -Name posh-git");
            File.WriteAllLines(psProfile, profileContents);
            // Change the font
            // Change the execution policy
        }

        public static void ConfigureNotepadPlusPlus(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Notepad++.Notepad++");

            // Enable multiple cursors (Settings / Preferences / Editing)
            // Go through app and decide on customizations
        }

        public static void ConfigureWindowsTools(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            // Go through these tools and decide which ones are worth including
            ConfigureNotepadPlusPlus(computerSystem);
            // () => Install the following applications
            // Revo Uninstaller
            psHost.Run("winget install --id RevoUninstaller.RevoUninstaller");
            // LibreOffice
            psHost.Run("winget install --id TheDocumentFoundation.LibreOffice.LTS");
            // qBittorrent
            psHost.Run("winget install --id qBittorrent.qBittorrent");
            // Adobe Acrobat Reader DC
            psHost.Run("winget install --id Adobe.Acrobat.Reader.64-bit");
            // X-Mouse
            psHost.Run("winget install --id Highresolution.X-MouseButtonControl");
            // MSI-Afterburner
            // N/A
            // VLC
            psHost.Run("winget install --id VideoLan.VLC");
            // 7-Zip
            psHost.Run("winget install --id 7zip.7zip");
            // Malwarebytes
            psHost.Run("winget install --id Malwarebytes.Malwarebytes");
            // WinDirStat
            psHost.Run("winget install --id WinDirStat.WinDirStat");
            // F.lux
            psHost.Run("winget install --id flux.flux");
            // TeraCopy
            psHost.Run("winget install --id CodeSector.TeraCopy");
            // Glary? or other system optimizer
            psHost.Run("winget install --id Glarysoft.GlaryUtilities");
            // Everything

            // DBeaver?
            psHost.Run("winget install --id dbeaver.dbeaver");
            // Zotero?
            psHost.Run("winget install --id DigitalScholar.Zotero");
            // Remote Desktop Client?
            // Recovery Software?
        }

        public static void ConfigureWSL(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Canonical.Ubuntu.2204");
            // psHost.Run("wsl --install");

            // () => Need UbuntuHost
            // ubuntuHost.Run("sudo apt update")
            // ubuntuHost.Run("sudo apt upgrade --all")
            // Git
            // SDKMan
            // NVM
            // Python
            // GoLang
        }

        public static void ConfigureGit(ComputerSystem computerSystem)
        {
            // Go through settings and lock down a configuration
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Git.Git");
            // Go through these tools and decide if any should be included
            //psHost.Run("winget install --id GitHub.GitLFS");
            //psHost.Run("winget install --id GitHub.GitHubDesktop");
            //psHost.Run("winget install --id Atlassian.Sourcetree");
            //psHost.Run("winget install --id Microsoft.VFSforGit");

            // Enable symlinks
            // Configure SSH
            // Set "Main" to default branch
            // Set username
            // Set email
            // Also install for WSL
        }

        public static void ConfigurePodman(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id RedHat.Podman-Desktop");
            //psHost.Run("winget install --id RedHat.Podman");?
        }

        public static void ConfigureWindowsTerminal(ComputerSystem computerSystem)
        {
            // Go through and finalize settings
            
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Microsoft.WindowsTerminal");
            
            // Install fonts
            string hackFontUri = @"https://github.com/ryanoasis/nerd-fonts/releases/download/v3.0.2/Hack.zip";
            string hackFontFile = @$"C:\Users\{Environment.UserName}\Downloads\Hack.zip";
            string hackFontDirectory = hackFontFile.Replace(".zip", @"\", StringComparison.OrdinalIgnoreCase);
            psHost.Run(@$"Invoke-WebRequest -Uri {hackFontUri} -OutFile {hackFontFile}");
            ZipFile.ExtractToDirectory(hackFontFile, hackFontDirectory);
            //var hackFonts = Directory.GetFiles(@"\\gpauto\automation\GPAutoStatic\OfflineInstallers\Resources\Fonts\Hack\").Where(x => x.Contains(".tff", StringComparison.OrdinalIgnoreCase));
            //foreach (var font in hackFonts)
            //{
            //    var fontFileInfo = new FileInfo(font);
            //    File.Copy(fontFileInfo.FullName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", fontFileInfo.Name));
            //    RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
            //    string keyName = Path.GetFileNameWithoutExtension(fontFileInfo.Name) + " (TrueType)";
            //    key.SetValue(keyName, fontFileInfo.Name);
            //    key.Close();
            //}
            File.Delete(hackFontFile);
            Directory.Delete(hackFontDirectory);
            // Configure Profile
            string terminalProfile = @$"C:\Users\{Environment.UserName}\AppData\Local\Packages\Microsoft.WindowsTerminal.8wekyb3d8bbwe\LocalState\settings.json";
            string jsonContent = File.ReadAllText(terminalProfile);
            // Parse profile
            JsonNode json = JsonNode.Parse(jsonContent)!;
            JsonArray actions = json["actions"]!.AsArray();
            JsonArray profilesList = json["profiles"]!["list"]!.AsArray();
            JsonNode? cmdProfile = profilesList.Where(x => x!["name"]!.GetValue<string>() == "Command Prompt").First();
            JsonNode? windowsPowerShellProfile = profilesList.Where(x => x!["name"]!.GetValue<string>() == "Windows PowerShell").First();
            JsonNode? powershellProfile = profilesList.Where(x => x!["name"]!.GetValue<string>() == "PowerShell")?.First();
            JsonNode? ubuntuProfile = profilesList.Where(x => x!["name"]!.GetValue<string>() == "Ubuntu")?.First();
            JsonArray schemes = json["schemes"]!.AsArray();
            JsonArray themes = json["themes"]!.AsArray();
            // Edit profile
            json["defaultProfile"] = powershellProfile?["guid"]!.GetValue<string>() ?? cmdProfile?["guid"]!.GetValue<string>()!;
            json["profiles"]!["defaults"]!["font"]!["face"] = "Hack NFM";
            profilesList = new JsonArray()
            {
                cmdProfile,
                windowsPowerShellProfile,
                powershellProfile,
                ubuntuProfile
            };
            json["profiles"]!["list"] = profilesList;
            // Save
            jsonContent = json.ToJsonString();
            File.WriteAllText(terminalProfile, jsonContent);
        }

        public static void ConfigurePython(ComputerSystem computerSystem)
        {
            using PowerShellHost powerShellHost = new PowerShellHost();
            string username = Environment.UserName;
            // () => Call Configure Command Prompt
            // () => Call Configure Windows PowerShell
            // () => Call Configure PowerShell
            // Install MambaForge Conditionally (else update)
            powerShellHost.Run("winget install --id CondaForge.Mambaforge");
            // Initialize Conda and Mamba
            string powerShellProfile = $@"C:\Users\{username}\Documents\PowerShell\profile.ps1";
            List<string> profileContents = File.ReadAllLines(powerShellProfile).ToList();
            int aliasLineIndex = profileContents.IndexOf("# Aliases");
            profileContents.Insert(aliasLineIndex, @$"Set-Alias conda ""C:\Users\{username}\mambaforge3\Scripts\conda.exe""");
            profileContents.Insert(aliasLineIndex, @$"Set-Alias mamba ""C:\Users\{username}\mambaforge3\Scripts\mamba.exe""");
            File.WriteAllLines(powerShellProfile, profileContents);
            // () => see if you have to call conda init in cmd and if you have to add aliases there
            // () => see if you have to call conda init in Windows PowerShell
            powerShellHost.Run(@"conda init");
            powerShellHost.Restart();
            powerShellHost.Run(@"mamba init");
            powerShellHost.Restart();
            powerShellHost.Run(@"mamba upgrade --all");
            // Create a dev environment
            string devCondaEnvironment = @"";
            powerShellHost.Run(@$"conda env create -f {devCondaEnvironment}");
            // () => might have to do configuration for ipython and jupyter notebook
            // () => might be worth assessing the editor(s) you want to use with Python
            // Install pycharm

            // May need to have a python.exe registered to the path
        }

        public static void ConfigureNodeJs(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id CoreyButler.NVMforWindows");
            // This will take some work as you are unfamiliar with NodeJs
        }

        public static void ConfigureJava(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id EclipseAdoptium.Temurin.17.JDK");
            // IntelliJ IDEA
            // Eclipse
            // This will take some work as you are unfamiliar with Java
        }

        public static void ConfigureGolang(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            //psHost.Run("winget install --id GoLang.Go");?
            // This will take some work as you are unfamiliar with GoLang
        }

        public static void ConfigureVisualStudio(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Microsoft.VisualStudio.2022.Community");
            string visualStudioInstaller = @"C:\Program Files (x86)\Microsoft Visual Studio\Installer\setup.exe";
            string responseFile = @"Resources\response.json";

            // Go through deployment documentation and dial in an unattended installation
            // Call the visual studio installer with the response file

            // Either add to the path or create aliases for useful executables

            // Look into the ability to customize the way the app looks from here
        }

        public static void ConfigureVisualStudioCode(ComputerSystem computerSystem)
        {
            // () => Need to figure out how profile names are associated with ids
            // () => Need to figure out how workspaces are associated with ids
            // () => Need to figure out how to install extensions programmatically
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Microsoft.VisualStudioCode");
            // Customize settings
            string settings = @$"C:\Users\{Environment.UserName}\AppData\Roaming\Code\User\settings.json";
            string keybindings = @$"C:\Users\{Environment.UserName}\AppData\Roaming\Code\User\keybindings.json";
            string preferences = @$"C:\Users\{Environment.UserName}\AppData\Roaming\Code\Preferences";
            string storage = @$"C:\Users\{Environment.UserName}\AppData\Roaming\Code\User\globalStorage\storage.json";
            string workspaceStorageDir = $@"C:\Users\{Environment.UserName}\AppData\Roaming\Code\User\workspaceStorage\";
            string extensionsDir = $@"C:\Users\{Environment.UserName}\.vscode\extensions\";
            string profilesDir = $@"C:\Users\codyl\AppData\Roaming\Code\User\profiles\";
            
            string jsonContent = File.ReadAllText(settings);
            JsonNode json = JsonNode.Parse(jsonContent)!;
            // Add custom settings
            jsonContent = json.ToJsonString();
            File.WriteAllText(settings, jsonContent);
        }

        public static void ConfigureVim(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id vim.vim");
            psHost.Run("winget install --id Neovim.Neovim");
            // Learn Vim
            // Decide on Vim or Neovim
        }

        public static void ConfigureTypora(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id appmakes.Typora");

            // Assess whether this is worth keeping in your stack as it is not free
        }

        public static void ConfigureSublime(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id SublimeHQ.SublimeText.4");
            //psHost.Run("winget install --id SublimeHQ.SublimeText.4.Portable");
            // Assess whether this is worth keeping in your stack as it is not free
        }

        

        public static void ConfigureEdge(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Microsoft.Edge");
            // For searching
        }

        public static void ConfigureChrome(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Google.Chrome");
            // For developer tools
        }

        public static void ConfigureFirefox(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id Mozilla.Firefox");
            // For personal browsing
        }





















        // Possibly include standard note?

        public static void SetupFerdium(ComputerSystem computerSystem)
        {

        }

        public static void SetupZotero(ComputerSystem computerSystem)
        {
            using PowerShellHost psHost = new PowerShellHost();
            psHost.Run("winget install --id DigitalScholar.Zotero");
            psHost.Run("winget install --id DigitalScholar.Tropy");
        }

        public static void SetupTeams(ComputerSystem computerSystem)
        {

        }

        public static void SetupSlack(ComputerSystem computerSystem)
        {

        }

        public static void SetupDiscord(ComputerSystem computerSystem)
        {

        }

        public static void SetupZoom(ComputerSystem computerSystem)
        {

        }

        public static void SetupDropbox(ComputerSystem computerSystem)
        {
            // Need cloud storage for 1. personal files 2. temporary files 3. Photo and Video 4. Computer Backups
        }

        public static void SetupOneDrive(ComputerSystem computerSystem)
        {

        }

        public static void SetupOBS(ComputerSystem computerSystem)
        {

        }

        public static void SetupMicrosoftOffice(ComputerSystem computerSystem)
        {

        }

        public static void SetupLogseq(ComputerSystem computerSystem)
        {
            // Local Variables
            string registryQuery = "Logseq";
        }

        public static void SetupSpotify(ComputerSystem computerSystem)
        {
            // Local Variables
            string registryQuery = "Spotify";
        }
    }
}
