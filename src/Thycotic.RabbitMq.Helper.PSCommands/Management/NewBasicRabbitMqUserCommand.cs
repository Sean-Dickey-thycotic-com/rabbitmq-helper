﻿using System;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.OS;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Adds a basic user to RabbitMq. This used has publish and consumer permissions
    /// </summary>
    /// <para type="synopsis">Adds a basic user to RabbitMq. This used has publish and consumer permissions</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>New-BasicRabbitMqUser</code>
    /// </example>
    [Cmdlet(VerbsCommon.New, "BasicRabbitMqUser")]
    [Alias("addRabbitMqUser")]
    public class NewBasicRabbitMqUserCommand : Cmdlet
    {
        /// <summary>
        ///     Gets or sets the name of the rabbit mq user.
        /// </summary>
        /// <value>
        ///     The name of the rabbit mq user.
        /// </value>
        /// <para type="description">Gets or sets the name of the rabbit mq user.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [Alias("RabbitMqUserName")]
        public string UserName { get; set; }

        /// <summary>
        ///     Gets or sets the rabbit mq password.
        /// </summary>
        /// <value>
        ///     The rabbit mq password.
        /// </value>
        /// <para type="description">Gets or sets the rabbit mq password.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [Alias("RabbitMqPw", "RabbitMqPassword")]
        public string Password { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        /// <exception cref="System.ApplicationException">
        ///     Failed to create user. Manual creation might be necessary
        ///     or
        ///     Failed to grant permissions to user. Manual grant might be necessary
        /// </exception>
        protected override void ProcessRecord()
        {
            var ctlInteractor = new CtlRabbitMqProcessInteractor();

            WriteVerbose($"Adding limited-access user {UserName}");

            var parameters2 = $"add_user {UserName} {Password}";

            try
            {
                var  output = ctlInteractor.Invoke(parameters2, TimeSpan.FromSeconds(15));
                WriteVerbose(output);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create user. Manual creation might be necessary", ex);
            }

            WriteVerbose($"Granting permissions to user {UserName}");

            parameters2 = $"set_permissions -p / {UserName} \".*\" \".*\" \".*\"";

            try
            {
                var output = ctlInteractor.Invoke(parameters2, TimeSpan.FromSeconds(15));
                WriteVerbose(output);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to grant permissions to user. Manual grant might be necessary",
                    ex);
            }
        }
    }
}