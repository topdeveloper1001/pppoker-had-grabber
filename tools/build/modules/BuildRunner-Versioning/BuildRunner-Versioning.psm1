<# 
    .SYNOPSIS

        Versioning tools

        creation date: 09/20/2016        

#>

Import-Module BuildRunner-Log
Import-Module BuildRunner-Tools

$ModuleName = 'VERSION'

function Set-Version($session)
{
   Write-LogInfo $ModuleName 'Configuring version...'

   $revisionNumber = [int]((&$session.Git rev-list HEAD | Measure-Object -Line).Lines) + $session.StartRevision
   Write-LogInfo $ModuleName "Revision number: $revisionNumber"

   if($revisionNumber -lt 1)
   {
        throw "Revision number must be more than 1. Check the branch."
   }

   [string[]]$excludeFilters = @()

   if(-Not [string]::IsNullOrWhiteSpace($session.VersionExlcudeFilter))
   {
        $excludeFilters = $session.VersionExlcudeFilter -split ',' | ForEach-Object { Get-MatchPattern $_ }        
   }    

   $version = [Version]($session.Version)
   $version = New-Object System.Version($version.Major, $version.Minor, $version.Build, $revisionNumber)
      
   Write-LogInfo $ModuleName "Product version number: $version"

   $versionFiles = @('AssemblyInfo.cs', 'Bundle.wxs', 'Variables.wxi')

   Write-LogInfo $ModuleName "Updating all $versionFiles"

   foreach($fileTemplate in $versionFiles)
   {
        Get-ChildItem -Path $session.BaseDir -Filter $fileTemplate  -Recurse | ForEach-Object {
            
            $doExclude = Get-IsFilterMatch $_.FullName $excludeFilters

            if(-Not $doExclude) {

                Write-LogInfo $ModuleName "Updating $($_.FullName)"

                if($_.Extension -eq '.cs')
                {
                    Update-AssemblyInfo $_.FullName $version
                }

                if($_.Extension -eq '.wxs' -Or $_.Extension -eq '.wxi')
                {
                    Update-Wix $_.FullName $version
                }
            }
        }
   }
}

function Update-AssemblyInfo()
{
    param
    (
        [string] $File,

        [Version] $Version
    )
     
    $versionAttributes = @('AssemblyVersion', 'AssemblyFileVersion')
    
    $content = (Get-Content $File -Encoding UTF8) | ForEach-Object {

        $match = $false;

        foreach($versionAttribute in $versionAttributes)
        {
            $pattern = '\[assembly: ' + $versionAttribute + '\("(.*)"\)\]'

            if($_ -match $pattern) 
            {                         
                $fileVersion = $matches[1]            
                $_.Replace($fileVersion, $Version)                

                $match = $true
            } 
        }
        
        if(-Not $match) {            
            $_
        }
    } 	
	
	[System.IO.File]::WriteAllLines($File, $content)
}

function Update-Wix()
{
    param
    (
        [string] $File,

        [Version] $Version
    )
        
    $pattern = 'ProductVersion\s*=\s*"(.*)"'

    (Get-Content $File) | ForEach-Object {
                          
       if($_ -match $pattern) 
       {           
            if($File.EndsWith('Variables.wxi'))
            {
                $Version = New-Object System.Version($Version.Major, $Version.Minor, $Version.Revision)
            }

            $fileVersion = [version]$matches[1]            
            $_.Replace($fileVersion, $Version)
       }                 
       else {            
           $_
       }

    } | Set-Content $File   
}

Export-ModuleMember Set-Version