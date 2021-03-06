param($argsfile = "")

$ClickOnceName = "MetroFire"
$CloneUrl = "git://github.com/ArildF/MetroFire.git"
$ProjectFile = "src\MetroFire\MetroFire.csproj"
$MsBuild = "$env:SystemRoot\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
$PublishUrl = "http://metrofire.s3.amazonaws.com/"
$Bukkit ="metrofire"
$CheckForChangesSince = $null
$VersionsFile="src\CommonVersion.cs"
$IncrementBuild=$False
$Branch = "master"
$epoch = [datetime]"2012.07.17"


if($argsfile -ne "" -and (Test-Path $argsfile))
{
    . $argsfile
}

Write-Host "$ClickOnceName.application"

$dir = [Guid]::NewGuid().ToString()
$dir = Join-Path $env:temp $dir

$publishDir = [Guid]::NewGuid().ToString() + "\"
$publishDir = Join-Path $env:temp $publishDir

git clone --depth 1 $CloneUrl $dir

pushd
cd $dir

git checkout $Branch

if ($CheckForChangesSince)
{
    $count = (git log --since $CheckForChangesSince | measure-object)
    if($count.Count -eq 0)
    {
        Write-Host "No changes, exiting"
        popd
        Remove-Item $dir -re -fo
        exit
    }
}

$VersionString = Get-Content $VersionsFile  | ? {$_ -match 'AssemblyFileVersion\("(.*)"\)' } | % {$matches[1] }
$Version = new-object Version $VersionString


if ($IncrementBuild)
{
    $Build = ([datetime]::now - $epoch).TotalHours
    $Version = new-object Version $Version.Major,$Version.Minor,$Build,$Version.Revision
    $VersionString = $Version.ToString()
    
    $str = "using System.Reflection;`r`n[assembly: AssemblyVersion(`"{0}`")]`r`n[assembly: AssemblyFileVersion(`"{0}`")]`r`n" -f $VersionString
    Set-Content $VersionsFile $str
}

$VersionString = $Version.ToString()


& $MsBuild $ProjectFile /p:Configuration=Release `
    /t:Publish `
    /p:PublishDir=$publishDir `
    /p:PublishUrl=$PublishUrl `
    /p:InstallUrl=$PublishUrl `
    /p:ProductName=$ClickOnceName `
    /p:ApplicationName=$ClickOnceName `
    /p:AssemblyNameOverride=$ClickOnceName `
    /p:TargetDeployManifestFilename=$ClickOnceName.application `
    /p:ApplicationVersion=$VersionString

popd

S3Uploader $publishDir $Bukkit

Write-Host $publishDir

#Remove-Item $dir -re -fo
#Remove-Item $publishDir -re -fo


