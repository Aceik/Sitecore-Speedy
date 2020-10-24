Set-Variable -Name "speedyRoot" -Value "C:\inetpub\wwwroot\habitat-combo.dev.local - Copy"

Write-Host "Running cleanup script..."
Remove-Item "$speedyRoot\views\speedy\" -Recurse
Remove-Item "$speedyRoot\views\SxaLayout\SxaLayoutPageSpeed.cshtml"
Write-Host "removed speed views $speedyRoot\views\speedy\"


Remove-Item "$speedyRoot\scripts\lazy.js"
Remove-Item "$speedyRoot\scripts\mobile-vanilla-critical.js"
Write-Host "removed javascript files"


Remove-Item "$speedyRoot\bin\Sitecore.Foundation.Speedy.dll"
Remove-Item "$speedyRoot\bin\Sitecore.Foundation.Speedy.pdb"
Remove-Item "$speedyRoot\bin\RestSharp.dll"
Write-Host "removed dlls"