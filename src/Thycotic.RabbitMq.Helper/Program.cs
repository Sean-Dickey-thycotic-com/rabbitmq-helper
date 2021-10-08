﻿using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Threading.Tasks;
using Thycotic.RabbitMq.Helper.PSCommands.Installation.Workflow;

namespace Thycotic.RabbitMq.Helper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Preparing helper and starting PowerShell...");

                var task = Task.Run(() =>
                {
                    var module = typeof(InstallConnectorCommand).Assembly;

                    var psi = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "powershell.exe",
                        Arguments = $"-Version 3.0 -ExecutionPolicy RemoteSigned & {{& Write-Host $PSVersionTable.PSVersion }}"

                    };
                    var process = new Process { StartInfo = psi };
                    process.Start();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new ApplicationFailedException($"PowerShell version check failed. Process existed with code {process.ExitCode}. RabbitMq Helper requires PowerShell version 3 or higher. Please verify your installation.");
                    }

                    //var exampleFolder = ".\\Examples";
                    var preparationScript =
                        $"Write-Host 'Running RabbitMq Helper version {module.GetName().Version} as administrator'; " +
                        $"Write-Host 'This is open source software: https://github.com/thycotic/rabbitmq-helper. See LICENSE file for details'; " +
                        $"Write-Host 'Documentation and examples located: https://thycotic.github.io/rabbitmq-helper'; " +
                        $"Write-Host -NoNewLine 'Execution Policy: ';" +
                        $"Get-ExecutionPolicy | Write-Host;" +
                        $"Write-Host;" +

                        $"Import-Module '{module.Location}'; " +

                        $"Write-Warning 'IMPORTANT: *** The helper re-installs RabbitMq and Erlang during install/re-install/upgrade, even if the same version(s) are installed. ***';" +
                        $"Write-Host;" +

                        $"Write-Warning 'IMPORTANT: *** Always use a local administrator account that is NOT a domain account to install RabbitMq. Otherwise, exit now ***';" +
                        $"Write-Host;" +

                        $"Write-Host 'Available command-lets in the ''{module.GetName().Name}'' module (use ''get-help CMDLETNAME'' for help and usage):';" +
                        $"Get-Command -Module {module.GetName().Name} | Sort | % {{ Write-Host \"\"`t $_.Name\"\" }};" +
                        $"Write-Host;";

                    psi = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-Version 3.0 -NoExit -ExecutionPolicy RemoteSigned & {{& {preparationScript} }}"

                    };
                    process = new Process { StartInfo = psi };
                    process.Start();

                    process.WaitForExit();

                    if (process.ExitCode != 0 && process.ExitCode != -1073741510)
                    {
                        throw new ApplicationFailedException($"PowerShell exited with unexpected code {process.ExitCode}");
                    }

                });

                if (!task.IsCanceled && !task.IsFaulted)
                {
                    Console.WriteLine("PowerShell running. This window will close when the PowerShell host closes.");
                }

                task.Wait();

                if (task.Exception != null)
                {
                    throw task.Exception;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occurred:");

                var ex2 = ex;

                while (ex2 != null)
                {
                    Console.WriteLine($"Error: {ex2.Message}");
                    Console.WriteLine($"Stack Trace: {ex2.StackTrace}");
                    Console.WriteLine();

                    ex2 = ex2.InnerException;
                }
                Console.ResetColor();

                Console.ReadKey();
            }
        }

    }
}