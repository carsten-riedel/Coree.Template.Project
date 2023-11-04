function Test-FileExistence {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true)]
        [string[]]$SearchDirs,

        [Parameter(Mandatory=$true)]
        [string]$FileName
    )

    foreach ($dir in $SearchDirs) {
        $filePath = Join-Path $dir $FileName
        if (Test-Path $filePath) {
            Write-Host "File found: $filePath"
            return $filePath
        }
    }

    Write-Host "File not found."
    return $null
}

function Get-DllAssemblyProperties {
  param(
    [string]$directory
  )

  $resultList = @()

  # Check if the directory exists
  if (Test-Path $directory) {
    # Get all DLL files in the directory, including sub-directories
    $dllFiles = Get-ChildItem -Path $directory -Filter "*.dll" -Recurse

    foreach ($dll in $dllFiles) {
      try {
        # Get the AssemblyName object for each DLL
        $assemblyName = [System.Reflection.AssemblyName]::GetAssemblyName($dll.FullName)
        
        # Create a PSCustomObject and add it to the list
        $resultList += [PSCustomObject]@{
          DllPath        = $dll.FullName
          AssemblyName   = $assemblyName.FullName
          Name           = $assemblyName.Name
          Version        = $assemblyName.Version
        }
      } catch {
        Write-Host "Failed to inspect $($dll.FullName). Skipping."
      }
    }
  } else {
    Write-Host "The directory '$directory' does not exist."
  }

  return $resultList
}

function Update-RedirectPaths {
    param(
        $assemblyInfo,
        $redirects
    )

    foreach ($redirect in $redirects) {
        $matchingAssemblies = $assemblyInfo | Where-Object {
            $_.Name -eq $redirect.AssemblyName -and $_.Version -eq [System.Version]$redirect.NewVersion
        }

        foreach ($assembly in $matchingAssemblies) {
          $redirect.RedirectPath = $assembly.DllPath
        }
    }
}

$onassemblyresolve = [System.ResolveEventHandler] {
  param ($sender, $e)

  #Write-Host "------------------------------"
  #Write-Host "$($sender.FriendlyName)"
  #Write-Host "$($e.Name)"
  $assemblyNameObj = New-Object System.Reflection.AssemblyName($e.Name)

  # Iterate through the redirects
  foreach ($redirect in $redirects) {
      if ($assemblyNameObj.Name -eq $redirect.AssemblyName) {
          $assemblyVersion = [System.Version]::Parse($assemblyNameObj.Version)

          # Check if the assembly version falls within the specified range
          $minVersion = [System.Version]::Parse($redirect.OldVersion.Min)
          $maxVersion = [System.Version]::Parse($redirect.OldVersion.Max)

          if ($assemblyVersion -ge $minVersion -and $assemblyVersion -le $maxVersion -and $redirect.RedirectPath -ne $null) {
              $redirectedAssembly = [Reflection.Assembly]::LoadFrom($redirect.RedirectPath)
              Write-Host "loaded: $($redirect.RedirectPath)"
              return $redirectedAssembly
          }
      }
  }

  return $null
}

$assemblyInfo = Get-DllAssemblyProperties -directory "$PSScriptRoot"
$redirects = @(
    @{ AssemblyName = 'System.Runtime.CompilerServices.Unsafe'; NewVersion = '6.0.0.0'; OldVersion = @{ Min = '0.0.0.0'; Max='4.0.4.1' };  RedirectPath=$null }
)
Update-RedirectPaths -assemblyInfo $assemblyInfo -redirects $redirects


[system.appdomain]::currentdomain.add_assemblyresolve($onassemblyresolve)


#$loc = Test-FileExistence -SearchDirs @("$PSScriptRoot","$PSScriptRoot/net6","$PSScriptRoot/netstandard2.0") -FileName "CoreePower.Net.dll"
#[System.Reflection.Assembly]::LoadWithPartialName("$PSScriptRoot/System.Runtime.CompilerServices.Unsafe.dll")
#Add-Type -Assembly System.Runtime.CompilerServices.Unsafe
#Import-Module -Name "$PSScriptRoot/System.Runtime.CompilerServices.Unsafe.dll"
#[System.Reflection.Assembly]::LoadFile("$PSScriptRoot/System.Runtime.CompilerServices.Unsafe.dll")
#[System.Reflection.Assembly]::LoadFile("C:\Users\Valgrind\.nuget\packages\system.runtime.compilerservices.unsafe\4.0.0\lib\netstandard1.0\System.Runtime.CompilerServices.Unsafe.dll")
#[System.Reflection.Assembly]::LoadFile("C:\Users\Valgrind\.nuget\packages\system.runtime.compilerservices.unsafe\4.5.2\lib\netstandard2.0\System.Runtime.CompilerServices.Unsafe.dll")

#[System.Reflection.Assembly]::LoadFile("C:\Users\Valgrind\.nuget\packages\system.runtime.compilerservices.unsafe\4.5.2\lib\netstandard2.0\System.Runtime.CompilerServices.Unsafe.dll")
#[System.Reflection.Assembly]::LoadFile("C:\Users\Valgrind\.nuget\packages\system.buffers\4.4.0\lib\netstandard2.0\System.Buffers.dll")
#[System.Reflection.Assembly]::LoadFile("C:\Users\Valgrind\.nuget\packages\system.numerics.vectors\4.4.0\lib\netstandard2.0\System.Numerics.Vectors.dll")

#[System.Reflection.Assembly]::LoadFile("$PSScriptRoot\System.Runtime.CompilerServices.Unsafe.dll")
#[System.Reflection.Assembly]::LoadFile("$PSScriptRoot\System.Buffers.dll")
#[System.Reflection.Assembly]::LoadFile("$PSScriptRoot\System.Numerics.Vectors.dll")
#[System.Reflection.Assembly]::LoadFile("$PSScriptRoot\System.Text.Json.dll")
#[System.Reflection.Assembly]::LoadFile("$PSScriptRoot\System.Threading.Tasks.Extensions.dll")

#[System.Reflection.Assembly]::LoadFile("$PSScriptRoot/netstandard.dll")

#LoadAssemblies -directoryPath "$PSScriptRoot"


Import-Module -Name "$PSScriptRoot/CoreePower.Net.dll" -Force
#Save-Track -TrackUrl "https://soundcloud.com/slanderofficial/nguoy"




#Set-Location -Path $PSHOME
#$AccType = Add-Type -AssemblyName *jsonschema* -PassThru

# Load assembly
#$AccType = Add-Type -AssemblyName System.Windows.Forms
#[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
#$oReturn=[System.Windows.Forms.Messagebox]::Show("This is the Message text")

#$loc = Test-FileExistence -SearchDirs @("$PSScriptRoot","$PSScriptRoot/net6","$PSScriptRoot/netstandard2.0") -FileName "CoreePower.Net.dll"

#Import-Module -Name "$loc" -Force



