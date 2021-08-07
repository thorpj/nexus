param (
    [switch]$WatchFileChange
)

if ($WatchFileChange) {
    $File = ".\Client\wwwroot\css\app.css"
    Write-Host "Started"
    while ($true) {
        $Before = (Get-Item $File).LastWriteTime
        Start-Sleep -Milliseconds 400
        $After = (Get-Item $File).LastWriteTime
        if ($Before -ne $After) {
            Write-Host "$File changed $(Get-Date)"
            " " | Add-Content $File
        } 
    }
    exit
}

$Path = (Get-Location).Path

Start-Process -NoNewWindow npm "run buildcss:watch" -WorkingDirectory ".\Client"
Start-Job { 
    $File = ".\Client\wwwroot\css\app.css"

    Set-Location $using:Path
    Write-Host "Started"
    while ($true) {
        $Before = (Get-Item $File).LastWriteTime
        Start-Sleep -Milliseconds 400
        $After = (Get-Item $File).LastWriteTime
        if ($Before -ne $After) {
            Write-Host "$File changed $(Get-Date)"
            " " | Add-Content $File
        } 
    }
 }
Set-Location ".\Server"
dotnet watch run