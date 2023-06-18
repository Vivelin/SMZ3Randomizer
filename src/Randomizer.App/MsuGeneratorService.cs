﻿using System;
using System.IO;
using System.Linq;

namespace Randomizer.App;

public class MsuGeneratorService
{
    /// <summary>
    /// Enables MSU support for a rom
    /// </summary>
    /// <param name="msuPath">The path to the msu file</param>
    /// <param name="romPath">The path to the rom file</param>
    /// <param name="error">Any error that was ran into when updating the rom</param>
    /// <returns>True if successful, false otherwise</returns>
    public bool EnableMsu1Support(string msuPath, string romPath, out string error)
    {
        if (!File.Exists(msuPath))
        {
            error = "";
            return false;
        }

        var romDrive = Path.GetPathRoot(romPath);
        var msuDrive = Path.GetPathRoot(msuPath);
        if (romDrive?.Equals(msuDrive, StringComparison.OrdinalIgnoreCase) == false)
        {
            error = $"Due to technical limitations, the MSU-1 " +
                $"pack and the ROM need to be on the same drive. MSU-1 " +
                $"support cannot be enabled.\n\nPlease move or copy the MSU-1 " +
                $"files to somewhere on {romDrive}, or change the ROM output " +
                $"folder setting to be on the {msuDrive} drive.";
            return false;
        }

        var romFolder = Path.GetDirectoryName(romPath);
        var msuFolder = Path.GetDirectoryName(msuPath);
        var romBaseName = Path.GetFileNameWithoutExtension(romPath);
        var msuBaseName = Path.GetFileNameWithoutExtension(msuPath);

        var swap = false;

        var hasTrack133 = File.Exists(msuPath.Replace(".msu", "-133.pcm"));
        var hasTrack41 = File.Exists(msuPath.Replace(".msu", "-41.pcm"));
        var hasTrack141 = File.Exists(msuPath.Replace(".msu", "-141.pcm"));

        // Swap if we see the Skull Woods theme above 100
        if (hasTrack141)
        {
            swap = true;
        }
        // Swap if we don't see the Skull Woods theme above 100 and if either the Z3 epilogue or SM credits themes are incorrectly set to loop
        else if (!hasTrack41 && hasTrack133 && (DoesPCMLoop(msuPath.Replace(".msu", "-33.pcm")) || DoesPCMLoop(msuPath.Replace(".msu", "-130.pcm"))))
        {
            swap = true;
        }

        // Copy pcm files
        foreach (var msuFile in Directory.EnumerateFiles(msuFolder!, $"{msuBaseName}*"))
        {
            var fileName = Path.GetFileName(msuFile);
            var suffix = fileName.Replace(msuBaseName, "");
            var link = Path.Combine(romFolder!, romBaseName + suffix);

            // Swap SM and Z3 tracks if needed
            if (swap && suffix.Contains(".pcm"))
            {
                if (int.TryParse(suffix.Replace("-", "").Replace(".pcm", ""), out var trackNumber) && trackNumber > 0)
                {
                    // Skip track 99 (SMZ3 combined credits) as it doesn't change
                    if (trackNumber < 99)
                    {
                        trackNumber += 100;
                    }
                    else if (trackNumber > 99)
                    {
                        trackNumber -= 100;
                    }

                    var oldLink = link;
                    link = Path.Combine(romFolder!, romBaseName + "-" + trackNumber + ".pcm");
                }
            }
            NativeMethods.CreateHardLink(link, msuFile, IntPtr.Zero);
        }

        error = "";
        return true;
    }

    private static bool DoesPCMLoop(string fileName)
    {
        using var fileStream = File.OpenRead(fileName);
        fileStream.Seek(4, SeekOrigin.Begin);
        var bytes = new byte[4];
        fileStream.Read(bytes, 0, 4);
        return bytes.Any(x => x != 0);
    }

}
