<# 
    .SYNOPSIS

        Versioning tools

        creation date: 09/20/2016        

#>


function Get-TestAssemblies
{
    param
    (
        $session
    )
    
    $tests = @()    

    if([string]::IsNullOrWhiteSpace($session.IncludeFilter))
    {
        Write-LogError 'TOOLS' 'You must specify IncludeFilter'
        throw ''
    }

    Write-LogInfo 'TOOLS' "Getting the list of test assemblies (include) using: $($session.IncludeFilter)"

    [string[]]$includeFilters = $session.IncludeFilter -split ',' | ForEach-Object { Get-MatchPattern $_ }

    [string[]] $excludeFilters = @()

    if(-Not [string]::IsNullOrWhiteSpace($session.ExcludeFilter))
    {
        $excludeFilters = $session.ExcludeFilter -split ',' | ForEach-Object { Get-MatchPattern $_ }
        Write-LogInfo 'TOOLS' "Getting the list of test assemblies (exclude) using: $($session.ExcludeFilter)"        
    }        
             
    Get-ChildItem -Path $session.TestsDir -Recurse | ForEach-Object {         
        
        $doExclude = Get-IsFilterMatch $_.FullName $excludeFilters

        if(-Not $doExclude) {

            $doInclude = Get-IsFilterMatch $_.FullName $includeFilters

            if($doInclude) {
                $tests += $_
            }            
        }        
    }
    
    return $tests
}

function Get-IsFilterMatch
{
    param
    (
        [string] $text,

        [string[]] $filters
    )

    $filters | ForEach-Object {
        if((-Not [string]::IsNullOrWhiteSpace($_)) -and ($text -match $_)) {
            return $true
        }
    }

    return $false
}

function Get-MatchPattern
{
    param
    (
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string] $pattern
    )
      
    if([string]::IsNullOrWhiteSpace($pattern))
    {
        return $pattern
    }

    $pattern = $pattern -replace '([^\*])\*([^\*])','$1[^/]*$2'
    $pattern = $pattern -replace '^\*([^\*])','[^/]*$1'
    $pattern = $pattern -replace '\.','\.'
    $pattern = $pattern -replace '\*\*','.*'   
    $pattern = $pattern -replace '/','\\'   
    
    return "$pattern$"
}


Export-ModuleMember *-*