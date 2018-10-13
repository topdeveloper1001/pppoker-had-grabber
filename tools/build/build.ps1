<# 
    .SYNOPSIS

        Script is used to build DriveHUD

        creation date: 09/20/2016      
#>

[CmdletBinding(SupportsShouldProcess)]
param
(
    [ValidateSet('Debug','Release')]
    [string] $Mode = 'Release',

    [string] $Source = 'PPPokerCardCatcher\bin',
	
	[string] $MsiSource = 'PPPokerCardCatcher.Setup\bin',
	
	[string] $WixSource = 'PPPokerCardCatcher.Bootstrapper\bin',

    [string] $Solution = 'PPPokerCardCatcher.sln',                       
    
    [string] $InstallerWix = 'PPPokerCardCatcher.Bootstrapper\PPPokerCardCatcher.Bootstrapper.wixproj',

    [string] $InstallerMSI = 'PPPokerCardCatcher.Setup\PPPokerCardCatcher.Setup.wixproj',
    
    [string] $Version = '1.0.0',

    [string] $VersionExlcudeFilter = '**DriveHUD.PlayerXRay**,**XR*Reg**',

    [string] $ObfuscatorIncludeFilter = 'PPPokerCardCatcher.exe,PPPokerCardCatcher.*dll',

    [string] $ObfuscatorStrongNamedAssemblies = '',

    [string] $ObfuscatorExcludeFilter = 'vshost',

    [string] $SigningIncludeFilter = 'PPPokerCardCatcher.exe,PPPokerCardCatcher.*dll',
	
	[string] $MsiName = 'PPPPoker-Card-Catcher-Install.msi',
	
	[string] $WixName = 'PPPoker-Card-Catcher-Install.exe',

    [string] $SigningExcludeFilter = 'vshost',

    [string] $SigningCertificate = 'Certificates/APSCertificate.pfx',

    [string] $SigningPassword = 'backup',

    [string] $StrongNameKey = 'Certificates/lic-assemb-sign.pfx',

    [string] $StrongNamePassword = 'backup1',
    
    [ValidateSet('Debug', 'Info', 'Notice', 'Warning', 'Error')]
    [string] $LogLevel = 'Debug',

    [int] $StartRevision = 1,

    [string] $LicSolution = 'PPPokerCardCatcherReg.sln',

    [string] $LicSource = 'PPTReg\bin',

    [string] $LicObfuscatorIncludeFilter = '*Reg.dll',        

    [string] $LicProjectsToUpdate = 'PPPokerCardCatcher\PPPokerCardCatcher.csproj',

    [string] $LicCSFileToUpdate = 'PPPokerCardCatcher\App.xaml.cs',

    [string] $LicOutputPath = 'dependencies',

    [string] $HashToolSolution = 'tools\BuildFileHash\BuildFileHash.sln',

    [string] $HashToolPath = 'tools\BuildFileHash\BuildFileHash\bin',

    [string] $HashTool = 'BuildFileHash.exe',

    [bool] $UpdateOnlyLic = $false
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot\include\Init.ps1"

[Environment]::CurrentDirectory = $script:BaseDir
Set-Location $script:BaseDir

$session = @{
  BaseDir = $BaseDir
  Source = Join-Path $BaseDir (Join-Path $Source $Mode)
  MsiSource = Join-Path $BaseDir (Join-Path $MsiSource $Mode)
  WixSource = Join-Path $BaseDir (Join-Path $WixSource $Mode)
  Obfuscator = 'c:\Program Files (x86)\Eziriz\.NET Reactor\dotNET_Reactor.Console.exe'
  SignTool = 'c:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe'
  Candle = 'C:\Program Files (x86)\WiX Toolset v3.11\bin\candle.exe'
  Light = 'C:\Program Files (x86)\WiX Toolset v3.11\bin\Light.exe'
  Insignia = 'C:\Program Files (x86)\WiX Toolset v3.11\bin\insignia.exe'
  MSBuild = 'c:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\msbuild.exe'
  Nuget = '.\.nuget\Nuget.exe'
  Git = 'c:\Program Files\Git\bin\git.exe'
  Mode = $Mode
  Solution = Join-Path $BaseDir $Solution  
  InstallerMSI = Join-Path $BaseDir $InstallerMSI
  InstallerWix = Join-Path $BaseDir $InstallerWix
  Version = $Version
  VersionExlcudeFilter = $VersionExlcudeFilter
  ObfuscatorIncludeFilter = $ObfuscatorIncludeFilter
  ObfuscatorExcludeFilter = $ObfuscatorExcludeFilter
  ObfuscatorStrongNamedAssemblies = $ObfuscatorStrongNamedAssemblies
  SigningIncludeFilter = $SigningIncludeFilter
  MsiName = $MsiName
  WixName = $WixName
  SigningExcludeFilter = $SigningExcludeFilter 
  SigningCertificate = Join-Path $BaseDir $SigningCertificate
  SigningPassword= $SigningPassword
  StrongNameKey = Join-Path $BaseDir $StrongNameKey
  StrongNamePassword = $StrongNamePassword
  StartRevision = $StartRevision
  WixExtensions = 'dependencies\Wix\WixUIExtension.dll,dependencies\Wix\WixNetFxExtension.dll'
  LicSolution = Join-Path $BaseDir $LicSolution
  LicSource = Join-Path $BaseDir (Join-Path $LicSource $Mode)
  LicObfuscatorIncludeFilter = $LicObfuscatorIncludeFilter      
  LicProjectsToUpdate = Join-Path $BaseDir $LicProjectsToUpdate
  LicCSFileToUpdate = Join-Path $BaseDir $LicCSFileToUpdate
  LicOutputPath = Join-Path $BaseDir $LicOutputPath
  HashToolSolution = Join-Path $BaseDir $HashToolSolution  
  HashTool = Join-Path $BaseDir (Join-Path $HashToolPath (Join-Path $Mode $HashTool))   
}

Import-Module BuildRunner-Log
Import-Module BuildRunner-Tools
Import-Module BuildRunner-Versioning
Import-Module BuildRunner-MSBuild
Import-Module BuildRunner-Nuget
Import-Module BuildRunner-Obfuscate
Import-Module BuildRunner-Sign
Import-Module BuildRunner-SignWixBundle
Import-Module BuildRunner-LicUpdater

# setup logging
if ($LogLevel)
{
    Set-LogLevel $LogLevel
}

try
{
   Write-LogInfo 'SETUP' 'Validating base tools...'
   
   if(-Not (Test-Path($session.Solution)))
   {
       throw "Solution not found '$($session.Solution)'"
   } 

   if(-Not (Test-Path($session.LicSolution)))
   {
       throw "Solution not found '$($session.LicSolution)'"
   } 

   if(-Not (Test-Path($session.HashToolSolution)))
   {
       throw "Solution not found '$($session.HashToolSolution)'"
   } 

   if(-Not (Test-Path($session.Obfuscator)))
   {
       throw "Obfuscator not found '$($session.Obfuscator)'"
   } 

   if(-Not (Test-Path($session.SignTool)))
   {
       throw "SignTool not found '$($session.SignTool)'"
   } 

   if(-Not (Test-Path($session.Candle)))
   {
       throw "Candle not found '$($session.Candle)'"
   }

   if(-Not (Test-Path($session.Light)))
   {
       throw "Light not found '$($session.Light)'"
   }

   if(-Not (Test-Path($session.MSBuild)))
   {
       throw "MSBuild not found '$($session.MSBuild)'"
   }

   if((-Not $session.SigningCertificate) -Or (-Not (Test-Path($session.SigningCertificate))))
   {
       throw "SigningCertificate not found '$($session.SigningCertificate)'"
   }

   if(-Not (Test-Path($session.Git)))
   {
       throw "Git not found '$($session.Git)'"
   }
  
   if(-Not (Test-Path($session.InstallerWix)))
   {
       throw "Installer wix not found '$($session.InstallerWix)'"
   }
   
   if(-Not (Test-Path($session.InstallerMSI)))
   {
		throw "InstallerMSI not found '$($session.InstallerMSI)'"
   }

   if(-Not (Test-Path($session.Nuget)))
   {
       throw "Nuget not found '$($session.Nuget)'"
   }

   if(Test-Path($session.Source))
   {
       Write-LogInfo 'SETUP' 'Clearing source directory'
       Remove-Item -Path $session.Source -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
   }
   
   if(Test-Path($session.MsiSource))
   {
       Write-LogInfo 'SETUP' 'Clearing MSI source directory'
       Remove-Item -Path $session.MsiSource -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
   }
   
   if(Test-Path($session.WixSource))
   {
       Write-LogInfo 'SETUP' 'Clearing Wix source directory'
       Remove-Item -Path $session.WixSource -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
   }
    
   # setup version
   Set-Version($session)  
        
   # nuget
   Use-Nuget $session $session.Solution 'nuget.log'   

   # build hash tools
   Use-MSBuild $session $session.HashToolSolution 'hashtool-msbuild.log'

   # build lic dlls
   Use-MSBuild $session $session.LicSolution 'lic-msbuild.log'

   # obfuscate lic dlls
   Use-Obfuscator $session $session.LicSource $session.LicObfuscatorIncludeFilter '' $session.LicObfuscatorIncludeFilter

   # sign lic dlls
   # Use-Sign $session $session.LicSource $session.LicObfuscatorIncludeFilter ''

   # copy lic dlls
   &robocopy $session.LicSource $session.LicOutputPath $session.LicObfuscatorIncludeFilter /s | Out-Null

   # update license dll hashes and version in specified projects
   Use-LicUpdater($session)
   
   if($UpdateOnlyLic)
   {
        Write-LogInfo 'SETUP' 'Done.'
        exit(0)
   }

   # msbuild
   Use-MSBuild $session $session.Solution 'msbuild.log'

   # obfuscate
   Use-Obfuscator $session $session.Source $session.ObfuscatorIncludeFilter $session.ObfuscatorExcludeFilter $session.ObfuscatorStrongNamedAssemblies

   # sign
   Use-Sign $session $session.Source $session.SigningIncludeFilter $session.SigningExcludeFilter   
   
   # build msi installer 
   Use-MSbuild $session $session.InstallerMSI 'msbuild_msi.log'
   
   # sign msi installer (we do not sign gg catcher)
   Use-Sign $session $session.MsiSource $session.MsiName $session.SigningExcludeFilter

   # build wix installer   
   Use-MSbuild $session $session.InstallerWix 'msbuild_installer.log'      

   # sign wix installer (we do not sign gg catcher)
   Use-SignWixBundle($session)
   
   # create release folder
   # copy installer exe
   # pack installer into zip file
   
   Write-LogInfo 'SETUP' 'Done.'
   
   Write-Host "Press any key to continue ..."

   $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")   
}
catch
{
    $ErrorMessage = $_.Exception.Message    
    Write-Host $ErrorMessage -ForegroundColor Red        
    
	Write-Host "Press any key to continue ..."

	$host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
	
    exit(1)
}
finally
{
    # remove all used modules (for debugging purpose)
    Remove-Module BuildRunner-Log           
    Remove-Module BuildRunner-Tools
    Remove-Module BuildRunner-Versioning 
    Remove-Module BuildRunner-MSBuild
    Remove-Module BuildRunner-Obfuscate
    Remove-Module BuildRunner-LicUpdater
    Remove-Module BuildRunner-Sign
	Remove-Module BuildRunner-SignWixBundle
}