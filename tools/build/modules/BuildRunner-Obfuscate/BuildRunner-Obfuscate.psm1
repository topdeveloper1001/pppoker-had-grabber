<# 
    .SYNOPSIS

        Obfuscate tool for build runner

        creation date: 05/20/2016    
#>

Import-Module BuildRunner-Log
Import-Module BuildRunner-Tools

$ModuleName = 'OBFUSCATE'

function Use-Obfuscator($session, $source, $includeFilter, $excludeFilter, $strongNameAssemblies)
{    
    Write-LogInfo $ModuleName "Obfuscating $source..."
    Write-LogInfo $ModuleName "Include filter: $includeFilter"
    Write-LogInfo $ModuleName "Exclude filter: $excludeFilter"

    $assemblies = @()
    
    [string[]]$includeFilters = $includeFilter -split ',' | ForEach-Object { Get-MatchPattern $_ }
    [string[]]$excludeFilters = @()
    [string[]]$strongAssembliesFilters =  $strongNameAssemblies -split ',' | ForEach-Object { Get-MatchPattern $_ }
    
    if(-Not [string]::IsNullOrWhiteSpace($excludeFilter))
    {
        $excludeFilters = $excludeFilter -split ',' | ForEach-Object { Get-MatchPattern $_ }        
    }    

    Get-ChildItem -Path $source -Recurse | ForEach-Object {

        $doExclude = Get-IsFilterMatch $_.FullName $excludeFilters

        if(-Not $doExclude) {

            $doInclude = Get-IsFilterMatch $_.FullName $includeFilters

            if($doInclude) {
                $assemblies += $_
            }            
        }       
    }

    $assembliesArg = $assemblies -join ' '
    
    foreach($assembly in $assemblies)
    {        
        $args = @(
            '-file',
            $assembly.FullName,
            '-targetfile',
            $assembly.FullName,
            '-suppressildasm',
            '1',
            '-stringencryption',
            '1',
            '-obfuscation',
            '1',
			'-mapping_file',
			'1'
        )

        if(Get-IsFilterMatch $assembly.FullName $strongAssembliesFilters)
        {
            $args += '-snkeypair' 
            $args += $session.StrongNameKey
            $args += '-snpassword'
            $args += $session.StrongNamePassword
        }

        Write-LogInfo $ModuleName "Executing: $($session.Obfuscator) $args"

        &$session.Obfuscator $args | Out-File 'obfuscator.log'

        if($LastExitCode -ne 0)
        {
            Write-LogError $ModuleName "$assembly wasn't obfuscated. Exit code: $LastExitCode"
            throw ""
        }
       
        Write-LogInfo $ModuleName "$assembly was obfuscated."
    }
}

Export-ModuleMember Use-Obfuscator