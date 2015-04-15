<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns="http://schemas.microsoft.com/wix/2006/wi"
    xmlns:wix="http://schemas.microsoft.com/wix/2006/wi"
    xmlns:util="http://schemas.microsoft.com/wix/UtilExtension">

    <xsl:template match="wix:Component[@Id='Thycotic.MemoryMq.Pipeline.Service.exe']">
        <xsl:copy>
            <xsl:apply-templates select="@*" />
            <xsl:apply-templates select="*" />
            <ServiceInstall Arguments="-installerVersion=$(var.InstallerVersion)" Account="[SERVICE_USERNAME]" Password="[SERVICE_PASSWORD]" Id="ServiceInstallMemoryMqPipelineService" Name="Thycotic.MemoryMq.Pipeline.Service" Type="ownProcess" Start="auto" ErrorControl="normal" DisplayName="Thycotic MemoryMq Pipeline Service">
                <util:ServiceConfig FirstFailureActionType="restart" SecondFailureActionType="restart" ThirdFailureActionType="none" RestartServiceDelayInSeconds="60" ResetPeriodInDays="0" />
            </ServiceInstall>
            <ServiceControl Id="ServiceControlMemoryMqPipelineService" Name="Thycotic.MemoryMq.Pipeline.Service" Start="install" Stop="uninstall" Remove="uninstall"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="*">
        <xsl:copy>
            <xsl:apply-templates select="@*" />
            <xsl:apply-templates select="* | text()"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="@* | text()">
        <xsl:copy />
    </xsl:template>

    <xsl:template match="/">
        <xsl:apply-templates />
    </xsl:template>

</xsl:stylesheet>