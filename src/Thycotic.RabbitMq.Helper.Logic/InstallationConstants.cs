using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Thycotic.RabbitMq.Helper.Logic
{
    /// <summary>
    ///     Installation constants
    /// </summary>
    public static class InstallationConstants
    {
        private static class EnvironmentalVariables
        {
            public static readonly string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            public static readonly string ProgramFiles32Bit = Environment.Is64BitOperatingSystem
                ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                : ProgramFiles;
        }

        /// <summary>
        ///     Erlang constants
        /// </summary>
        public static class Erlang
        {
            /// <summary>
            /// The erlang cookie file name
            /// </summary>
            public const string CookieFileName = ".erlang.cookie";

            /// <summary>
            /// The erlang cookie system path
            /// </summary>
            /// <remarks>Usually something like C:\Windows\System32\config\systemprofile\</remarks>

            public static readonly string CookieSystemPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "config", "systemprofile", CookieFileName);

            /// <summary>
            /// The erlang cookie user profile path
            /// </summary>
            public static readonly string CookieUserProfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), CookieFileName);


            /// <summary>
            ///     The erlang installer checksum
            /// </summary>
            public static readonly string InstallerChecksum =
                "d32322080dc3d36a9a3b2ccaea60932dd8a857058f5dc93ffcd887cb0bffec977eeaa628213830f4e74eb09274cea9e3cb32110b291fd645d28be81d34232d45";

            /// <summary>
            ///     The version
            /// </summary>
            public static readonly Version Version = new Version(24, 0);

            /// <summary>
            ///     The download URL
            /// </summary>
            public static readonly string DownloadUrl = "https://erlang.org/download/otp_win64_24.0.exe";

            /// <summary>
            ///     The download URL using the Thycotic mirror
            /// </summary>
            public static readonly string ThycoticMirrorDownloadUrl = "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/erlang/otp_win64_24.0.exe";

            /// <summary>
            ///     The install path
            /// </summary>
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles, "erl-24.0");

            /// <summary>
            ///     The uninstaller path
            /// </summary>
            public static readonly string[] UninstallerPaths =
                new DirectoryInfo(EnvironmentalVariables.ProgramFiles)
                    .GetDirectories("erl*")
                    .Where(d => Regex.IsMatch(d.Name, @"erl-?\d\d?\.\d"))
                    .SelectMany(d => d.GetFiles("uninstall.exe"))
                    .Select(f => f.FullName)
                    .ToArray();
        }

        /// <summary>
        ///     RabbitMq constants
        /// </summary>
        public static class RabbitMq
        {

            /// <summary>
            /// The installer RabbitMq checksum
            /// </summary>
            public static readonly string InstallerChecksum =
                "9bc2c72f6f9237981ca8994377c75ac5fbac5a99efefa44910355bef02bab51b18580a49b797a71a468759e0e43de4c65363e05dcbd0175e10db3e5098827856";

            /// <summary>
            ///     The download URL
            /// </summary>
            public const string DownloadUrl =
                "https://github.com/rabbitmq/rabbitmq-server/releases/download/v3.9.5/rabbitmq-server-3.9.5.exe";

            /// <summary>
            ///     The download URL using the Thycotic mirror
            /// </summary>
            public const string ThycoticMirrorDownloadUrl =
                "https://thycocdn.azureedge.net/rabbitmqhelperfiles-master/rabbitmq/rabbitmq-server-3.9.5.exe";

            /// <summary>
            ///     The version
            /// </summary>
            public static readonly Version Version = new Version(3, 9, 5);

            /// <summary>
            ///     The configuration path
            /// </summary>
            public static readonly string ConfigurationPath = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "RabbitMq");

            /// <summary>
            ///     The install path
            /// </summary>
            public static readonly string InstallPath = Path.Combine(EnvironmentalVariables.ProgramFiles,
                "RabbitMQ Server", "rabbitmq_server-3.9.5");

            /// <summary>
            ///     The bin dir
            /// </summary>
            public static readonly string BinDir = "sbin";

            /// <summary>
            ///     The bin path
            /// </summary>
            public static readonly string BinPath = Path.Combine(InstallPath, BinDir);

            /// <summary>
            ///     The uninstaller path
            /// </summary>
            public static readonly string[] UninstallerPaths = new[]
            {
                Path.Combine(EnvironmentalVariables.ProgramFiles, "RabbitMQ Server", "uninstall.exe")
            };
        }
    }
}