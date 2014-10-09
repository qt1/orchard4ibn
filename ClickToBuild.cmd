if "%WindowsSdkDir%" neq "" goto build

FOR %%b in ( 
       "%VS120COMNTOOLS%..\..\VC\vcvarsall.bat"
       "%VS110COMNTOOLS%..\..\VC\vcvarsall.bat"
    ) do (
    if exist %%b ( 
       call %%b x86
       goto build
    )
)


FOR %%v in (12 11) do ( 
  echo %%v 
  FOR %%b in ( 
 	 "%ProgramFiles(x86)%\Microsoft Visual Studio %%v.0\VC\vcvarsall.bat"
   	 "%ProgramFiles%\Microsoft Visual Studio %%v.0\VC\vcvarsall.bat" 
         "D:\Program Files (x86)\Microsoft Visual Studio %%v.0\VC\vcvarsall.bat" 
         "E:\Program Files (x86)\Microsoft Visual Studio %%v.0\VC\vcvarsall.bat" 
      ) do (
      echo %%b
      if exist %%b ( 
         call %%b x86
         goto build
      )
   )
)
  
echo "Unable to detect suitable environment. Build may not succeed."
<<<<<<< HEAD

:build
=======
goto build


:initialize2k8Dev12
call "%ProgramFiles(x86)%\Microsoft Visual Studio 12.0\VC\vcvarsall.bat" x86
goto build

:initialize2k8on64Dev12
call "%ProgramFiles%\Microsoft Visual Studio 12.0\VC\vcvarsall.bat" x86
goto build

:initialize2k8Dev11
call "%ProgramFiles(x86)%\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" x86
goto build

:initialize2k8on64Dev11
call "%ProgramFiles%\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" x86
goto build

:build
if "%~1"=="" msbuild /t:Build Orchard.proj
msbuild /t:%~1 Orchard.proj

pause
goto end
>>>>>>> Orchard@Codeplex/1.8.x

call build %*

pause
