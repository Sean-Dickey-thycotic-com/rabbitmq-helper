﻿using System;
using System.Diagnostics;
using Thycotic.CLI;
using Thycotic.InstallerGenerator.InteractiveRunner.ConsoleCommands;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.InteractiveRunner
{
    class Program
    {

        public static class SupportedSwitches
        {
            public const string GenerateMemoryMqMsi = "generateGenericMemoryMqMsiCommand";
            public const string GenerateConfiguredMemoryMqZip = "generateConfiguredMemoryMqZipCommand";
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                Log.Configure();

                var cli = new CommandLineInterface("Thycotic.InstallerGenerator.InteractiveRunner");

                //TODO: harvest all commands automatically
                cli.AddCustomCommand(new GenerateGenericMemoryMqMsiCommand());
                cli.AddCustomCommand(new GenerateConfiguredMemoryMqZipCommand());

                var input = string.Join(" ", args);

                if (string.IsNullOrWhiteSpace(input))
                {
                    input = SupportedSwitches.GenerateMemoryMqMsi;
                }

                cli.ConsumeInput(input);

            }
            catch (Exception ex)
            {
                //superfluous, mostly used for testing and consuming exceptions that are already logged but we want to bubble to the OS
                Trace.TraceError(ex.Message);
                throw;
            }
        }


        //private const string Version = "5.0.0.0";

        //private const string EngineToServerConnectionString = "http://localhost/ihawu";
        //private const string EngineToServerUseSsl = "false";
        //private const string EngineToServerSiteId = "3";
        //private const string EngineToServerOrganizationId = "1";

        //private static void Main(string[] args)
        //{
        //    Log.Configure();

        //    try
        //    {
        //        var generations = new List<Func<string>>
        //        {
        //            GenerateAgentToDistributedEnginePreliminaryZip,
        //            //GenerateMemoryMqMsi,
        //            //GenerateDistributedEngineMsi,
        //            //GenerateDistributedEngineMsi32Bit,
        //            //GenerateDistributedEngineUpdateMsi,
        //            //GenerateDistributedEngineUpdateMsi32Bit
        //        };

        //        generations.ForEach(g =>
        //        {
        //            Console.WriteLine("Artifact generator and stored in {0}", g.Invoke());
        //        });
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Generator failed");
        //        Console.WriteLine(ex.Message);
        //        Console.ReadKey();
        //    }

        //    //Console.WriteLine("Press any key to exit.");
        //    //Console.ReadKey();
        //}

        //private static string GenerateMemoryMqMsi()
        //{
        //    const string someSecretServerArbitraryPathForWixRecipe =
        //         @"M:\development\repos\distributedengine\src\Thycotic.MemoryMq.SiteConnector.Service.Wix";
        //    //@"C:\development\distributedengine\src\Thycotic.MemoryMq.SiteConnector.Service.Wix";

        //    const string someSecretServerArbitraryPathForBits =
        //        @"M:\development\repos\distributedengine\src\Thycotic.MemoryMq.SiteConnector.Service\bin\Release";
        //    //@"C:\development\distributedengine\src\Thycotic.MemoryMq.SiteConnector.Service\bin\Release";
        //    const string currentSnapshottedVersion = Version;


        //    var steps = new MemoryMqSiteConnectorServiceWiXMsiGeneratorRunbook
        //    {
        //        RecipePath = someSecretServerArbitraryPathForWixRecipe,
        //        SourcePath = someSecretServerArbitraryPathForBits,
        //        Version = currentSnapshottedVersion
        //    };

        //    var wrapper = new InstallerGeneratorWrapper();

        //    return wrapper.Generate(new Generator(), steps);
        //}

        //private static string GenerateAgentToDistributedEnginePreliminaryZip()
        //{
        //    const string someSecretServerArbitraryPathForBits =
        //        @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
        //    //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
        //    const string currentSnapshottedVersion = Version;

        //    var steps = new LegacyAgentToEngineServicePreliminaryZipGeneratorRunbook
        //    {
        //        SourcePath = someSecretServerArbitraryPathForBits,
        //        Version = currentSnapshottedVersion

        //    };

        //    var wrapper = new InstallerGeneratorWrapper();

        //    return wrapper.Generate(new Generator(), steps);
        //}

        //private static string GenerateDistributedEngineMsi()
        //{
        //    const string someSecretServerArbitraryPathForWixRecipe =
        //        //@"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix";
        //        @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix";
        //        //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service.Wix";

        //    const string someSecretServerArbitraryPathForBits =
        //        @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
        //    //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
        //    const string currentSnapshottedVersion = Version;


        //    var steps = new DistributedEngineServiceWiXMsiGeneratorRunbook
        //    {
        //        RecipePath = someSecretServerArbitraryPathForWixRecipe,
        //        SourcePath = someSecretServerArbitraryPathForBits,
        //        Version = currentSnapshottedVersion

        //    };

        //    var wrapper = new InstallerGeneratorWrapper();

        //    return wrapper.Generate(new Generator(), steps);
        //}

        //private static string GenerateDistributedEngineMsi32Bit()
        //{
        //    const string someSecretServerArbitraryPathForWixRecipe =
        //        //@"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.32bit";
        //       @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.32bit";
        //    //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.32bit";

        //    const string someSecretServerArbitraryPathForBits =
        //        @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release.32bit";
        //    //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release.32bit";
        //    const string currentSnapshottedVersion = Version;


        //    var steps = new DistributedEngineServiceWiXMsiGeneratorRunbook
        //    {
        //        Is64Bit = false,
        //        RecipePath = someSecretServerArbitraryPathForWixRecipe,
        //        SourcePath = someSecretServerArbitraryPathForBits,
        //        Version = currentSnapshottedVersion

        //    };

        //    var wrapper = new InstallerGeneratorWrapper();

        //    return wrapper.Generate(new Generator(), steps);
        //}


        //private static string GenerateDistributedEngineUpdateMsi()
        //{
        //    const string someSecretServerArbitraryPathForWixRecipe =
        //        //@"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update";
        //        @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update";
        //    //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update";

        //    const string someSecretServerArbitraryPathForBits =
        //        @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
        //    //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release";
        //    const string currentSnapshottedVersion = Version;


        //    var steps = new DistributedEngineServiceWiXMsiGeneratorRunbook
        //    {
        //        ArtifactNameSuffix = "Update",
        //        RecipePath = someSecretServerArbitraryPathForWixRecipe,
        //        SourcePath = someSecretServerArbitraryPathForBits,
        //        Version = currentSnapshottedVersion
        //    };

        //    var wrapper = new InstallerGeneratorWrapper();

        //    return wrapper.Generate(new Generator(), steps);
        //}


        //private static string GenerateDistributedEngineUpdateMsi32Bit()
        //{
        //    const string someSecretServerArbitraryPathForWixRecipe =
        //        //@"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update.32bit";
        //       @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update.32bit";
        //    //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service.Wix.Update.32bit";

        //    const string someSecretServerArbitraryPathForBits =
        //        @"M:\development\repos\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release.32bit";
        //    //@"C:\development\distributedengine\src\Thycotic.DistributedEngine.Service\bin\Release.32bit";
        //    const string currentSnapshottedVersion = Version;


        //    var steps = new DistributedEngineServiceWiXMsiGeneratorRunbook
        //    {
        //        Is64Bit = false,
        //        ArtifactNameSuffix = "Update",
        //        RecipePath = someSecretServerArbitraryPathForWixRecipe,
        //        SourcePath = someSecretServerArbitraryPathForBits,
        //        Version = currentSnapshottedVersion
        //    };


        //    var wrapper = new InstallerGeneratorWrapper();

        //    return wrapper.Generate(new Generator(), steps);
        //}

    }
}