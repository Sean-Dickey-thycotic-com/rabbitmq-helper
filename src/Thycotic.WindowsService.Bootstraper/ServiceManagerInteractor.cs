﻿using System;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.WindowsService.Bootstraper.Wmi;

namespace Thycotic.WindowsService.Bootstraper
{
    public class ServiceManagerInteractor : IServiceManagerInteractor
    {
        private readonly CancellationTokenSource _cts;
        private readonly string _serviceName;


        private readonly ILogWriter _log = Log.Get(typeof (ServiceManagerInteractor));

        public ServiceManagerInteractor(CancellationTokenSource cts, string serviceName)
        {
            _cts = cts;
            _serviceName = serviceName;

        }

        #region Win32
        private static ManagementObject GetManagementObject(ManagementPath computerPath)
        {
            var path = computerPath;
            var managementObject = new ManagementObject(path);
            return managementObject;
        }

        private IWin32Service GetService()
        {
            var computerPath = Win32Service.GetLocalServiceManagementPath(_serviceName);
            var managementObject = GetManagementObject(new ManagementPath(computerPath));

            //Task.Delay(TimeSpan.FromSeconds(5)).Wait();

            managementObject.Scope.Connect();

            return new Win32Service(managementObject);

        }

        private void InteractiveWithService(Action<IWin32Service> action)
        {
            try
            {
                using (var win32Service = GetService())
                {
                    action.Invoke(win32Service);
                }

            }
            catch (Exception ex)
            {
                _log.Error("Interaction with service failed", ex);
                throw;
            }
        }

        private string GetServiceState()
        {
            try
            {
                using (var win32Service = GetService())
                {
                    return win32Service.State;
                }

            }
            catch (Exception ex)
            {
                _log.Error("Interaction with service failed", ex);
                throw;
            }
        }
        #endregion

        #region Service start/stop

        public void StartService()
        {
            InteractiveWithService(service =>
            {
                _log.Info("Starting service");
                service.StartService();

            });

            while (!_cts.Token.IsCancellationRequested)
            {
                if (GetServiceState() == ServiceStates.Running)
                {
                    _log.Info("Service running");
                    break;
                }

                Task.Delay(TimeSpan.FromSeconds(5), _cts.Token).Wait(_cts.Token);
            }
        }

        public void StopService()
        {
            InteractiveWithService(service =>
            {
                _log.Info("Stopping service");
                service.StopService();
            });

            while (!_cts.Token.IsCancellationRequested)
            {
                if (GetServiceState() == ServiceStates.Stopped)
                {
                    _log.Info("Service stopped");
                    break;
                }

                Task.Delay(TimeSpan.FromSeconds(5), _cts.Token).Wait(_cts.Token);
            }
        }
        #endregion
    }
}