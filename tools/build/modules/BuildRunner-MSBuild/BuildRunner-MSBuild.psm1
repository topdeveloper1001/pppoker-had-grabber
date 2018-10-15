<# 
    .SYNOPSIS

        MSBuild tools

        creation date: 09/20/2016        

#>

Import-Module BuildRunner-Log

$ModuleName = 'MSBuild'

function Use-MSBuild($session, $file, $logfile)
{
   Write-LogInfo $ModuleName 'Running msbuild...'
   Write-LogInfo $ModuleName "msbuild $file /t:Rebuild /p:Configuration=$($session.Mode)"

   &$session.MSBuild $file /t:Rebuild "/p:Configuration=$($session.Mode)" | Out-File $logfile

   if($LastExitCode -ne 0)
   {
        Write-LogError $ModuleName "Failed to build $file. Please check the log. Exit code: $LastExitCode"
        throw ""
   }

   Write-LogInfo $ModuleName "Successfully built $file. Exit code: $LastExitCode"
}


Export-ModuleMember Use-MSBuild