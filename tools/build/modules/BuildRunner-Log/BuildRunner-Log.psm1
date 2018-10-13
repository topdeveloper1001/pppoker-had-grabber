<#
    .SYNOPSIS
        This module provides command lets for working with ZIP archives.

#>
Set-StrictMode -Version Latest

Add-Type -TypeDefinition @"
/// <summary>
/// The log level.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Nothing will be written to the log-file
    /// </summary>
    NoLogging = 0,

    /// <summary>The system is unusable.</summary>
    Emergency = 25,

    /// <summary>Action must be taken immediately.</summary>
    Alert = 50,

    /// <summary>Critical conditions .</summary>
    Critical = 75,

    /// <summary>
    /// Only errors should be written to the log-file
    /// </summary>
    Error = 100,

    /// <summary>
    /// Only warnings and errors should be written to the log
    /// </summary>
    Warning = 200,

    /// <summary>Normal but significant condition.</summary>
    Notice = 250,

    /// <summary>
    /// Errors, warnings and information messages should be written to the log
    /// </summary>
    Info = 300,

    /// <summary>
    /// Everything will be written to the log
    /// </summary>
    Debug = 400
}
"@

$LogLevel = [LogLevel]::Notice

$LogFacilityPreference = @{
    # nothing here because everything is on by default
}

$LogFacilityColorPreference = @{
    SQL = [ConsoleColor]::Gray
    "TEAMCITY-REQUEST" = [ConsoleColor]::Yellow
    "TEAMCITY-RESPONSE" = [ConsoleColor]::Green
}

function Write-Log([string] $Facility, [string] $Message, [LogLevel] $Level = [LogLevel]::Info, $ForegroundColor)
{
    if (Test-Log $Facility $Level)
    {
        if ([String]::IsNullOrEmpty($Message))
        {
            Write-Host ""
            Return
        }

        if ($ForegroundColor)
        {
            $color = $ForegroundColor
            $levelColor = $ForegroundColor
        }
        else
        {
            $color = Get-LogColor $Facility $Level
            $levelColor = Get-LogLevelColor $Level

            if (-Not $levelColor)
            {
                $levelColor = $color
            }
            elseif (-Not $color)
            {
                $color = $levelColor
            }
            if (-Not $color)
            {
                $color = [ConsoleColor]::Gray
            }
            if (-Not $levelColor)
            {
                $levelColor = [ConsoleColor]::Gray
            }
        }

        Write-Host -ForegroundColor $levelColor -NoNewline "[$Facility] "
        Write-Host -ForegroundColor $color $Message
    }
}

function Write-LogError([string] $Facility, [string] $message)
{
    Write-Log $Facility $message ([LogLevel]::Error)
}

function Write-LogWarning([string] $Facility, [string] $message)
{
    Write-Log $Facility $message ([LogLevel]::Warning)
}

function Write-LogNotice([string] $Facility, [string] $message)
{
    Write-Log $Facility $message ([LogLevel]::Notice)
}

function Write-LogInfo([string] $Facility, [string] $message)
{
    Write-Log $Facility $message ([LogLevel]::Info)
}

function Write-LogDebug([string] $Facility, [string] $message)
{
    Write-Log $Facility $message ([LogLevel]::Debug)
}

function Test-Log([string] $facility, [LogLevel] $level)
{
    if ($level -gt $LogLevel)
    {
        return $False
    }
    if ($LogFacilityPreference.ContainsKey($facility) -And -Not $LogFacilityPreference[$facility])
    {
        return $False
    }
    return $True
}

function Get-LogLevelColor([LogLevel] $level)
{
    if ($level -le [LogLevel]::Error)
    {
        Return [ConsoleColor]::Red
    }   
    if ($level -le [LogLevel]::Warning)
    {
        Return [ConsoleColor]::Yellow
    }  
    if ($level -le [LogLevel]::Notice)
    {
        Return [ConsoleColor]::White
    }  
    if ($level -le [LogLevel]::Info)
    {
        Return [ConsoleColor]::Gray
    }  
    if ($level -le [LogLevel]::Debug)
    {
        Return [ConsoleColor]::DarkCyan
    }  
}

function Get-LogColor([string] $facility)
{
    if ($LogFacilityColorPreference.ContainsKey($facility))
    {
        Return $LogFacilityColorPreference[$facility]
    }
}

function Enable-LogFacility($facility)
{
    $LogFacilityPreference[$facility] = $True
}

function Disable-LogFacility($facility)
{
    $LogFacilityPreference[$facility] = $False
}

function Set-LogLevel([LogLevel] $level)
{
    ${script:LogLevel} = $level
}

function Get-LogLevel()
{
    return $LogLevel
}

Export-ModuleMember *-*