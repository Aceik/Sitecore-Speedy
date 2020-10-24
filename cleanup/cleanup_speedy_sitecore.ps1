Get-Item -Path "master:\sitecore\templates\Foundation\Speedy" | Remove-Item -Recurse -Permanently -Force
Get-Item -Path "master:\sitecore\system\Settings\Foundation\Speedy" | Remove-Item -Recurse -Permanently -Force
Get-Item -Path "master:\sitecore\layout\Renderings\Foundation\Speedy" | Remove-Item -Recurse -Permanently -Force
Get-Item -Path "master:\sitecore\layout\Layouts\Foundation\Speedy" | Remove-Item -Recurse -Permanently -Force

Write-Host "done"