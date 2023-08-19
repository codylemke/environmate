using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Environmate
{
    public static class FileSystemUtils
    {
        // Fields
        // N/A

        // Properties
        // N/A

        // Methods
        public static bool IdenticalFiles(string sourceFile, string targetFile)
        {
            // Compare file names
            string sourceFileName = Path.GetFileName(sourceFile);
            string targetFileName = Path.GetFileName(targetFile);
            if (sourceFileName != targetFileName)
            {
                return false;
            }
            // Compare files sizes
            long sourceFileSize = new FileInfo(sourceFile).Length;
            long targetFileSize = new FileInfo(targetFile).Length;
            if (sourceFileSize != targetFileSize)
            {
                return false;
            }
            // Hash and compare file contents
            byte[] sourceFileContents = File.ReadAllBytes(sourceFile);
            byte[] targetFileContents = File.ReadAllBytes(targetFile);
            byte[] sourceFileHash = SHA256.HashData(sourceFileContents);
            byte[] targetFileHash = SHA256.HashData(targetFileContents);
            if (sourceFileHash != targetFileHash)
            {
                return false;
            }
            // Files are identical
            else
            {
                return true;
            }
        }

        public static Collection<string> GetFilesRecursively(string directory)
        {
            var files = new List<string>();
            foreach (string entry in Directory.GetFileSystemEntries(directory))
            {
                if (File.Exists(entry))
                {
                    files.Add(entry);
                }
                else if (Directory.Exists(entry))
                {
                    files.AddRange(GetFilesRecursively(entry));
                }
            }
            return new Collection<string>(files);
        }

        public static bool IdenticalFileInDirectory(string sourceFile, string targetDirectory)
        {
            foreach (string targetFile in GetFilesRecursively(targetDirectory))
            {
                if (IdenticalFiles(sourceFile, targetFile))
                {
                    Console.WriteLine($"Identical file to {sourceFile} was found in {targetDirectory}");
                    return true;
                }
            }
            Console.WriteLine($"{sourceFile} was not found in {targetDirectory}");
            return false;
        }

        public static void CreateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                string parentDirectory = Path.GetDirectoryName(directoryPath);
                if (!string.IsNullOrEmpty(parentDirectory))
                {
                    CreateDirectory(parentDirectory);
                }
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static bool PathIsSymlink(string path)
        {
            return new FileInfo(path).Attributes.HasFlag(FileAttributes.ReparsePoint);
        }

        public static void MoveFile(string sourceFile, string targetDirectory, string? searchDirectory = null)
        {
            if (string.IsNullOrWhiteSpace(searchDirectory))
            {
                searchDirectory = targetDirectory;
            }
            if (IdenticalFileInDirectory(sourceFile, searchDirectory))
            {
                File.Delete(sourceFile);
                Console.WriteLine($"{sourceFile} was deleted due to an identical file in the search directory being found.");
            }
            else
            {
                CreateDirectory(targetDirectory);
                string fileName = Path.GetFileName(sourceFile);
                string targetFile = Path.Combine(targetDirectory, fileName);
                if (File.Exists(targetFile))
                {
                    // Generate new file name
                    string newFile = new string(targetFile);
                    while (File.Exists(newFile))
                    {
                        Match fileNumberMatch = Regex.Match(fileName, @" \((\d+)\)$");
                        if (!fileNumberMatch.Success)
                        {
                            newFile = Path.Combine(targetDirectory, fileName + " (1)");
                        }
                        else
                        {
                            string fileNumberText = fileNumberMatch.Value;
                            int fileNumber = int.Parse(fileNumberMatch.Groups[0].Value, CultureInfo.InvariantCulture);
                            string fileNameNoNumber = fileName.Replace(fileNumberText, "", StringComparison.OrdinalIgnoreCase);
                            string newFileName = fileNameNoNumber + $" ({fileNumber + 1})";
                            newFile = Path.Combine(targetDirectory, newFileName);
                        }
                    }
                    while (true)
                    {
                        string userInput;
                        FileInfo sourceFileInfo = new FileInfo(sourceFile);
                        FileInfo targetFileInfo = new FileInfo(targetFile);
                        string userPrompt = $@"
                            A file with the same name as the source file ""{fileName}"" already exists in the target directory ""{targetDirectory}"".
                            Which file would you like to keep?
                            [1] - Source File : Date Last Modified : { sourceFileInfo.LastWriteTime }, Size: { sourceFileInfo.Length }
                            [2] - Existing File : Date Last Modified : { targetFileInfo.LastWriteTime }, Size: { targetFileInfo.Length }
                            [3] - Both : Source File => { Path.GetFileName(newFile) }
                        ";
                        string formattedUserPrompt = Regex.Replace(userPrompt, @"^\s*", string.Empty, RegexOptions.Multiline);
                        Console.WriteLine("\n");
                        Console.Write(formattedUserPrompt);
                        Console.Write("Input:  ");
                        userInput = Console.ReadLine();
                        Console.WriteLine(userInput);
                        switch (userInput)
                        {
                            case "1":
                                File.Delete(targetFile);
                                File.Move(sourceFile, targetFile);
                                break;
                            case "2":
                                File.Delete(sourceFile);
                                break;
                            case "3":
                                File.Move(sourceFile, newFile, false);
                                break;
                            default: continue;
                        }
                        break;
                    }
                }
                else
                {
                    File.Move(sourceFile, targetFile);
                }
            }
        }

        public static void MoveDirectoryContents(string sourceDirectory, string targetDirectory, string? searchDirectory = null)
        {
            CreateDirectory(targetDirectory);
            foreach (string sourceFile in GetFilesRecursively(sourceDirectory))
            {
                string relativeSourcePath = sourceFile.Replace(sourceDirectory, "", StringComparison.OrdinalIgnoreCase);
                string relativeTargetDirectory = Path.Combine(targetDirectory, relativeSourcePath);
                MoveFile(sourceFile, relativeTargetDirectory, searchDirectory);
            }
        }

        public static void CreateSymlink(string symlinkPath, string targetPath, string symlinkType)
        {
            
            if (symlinkType == "File")
            {
                if (!Path.Exists(targetPath))
                {
                    throw new ArgumentException($"The target file does not exist: {targetPath}");
                }
                if (Path.Exists(symlinkPath))
                {
                    if (File.Exists(symlinkPath))
                    {
                        FileSystemInfo? fileInfo = File.ResolveLinkTarget(symlinkPath, false);
                        if (fileInfo == null || fileInfo.LinkTarget != targetPath)
                        {
                            File.Delete(symlinkPath);
                            File.CreateSymbolicLink(symlinkPath, targetPath);
                        }
                    }
                    else if (Directory.Exists(symlinkPath))
                    {
                        Directory.Delete(symlinkPath, true);
                        File.CreateSymbolicLink(symlinkPath, targetPath);
                    }
                }
                else
                {
                    File.CreateSymbolicLink(symlinkPath, targetPath);
                }
            }
            else if (symlinkType == "Directory")
            {
                if (!Path.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                if (Path.Exists(symlinkPath))
                {
                    if (Directory.Exists(symlinkPath))
                    {
                        FileSystemInfo? directoryInfo = Directory.ResolveLinkTarget(symlinkPath, false);
                        if (directoryInfo == null || directoryInfo.LinkTarget != targetPath)
                        {
                            Directory.Delete(symlinkPath, true);
                            Directory.CreateSymbolicLink(symlinkPath, targetPath);
                        }
                    }
                    else if (File.Exists(symlinkPath))
                    {
                        File.Delete(symlinkPath);
                        Directory.CreateSymbolicLink(symlinkPath, targetPath);
                    }
                }
                else
                {
                    Directory.CreateSymbolicLink(symlinkPath, targetPath);
                }
            }
        }
    }
}
