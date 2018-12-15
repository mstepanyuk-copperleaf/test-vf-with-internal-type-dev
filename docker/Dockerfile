# escape=`

# Use the latest Windows Server Core image with .NET Framework 4.7.1.
FROM microsoft/dotnet-framework:4.7.1-sdk

# Download the Build Tools bootstrapper.
ADD https://aka.ms/vs/15/release/vs_buildtools.exe C:\TEMP\vs_buildtools.exe

# Install Build Tools excluding workloads and components with known issues.
RUN cmd.exe C:\TEMP\vs_buildtools.exe --quiet --wait --norestart --nocache `
    --installPath C:\BuildTools `
    --all `
    --remove Microsoft.VisualStudio.Component.Windows10SDK.10240 `
    --remove Microsoft.VisualStudio.Component.Windows10SDK.10586 `
    --remove Microsoft.VisualStudio.Component.Windows10SDK.14393 `
    --remove Microsoft.VisualStudio.Component.Windows81SDK `
  -or IF "%ERRORLEVEL%"=="3010" EXIT 0

# Note: Add NuGet
RUN Invoke-WebRequest "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" `
	-OutFile "C:\windows\nuget.exe" `
	-UseBasicParsing

# Copy and build source
WORKDIR C:\build\

ADD docker/phantomjs-2.1.1-windows.zip .

RUN Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::ExtractToDirectory('c:/build/phantomjs-2.1.1-windows.zip', 'c:/build/');

ADD . .
RUN nuget restore value-framework/framework/C55/MeasureFormulas/
RUN msbuild value-framework/framework/C55/MeasureFormulas/MeasureFormulas.sln /p:Configuration=CI
