<# 
    .SYNOPSIS

        Obfuscate tool for build runner

        creation date: 05/20/2016    
#>

Import-Module BuildRunner-Log

$ModuleName = 'LICUPDATER'

function Use-LicUpdater($session)
{
    Write-LogInfo $ModuleName 'Running lic updater...'
    Write-LogInfo $ModuleName 'Validating tools....'

    # validate paths to required tools and files
    ValidateTools

    $licAssemblies = @()

    Get-ChildItem -Path $session.LicOutputPath -Filter $session.LicObfuscatorIncludeFilter | ForEach-Object {
       
       $licData = @{
            Name = [System.IO.Path]::GetFileNameWithoutExtension($_.Name)
            Version = $_.VersionInfo.FileVersion            
       }

       $licAssemblies += $licData
    }

    Patch-CSFile $session $licAssemblies
    Patch-ProjectFile $session $licAssemblies
}

function ValidateTools
{
   if(-Not (Test-Path($session.HashTool)))
   {
       throw "HashTool not found '$($session.HashTool)'"
   }

   if(-Not (Test-Path($session.LicCSFileToUpdate)))
   {
       throw "LicCSFileToUpdate not found '$($session.LicCSFileToUpdate)'"
   }

   if(-Not (Test-Path($session.LicProjectsToUpdate)))
   {
       throw "LicProjectsToUpdate not found '$($session.LicProjectsToUpdate)'"
   }
}

function Patch-CSFile($session, $licAssemblies)
{
    Write-LogInfo $ModuleName "Patching $($session.LicCSFileToUpdate)"

    $csContext = Get-Content $session.LicCSFileToUpdate

    for($i = 0; $i -lt $csContext.Length; $i++)
    {
        if($csContext[$i].Contains('void ValidateLicenseAssemblies'))
        {
            Write-LogInfo $ModuleName 'Found ValidateLicenseAssemblies method'

            $assembliesLine = $csContext[$i + 2]

            $bracketStartIndex = $assembliesLine.IndexOf('{');
            $bracketEndIndex = $assembliesLine.IndexOf('}');

            $assembliesCode = $assembliesLine.Substring($bracketStartIndex + 1, $bracketEndIndex - $bracketStartIndex - 1).Trim();

            Write-LogInfo $ModuleName "Found lic assemblies code: $assembliesCode"

            $assemblyHashes = @()
            $assemblySizes = @()

            $assembliesSplit = $assembliesCode -split ',' | ForEach-Object {            
                $assembly = Join-Path $session.LicOutputPath $_.Trim("""", " ")                
                $hash = (&$session.HashTool $assembly)
                $size =  (Get-Item $assembly).Length

                $assemblyHashes += $hash
                $assemblySizes += $size
            }

            $assembliesHashesLine = $csContext[$i + 3]
            
            $csContext[$i + 3] = Patch-CSFileLine $csContext[$i + 3]  ($assemblyHashes | ForEach-Object { """$_""" })
            $csContext[$i + 4] = Patch-CSFileLine $csContext[$i + 4]  $assemblySizes
        }
    }    

    $csContext | Out-File -FilePath $session.LicCSFileToUpdate -Force
}

function Patch-CSFileLine($line, $patch)
{
    $bracketStartIndex = $line.IndexOf('{');
    $bracketEndIndex = $line.IndexOf('}');
    
    $lineToInsert = $patch -join ', '

    $line = $line.Remove($bracketStartIndex + 2, $bracketEndIndex - $bracketStartIndex - 3).Insert($bracketStartIndex + 2, $lineToInsert)
    
    return $line
}

function Patch-ProjectFile($session, $licAssemblies)
{
    Write-LogInfo $ModuleName "Patching $($session.LicProjectsToUpdate)"

    $project = [xml](Get-Content $session.LicProjectsToUpdate)

    $ns = New-Object System.Xml.XmlNamespaceManager($project.NameTable)
    $ns.AddNamespace("ns", $project.DocumentElement.NamespaceURI)

    $references = $project.SelectNodes("//ns:Reference", $ns);

    foreach($reference in $references)
    {
        foreach($licAssembly in $licAssemblies)
        {
            if($reference.Include.StartsWith($licAssembly.Name))
            {
                Write-LogInfo $ModuleName "Found reference: '$($reference.Include)'"

                $indexOfVersion = $reference.Include.IndexOf("Version=")
                $lastIndexOfVersion = $reference.Include.IndexOf(",", $indexOfVersion)

                $reference.Include = $reference.Include.Remove($indexOfVersion + 8, $lastIndexOfVersion - $indexOfVersion - 8).Insert($indexOfVersion + 8, $licAssembly.Version)

                Write-LogInfo $ModuleName "Reference was updated: '$($reference.Include)'"
            }
        }
    }

    $project.Save($session.LicProjectsToUpdate);
}

Export-ModuleMember Use-LicUpdater