<# 
    .SYNOPSIS

        Script is used to sign DriveHUD installer

        creation date: 10/31/2016      
#>

Import-Module BuildRunner-Log
Import-Module BuildRunner-Sign

$ModuleName = 'SignWixBundle'

function Use-SignWixBundle($session)
{
    Write-LogInfo $ModuleName 'Signing wix bundle...'   

	$engineName = 'engine.exe'
	$engineFullPath = Join-Path $session.WixSource $engineName
	$bundle = Join-Path $session.WixSource $session.WixName
	
	# detach engine
	&$session.Insignia -ib $bundle -o $engineFullPath -nologo -wx
	Write-LogInfo $ModuleName 'Engine detached'
	# sign engine
	Use-Sign $session $session.WixSource $engineName $session.SigningExcludeFilter
	# attach engine
	&$session.Insignia -ab $engineFullPath $bundle -o $bundle -nologo -wx
	Write-LogInfo $ModuleName 'Engine attached'
	# sign wix
	Use-Sign $session $session.WixSource $session.WixName $session.SigningExcludeFilter
}

Export-ModuleMember Use-SignWixBundle