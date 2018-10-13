<#
    .SYNOPSIS
        This script is intended to be included right at the start of
        every powershell script in the unit test environment.

        It sets up the powershell module path and sets variable $BaseDir        
#>

function Init-BuildRunner()
{
    <# 
        .SYNOPSIS
            Find the modules directory add it to the module path,
            so that Import-Module works.
    #>

    $ScriptRoot = split-path -parent $PSScriptRoot
    $ModuleDir = "modules"

    $modulePath = Join-Path $ScriptRoot $ModuleDir

    if (-Not (Test-Path $modulePath))
    {
        $BaseDir = pwd
        $modulePath = Join-Path $ScriptRoot $ModuleDir
    }
    else
    {
        
        $UpstreamDir = $ScriptRoot | split-path -parent | split-path -parent

        if ($UpstreamDir)
        {
            $BaseDir = $UpstreamDir
        }
        else
        {
            $BaseDir = pwd
        }
    }

    $env:PSModulePath = [System.Environment]::GetEnvironmentVariable("PSModulePath", "Machine") + ";" + $modulePath

    $script:BaseDir = $BaseDir
}

Init-BuildRunner