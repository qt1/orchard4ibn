<<<<<<< HEAD
if "%~1"=="" build Build
if "%~2"=="" build %1 Orchard.proj
msbuild /t:%~1 %2

=======
if "%~1"=="" call clicktobuild 
call clicktobuild %~1 
>>>>>>> Orchard@Codeplex/1.8.x
