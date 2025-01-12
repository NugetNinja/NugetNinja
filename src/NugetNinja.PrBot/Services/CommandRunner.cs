﻿

using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Aiursoft.NugetNinja.PrBot;

public class CommandRunner
{
    private readonly ILogger<CommandRunner> _logger;

    public CommandRunner(ILogger<CommandRunner> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Run git command.
    /// </summary>
    /// <param name="path">Path</param>
    /// <param name="arguments">Arguments</param>
    /// <param name="integrateResultInProcess">integrateResultInProcess</param>
    /// <returns>Task</returns>
    public async Task<string> RunGit(string path, string arguments, bool integrateResultInProcess = true)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                WindowStyle = integrateResultInProcess ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Minimized,
                UseShellExecute = false,
                CreateNoWindow = integrateResultInProcess,
                RedirectStandardOutput = integrateResultInProcess,
                RedirectStandardError = integrateResultInProcess,
                WorkingDirectory = path
            }
        };

        _logger.LogTrace($"Running command: {path.TrimEnd('\\').Trim()} git {arguments}");

        try
        {
            process.Start();
        }
        catch (Win32Exception)
        {
            throw new GitCommandException(
                "Start Git failed! Please install Git at https://git-scm.com .",
                command: arguments,
                result: "Start git failed.",
                path: path);
        }

        await Task.Run(process.WaitForExit);

        if (!integrateResultInProcess)
        {
            return string.Empty;
        }

        var consoleOutput = string.Empty;
        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();
        if (
            output.Contains("'git-lfs' was not found") ||
            error.Contains("'git-lfs' was not found") ||
            output.Contains("git-lfs: command not found") ||
            error.Contains("git-lfs: command not found"))
        {
            throw new GitCommandException(
                "Start Git failed! Git LFS not found!",
                command: arguments,
                result: "Start git failed.",
                path: path);
        }

        if (!string.IsNullOrWhiteSpace(error))
        {
            consoleOutput = error;
            if (error.Contains("fatal") || error.Contains("error:"))
            {
                _logger.LogTrace(consoleOutput);
                throw new GitCommandException(
                    $"Git command resulted an error: git {arguments} on {path} got result: {error}",
                    command: arguments,
                    result: error,
                    path: path);
            }
        }

        consoleOutput += output;
        _logger.LogTrace(consoleOutput);
        return consoleOutput;
    }
}
