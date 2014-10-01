if "%~1"=="" build Build
if "%~2"=="" build %1 Orchard.proj
msbuild /t:%~1 %2

