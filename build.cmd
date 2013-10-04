@echo off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=0.2.0-alpha
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=src\.nuget\nuget.exe
)

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild src\ReSharper.ComGuidChecker.sln /t:Rebuild /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
 
%nuget% pack "src\.nuget\ReSharper.ComGuidChecker.nuspec" -NoPackageAnalysis -verbosity detailed -o . -Version %version% -p Configuration="%config%"