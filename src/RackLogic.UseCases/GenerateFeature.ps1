param (
    [string]$FeatureName,
    [string]$OutputDir
)

if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir | Out-Null
    Write-Host "Created output directory: $OutputDir"
}

function CreateFile($FileType, $FeatureName, $OutputDir) {
    $FileName = "${FeatureName}${FileType}.cs"
    $FilePath = Join-Path $OutputDir $FileName

    switch ($FileType) {
        "Command" {
            $Content = @"
using Ardalis.Result;
using Ardalis.SharedKernel;

public record ${FeatureName}Command(): ICommand<Result>;
"@
        }
        "Handler" {
            $Content = @"
using Ardalis.Result;
using Ardalis.SharedKernel;

public class ${FeatureName}Handler : ICommandHandler<${FeatureName}Command, Result>
{
    public Task<Result> Handle(${FeatureName}Command request, CancellationToken cancellationToken)
    {
        // Add your logic here
    }
}
"@
        }
    }

    Set-Content -Path $FilePath -Value $Content
    Write-Host "Created: $FilePath"
}

CreateFile "Command" $FeatureName $OutputDir
CreateFile "Handler" $FeatureName $OutputDir
Write-Host "Feature '$FeatureName' files generated successfully in $OutputDir"