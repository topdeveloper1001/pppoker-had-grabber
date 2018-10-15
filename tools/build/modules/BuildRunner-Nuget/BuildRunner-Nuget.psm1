<# 
    .SYNOPSIS

        Nuget tools

        creation date: 10/29/2016        

#>

Import-Module BuildRunner-Log

$ModuleName = 'Nuget'

function Use-Nuget($session, $file, $logfile)
{
   Write-LogInfo $ModuleName 'Running nuget...'
   Write-LogInfo $ModuleName "nuget restore $file"

   &$session.Nuget restore $file | Out-File $logfile

   if($LastExitCode -ne 0)
   {
        Write-LogError $ModuleName "Failed to restore nuget $file. Please check the log. Exit code: $LastExitCode"
        throw ""
   }

   Write-LogInfo $ModuleName "Successfully restored nuget $file. Exit code: $LastExitCode"
}


Export-ModuleMember Use-Nuget